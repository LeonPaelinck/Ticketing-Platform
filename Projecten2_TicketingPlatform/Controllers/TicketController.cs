using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projecten2_TicketingPlatform.Models.Domein;
using Projecten2_TicketingPlatform.Models.TicketViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IContractRepository _contractRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHostingEnvironment _hosting;

        public TicketController(ITicketRepository ticketRepository, IContractRepository contractRepository, UserManager<IdentityUser> userManager, IHostingEnvironment hosting=null)
        {
            _ticketRepository = ticketRepository;
            _contractRepository = contractRepository;
            _userManager = userManager;
            _hosting = hosting;
        }
    

        public IActionResult Index(TicketStatus ticketStatus = TicketStatus.Standaard)
        {
            IEnumerable<Ticket> tickets;
            if (User.IsInRole("supportmanager"))
            {
                if (ticketStatus == TicketStatus.Standaard)
                {
                    tickets = _ticketRepository.GetAllByTicketStatus(new List<TicketStatus> { TicketStatus.Aangemaakt, TicketStatus.InBehandeling });
                }
                else
                if (ticketStatus == TicketStatus.Alle)
                {
                    tickets = _ticketRepository.GetAll();
                }
                else
                {
                    tickets = _ticketRepository.GetAllByTicketStatus(new List<TicketStatus> { ticketStatus });
                }
                if (tickets.Count() == 0)
                {
                    if (!ticketStatus.Equals(TicketStatus.Standaard))
                    {
                        TempData["Waarschuwing"] = $"Er zijn geen tickets met status {ticketStatus.GetDisplayAttributeFrom(typeof(TicketStatus))}";
                    }
                }
            }
            else
            {
                if (ticketStatus == TicketStatus.Standaard) 
                {
                    tickets = _ticketRepository.GetAllByClientIdByTicketStatus(_userManager.GetUserId(User), 
                        new List<TicketStatus> { TicketStatus.Aangemaakt, TicketStatus.InBehandeling });
                } 
                else
                if (ticketStatus == TicketStatus.Alle)
                {
                    tickets = _ticketRepository.GetAllByClientId(_userManager.GetUserId(User));
                }
                else
                {
                    tickets = _ticketRepository.GetAllByClientIdByTicketStatus(_userManager.GetUserId(User),
                        new List<TicketStatus> {  ticketStatus });
                }
                if (tickets.Count() == 0)
                {
                    if (!ticketStatus.Equals(TicketStatus.Standaard))
                    {
                        TempData["Waarschuwing"] = $"Uw account beschikt niet over tickets met status {ticketStatus.GetDisplayAttributeFrom(typeof(TicketStatus))}";
                    }
                }
            }

            TempData["TicketStatus"] = ticketStatus.ToString();
            ViewData["TicketStatussen"] = new SelectList(new List<TicketStatus> { TicketStatus.Aangemaakt, TicketStatus.InBehandeling, TicketStatus.Afgehandeld, TicketStatus.Geannuleerd, TicketStatus.WachtenOpInformatieKlant, TicketStatus.InformatieKlantOntvangen, TicketStatus.InDevelopment });
            return View(tickets);
        }
        #region == Create Methodes ==
        public IActionResult Create()
        {
           if (IsAllowedToCreateTickets(_userManager.GetUserId(User)) || User.IsInRole("supportmanager"))
            {
                ViewData["IsEdit"] = false;
                ViewData["TicketType"] = TicketTypesAsSelectList();
                return View("Edit", new EditViewModel()); 
            }
            else
            {                
                TempData["Waarschuwing"] = $"Uw account beschikt niet over actieve contracten";
                return RedirectToAction("Index");
            }


        }
        [HttpPost]
        public IActionResult Create(EditViewModel ticketVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string klantId = _userManager.GetUserId(User);
                    if (ticketVm.KlantId != "" && ticketVm.KlantId != null)
                    {
                        klantId = ticketVm.KlantId;
                    }
                    if (!IsAllowedToCreateTickets(klantId))
                    {
                        throw new ArgumentException("Dit account heeft geen actieve contracten.");
                    }
                    else
                    {
                        string bestandsNaam = UploadBestand(ticketVm.Bijlage);

                        Ticket ticket = new Ticket
                        {
                            DatumAanmaken = ticketVm.DatumAanmaken,
                            Titel = ticketVm.Titel,
                            Omschrijving = ticketVm.Omschrijving,
                            TypeTicket = ticketVm.TypeTicket,
                            /*ticketVm.Technieker, ticketVm.Opmerkingen,*/
                            Bijlage = bestandsNaam,
                            KlantId = klantId,
                            Status = TicketStatus.Aangemaakt
                        };

                        _ticketRepository.Add(ticket);
                        _ticketRepository.SaveChanges();
                        TempData["Succes"] = "Aanmaken ticket gelukt!";
                    }
                }
                catch (ArgumentException ae)
                {
                    TempData["FoutMelding"] = "Aanmaken ticket mislukt. " + ae.Message;
                }
                return RedirectToAction(nameof(Index)); 
            }
            ViewData["IsEdit"] = false;
            ViewData["TicketType"] = TicketTypesAsSelectList();
            return View("Edit", ticketVm);

        }

        #endregion

        #region == Edit Methodes ==
        public IActionResult Edit(int ticketId)
        {
            Ticket ticket = _ticketRepository.GetById(ticketId);
            if (ticket == null)
                return new NotFoundResult();
            ViewData["IsEdit"] = true;
            ViewData["TicketType"] = TicketTypesAsSelectList();

            return View(new EditViewModel(ticket));
        }
        [HttpPost]
        public IActionResult Edit(EditViewModel ticketVm, int ticketId)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    Ticket ticket = _ticketRepository.GetById(ticketId);
                    if (ticket == null)
                        return new NotFoundResult();
                    ticket.DatumAanmaken = ticketVm.DatumAanmaken;
                    ticket.Titel = ticketVm.Titel;
                    ticket.Omschrijving = ticketVm.Omschrijving;
                    ticket.TypeTicket = ticketVm.TypeTicket;
                    /*ticketVm.Technieker, ticketVm.Opmerkingen,*/

                    string bestandsNaam = UploadBestand(ticketVm.Bijlage);
                    if (bestandsNaam != null) 
                        ticket.Bijlage = bestandsNaam;

                    //ticket.KlantId = _userManager.GetUserId(User); //niet nodig denk ik, verandert niet
                    ticket.Status = TicketStatus.Aangemaakt; 
                    _ticketRepository.SaveChanges();
                    TempData["Succes"] = "Bewerken ticket gelukt!";
                }
                catch (ArgumentException ae)
                {
                    TempData["FoutMelding"] = "Bewerken ticket mislukt. " + ae.Message;
                }
                return RedirectToAction(nameof(Index)); 
            }
            ViewData["IsEdit"] = true;
            ViewData["TicketType"] = TicketTypesAsSelectList();
            return View(ticketVm);
        }

        #endregion

        #region == Delete Methodes ==
        public IActionResult Annuleer(int ticketId)
        {
            Ticket ticket = _ticketRepository.GetById(ticketId);
            if(ticket.Status.Equals(TicketStatus.Geannuleerd))
            {
                TempData["FoutMelding"] = "Dit ticket is reeds geannuleerd";
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }



        [HttpPost]
        public IActionResult AnnuleerConfirmed(int ticketId)
        {
            Ticket ticket = _ticketRepository.GetById(ticketId);
            ticket.Status = TicketStatus.Geannuleerd;
            _ticketRepository.SaveChanges();
            TempData["Succes"] = "Ticket is geannuleerd";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region == Opvolging Methodes ==
        public IActionResult Opvolging(int ticketId)
        {
            Ticket ticket = _ticketRepository.GetById(ticketId);
            if (ticket == null)
                return new NotFoundResult();
            return View(new OpvolgingViewModel(ticket));
        }
        [HttpPost]
        public IActionResult Opvolging(OpvolgingViewModel ticketVm, int ticketId)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    Ticket ticket = _ticketRepository.GetById(ticketId);
                    if (ticket == null)
                        return new NotFoundResult();
                    ticket.Oplossing = ticketVm.Oplossing;
                    ticket.Kwaliteit = ticketVm.Kwaliteit;
                    ticket.SupportNodig = ticketVm.SupportNodig;

                    _ticketRepository.SaveChanges();
                    TempData["Succes"] = "Opvolging succesvol toegevoegd!";
                }
                catch (ArgumentException ae)
                {
                    TempData["FoutMelding"] = "Bewerken ticket mislukt. " + ae.Message;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IsEdit"] = true;
            ViewData["TicketType"] = TicketTypesAsSelectList();
            return View(ticketVm);
        } 
        #endregion

        [HttpGet]
        public ActionResult GetPdf(string filePath, string fileName)
        {
            try
            {
                Response.Headers.Add("Content-Disposition", String.Format("inline; filename = {0}", fileName));
                return File(filePath, "application/pdf");
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        #region == Private Methodes ==
        private SelectList TicketTypesAsSelectList(int selected = 0)
        {
            SelectListItem selListItem = new SelectListItem() { Value = "1", Text = "Productie geïmpacteerd" };
            SelectListItem selListItem2 = new SelectListItem() { Value = "2", Text = "Productie zal binnen een tijd stilvallen" };
            SelectListItem selListItem3 = new SelectListItem() { Value = "3", Text = "Geen productie impact" };

            List<SelectListItem> newList = new List<SelectListItem>
            {
                selListItem,
                selListItem2,
                selListItem3
            };

            return new SelectList(newList, "Value", "Text", selected); ;
        }

        private bool IsAllowedToCreateTickets(string klantId)
        {
            IEnumerable<ManierVanAanmakenTicket> applicatieStatussen = new List<ManierVanAanmakenTicket> { ManierVanAanmakenTicket.Applicatie, ManierVanAanmakenTicket.EmailEnApplicatie, ManierVanAanmakenTicket.EmailEnTelefonischEnApplicatie, ManierVanAanmakenTicket.TelefonischEnApplicatie };
            IEnumerable<Contract> contracts = _contractRepository.GetAllByClientId(klantId);

            IEnumerable<Contract> actieveContracten = contracts.Where(t => t.ContractStatus.Equals(ContractEnContractTypeStatus.Actief));
            bool HeeftApplicatieContractMetMinimaleDoorlooptijd = actieveContracten.Any(t => applicatieStatussen.Contains(t.ContractType.ManierVanAanmakenTicket) && t.ContractType.MinimaleDoorlooptijd <= t.Doorlooptijd);

            return HeeftApplicatieContractMetMinimaleDoorlooptijd;
        }

        private string UploadBestand(IFormFile bijlage)
        {
            string bestandsNaam = null;
            if (bijlage != null)
            {
                string uploadFolder = Path.Combine(_hosting.WebRootPath, "bijlagen");
                //guid zorgt ervoor dat de bestandsnaam uniek is
                bestandsNaam = Guid.NewGuid().ToString() + "_" + bijlage.FileName;
                string filePath = Path.Combine(uploadFolder, bestandsNaam);

                bijlage.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            return bestandsNaam;
        }
        #endregion
    }
}
/*IEnumerable<ManierVanAanmakenTicket> applicatieStatussen = new List<ManierVanAanmakenTicket> { ManierVanAanmakenTicket.Applicatie, ManierVanAanmakenTicket.EmailEnApplicatie, ManierVanAanmakenTicket.EmailEnTelefonischEnApplicatie, ManierVanAanmakenTicket.TelefonischEnApplicatie };
return _contracten
    .Where(t => t.ClientId.Equals(klantId))
    .Any(
        t => (t.ContractStatus.Equals(ContractEnContractTypeStatus.Actief)
        && applicatieStatussen.Contains(t.ContractType.ManierVanAanmakenTicket))
     );*/


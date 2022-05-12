using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Projecten2_TicketingPlatform.Models.ContractViewModels;
using Projecten2_TicketingPlatform.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Controllers
{
    public class ContractController : Controller
    {
        private readonly IContractRepository _contractRepository;
        private readonly IContractTypeRepository _contractTypeRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public ContractController(IContractRepository contractRepository, UserManager<IdentityUser> userManager, IContractTypeRepository contractTypeRepository)
        {
            _contractRepository = contractRepository;
            _contractTypeRepository = contractTypeRepository;
            _userManager = userManager;
        }
        public IActionResult Index(ContractEnContractTypeStatus contractStatus = ContractEnContractTypeStatus.Standaard)
        {
            IEnumerable<Contract> contracten = _contractRepository.GetAllByClientId(_userManager.GetUserId(User));
            if (contractStatus == ContractEnContractTypeStatus.Alle)
            {
                //geen gefilter, alle contracten meegeven
            }
            else if (contractStatus== ContractEnContractTypeStatus.Standaard)
            {
                contracten = FilterContractenOpStatussen(contracten, new List<ContractEnContractTypeStatus> { ContractEnContractTypeStatus.Actief, ContractEnContractTypeStatus.InBehandeling } );
            }
            else
            {
                contracten = FilterContractenOpStatussen((contracten), new List<ContractEnContractTypeStatus> { contractStatus });
            }
            if (contracten.Count() == 0)
            {
                TempData["Waarschuwing"] = $"Uw account beschikt niet over contracten met status {contractStatus.GetDisplayAttributeFrom(typeof(ContractEnContractTypeStatus))}";
            }
            ViewData["ContractStatussen"] = new SelectList(new List<ContractEnContractTypeStatus> { ContractEnContractTypeStatus.Alle, ContractEnContractTypeStatus.Standaard, ContractEnContractTypeStatus.Actief, ContractEnContractTypeStatus.Afgelopen, ContractEnContractTypeStatus.InBehandeling, ContractEnContractTypeStatus.NietActief, ContractEnContractTypeStatus.Stopgezet });       
            return View(contracten);
        }

        public IActionResult Create()
        {
            IEnumerable<Contract> contracten = _contractRepository.GetAllByClientId(_userManager.GetUserId(User));
            IEnumerable<Contract> afgelopenContracten = contracten.Where(c => c.ContractStatus.Equals(ContractEnContractTypeStatus.Afgelopen));

            ViewData["contractTypes"] = new SelectList(_contractTypeRepository.GetAll(), nameof(ContractType.ContractTypeId), nameof(ContractType.Naam));
            //als er al een contract
            if (contracten.Any(c=> c.ContractStatus.Equals(ContractEnContractTypeStatus.Actief) || c.ContractStatus.Equals(ContractEnContractTypeStatus.InBehandeling) ) || contracten.Count() == 0 || afgelopenContracten.Count() == 0)
            {
                return View(new EditViewModel());
            }
            else
            {   //Gegevens oude laatst afgelopen contract ophalen voor nieuw contract vooraf in te vullen.
                return View(new EditViewModel(afgelopenContracten.Last()));
            }
        }

        [HttpPost]
        public IActionResult Create(EditViewModel contractVm) {
            if (ModelState.IsValid)
            {
                try
                {
                    ContractType ct = _contractTypeRepository.GetById(contractVm.ContractTypeId);
                    if (contractVm.Doorlooptijd < ct.MinimaleDoorlooptijd)
                    {
                        throw new ArgumentException($"Het contract type {ct.Naam} vereist een minimale doorlooptijd van {ct.MinimaleDoorlooptijd} jaar.");
                    }
                    Contract contract = new Contract(contractVm.Startdatum, ct, contractVm.Doorlooptijd, _userManager.GetUserId(User));

                    IEnumerable<Contract> contracten = _contractRepository.GetAllByClientId(_userManager.GetUserId(User));
                    contracten = FilterContractenOpStatussen(contracten, new List<ContractEnContractTypeStatus> { ContractEnContractTypeStatus.Actief, ContractEnContractTypeStatus.InBehandeling });
                    //Een klant kan per contracttype maar één contract met de status “in behandeling” of  “actief” hebben 
                    if (contracten.Any(c => c.ContractType.ContractTypeId == contractVm.ContractTypeId)) 
                    {
                        throw new ArgumentException("Er is al een contract van dit type in behandeling");
                    }
                    else
                    {
                        _contractRepository.Add(contract);
                        _contractRepository.SaveChanges();
                        TempData["Succes"] = "Aanvragen contract gelukt!";

                    }
                }
                catch (ArgumentException ae)
                {
                    TempData["FoutMelding"] = "Aanvragen contract mislukt. " + ae.Message;
                }
                return RedirectToAction(nameof(Index));

            }
            ViewData["contractTypes"] = new SelectList(_contractTypeRepository.GetAll(), nameof(ContractType.ContractTypeId), nameof(ContractType.Naam));
            return View("Create", contractVm);

        }


        #region == Delete Methodes ==
        public IActionResult Annuleer(int contractId)
        {
            Contract contract = _contractRepository.GetById(contractId);
            //if (contract.ContractStatus.Equals(ContractStatus.Stopgezet))
            //{
            //    TempData["Boodschap"] = "Dit contract is reeds stopgezet";
            //    return RedirectToAction(nameof(Index));
            //}
            return View(contract);
        }

        [HttpPost]
        public IActionResult AnnuleerConfirmed(int contractId)
        {
            Contract contract = _contractRepository.GetById(contractId);
            contract.ZetStop();
            _contractRepository.SaveChanges();
            TempData["Succes"] = "Contract is stopgezet";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region === Private ===

        private IEnumerable<Contract> FilterContractenOpStatussen(IEnumerable<Contract> contracten, IEnumerable<ContractEnContractTypeStatus> contractStatuses)
        {
            return contracten.Where(p => contractStatuses.Contains(p.ContractStatus)).OrderByDescending(p => p.EindDatum).ToList();
        }

        #endregion
    }


}

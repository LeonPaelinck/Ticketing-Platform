using Microsoft.AspNetCore.Http;
using Projecten2_TicketingPlatform.Models.Domein;
using Projecten2_TicketingPlatform.Models.Extenties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Models.TicketViewModels
{
    public class EditViewModel
    {
        public string KlantId { get; set; }

        [Required(ErrorMessage = "U moet een startdatum kiezen.")]
        [DataType(DataType.Date)]
        [Display(Name = "Datum aanmaken")]
        public DateTime DatumAanmaken { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "U moet een titel ingeven.")]
        public string Titel { get; set; }

        [Required(ErrorMessage = "U moet een omschrijving ingeven.")]
        public string Omschrijving { get; set; }

        [Required(ErrorMessage = "U moet een type kiezen.")]
        [Display(Name = "Type")]
        public int TypeTicket { get; set; }
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".pjpeg", ".gif", ".x-png", ".png", ".pdf"})]
        public IFormFile Bijlage { get; set; }
        public string BijlagePad { get; private set; }

        public EditViewModel()
        {
        }

        public EditViewModel(Ticket ticket) : this()
        {
            KlantId = ticket.KlantId;
            DatumAanmaken = ticket.DatumAanmaken;
            Titel = ticket.Titel;
            Omschrijving = ticket.Omschrijving;
            TypeTicket = ticket.TypeTicket;
            /*Technieker = ticket.Technieker;
            Opmerkingen = ticket.Opmerkingen;*/
            Bijlage = null;
            BijlagePad = ticket.Bijlage;
        }
    }
}

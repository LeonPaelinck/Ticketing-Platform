using Microsoft.AspNetCore.Http;
using Projecten2_TicketingPlatform.Models.Domein;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Models.TicketViewModels
{
    public class OpvolgingViewModel
    {
        [Required]
        [Range(1, 5)]
        [Display(Name = "Was er voldoende informatie? Beoordeel met een schaal van 1 tot 5")]
        public int Kwaliteit { get; set; }

        [Required]
        [Display(Name = "Heeft u de oplossing verkregen via de knowledgebase?")]
        public bool Oplossing { get; set; }

        [Required]
        [Display(Name = "Had u support nodig om het ticket aan te maken?")]
        public bool SupportNodig { get; set; }

        public OpvolgingViewModel()
        {
        }

        public OpvolgingViewModel(Ticket ticket) : this()
        {
            Kwaliteit = ticket.Kwaliteit;
            Oplossing = ticket.Oplossing;
            SupportNodig = ticket.SupportNodig;
        }
    }
}

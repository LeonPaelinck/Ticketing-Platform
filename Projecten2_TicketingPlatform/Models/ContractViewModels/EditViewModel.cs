using Projecten2_TicketingPlatform.Models.Domein;
using System;
using System.ComponentModel.DataAnnotations;

namespace Projecten2_TicketingPlatform.Models.ContractViewModels
{
    public class EditViewModel
    {
        [Required(ErrorMessage = "U moet een type kiezen.")]
        [Display(Name = "Contract type")]
        public int ContractTypeId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Startdatum { get; set; } = DateTime.Today;
        [Required]
        [Range (1,3, ErrorMessage = "Doorlooptijd moet tussen 1 en 3 jaar zijn.")]
        public int Doorlooptijd { get; set; }
        public EditViewModel()
        {
        }
        public EditViewModel(Contract contract):this()
        {
            ContractTypeId = contract.ContractType.ContractTypeId;
            Startdatum = contract.StartDatum;
            Doorlooptijd = contract.Doorlooptijd;
        }

       
    }
}

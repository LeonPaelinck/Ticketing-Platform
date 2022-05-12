using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Projecten2_TicketingPlatform
{ 
    public enum ContractEnContractTypeStatus
    {
        [Display(Name = "In behandeling")]
        InBehandeling,
        Actief,
        Afgelopen,
        Stopgezet,
        [Display(Name = "Niet actief")]
        NietActief,
        Standaard,
        [Display(Name = "Alle contracten")]
        Alle
    }
}
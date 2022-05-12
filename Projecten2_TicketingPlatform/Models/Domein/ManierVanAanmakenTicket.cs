using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Models.Domein
{
    public enum ManierVanAanmakenTicket
    {
        Email,
        Telefonisch,
        Applicatie,
        EmailEnApplicatie,
        EmailEnTelefonisch,
        TelefonischEnApplicatie,
        EmailEnTelefonischEnApplicatie
    }
}

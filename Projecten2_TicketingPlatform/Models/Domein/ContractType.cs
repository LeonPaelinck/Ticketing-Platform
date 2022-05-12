using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Models.Domein
{
    public class ContractType
    {
        public int ContractTypeId { get; set; }
        public string Naam { get; set; }
        public ContractEnContractTypeStatus Status { get; set; }
        public ManierVanAanmakenTicket ManierVanAanmakenTicket { get; set; }
        public TijdstipTicketAanmaken TijdstipTicketAanmaken { get; set; }
        public int MinimaleDoorlooptijd { get; set; }
        public int MaximaleAfhandeltijd { get; set; }
        public double ContractPrijs { get; set; }

        public ContractType()
        {

        }
        public ContractType(string naam, ContractEnContractTypeStatus status, ManierVanAanmakenTicket manierVanAanmakenTicket, TijdstipTicketAanmaken tijdstipTicketAanmaken, int minimaleDoorloopTijd, int maximaleAfhandeltijd, double contractPrijs)
        {
            Naam = naam;
            Status = status;
            ManierVanAanmakenTicket = manierVanAanmakenTicket;
            TijdstipTicketAanmaken = tijdstipTicketAanmaken;
            MinimaleDoorlooptijd = minimaleDoorloopTijd;
            MaximaleAfhandeltijd = maximaleAfhandeltijd;
            ContractPrijs = contractPrijs;
        }
       
    }
}

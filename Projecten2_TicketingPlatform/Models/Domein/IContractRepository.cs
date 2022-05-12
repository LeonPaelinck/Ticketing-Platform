using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Models.Domein
{
    public interface IContractRepository
    {
        Contract GetById(int contractId);
        IEnumerable<Contract> GetAllByClientId(string clientId);
        void Add(Contract contract);
        void SaveChanges();
    }
}

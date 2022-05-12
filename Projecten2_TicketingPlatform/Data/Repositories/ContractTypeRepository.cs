using Microsoft.EntityFrameworkCore;
using Projecten2_TicketingPlatform.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Projecten2_TicketingPlatform.Data.Repositories
{
    public class ContractTypeRepository : IContractTypeRepository
    {
        private readonly DbSet<ContractType> _contractTypes;

        public ContractTypeRepository(ApplicationDbContext context)
        {
            _contractTypes = context.ContractTypes;
        }
        public IEnumerable<ContractType> GetAll()
        {
            return _contractTypes.OrderBy(t => t.Naam).AsNoTracking().ToList();
        }

        public ContractType GetById(int id)
        {
            return _contractTypes.SingleOrDefault(p => p.ContractTypeId == id);
        }
    }
}

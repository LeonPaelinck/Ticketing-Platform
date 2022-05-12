using Microsoft.EntityFrameworkCore;
using Projecten2_TicketingPlatform.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Data.Repositories
{
    public class KnowledgeBaseRepository : IKnowledgeBaseRepository
    {
        
        private readonly ApplicationDbContext _context;
        private readonly DbSet<KnowledgeBase> _knowledgeBase;

        public KnowledgeBaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _knowledgeBase = context.KnowledgeBase;
        }

        public void Add(KnowledgeBase knowledgeBase)
        {
            _knowledgeBase.Add(knowledgeBase);
        }

        public void Delete(KnowledgeBase knowledgeBase)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<KnowledgeBase> GetAll()
        {
            return _knowledgeBase.OrderBy(t => t.DatumToevoegen).AsNoTracking().ToList();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}

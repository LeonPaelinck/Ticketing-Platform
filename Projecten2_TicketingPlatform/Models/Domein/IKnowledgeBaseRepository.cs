using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Models.Domein
{
    public interface IKnowledgeBaseRepository
    {     
        IEnumerable<KnowledgeBase> GetAll();        
        void Add(KnowledgeBase knowledgeBase);
        void Delete(KnowledgeBase knowledgeBase);
        void SaveChanges();    
    }
}

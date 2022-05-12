using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projecten2_TicketingPlatform.Data;
using Projecten2_TicketingPlatform.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Controllers
{
    public class KnowledgeBaseController : Controller
    {
        private readonly IKnowledgeBaseRepository _knowledgeBaseRepository;
        public KnowledgeBaseController(IKnowledgeBaseRepository knowledgeBaseRepository)
        {
            _knowledgeBaseRepository = knowledgeBaseRepository;
        }
        public IActionResult Index(string searchString)
        {
            IEnumerable<KnowledgeBase> knowledgeBases;
            knowledgeBases = _knowledgeBaseRepository.GetAll();
            if (!String.IsNullOrEmpty(searchString))
            {
                knowledgeBases = knowledgeBases.Where(s => s.Titel.Contains(searchString));
            }
            TempData["searchString"] = searchString;
            return View(knowledgeBases);
        }
    }
}

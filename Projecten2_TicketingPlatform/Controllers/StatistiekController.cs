using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Projecten2_TicketingPlatform.Models.Domein;

namespace Projecten2_TicketingPlatform.Controllers
{
    public class StatistiekController : Controller
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IContractRepository _contractRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public StatistiekController(ITicketRepository ticketRepository, IContractRepository contractRepository, UserManager<IdentityUser> userManager)
        {
            _ticketRepository = ticketRepository;
            _contractRepository = contractRepository;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }


    }
}

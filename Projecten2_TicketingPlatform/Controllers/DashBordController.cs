using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projecten2_TicketingPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Projecten2_TicketingPlatform.Models.Domein;
using Microsoft.AspNetCore.Identity;

namespace Projecten2_TicketingPlatform.Controllers
{
    public class DashbordController : Controller
    {
        private readonly IContractRepository _contractRepository;
        private readonly IContractTypeRepository _contractTypeRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private Contract _contract;

        public DashbordController(IContractRepository contractRepository, UserManager<IdentityUser> userManager, IContractTypeRepository contractTypeRepository)
        {
            _contractRepository = contractRepository;
            _userManager = userManager;
            _contractTypeRepository = contractTypeRepository;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (HeeftContractDatVerloopt(_userManager.GetUserId(User)))
            {
                TempData["ContractVerloopt"] = $"Het contract met ID {_contract.ContractId} verloopt op {_contract.EindDatum.ToString("dd/MM/yyyy")}";
            }
            return View();

        }
        private bool HeeftContractDatVerloopt(string klantId)
        {
            IEnumerable<Contract> actieveContracten= _contractRepository.GetAllByClientId(klantId).Where(t => t.ContractStatus.Equals(ContractEnContractTypeStatus.Actief));
            _contract= actieveContracten.FirstOrDefault(t => t.EindDatum < DateTime.Today.AddDays(30));
            if (_contract != null)
            {
                return true;
            }
            else return false;
        }

    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Projecten2_TicketingPlatform.Controllers;
using Projecten2_TicketingPlatform.Models.ContractViewModels;
using Projecten2_TicketingPlatform.Models.Domein;
using Projecten2_TicketingPlatform.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Xunit;

namespace Projecten2_TicketingPlatform.Tests.Controllers
{
    public class ContractControllerTest
    {
        private readonly ContractController _contractController;
        private readonly Mock<IContractRepository> _mockContractRepository;
        private readonly Mock<IContractTypeRepository> _mockContractTypeRepository;
        private readonly Contract _contractActief;
        private readonly Contract _contractNietActief;
        private readonly Contract _contractAfgelopen;
        private readonly DummyApplicationDbContext _dummyContext = new DummyApplicationDbContext();
        private readonly List<Contract> _contracts;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly IEnumerable<ContractEnContractTypeStatus> _statussen;
        private readonly string _userId; //"bff6a934 - 0dca - 4965 - b9fc - 91c3290792c8"

        public ContractControllerTest()
        {
            _userId = _dummyContext.UserId;

            _contractActief = _dummyContext.ContractActief;
            _contractNietActief = _dummyContext.ContractNietActief;
            _contractAfgelopen = _dummyContext.ContractAfgelopen;

            var store = new Mock<IUserStore<IdentityUser>>();
            store.Setup(x => x.FindByIdAsync("123", CancellationToken.None))
                .ReturnsAsync(new IdentityUser()
                {
                    UserName = "test@email.com",
                    Id = _userId
                }) ;

            _mockUserManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            _mockUserManager.Setup(mum => mum.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(_userId); //It.IsAny zal eignelijk altijd null zijn


            _mockContractRepository = new Mock<IContractRepository>();
            _mockContractTypeRepository = new Mock<IContractTypeRepository>();
            _contracts = new List<Contract>() { _contractActief, _contractNietActief };
            _statussen = new List<ContractEnContractTypeStatus> { ContractEnContractTypeStatus.Actief, ContractEnContractTypeStatus.InBehandeling };

            _contractController = new ContractController(_mockContractRepository.Object, _mockUserManager.Object, _mockContractTypeRepository.Object)
            {
                TempData = new Mock<ITempDataDictionary>().Object
            };
        }
        #region == Index Methodes ==

        [Fact]
        public void IndexAlle_ReturnsAViewResult_WithAListOfContracts()
        {
            //Arrange
            List<ContractEnContractTypeStatus> statussen = new List<ContractEnContractTypeStatus> { ContractEnContractTypeStatus.Actief, ContractEnContractTypeStatus.InBehandeling };
            _mockContractRepository.Setup(r => r.GetAllByClientId(_userId))
                .Returns(_contracts); //1 actief en 1 niet actief
            //Act
            var result = _contractController.Index(ContractEnContractTypeStatus.Alle);
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Contract>>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
            //Verify
            _mockContractRepository.Verify(r => r.GetAllByClientId(_userId), Times.Once);
        }

        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfContracts()
        {
            //Arrange
            List<ContractEnContractTypeStatus> statussen = new List<ContractEnContractTypeStatus> { ContractEnContractTypeStatus.Actief, ContractEnContractTypeStatus.InBehandeling };
            _mockContractRepository.Setup(r => r.GetAllByClientId(_userId))
                .Returns(_contracts); //1 actief en 1 niet actief
            //Act
            var result = _contractController.Index(); //enkel de actieve en degene in behandeling
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Contract>>(
                viewResult.ViewData.Model);
            Assert.Single(model); // er zal slechts 1 getoon worden namelijk het actieve contract
            //Verify
            _mockContractRepository.Verify(r => r.GetAllByClientId(_userId), Times.Once);
        }

        [Fact]
        public void Index_NoContracts_ReturnsNoContracts()
        {
            _mockContractRepository.Setup(r => r.GetAllByClientId("bff6a934 - 0dca - 4965 - b9fc - onbestaande id")) //wat is hier het nut? Als we geen contracten willen linken aan ons userid waarom moeten we dan contracten linken aan een niet geburikt id?
                .Returns(_contracts);
            var result = _contractController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Contract>>(
                viewResult.ViewData.Model);
            Assert.Empty(model);
        } 
        #endregion

        #region == Create Methodes ==
        [Fact]
        public void CreateHttpGet_Contract_NoActiveContracts_PassesDetailsOfExpiredContractInEditViewModelToView()
        {
            //_mockUserManager.Setup(u => u.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(USERID); //dit werkt niet, volgens mij wordt dat toch nooit opgeroepen
            _mockContractRepository.Setup(p => p.GetAllByClientId(_userId)).Returns(new List<Contract>() { _contractNietActief, _contractAfgelopen });

            var result = Assert.IsType<ViewResult>(_contractController.Create());
            var contractVm = Assert.IsType<EditViewModel>(result.Model);

            Assert.Equal(_contractAfgelopen.Doorlooptijd, contractVm.Doorlooptijd);
           _mockContractRepository.Verify(mock => mock.GetAllByClientId(_userId), Times.Once);
        }
        [Fact]
        public void CreateHttpGet_ActiveContract_PassesNoDetailsOfANewTicketInEditViewModelToView()
        {
            //Arrange
            _mockContractRepository.Setup(p => p.GetAllByClientId(_userId)).Returns(_contracts);
            //Act
            var result = Assert.IsType<ViewResult>(_contractController.Create());
            //Assert
            var contractVm = Assert.IsType<EditViewModel>(result.Model);
            Assert.Equal(0, contractVm.Doorlooptijd);
            //Verify
            _mockContractRepository.Verify(mock => mock.GetAllByClientId(_userId), Times.Once);

        }


        
        [Fact]
        public void CreateHttpPost_ValidTicket_AddsNewContractToRepositoryAndRedirectsToIndex()
        {
            _mockContractRepository.Setup(p => p.Add(It.IsNotNull<Contract>()));
            _mockContractRepository.Setup(p => p.GetAllByClientId(It.IsNotNull<string>()))
                .Returns(_contracts);
            _mockContractTypeRepository.Setup(p => p.GetById(It.IsNotNull<int>())).Returns(_dummyContext.Contract24_7);
            var contractVm = new EditViewModel()
            {
                Startdatum = DateTime.Today,
                ContractTypeId = 1,
                Doorlooptijd = 2
            };
            var result = Assert.IsType<RedirectToActionResult>(_contractController.Create(contractVm));
            Assert.Equal("Index", result.ActionName);
            _mockContractTypeRepository.Verify(m => m.GetById(It.IsNotNull<int>()), Times.Once);
            _mockContractRepository.Verify(m => m.Add(It.IsNotNull<Contract>()), Times.Once);
            _mockContractRepository.Verify(m => m.GetAllByClientId(It.IsNotNull<string>()), Times.Once);
            _mockContractRepository.Verify(m => m.SaveChanges(), Times.Once);
        }

        
        [Fact]
        public void CreateHttpPost_InvalidContract_DoesNotCreateNorPersistsContractAndRedirectsToActionIndex()
        {
            _mockContractRepository.Setup(p => p.Add(It.IsNotNull<Contract>()));
            _mockContractRepository.Setup(p => p.GetAllByClientId(It.IsNotNull<string>()))
                .Returns(_contracts);
            _mockContractTypeRepository.Setup(p => p.GetById(It.IsNotNull<int>())).Returns(_dummyContext.Contract24_7);

            var contractVm = new EditViewModel()
            {
                Startdatum = DateTime.Today,
                ContractTypeId = 1,
                Doorlooptijd = 20000 //fout
            };
            var result = Assert.IsType<RedirectToActionResult>(_contractController.Create(contractVm));
            Assert.Equal("Index", result.ActionName);
            _mockContractRepository.Verify(m => m.Add(It.IsNotNull<Contract>()), Times.Never);
            _mockContractRepository.Verify(m => m.SaveChanges(), Times.Never);
        }

        
        [Fact]
        public void CreateHttpPost_ModelStateErrors_DoesNotChangeNorPersistContract()
        {
            var contractVm = new EditViewModel(_contractActief);
            _contractController.ModelState.AddModelError("", "Any error");

            _contractController.Create(contractVm);

            Assert.Equal(_contractActief.ContractType.ContractTypeId, contractVm.ContractTypeId);
            Assert.Equal(_contractActief.StartDatum, contractVm.Startdatum);
            Assert.Equal(_contractActief.Doorlooptijd, contractVm.Doorlooptijd);
            _mockContractRepository.Verify(m => m.Add(It.IsNotNull<Contract>()), Times.Never);
            _mockContractRepository.Verify(m => m.SaveChanges(), Times.Never);
        }

        [Fact]
        public void CreateHttpPost_DomainErrors_AlreadyAnActiveContractsSameType_DoesNotPersistContract()
        {
            //Arrange
            //dit zorgt ervoor dat er geen contracten van type1 mogen gemaakt worden
            _contracts.Add(new Contract(DateTime.Now, _dummyContext.Contract24_7, 1, _userId, ContractEnContractTypeStatus.InBehandeling));

            _mockContractRepository.Setup(p => p.GetAllByClientId(It.IsNotNull<string>()))
                .Returns(_contracts);
            _mockContractTypeRepository.Setup(p => p.GetById(It.IsNotNull<int>()))
                .Returns(_dummyContext.Contract24_7);

            var contractVm = new EditViewModel()
            {
                Startdatum = DateTime.Today,
                ContractTypeId = 0, //0 omdat het contracttype uit de dummy nog geen id heeft.
                Doorlooptijd = 2
            };
            //Act
            var result = Assert.IsType<RedirectToActionResult>(_contractController.Create(contractVm));
            //Assert
            Assert.Equal("Index", result.ActionName);
            //Verify
            _mockContractRepository.Verify(m => m.GetAllByClientId(It.IsNotNull<string>()), Times.Once);
            _mockContractTypeRepository.Verify(m => m.GetById(It.IsNotNull<int>()), Times.Once);

            _mockContractRepository.Verify(m => m.Add(It.IsNotNull<Contract>()), Times.Never);
            _mockContractRepository.Verify(m => m.SaveChanges(), Times.Never);
        }

        #endregion
    }
}

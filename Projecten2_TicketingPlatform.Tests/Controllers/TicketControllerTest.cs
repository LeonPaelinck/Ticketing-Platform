using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Projecten2_TicketingPlatform.Controllers;
using Projecten2_TicketingPlatform.Models.Domein;
using Projecten2_TicketingPlatform.Models.TicketViewModels;
using Projecten2_TicketingPlatform.Tests.Data;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using Xunit;

namespace Projecten2_TicketingPlatform.Tests.Controllers
{
    public class TicketControllerTest
    {
        private readonly TicketController _ticketController;
        private readonly Mock<ITicketRepository> _mockTicketRepository;
        private readonly Ticket _ticket;
        private readonly DummyApplicationDbContext _dummyContext = new DummyApplicationDbContext();
        private readonly int _onbestaandeId;
        private readonly List<Contract> _contracts;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly Mock<IContractRepository> _mockContractRepository;
        private readonly string _userId; //"bff6a934 - 0dca - 4965 - b9fc - 91c3290792c8"

        public TicketControllerTest()
        {
            _userId = _dummyContext.UserId;

            _mockTicketRepository = new Mock<ITicketRepository>();
            _mockTicketRepository.Setup(p => p.GetById(1)).Returns(_dummyContext.Ticket);
            _ticket = _dummyContext.Ticket;
            _onbestaandeId = 9999;
            _contracts = new List<Contract>
            {
                _dummyContext.ContractActief,
                _dummyContext.ContractNietActief
            };
            var store = new Mock<IUserStore<IdentityUser>>();
            store.Setup(x => x.FindByIdAsync("123", CancellationToken.None))
                .ReturnsAsync(new IdentityUser()
                {
                    UserName = "test@email.com",
                    Id = _userId
                });

            _mockUserManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            _mockUserManager.Setup(mum => mum.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(_userId); //It.IsAny zal eignelijk altijd null zijn
            
            _mockContractRepository = new Mock<IContractRepository>();

            _ticketController = new TicketController(_mockTicketRepository.Object, _mockContractRepository.Object, _mockUserManager.Object)
            {
                TempData = new Mock<ITempDataDictionary>().Object
            };
        }
        //Nog eventuele wijziging in verband met een actief of niet actief contract. Moet besproken worden waar dit wordt getest
        #region == Create Methodes ==
        [Fact]
        public void CreateHttpGet_ActiefContract_PassesDetailsOfANewTicketInEditViewModelToView()
        {
            _mockContractRepository.Setup(r => r.GetAllByClientId(_userId))
                .Returns(_contracts); //1 actief en 1 niet actief

            var result = Assert.IsType<ViewResult>(_ticketController.Create());

            var ticketVm = Assert.IsType<EditViewModel>(result.Model);
            Assert.Null(ticketVm.Titel);

            _mockContractRepository.Verify(mock => mock.GetAllByClientId(_userId), Times.Once);

        }
        //public void CreateHttpGet_NietActiefContract_PassesNoDetailsOfANewTicketInEditViewModelToView()
        //{
        //
        //}
        [Fact]
        public void CreateHttpPost_ValidTicket_AddsNewTicketToRepositoryAndRedirectsToIndex()
        {
            _mockContractRepository.Setup(r => r.GetAllByClientId(_userId))
                .Returns(_contracts); //1 actief en 1 niet actief

            var ticketVm = new EditViewModel()
            {
                Titel = "Fout2098 Fase7",
                Omschrijving = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                TypeTicket = 1
            };
            var result = Assert.IsType<RedirectToActionResult>(_ticketController.Create(ticketVm));

            Assert.Equal("Index", result.ActionName);
            _mockContractRepository.Verify(mock => mock.GetAllByClientId(_userId), Times.Once);
            _mockTicketRepository.Verify(m => m.Add(It.IsNotNull<Ticket>()), Times.Once);
            _mockTicketRepository.Verify(m => m.SaveChanges(), Times.Once);
           

        }

        [Fact]
        public void CreateHttpPost_ValidTicketFromSupportManager_AddsNewTicketToRepositoryAndRedirectsToIndex()
        {
            _mockContractRepository.Setup(r => r.GetAllByClientId(_userId))
                 .Returns(_contracts); //1 actief en 1 niet actief

            var ticketVm = new EditViewModel()
            {
                KlantId = "bff6a934 - 0dca - 4965 - b9fc - 91c3290792c8",
                Titel = "Fout2098 Fase7",
                Omschrijving = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                TypeTicket = 1
            };
            var result = Assert.IsType<RedirectToActionResult>(_ticketController.Create(ticketVm));

            Assert.Equal("Index", result.ActionName);
            _mockTicketRepository.Verify(m => m.Add(It.IsNotNull<Ticket>()), Times.Once);
            _mockTicketRepository.Verify(m => m.SaveChanges(), Times.Once);
            _mockContractRepository.Verify(mock => mock.GetAllByClientId(_userId), Times.Once);

        }
        [Fact]
        public void CreateHttpPost_InvalidTicket_DoesNotCreateNorPersistsTicketAndRedirectsToActionIndex()
        {
            _mockTicketRepository.Setup(m => m.Add(It.IsAny<Ticket>()));
            var ticketVm = new EditViewModel()
            {
                Titel = null,
                Omschrijving = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                TypeTicket = 1
            };
            var action = Assert.IsType<RedirectToActionResult>(_ticketController.Create(ticketVm));
            Assert.Equal("Index", action?.ActionName);
            _mockTicketRepository.Verify(m => m.SaveChanges(), Times.Never());
            _mockTicketRepository.Verify(m => m.Add(It.IsAny<Ticket>()), Times.Never());
        }
        [Fact]
        public void CreateHttpPost_ModelStateErrors_DoesNotChangeNorPersistTicket()
        {
            var ticketVm = new EditViewModel(_ticket);
            _ticketController.ModelState.AddModelError("", "Any error");
            _ticketController.Create(ticketVm);
            Assert.Equal("Ticket20", ticketVm.Titel);
            Assert.Equal(1, ticketVm.TypeTicket);
            _mockTicketRepository.Verify(m => m.SaveChanges(), Times.Never);
        }
        #endregion

        #region == Edit Methodes ==
        [Fact]
        public void EditHttpGet_ValidTicketId_PassesTicketDetailsInAnEditViewModel()
        {
            _mockTicketRepository.Setup(p => p.GetById(1)).Returns(_dummyContext.Ticket);
            var result = Assert.IsType<ViewResult>(_ticketController.Edit(1));
            var ticketVm = Assert.IsType<EditViewModel>(result.Model);
            Assert.Equal("Ticket20", ticketVm?.Titel);
        }
        [Fact]
        public void EditHttpGet_TicketNotFound_ReturnsNotFound()
        {
            _mockTicketRepository.Setup(p => p.GetById(1)).Returns(null as Ticket);
            Assert.IsType<NotFoundResult>(_ticketController.Edit(1));
        }
        [Fact]
        public void EditHttpPost_ValidEdit_UpdatesAndPersistsTicketAndRedirectsToIndex()
        {
            _mockTicketRepository.Setup(p => p.GetById(1)).Returns(_dummyContext.Ticket);
            var ticketVm = new EditViewModel(_dummyContext.Ticket)
            {
                Titel = "TitelTicket20Gewijzigd",
                TypeTicket = 3
            };
            var result = Assert.IsType<RedirectToActionResult>(_ticketController.Edit(ticketVm, 1));
            Assert.Equal("TitelTicket20Gewijzigd", _ticket.Titel);
            Assert.Equal(3, _ticket.TypeTicket);
            Assert.Equal("Index", result.ActionName);
            _mockTicketRepository.Verify(m => m.SaveChanges(), Times.Once);
        }
        [Fact]
        public void EditHttpPost_InValidEdit_DoesNotChangeNorPersistTicketAndRedirectsToActionIndex()
        {
            _mockTicketRepository.Setup(m => m.GetById(1)).Returns(_dummyContext.Ticket);
            var ticketVm = new EditViewModel(_dummyContext.Ticket) { Titel = null };
            var result = Assert.IsType<RedirectToActionResult>(_ticketController.Edit(ticketVm , 1));
            var ticket = _dummyContext.Ticket;
            //Assert.Equal("Jan de technieker", ticket.Technieker);
            Assert.Equal("Ticket20", ticket.Titel); // Titel is toch null?
            Assert.Equal("Index", result?.ActionName);
            _mockTicketRepository.Verify(m => m.SaveChanges(), Times.Never());
        }
        [Fact]
        public void EditHttpPost_TicketNotFound_ReturnsNotFoundResult()
        {
            var ticketVm = new EditViewModel(_dummyContext.Ticket);
            var result = _ticketController.Edit(ticketVm, _onbestaandeId);
            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public void EditHttpPost_ModelStateErrors_DoesNotChangeNorPersistTicket()
        {
            var ticketVm = new EditViewModel(_dummyContext.Ticket);
            _ticketController.ModelState.AddModelError("", "Any error");
            var result = _ticketController.Edit(ticketVm, 1);
            Assert.Equal("Ticket20", ticketVm.Titel); //wat is hier het nut van?
            Assert.Equal(1, ticketVm.TypeTicket); //hetzelfde hier, het ticketVm wordt volledig door de testen bepaald
            _mockTicketRepository.Verify(m => m.SaveChanges(), Times.Never());
        }
        [Fact]
        public void EditHttpPost_ModelStateErrors_PassesEditViewModelInViewResultModel()
        {

            var ticketVm = new EditViewModel(_dummyContext.Ticket);
            _ticketController.ModelState.AddModelError("", "Any error");
            var result = Assert.IsType<ViewResult>(_ticketController.Edit(ticketVm, 1));
            ticketVm = Assert.IsType<EditViewModel>(result.Model);
            Assert.Equal("Ticket20", ticketVm.Titel);
        }
        #endregion

        #region == Annuleer Methodes ==
        [Fact]
        public void AnulleerHttpGet_ValidTicketId_ReturnsView()
        {
            _mockTicketRepository.Setup(p => p.GetById(1)).Returns(_dummyContext.Ticket);
            var result = Assert.IsType<ViewResult>(_ticketController.Edit(1));
        }

        [Fact]
        public void AnnuleerHttpPost_ValidTicketId_TicketGeannuleerdAndRedirectsToActionIndex()
        {
            _mockTicketRepository.Setup(p => p.GetById(1)).Returns(_dummyContext.Ticket);
            var result = Assert.IsType<RedirectToActionResult>(_ticketController.AnnuleerConfirmed(1));
            Assert.Equal(TicketStatus.Geannuleerd, _ticket.Status);
            _mockTicketRepository.Verify(m => m.SaveChanges(), Times.Once);
        } 
        #endregion

    }
}

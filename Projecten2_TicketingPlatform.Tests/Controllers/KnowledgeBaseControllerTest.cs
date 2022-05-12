using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Projecten2_TicketingPlatform.Controllers;
using Projecten2_TicketingPlatform.Models.Domein;
using Projecten2_TicketingPlatform.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Projecten2_TicketingPlatform.Tests.Controllers
{
    public class KnowledgeBaseControllerTest
    {
        private readonly KnowledgeBaseController _knowledgeBaseController;
        private readonly Mock<IKnowledgeBaseRepository> _mockKnowledgeBaseRepository;
        private readonly KnowledgeBase _knowledgeBase;
        private readonly KnowledgeBase _knowledgeBase1;
        private readonly List<KnowledgeBase> _knowledgeBases;
        private readonly DummyApplicationDbContext _dummyContext = new DummyApplicationDbContext();

        public KnowledgeBaseControllerTest()
        {
            _mockKnowledgeBaseRepository = new Mock<IKnowledgeBaseRepository>();

            _knowledgeBase = _dummyContext.KnowledgeBase;
            _knowledgeBase1 = _dummyContext.KnowledgeBase;
            _knowledgeBases = new List<KnowledgeBase>() { _knowledgeBase, _knowledgeBase1 };

            _knowledgeBaseController = new KnowledgeBaseController(_mockKnowledgeBaseRepository.Object)
            {
                TempData = new Mock<ITempDataDictionary>().Object
            };
        }

        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfKnowledgeBase()
        {
            //Arrange
            _mockKnowledgeBaseRepository.Setup(r => r.GetAll())
                .Returns(_knowledgeBases);
            //Act
            var result = _knowledgeBaseController.Index("");
            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<KnowledgeBase>>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
            //Verify
            _mockKnowledgeBaseRepository.Verify(r => r.GetAll(), Times.Once);
        }
    }
}

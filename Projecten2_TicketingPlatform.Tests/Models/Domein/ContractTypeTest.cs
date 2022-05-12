using Projecten2_TicketingPlatform.Models.Domein;
using Xunit;

namespace Projecten2_TicketingPlatform.Tests.Models.Domein
{
    public class ContractTypeTest
    {
        [Fact]
        public void Nieuw_ContractType_CorrectContractType_MaaktContractType()
        {
            //Arrange
            var contractType = new ContractType("Contract24/7", ContractEnContractTypeStatus.Actief, ManierVanAanmakenTicket.EmailEnTelefonischEnApplicatie, TijdstipTicketAanmaken.Altijd, 1, 10, 100);
            //Assert
            Assert.Equal("Contract24/7", contractType.Naam);
            Assert.Equal(ManierVanAanmakenTicket.EmailEnTelefonischEnApplicatie, contractType.ManierVanAanmakenTicket);
            Assert.Equal(1, contractType.MinimaleDoorlooptijd);
            Assert.Equal(100, contractType.ContractPrijs);
        }
    }
}

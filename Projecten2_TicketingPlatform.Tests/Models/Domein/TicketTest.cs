using Projecten2_TicketingPlatform.Models.Domein;
using System;
using Xunit;

namespace Projecten2_TicketingPlatform.Tests.Models.Domein
{
    public class TicketTest
    {
        [Fact]
        public void NieuwTicket_CorrectTicket_MaaktTicket()
        {
            var ticket = new Ticket("Ticket20", TicketStatus.Aangemaakt, DateTime.Today, "Ik heb een probleem", "1", "bff6a934 - 0dca - 4965 - b9fc - 91c3290792c8", "Jan de technieker", null, null );
            Assert.Equal("Ticket20", ticket.Titel);
            Assert.Equal("Ik heb een probleem", ticket.Omschrijving);
            Assert.Equal(1, ticket.TypeTicket);
            Assert.Equal(DateTime.Today, ticket.DatumAanmaken);
        }
        


    }
}

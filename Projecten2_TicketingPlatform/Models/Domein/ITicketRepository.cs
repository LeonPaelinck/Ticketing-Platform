using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Models.Domein
{
    public interface ITicketRepository
    {
        Ticket GetById(int ticketId);
        IEnumerable<Ticket> GetAll();
        IEnumerable<Ticket> GetAllByTicketStatus(IEnumerable<TicketStatus> lists);
        IEnumerable<Ticket> GetAllByClientId(string klantId);
        IEnumerable<Ticket> GetAllByClientIdByTicketStatus(string v, IEnumerable<TicketStatus> lists);

        void Add(Ticket ticket);
        void Delete(Ticket ticket);
        void SaveChanges();    }
}

using Microsoft.EntityFrameworkCore;
using Projecten2_TicketingPlatform.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Data.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Ticket> _tickets;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
            _tickets = context.Tickets;
        }

        public void Add(Ticket ticket)
        {
            _tickets.Add(ticket);
        }

        public void Delete(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Ticket> GetAll()
        {
            return _tickets.OrderBy(t => t.DatumAanmaken).AsNoTracking().ToList();
        }

        public IEnumerable<Ticket> GetAllByTicketStatus(IEnumerable<TicketStatus> ticketStatussen)
        {
            return _tickets.Where(t => ticketStatussen.Contains(t.Status)).OrderBy(t => t.DatumAanmaken).AsNoTracking().ToList();
        }

        public IEnumerable<Ticket> GetAllByClientId(string klantId)
        {
            return _tickets.Where(t => t.KlantId.Equals(klantId)).OrderBy(t => t.DatumAanmaken).AsNoTracking().ToList();
        }

        public IEnumerable<Ticket> GetAllByClientIdByTicketStatus(string klantId, IEnumerable<TicketStatus> ticketStatussen)
        {
            return _tickets.Where(t => t.KlantId.Equals(klantId)).Where(t => ticketStatussen.Contains(t.Status)).OrderBy(t => t.DatumAanmaken).AsNoTracking().ToList();
        }

        public Ticket GetById(int ticketId)
        {
            return _tickets.SingleOrDefault(p => p.Ticketid == ticketId);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}

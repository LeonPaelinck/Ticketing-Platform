using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Models.Domein
{
    public class Ticket
    {
        private string _titel;
        private DateTime _datumAanmaken;
        private string _omschrijving;
        private int _typeTicket;

        public string Titel
        {
            get => _titel;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ticket moet een naam hebben");
                _titel = value;
            }
        }
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Ticketid { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime DatumAanmaken {
            get => _datumAanmaken; 
            set
            {
                if (value == null)
                    throw new ArgumentException("Ticket moet een datum bevatten");
                _datumAanmaken = value;
            }
        }
        public string Omschrijving
        {
            get => _omschrijving;
            set
            {
                if (value == null)
                    throw new ArgumentException("Ticket moet een omschrijving bevatten");
                _omschrijving = value;
            }
        }
        public int TypeTicket
        {
            get => _typeTicket;
            set
            {
                if (value == null)
                    throw new ArgumentException("Ticket moet een type bevatten");
                _typeTicket = value;
            }
        }
        public string KlantId { get; set; }
        public string TechniekerId { get; set; }
        public string Opmerkingen { get; set; }
        public string Bijlage { get; set; }
        public int Kwaliteit { get; set; }
        public bool Oplossing { get; set; }
        public bool SupportNodig { get; set; }

        /*public Klant Klant { get; set; }*/
        public Ticket()
        {

        }
        public Ticket(string titel, TicketStatus ticketStatus, DateTime date, string omschrijving, string typeTicket, string klantId, string techniekerId = "Geen technieker", string opmerkingen = "Geen opmerkingen", string bijlagePad = null, int kwaliteit = 3, bool oplossing = false, bool supportNodig = false)
        {
            Titel = titel;
            //Ticketid = ticketId;
            Status = ticketStatus;
            DatumAanmaken = date;
            Omschrijving = omschrijving;
            TypeTicket = Int32.Parse(typeTicket);
            KlantId = klantId;
            TechniekerId = techniekerId;
            Opmerkingen = opmerkingen;
            Bijlage = bijlagePad;
            Kwaliteit = kwaliteit;
            Oplossing = oplossing;
            SupportNodig = supportNodig;

            //Nice to have
            //public int Waardering { get; set; }
            //public bool ViaKnowledgebase { get; set; }
            //public bool SupportNodig { get; set; }
        }
    }
}

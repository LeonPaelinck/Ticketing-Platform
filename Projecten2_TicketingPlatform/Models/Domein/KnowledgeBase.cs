using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Models.Domein
{
    public class KnowledgeBase
    {
        public int KnowledgeBaseId { get; set; }
        public string Titel { get; set; }
        public DateTime DatumToevoegen { get; set; } = DateTime.Today;
        public string Omschrijving { get; set; }
        public KnowledgeBase(string titel, string omschrijving, DateTime datumToevoegen)
        {
            Titel = titel;
            DatumToevoegen = datumToevoegen;
            Omschrijving = omschrijving;
        }
    }
}

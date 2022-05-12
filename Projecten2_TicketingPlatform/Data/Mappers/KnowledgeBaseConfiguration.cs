using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projecten2_TicketingPlatform.Models.Domein;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projecten2_TicketingPlatform.Data.Mappers
{
    public class KnowledgeBaseConfiguration : IEntityTypeConfiguration<KnowledgeBase>
    {
        public void Configure(EntityTypeBuilder<KnowledgeBase> builder)
        {
            builder.ToTable("KnowledgeBase");
            builder.HasKey(t => t.KnowledgeBaseId);

        }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projecten2_TicketingPlatform.Data.Mappers;
using Projecten2_TicketingPlatform.Models.Domein;

namespace Projecten2_TicketingPlatform.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Contract> Contracten { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<KnowledgeBase> KnowledgeBase { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new ContractTypeConfiguration());
            builder.ApplyConfiguration(new ContractConfiguration());
            builder.ApplyConfiguration(new ContractTypeConfiguration());
            builder.ApplyConfiguration(new KnowledgeBaseConfiguration());

        }
    }
}

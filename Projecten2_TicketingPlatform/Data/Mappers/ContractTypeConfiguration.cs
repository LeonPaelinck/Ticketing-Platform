using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projecten2_TicketingPlatform.Models.Domein;

namespace Projecten2_TicketingPlatform.Data.Mappers
{
    public class ContractTypeConfiguartion : IEntityTypeConfiguration<ContractType>
    {
        public void Configure(EntityTypeBuilder<ContractType> builder)
        {
            builder.ToTable("ContractType");

            builder.HasKey(c => c.ContractTypeId);

        }
    }
}

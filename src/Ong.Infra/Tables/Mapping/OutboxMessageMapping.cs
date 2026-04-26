using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ong.Infra.Tables.Mapping
{
    public class OutboxMessageMapping : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable("OutboxMessages");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();

            builder.Property(u => u.Type)
                   .IsRequired();

            builder.Property(u => u.Payload)
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(u => u.OccurredOn)
                   .IsRequired();

            builder.Property(u => u.ProcessedOn)
                   .IsRequired(false);

            builder.Property(u => u.Error)
                   .IsRequired(false);
        }
    }
}

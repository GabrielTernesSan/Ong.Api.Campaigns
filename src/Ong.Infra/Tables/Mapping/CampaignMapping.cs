using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ong.Infra.Tables.Mapping
{
    public class CampaignMapping : IEntityTypeConfiguration<Campaign>
    {
        public void Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.ToTable("Campaigns");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .IsRequired()
                   .ValueGeneratedNever();

            builder.Property(u => u.Title)
                .HasMaxLength(Domain.Campaign.MaxTitleLength)
                .IsRequired();

            builder.Property(u => u.Description)
                .HasMaxLength(Domain.Campaign.MaxDescriptionLength)
                .IsRequired();

            builder.Property(u => u.StartDate)
                   .IsRequired();

            builder.Property(u => u.EndDate)
                   .IsRequired();

            builder.Property(u => u.FinancialGoal)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(u => u.Status)
                   .HasConversion<int>()
                   .IsRequired();
        }
    }
}
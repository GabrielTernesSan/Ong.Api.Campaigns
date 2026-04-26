using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ong.Infra.Tables.Mapping
{
    public class DonationMapping : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.ToTable("Donations");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();

            builder.HasOne(u => u.Campaign)
                .WithMany()
                .HasForeignKey(u => u.CampaignId)
                .IsRequired();

            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            builder.Property(u => u.Amount)
                   .IsRequired();

            builder.Property(u => u.Date)
                   .IsRequired();
        }
    }
}

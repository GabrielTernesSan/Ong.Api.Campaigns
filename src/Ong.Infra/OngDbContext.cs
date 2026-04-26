using Microsoft.EntityFrameworkCore;
using Ong.Infra.Tables;
using System.Reflection;

namespace Ong.Infra
{
    public class OngDbContext : DbContext
    {
        public OngDbContext(DbContextOptions<OngDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<OutboxMessage> OutboxMessages{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

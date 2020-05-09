using Microsoft.EntityFrameworkCore;

namespace pushnotificationservice.Model
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<AppPushSubscription> AppPushSubscriptions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppPushSubscription>().ToTable("AppPushSubscription");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using RandomCappuccino.Server.Data.Models;

namespace RandomCappuccino.Server.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(c => new { c.UserId, c.Role });
            modelBuilder.Entity<TourPair>()
                .HasKey(c => new { c.TourId, c.Participant1Id, c.Participant2Id });
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<UserRole> UserRoles {  get; set; }

        public DbSet<Tour> Tours { get; set; }

        public DbSet<TourPair> TourPairs { get; set; }

        public DbSet<Participant> Participants {  get; set; }
    }


}

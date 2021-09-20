﻿using Microsoft.EntityFrameworkCore;
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
        }

        public DbSet<User> Users { get; set; }       

        public DbSet<UserRole> UserRoles {  get; set; }

        public DbSet<Tour> Tours { get; set; }

        public DbSet<TourPair> TourPairs { get; set; }

        public DbSet<Participant> Participants {  get; set; }
    }


}
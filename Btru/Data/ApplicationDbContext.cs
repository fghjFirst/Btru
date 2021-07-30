using System;
using System.Collections.Generic;
using System.Text;
using Btru.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Btru.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<ReadingAssignment> ReadingAssignments { get; set; }
        public DbSet<FavoriteBook> FavoriteBooks { get; set; }
        public DbSet<SleepSchedule> SleepSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

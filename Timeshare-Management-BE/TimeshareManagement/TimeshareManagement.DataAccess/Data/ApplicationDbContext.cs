using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TimeshareManagement.DataAccess.Migrations;
using TimeshareManagement.Models.Models;

namespace TimeshareManagement.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Timeshare> Timeshares { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<TimeshareStatus> TimesharesStatus { get; set; }
        /*public DbSet<Room> Rooms { get; set; }*/
        public DbSet<TimeshareDetail> TimesharesDetail { get; set; }
        public DbSet<RoomAmenities> RoomAmenities { get; set; }
        /*public DbSet<RoomDetail> RoomDetail { get; set; }*/
        public DbSet<BookingRequest> BookingRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.HasKey(x => x.UserId); // Make sure to specify the primary key
            });
            /*modelBuilder.Entity<Timeshare>().HasOne(u => u.User).WithMany(t => t.timeshares).HasForeignKey(u => u.Id);*/
        }
    }
}

using ClubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubAPI
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=sample_club_v2;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasAlternateKey( u => u.Email );
            modelBuilder.Entity<Club>().HasMany(c => c.Fields).WithOne(f => f.club).HasForeignKey(fk => fk.clubId).IsRequired();
            modelBuilder.Entity<Club>().HasMany(c => c.Teams).WithOne(f => f.club).HasForeignKey(fk => fk.clubId).IsRequired();
            modelBuilder.Entity<Club>().HasMany(e => e.Users).WithMany(c => c.Clubs).UsingEntity<UserRoles>(
                 j => j.Property(e => e.Role).HasDefaultValue("")
                );
            modelBuilder.Entity<Team>().HasMany(e => e.members).WithMany(c => c.Team).UsingEntity<TeamMember>(
                 j => j.Property(e => e.Role).HasDefaultValue("")
                );
            modelBuilder.Entity<Field>().HasMany(x => x.Parts).WithOne(f => f.field).HasForeignKey(fk => fk.FieldId);
            modelBuilder.Entity<Booking>().HasOne(e => e.part).WithMany(e => e.booking).HasForeignKey(e => e.FieldPartId).IsRequired();
            modelBuilder.Entity<Booking>().HasOne(e => e.user).WithMany(e => e.Bookings).HasForeignKey(e => e.UserId).IsRequired();
        }
        public DbSet<ClubAPI.Models.Team> Team { get; set; } = default!;
        public DbSet<ClubAPI.Models.Booking> Booking { get; set; } = default!;
    }
}

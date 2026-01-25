using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OfficeMaster.Models;

namespace OfficeMaster.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<ConferenceRoom> ConferenceRooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<User>(b =>
        {
            b.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            b.Property(u => u.LastName).HasMaxLength(100).IsRequired();
            b.Property(u => u.CompanyName).HasMaxLength(200);
            
            b.HasMany(u => u.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        builder.Entity<ConferenceRoom>(b =>
        {
            b.ToTable("ConferenceRooms");

            b.Property(r => r.Name).IsRequired().HasMaxLength(100);
            b.Property(r => r.Description).HasMaxLength(500);
            b.Property(r => r.ImageUrl).HasMaxLength(200);
            b.Property(r => r.RoomType)
                .HasConversion<string>()
                .HasMaxLength(20);
            
            b.Property(r => r.PricePerHour).HasColumnType("decimal(18,2)"); 
            
            b.HasMany(r => r.Reservations)
                .WithOne(r => r.ConferenceRoom)
                .HasForeignKey(r => r.ConferenceRoomId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        builder.Entity<Reservation>(b =>
        {
            b.ToTable("Reservations");

            b.Property(r => r.TotalPrice).HasColumnType("decimal(18,2)");
            b.Property(r => r.Status)
                .HasConversion<string>()
                .HasMaxLength(50);
        });
    }
}
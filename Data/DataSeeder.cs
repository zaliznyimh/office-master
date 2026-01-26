using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeMaster.Models;
using OfficeMaster.Models.Enum;

namespace OfficeMaster.Data;

public static class DataSeeder
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();

        await context.Database.MigrateAsync();

        string[] roleNames = { "Admin", "User" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<long>(roleName));
            }
        }

        if (!context.Users.Any())
        {
            var adminUser = new User
            {
                FirstName = "Cool",
                LastName = "Admin",
                UserName = "admin@officemaster.com",
                Email = "admin@officemaster.com",
                EmailConfirmed = true,
                CompanyName = "OfficeMaster HQ",
                DateOfBirth = DateTime.UtcNow.AddYears(-30)
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        if (!context.ConferenceRooms.Any())
        {
            var rooms = new List<ConferenceRoom>
            {
                new ConferenceRoom
                {
                    Name = "Focus One Pod",
                    Description = "Compact pod for individual focused work or private calls.",
                    Capacity = 2,
                    PricePerHour = 12m,
                    RoomType = RoomType.MeetingRoom,
                    HasProjector = false,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1587825140708-dfaf72ae4b04?auto=format&fit=crop&w=600&q=80"
                },
                new ConferenceRoom
                {
                    Name = "Quick Sync Room",
                    Description = "Small meeting room for quick team syncs.",
                    Capacity = 6,
                    PricePerHour = 20m,
                    RoomType = RoomType.MeetingRoom,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1524758631624-e2822e304c36?auto=format&fit=crop&w=600&q=80"
                },
                new ConferenceRoom
                {
                    Name = "Creative Collaboration Room",
                    Description = "Cozy room designed for brainstorming and collaboration.",
                    Capacity = 8,
                    PricePerHour = 25m,
                    RoomType = RoomType.MeetingRoom,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1556761175-5973dc0f32e7?auto=format&fit=crop&w=600&q=80"
                },

                new ConferenceRoom
                {
                    Name = "Main Conference Auditorium",
                    Description = "Large conference hall suitable for presentations and corporate events.",
                    Capacity = 50,
                    PricePerHour = 80m,
                    RoomType = RoomType.ConferenceHall,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1511578314322-379afb476865?auto=format&fit=crop&w=600&q=80"
                },
                new ConferenceRoom
                {
                    Name = "Grand Event Hall",
                    Description = "Spacious hall for large-scale conferences and seminars.",
                    Capacity = 120,
                    PricePerHour = 150m,
                    RoomType = RoomType.ConferenceHall,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1503428593586-e225b39bddfe?auto=format&fit=crop&w=600&q=80"
                },

                new ConferenceRoom
                {
                    Name = "Professional Training Room",
                    Description = "Classroom equipped for workshops and training sessions.",
                    Capacity = 25,
                    PricePerHour = 40m,
                    RoomType = RoomType.Classroom,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1503428593586-e225b39bddfe?auto=format&fit=crop&w=600&q=80"
                },
                new ConferenceRoom
                {
                    Name = "Modern Learning Lab",
                    Description = "Modern classroom for lectures and group learning.",
                    Capacity = 30,
                    PricePerHour = 45m,
                    RoomType = RoomType.Classroom,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1580582932707-520aed937b7b?auto=format&fit=crop&w=600&q=80"
                },

                new ConferenceRoom
                {
                    Name = "International Convention Center",
                    Description = "Premium conference hall for international events and keynote sessions.",
                    Capacity = 180,
                    PricePerHour = 220m,
                    RoomType = RoomType.ConferenceHall,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1497366216548-37526070297c?auto=format&fit=crop&w=600&q=80"
                },
                new ConferenceRoom
                {
                    Name = "Executive Event Hall",
                    Description = "Elegant hall for business conferences and large presentations.",
                    Capacity = 140,
                    PricePerHour = 180m,
                    RoomType = RoomType.ConferenceHall,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1505373877841-8d25f7d46678?auto=format&fit=crop&w=600&q=80"
                },
                new ConferenceRoom
                {
                    Name = "Product Launch Hall",
                    Description = "Versatile hall designed for product launches and tech talks.",
                    Capacity = 90,
                    PricePerHour = 130m,
                    RoomType = RoomType.ConferenceHall,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1551836022-deb4988cc6c0?auto=format&fit=crop&w=600&q=80"
                },

                new ConferenceRoom
                {
                    Name = "Corporate Training Space",
                    Description = "Bright classroom for corporate training and workshops.",
                    Capacity = 20,
                    PricePerHour = 35m,
                    RoomType = RoomType.Classroom,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1588072432836-e10032774350?auto=format&fit=crop&w=600&q=80"
                },
                new ConferenceRoom
                {
                    Name = "Hands-on Workshop Room",
                    Description = "Flexible classroom ideal for hands-on sessions and group work.",
                    Capacity = 28,
                    PricePerHour = 42m,
                    RoomType = RoomType.Classroom,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1524178232363-1fb2b075b655?auto=format&fit=crop&w=600&q=80"
                },
                new ConferenceRoom
                {
                    Name = "Extended Learning Room",
                    Description = "Comfortable learning environment for long educational sessions.",
                    Capacity = 35,
                    PricePerHour = 50m,
                    RoomType = RoomType.Classroom,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1503676260728-1c00da094a0b?auto=format&fit=crop&w=600&q=80"
                },
                new ConferenceRoom
                {
                    Name = "Lecture Classroom",
                    Description = "Spacious classroom designed for lectures and presentations.",
                    Capacity = 40,
                    PricePerHour = 55m,
                    RoomType = RoomType.Classroom,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1596495578065-6e0763fa1178?auto=format&fit=crop&w=600&q=80"
                },
                new ConferenceRoom
                {
                    Name = "Internal Seminar Room",
                    Description = "Medium-sized classroom for seminars and internal trainings.",
                    Capacity = 18,
                    PricePerHour = 30m,
                    RoomType = RoomType.Classroom,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1587825140708-dfaf72ae4b04?auto=format&fit=crop&w=600&q=80"
                },

                new ConferenceRoom
                {
                    Name = "Discussion & Panel Forum",
                    Description = "Hybrid space suitable for panels, lectures, and open discussions.",
                    Capacity = 60,
                    PricePerHour = 95m,
                    RoomType = RoomType.ConferenceHall,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1521737604893-d14cc237f11d?auto=format&fit=crop&w=600&q=80"
                },
                new ConferenceRoom
                {
                    Name = "Lecture & Talk Hall",
                    Description = "Modern lecture hall optimized for talks and educational events.",
                    Capacity = 75,
                    PricePerHour = 110m,
                    RoomType = RoomType.ConferenceHall,
                    HasProjector = true,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1503424886302-fdbd0b6e86b7?auto=format&fit=crop&w=600&q=80"
                }
            };
            await context.ConferenceRooms.AddRangeAsync(rooms);
            await context.SaveChangesAsync();
        }
    }
}
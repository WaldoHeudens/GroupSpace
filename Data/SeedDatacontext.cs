using GroupSpace.Areas.Identity.Data;
using GroupSpace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GroupSpace.Data
{
    public class SeedDatacontext
    {
        public static void Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService
                                                              <DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();    // Zorg dat de databank bestaat

                ApplicationUser user = null;

                if (!context.Users.Any())
                {
                    ApplicationUser dummy = new ApplicationUser { Id = "-", FirstName = "-", LastName = "-", UserName = "-", Email = "?@?.?" };
                    context.Users.Add(dummy);
                    context.SaveChanges();
                    user = new ApplicationUser
                    {
                        FirstName = "System",
                        LastName = "Administrator",
                        UserName = "Admin",
                        Email = "System.Administrator@GroupSpace.be",
                        EmailConfirmed = true
                    };
                    userManager.CreateAsync(user, "Abc!12345");
                }
                
                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new IdentityRole { Id = "User", Name = "User", NormalizedName = "user" },
                        new IdentityRole { Id = "SystemAdministrator", Name = "SystemAdmninistrator", NormalizedName = "systemadministrator"});
                    context.SaveChanges();
                }

                if (!context.Group.Any())      // Voeg enkele groepen toe
                {
                    context.Group.AddRange
                            (
                                     new Group { Name = "?", Description = "?", Started = DateTime.MinValue, Ended = DateTime.Now },
                                     new Group { Name = "Testgroep", Description = "Uitsluitend om te testen", Started = DateTime.Now, Ended = DateTime.MaxValue }
                            );
                    context.SaveChanges();
                }
 
                if (!context.Message.Any())   // Voeg enkele messages toe
                {
                    context.Message.AddRange(
                             new Message { Title = "-", Content = "-", Created = DateTime.Now, GroupID = 1, SenderId = "-" },
                             new Message { Title = "- (Naar gezin)", Content = "Een eerste boodschap", Created = DateTime.Now, GroupID = 2, SenderId = "-" });
                    context.SaveChanges();
                }

                if (!context.MediaType.Any())
                {
                    context.MediaType.AddRange(
                        new MediaType { Name = "-", Denominator = "-", Deleted = DateTime.Now },
                        new MediaType { Name = "Alles", Denominator = "All File (*.*)|*.*|Alle Bestanden", Deleted = DateTime.MaxValue },
                        new MediaType { Name = "Videos", Denominator = "MP4 (*.mpg)|*.mpg|Videos mp4", Deleted  = DateTime.MaxValue});
                    context.SaveChanges();
                }

                if (!context.Category.Any())
                {
                    context.Category.AddRange(
                        new Category { Name = "-", Description = "-", Deleted = DateTime.Now},
                        new Category { Name = "Family Pictures", Description = "All pictures concerning the whole family", Deleted = DateTime.MaxValue },
                        new Category { Name = "Holidays", Description = "All holiday media", Deleted = DateTime.MaxValue });
                    context.SaveChanges();
                }

                if (!context.Media.Any())
                {
                    context.Media.AddRange(
                        new Media { Name = "-", Description = "-", TypeId = 1, Added = DateTime.Now });
                    context.SaveChanges();
                }

                if (user != null)
                {
                    context.UserRoles.AddRange(
                        new IdentityUserRole<string> { UserId = user.Id, RoleId = "SystemAdministrator"});
                }
            }
        }
    }
}

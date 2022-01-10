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

                if (!context.Language.Any())
                {
                    context.Language.AddRange(
                        new Language() { Id = "-", Name = "-", Cultures = "-", IsSystemLanguage = false },
                        new Language() { Id = "de", Name = "Deutsch", Cultures = "DE", IsSystemLanguage = false },
                        new Language() { Id = "en", Name = "English", Cultures = "UK;US", IsSystemLanguage = true },
                        new Language() { Id = "es", Name = "Español", Cultures = "ES", IsSystemLanguage = false },
                        new Language() { Id = "fr", Name = "français", Cultures = "BE;FR", IsSystemLanguage = true },
                        new Language() { Id = "nl", Name = "Nederlands", Cultures = "BE;NL", IsSystemLanguage = true }
                    );
                    context.SaveChanges();
                }

                ApplicationUser user = null;

                if (!context.Users.Any())
                {
                    ApplicationUser dummy = new ApplicationUser { Id = "-", FirstName = "-", LastName = "-", UserName = "-", Email = "?@?.?", LanguageId = "-" };
                    context.Users.Add(dummy);
                    context.SaveChanges();
                    user = new ApplicationUser
                    {
                        FirstName = "System",
                        LastName = "Administrator",
                        UserName = "Admin",
                        Email = "System.Administrator@GroupSpace.be",
                        LanguageId = "nl",
                        EmailConfirmed = true
                    };
                    userManager.CreateAsync(user, "Abc!12345");
                }

                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new IdentityRole { Id = "User", Name = "User", NormalizedName = "user" },
                        new IdentityRole { Id = "SystemAdministrator", Name = "SystemAdmninistrator", NormalizedName = "systemadministrator" },
                        new IdentityRole { Id = "UserAdministrator", Name = "UserAdministrator", NormalizedName = "useradministrator" });
                    context.SaveChanges();
                }
                if (!context.Group.Any())      // Voeg enkele groepen toe
                {
                    context.Group.AddRange
                            (
                                     new Group { Name = "?", Description = "?", Started = DateTime.MinValue, Ended = DateTime.Now, StartedById = "-", EndedById = "-" },
                                     new Group { Name = "Testgroep", Description = "Uitsluitend om te testen", Started = DateTime.Now, Ended = DateTime.MaxValue, StartedById = "-", EndedById = "-" }
                            );
                    context.SaveChanges();
                }

                if (!context.UserGroup.Any())
                {
                    context.UserGroup.AddRange(
                        new UserGroup { UserId = "-", GroupId = 1 });
                    context.SaveChanges();
                }


                if (!context.Message.Any())   // Voeg enkele messages toe
                {
                    Message dummyMessage = new Message { Title = "-", Content = "-", Created = DateTime.Now, SenderId = "-" };
                    MessageDestination dummymd = new MessageDestination { Deleted = DateTime.MinValue, Message = dummyMessage, Read = DateTime.Now, Received = DateTime.Now, ReceiverId = "-"};
                    context.Message.Add(dummyMessage);
                    context.MessageDestinations.Add(dummymd);
                    context.SaveChanges();
                }

                if (!context.MediaType.Any())
                {
                    context.MediaType.AddRange(
                        new MediaType { Name = "-", Denominator = "-", Deleted = DateTime.Now },
                        new MediaType { Name = "Alles", Denominator = "All File (*.*)|*.*|Alle Bestanden", Deleted = DateTime.MaxValue },
                        new MediaType { Name = "Videos", Denominator = "MP4 (*.mpg)|*.mpg|Videos mp4", Deleted = DateTime.MaxValue });
                    context.SaveChanges();
                }

                if (!context.Category.Any())
                {
                    context.Category.AddRange(
                        new Category { Name = "-", Description = "-", Deleted = DateTime.Now },
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
                        new IdentityUserRole<string> { UserId = user.Id, RoleId = "UserAdministrator" },
                        new IdentityUserRole<string> { UserId = user.Id, RoleId = "SystemAdministrator" },
                        new IdentityUserRole<string> { UserId = user.Id, RoleId = "User" });
                    context.SaveChanges();
                }


                // Start initialisatie talen op basis van databank

                List<string> supportedLanguages = new List<string>();
                Language.AllLanguages = context.Language.ToList();
                Language.LanguageDictionary = new Dictionary<string, Language>();
                Language.SystemLanguages = new List<Language>();

                supportedLanguages.Add("nl-BE");
                foreach (Language l in Language.AllLanguages)
                {
                    Language.LanguageDictionary[l.Id] = l;
                    if (l.Id != "-")
                    {
                        if (l.IsSystemLanguage)
                            Language.SystemLanguages.Add(l);
                        supportedLanguages.Add(l.Id);
                        string[] even = l.Cultures.Split(";");
                        foreach (string e in even)
                        {
                            supportedLanguages.Add(l.Id + "-" + e);
                        }
                    }
                }
                Language.SupportedLanguages = supportedLanguages.ToArray();

            }
        }
    }
}

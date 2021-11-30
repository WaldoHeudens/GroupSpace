using GroupSpace.Models;
using Microsoft.EntityFrameworkCore;

namespace GroupSpace.Data
{
    public class SeedDatacontext
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new GroupSpaceContext(serviceProvider.GetRequiredService
                                                              <DbContextOptions<GroupSpaceContext>>()))
            {
                context.Database.EnsureCreated();    // Zorg dat de databank bestaat

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
                             new Message { Title = "-", Content = "-", Created = DateTime.Now, GroupID = 1 },
                             new Message { Title = "- (Naar gezin)", Content = "Een eerste boodschap", Created = DateTime.Now, GroupID = 2 });
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
            }
        }
    }
}

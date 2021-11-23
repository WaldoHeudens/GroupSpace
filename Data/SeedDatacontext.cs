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
            }
        }
    }
}

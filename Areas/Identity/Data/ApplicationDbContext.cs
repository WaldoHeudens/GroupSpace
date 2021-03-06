using GroupSpace.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GroupSpace.Models;

namespace GroupSpace.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<GroupSpace.Models.Group> Group { get; set; }

    public DbSet<GroupSpace.Models.Message> Message { get; set; }

    public DbSet<GroupSpace.Models.MediaType> MediaType { get; set; }

    public DbSet<GroupSpace.Models.Category> Category { get; set; }

    public DbSet<GroupSpace.Models.Media> Media { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<GroupSpace.Models.Language> Language { get; set; }

    public DbSet<GroupSpace.Models.UserGroup> UserGroup { get; set; }

    public DbSet<GroupSpace.Models.Token> Token { get; set; }

    public DbSet<GroupSpace.Models.MessageDestination> MessageDestinations { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GroupSpace.Models;

namespace GroupSpace.Data
{
    public class GroupSpaceContext : DbContext
    {
        public GroupSpaceContext (DbContextOptions<GroupSpaceContext> options)
            : base(options)
        {
        }

        public DbSet<GroupSpace.Models.Group> Group { get; set; }

        public DbSet<GroupSpace.Models.Message> Message { get; set; }

        public DbSet<GroupSpace.Models.MediaType> MediaType { get; set; }

        public DbSet<GroupSpace.Models.Category> Category { get; set; }

        public DbSet<GroupSpace.Models.Media> Media { get; set; }
    }
}

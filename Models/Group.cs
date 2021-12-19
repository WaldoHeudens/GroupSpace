using GroupSpace.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupSpace.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Display(Name = "Omschrijving")]
        public string Description { get; set; }


        [Display(Name = "Begonnen")]
        [DataType(DataType.Date)]
        public DateTime Started { get; set; }

        [Display(Name = "Beëindigd")]
        [DataType(DataType.Date)]
        public DateTime Ended { get; set; }
        [Display(Name = "Gebruikers")]
        public string StartedById { get; set; } = "-";
        public string EndedById { get; set; } = "-";
        public List<UserGroup>? UserGroups { get; set; }
    }

    public class UserGroup
    {
        public int Id { get; set; }

        [ForeignKey("Group")]
        public int GroupId { get; set; } = 1;
        public Group? Group { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } = "-";
        public ApplicationUser? User { get; set; }
        public DateTime Added { get; set; } = DateTime.Now;
        public DateTime Left { get; set; } = DateTime.MaxValue;
        public DateTime BecameHost { get; set; } = DateTime.MinValue;
        public DateTime NoHostAnymore { get; set; } = DateTime.MaxValue;

    }

    public class GroupViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Started")]
        public DateTime Started { get; set; }
        [Display(Name = "StartedBy")]
        public string StartedBy { get; set; }
        [Display(Name = "Hosts")]
        public List<string> Hosts { get; set; }
        [Display(Name = "Members")]
        public List<string> Members { get; set; }
        [Display(Name = "isHost")]
        public bool isHost { get; set; }
    }

    public class InviteViewModel
    {
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        public int GroupId { get; set; }
    }

    public class MemberViewModel
    {
        public string UserId { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Added")]
        public DateTime Added { get; set; }
        [Display(Name = "Host ?")]
        public Boolean isHost { get; set; }

    }
}

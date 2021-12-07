using GroupSpace.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GroupSpace.Models
{
    public class Message
    {
        public int ID { get; set; }

        [Display(Name = "Titel")]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Boodschap")]
        [Required]
        public string Content { get; set; }

        [Display(Name = "Gemaakt op")]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [ForeignKey("Group")]
        [Display(Name = "Groep")]
        public int GroupID { get; set; }

        [ForeignKey("ApplicationUser")]
        [Display(Name = "Verzender")]
        public string SenderId { get; set; } = "-";
        public Group? Group { get; set; }
        public ApplicationUser? Sender { get; set; } 
    }

    public class MessageIndexViewModel
    {
        public int SelectedGroup { get; set; }
        public string TitleFilter { get; set; }
        public List<Message> FilteredMessages { get; set; }
        public SelectList GroupsToSelect { get; set; }
        public SelectList ModesToSelect { get; set; }
        public string SelectedMode { get; set; }
    }
}

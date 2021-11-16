using System.ComponentModel.DataAnnotations;

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
        [DataType (DataType.Date)]
        public DateTime Started { get; set; }

        [Display(Name = "Beëindigd")]
        [DataType(DataType.Date)]
        public DateTime Ended { get; set; }
    }
}

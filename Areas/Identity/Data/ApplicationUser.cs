using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using GroupSpace.Models;
using Microsoft.AspNetCore.Identity;

namespace GroupSpace.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }   
    public string LastName { get; set; }

    [ForeignKey ("Language")]
    public string LanguageId { get; set; }
    public Language? Language { get; set; }
}


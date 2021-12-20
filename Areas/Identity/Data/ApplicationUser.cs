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

    [ForeignKey ("Group")]
    public int? ActualGroupId { get; set; }  // Nullable, to avoid cascading key conflicts with GroupUser
    public Group? ActualGroup { get; set; }
    public List<UserGroup>? Groups { get; set; }
}

public class ApplicationUserViewModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Language { get; set; }
    public string? PhoneNumber { get; set; }
    public Boolean Lockout { get; set; }
    public Boolean User { get; set; }
    public Boolean SystemAdministrator { get; set; }
    public Boolean UserAdministrator { get; set; }
}


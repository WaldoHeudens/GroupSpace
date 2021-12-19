#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupSpace.Data;
using GroupSpace.Models;
using Microsoft.AspNetCore.Authorization;
using GroupSpace.Areas.Identity.Data;
using GroupSpace.Services;

namespace GroupSpace.Controllers
{
    public class TokensController : ApplicationController
    {

        public TokensController(ApplicationDbContext context,
                                IHttpContextAccessor httpContextAccessor,
                                ILogger<ApplicationController> logger):base(context, httpContextAccessor, logger)
        {
        }
        [Authorize]
        public async Task<IActionResult> GroupInvitation(string email, Guid code)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == _user.Id);
            Token token = _context.Token.FirstOrDefault(t => t.Id == code);
            Group group = _context.Group.FirstOrDefault(g => g.Id == token.ConnectedId);
            UserGroup userGroup = new UserGroup
            {
                Added = DateTime.Now,
                BecameHost = DateTime.MaxValue,
                Group = group,
                GroupId = group.Id,
                Left = DateTime.MaxValue,
                NoHostAnymore = DateTime.MaxValue,
                User = user,
                UserId = user.Id
            };
            user.ActualGroup = group;
            user.ActualGroupId = group.Id;
            _context.Update(User);
            _context.Add(userGroup);
            await _context.SaveChangesAsync();
            SessionUser.Delete(_user.UserName);  // make sure that the new group is added to _user
            return RedirectToAction("Index", "Home");
        }
    }
}

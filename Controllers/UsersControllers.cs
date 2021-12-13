using GroupSpace.Areas.Identity.Data;
using GroupSpace.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroupSpace.Controllers
{
    public class UsersControllers : ApplicationController
    {

        public UsersControllers(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<ApplicationController> logger)
            : base(context, httpContextAccessor, logger)
        {
        }

        public IActionResult Index()
        {
            var users = _context.Users;
            return View(users.Include(u=>u).ToListAsync());
        }
    }
}

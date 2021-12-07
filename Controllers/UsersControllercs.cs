using GroupSpace.Areas.Identity.Data;
using GroupSpace.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroupSpace.Controllers
{
    public class UsersControllercs : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IRoleStore<IdentityRole> _roleStore;

        public UsersControllercs(ApplicationDbContext context)//_RoleStore = serviceProvider.GetRequiredService<RoleStore<IdentityRole>>());
        {

        }

        public IActionResult Index()
        {
            var users = _context.Users;
            return View(users.Include(u=>u).ToListAsync());
        }
    }
}

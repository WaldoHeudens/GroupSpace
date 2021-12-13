using GroupSpace.Areas.Identity.Data;
using GroupSpace.Data;
using GroupSpace.Services;
using Microsoft.AspNetCore.Mvc;

namespace GroupSpace.Controllers
{
    public class ApplicationController : Controller
    {
        protected readonly ApplicationUser _user;
        protected readonly ApplicationDbContext _context;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILogger<ApplicationController> _logger;

        protected ApplicationController(ApplicationDbContext context, 
                                        IHttpContextAccessor httpContextAccessor, 
                                        ILogger<ApplicationController> logger)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            //string? userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            //if (userName == null)
            //    userName = "-";
            //_user = _context.Users.FirstOrDefault(u => u.UserName == userName);
            _user = SessionUser.GetUser(httpContextAccessor.HttpContext);
        }
    }
}

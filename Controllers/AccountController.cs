using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using INF4001N_1814748_NVSAAY001_2024.Data;
using INF4001N_1814748_NVSAAY001_2024.ViewModels;
using INF4001N_1814748_NVSAAY001_2024.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace INF4001N_1814748_NVSAAY001_2024.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public AccountController(ApplicationDbContext context, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction("CastVote", "Vote");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check for duplicate IDNumber
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.IDNumber == model.IDNumber);
                if (existingUser != null)
                {
                    ModelState.AddModelError("IDNumber", "This ID Number is already registered.");
                    return View(model);
                }

                // Create a new user
                var newUser = new User
                {
                    FullName = model.FullName,
                    IDNumber = model.IDNumber,
                    Email = model.Email,
                    Province = model.Province,
                    PasswordHash = _passwordHasher.HashPassword(new ApplicationUser(), model.Password), // Hash the password
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }
    }
}

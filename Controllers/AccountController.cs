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
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(ApplicationDbContext context, UserManager<User> userManager, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _userManager = userManager;
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
                var existingUserByIdNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.IDNumber == model.IDNumber);
                if (existingUserByIdNumber != null)
                {
                    ModelState.AddModelError("IDNumber", "This ID Number is already registered.");
                    return View(model);
                }

                // Check for duplicate Email
                var existingUserByEmail = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUserByEmail != null)
                {
                    ModelState.AddModelError("Email", "This email address is already registered.");
                    return View(model);
                }

                // Create a new user
                var newUser = new User
                {
                    FullName = model.FullName,
                    IDNumber = model.IDNumber,
                    Email = model.Email,
                    UserName = model.Email,
                    Province = model.Province,
                    CreatedAt = DateTime.Now
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    // Assign the default role
                    await _userManager.AddToRoleAsync(newUser, "Voter");

                    return RedirectToAction("Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}

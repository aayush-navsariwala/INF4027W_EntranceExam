using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using INF4001N_1814748_NVSAAY001_2024.Data;
using INF4001N_1814748_NVSAAY001_2024.ViewModels;
using INF4001N_1814748_NVSAAY001_2024.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using INF4001N_1814748_NVSAAY001_2024.Services;

namespace INF4001N_1814748_NVSAAY001_2024.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly SignInManager<User> _signInManager;

        public AccountController(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                TempData["Message"] = "Login successful.";
                return RedirectToAction("CastVote", "Vote");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

            [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationViewModel model, [FromServices] IEmailValidationService emailValidationService)
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

                // Validate the email using Abstract API
                var isEmailValid = await emailValidationService.IsEmailValidAsync(model.Email);
                if (!isEmailValid)
                {
                    ModelState.AddModelError("Email", "The provided email address is invalid.");
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
                    await _userManager.AddToRoleAsync(newUser, "Voter");
                    TempData["Message"] = "Registration successful. You can now log in.";
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

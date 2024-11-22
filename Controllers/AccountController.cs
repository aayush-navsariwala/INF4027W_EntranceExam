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

        //Constructor to initialise dependencies for database, user management and authentication
        public AccountController(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordHasher = passwordHasher;
        }

        //Displays the default account index view
        public IActionResult Index()
        {
            return View();
        }

        // Displays the login page
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            //Store the return URL for redirection after successful login
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        //Handles user login logic
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            //Check if the provided data is valid
            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                //Return the login view with validation errors
                return View(model);
            }

            //Attempt to sign in the user with the provided credentials
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            //If the login is successful
            if (result.Succeeded)
            {
                TempData["Message"] = "Login successful.";
                //Redirect to the CastVote action
                return RedirectToAction("CastVote", "Vote");
            }

            //Error message if login fails
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            ViewData["ReturnUrl"] = returnUrl;
            //Return the login view with the error message
            return View(model);
        }

        //Redirects to a local URL or the default home page
        private IActionResult RedirectToLocal(string returnUrl)
        {
            //If the return URL is local
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            //Otherwise, redirect to the home page
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //Displays the registration page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //Handles user registration logic
        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationViewModel model, [FromServices] IEmailValidationService emailValidationService)
        {
            //Check if the provided data is valid
            if (ModelState.IsValid)
            {
                //Check if the ID number is already registered
                var existingUserByIdNumber = await _userManager.Users.FirstOrDefaultAsync(u => u.IDNumber == model.IDNumber);
                if (existingUserByIdNumber != null)
                {
                    //Return the registration view with the error message
                    ModelState.AddModelError("IDNumber", "This ID Number is already registered.");
                    return View(model);
                }

                //Validate the email using the Abstract API service
                var isEmailValid = await emailValidationService.IsEmailValidAsync(model.Email);
                //If the email is invalid
                if (!isEmailValid)
                {
                    //Return the registration view with the error message
                    ModelState.AddModelError("Email", "The provided email address is invalid.");
                    return View(model);
                }

                //Check if the email address is already registered
                var existingUserByEmail = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUserByEmail != null)
                {
                    //Return the registration view with the error message
                    ModelState.AddModelError("Email", "This email address is already registered.");
                    return View(model);
                }

                //Create a new user
                var newUser = new User
                {
                    FullName = model.FullName,
                    IDNumber = model.IDNumber,
                    Email = model.Email,
                    UserName = model.Email,
                    Province = model.Province,
                    CreatedAt = DateTime.Now
                };

                //Attempt to create the new user in the database
                var result = await _userManager.CreateAsync(newUser, model.Password);

                //If user creation is successful
                if (result.Succeeded)
                {
                    //Assign the Voter role to the new user
                    await _userManager.AddToRoleAsync(newUser, "Voter");
                    //Display a success message
                    TempData["Message"] = "Registration successful. You can now log in.";
                    //Redirect to the login page
                    return RedirectToAction("Login");
                }

                //Add errors to the model state if user creation fails
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            //Return the registration view with validation errors (if any)
            return View(model);
        }
    }
}
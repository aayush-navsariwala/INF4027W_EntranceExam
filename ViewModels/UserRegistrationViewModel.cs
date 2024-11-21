using System.ComponentModel.DataAnnotations;

namespace INF4001N_1814748_NVSAAY001_2024.ViewModels
{
    public class UserRegistrationViewModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(256, ErrorMessage = "Full name is too long.")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "ID Number is required.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "ID Number must be 13 digits.")]
        public string IDNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Province is required.")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

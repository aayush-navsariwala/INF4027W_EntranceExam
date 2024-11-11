using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace INF4001N_1814748_NVSAAY001_2024.Models
{
    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public override string Email { get; set; }

        [Required]
        [Display(Name = "Province")]
        public string Province { get; set; } 

        public bool HasVoted { get; set; } = false; 
    }
}

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace INF4001N_1814748_NVSAAY001_2024.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        [Required]
        public string IDNumber { get; set; } 
        public string Province { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
    }
}

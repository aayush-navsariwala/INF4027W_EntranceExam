using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace INF4001N_1814748_NVSAAY001_2024.Models
{
    public class User : IdentityUser
    {
        public Guid UserId { get; set; } 
        public string FullName { get; set; }
        public string IDNumber { get; set; } 
        public string Email { get; set; } 
        public string PasswordHash { get; set; }
        public string Province { get; set; }
        public string Role { get; set; } = "Voter"; 
        public DateTime CreatedAt { get; set; }
    }
}

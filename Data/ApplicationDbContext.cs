using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using INF4001N_1814748_NVSAAY001_2024.Models;
using Microsoft.EntityFrameworkCore;

namespace INF4001N_1814748_NVSAAY001_2024.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } 
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Election> Elections { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}

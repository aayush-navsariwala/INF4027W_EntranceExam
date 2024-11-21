using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using INF4001N_1814748_NVSAAY001_2024.Models;
using Microsoft.EntityFrameworkCore;

namespace INF4001N_1814748_NVSAAY001_2024.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Vote> Votes { get; set; }

        // Add other DbSets as needed
    }
}

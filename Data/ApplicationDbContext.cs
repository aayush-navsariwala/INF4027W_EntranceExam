using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using INF4001N_1814748_NVSAAY001_2024.Models;

namespace INF4001N_1814748_NVSAAY001_2024.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Election> Elections { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationship between Election and Candidate
            modelBuilder.Entity<Election>()
                .HasMany(e => e.Candidates)
                .WithOne(c => c.Election)
                .HasForeignKey(c => c.ElectionId);
        }
    }
}

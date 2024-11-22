using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using INF4001N_1814748_NVSAAY001_2024.Models;

namespace INF4001N_1814748_NVSAAY001_2024.Data
{
    //Defines the database context for the application, inheriting from IdentityDbContext to support user authentication and identity features
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        //Constructor to initialize the ApplicationDbContext with the specified options
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //Defines a DbSet for elections to allow CRUD operations on Election entities
        public DbSet<Election> Elections { get; set; }

        //Defines a DbSet for votes to allow CRUD operations on Vote entities
        public DbSet<Vote> Votes { get; set; }

        //Defines a DbSet for candidates to allow CRUD operations on Candidate entities
        public DbSet<Candidate> Candidates { get; set; }

        //Configures the relationships between entities in the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Call the base method to ensure default Identity configurations are applied
            base.OnModelCreating(modelBuilder);

            //Configure relationship between Election and Candidate
            modelBuilder.Entity<Election>()
                .HasMany(e => e.Candidates)
                .WithOne(c => c.Election)
                .HasForeignKey(c => c.ElectionId);
        }
    }
}
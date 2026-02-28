using Microsoft.EntityFrameworkCore;
using ControlTicket.Domain.Employees;

namespace ControlTicket.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

              modelBuilder.Entity<Employee>()
                .ToTable("workers")
                .HasKey(w => w.Rut); 

            
        }
    }
}
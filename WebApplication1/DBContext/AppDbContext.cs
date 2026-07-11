using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.DBContext
{
    public class AppDbContext:DbContext
    {
        public DbSet<Person> Persons { get; set; } // Replace YourEntity with your actual entity class 

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public void onModelcreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configuration if needed

            modelBuilder.Entity<Person>().HasKey(p => p.pId); // Specify the primary key for the Person entity
        }
    }
}

using LearnManager.Models;
using Microsoft.EntityFrameworkCore;
namespace LearnManager.Services
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public void SeedAdmin()
        {
            if (!Users.Any(u => u.Role == "admin"))
            {
                var admin = new User
                {
                    Nom = "Admin",
                    Prenom = "Admin",
                    Email = "admin@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("12345678"), 
                    Phone = "21365478",
                    Role = "admin",
                    CreatedAt = DateTime.Now
                };

                Users.Add(admin);
                SaveChanges();
            }
        }
    
}
}

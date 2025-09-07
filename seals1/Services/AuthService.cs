using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesSuite.Models;
using SalesSuite.Utils;

namespace SalesSuite.Services
{
    public class AuthService
    {
        // ✅ Register new user
        public async Task<bool> RegisterAsync(string username, string password, string role = "User")
        {
            using var db = new AppDb();
            if (await db.Users.AnyAsync(u => u.Username == username))
                throw new Exception("Username already exists.");

            var hash = BCrypt.Net.BCrypt.HashPassword(password); // Encrypt password
            db.Users.Add(new User
            {
                Username = username,
                PasswordHash = hash,
                Role = role,
                CreatedAt = DateTime.Now
            });
            await db.SaveChangesAsync();
            return true;
        }

        // ✅ Login
        public async Task<User?> LoginAsync(string username, string password)
        {
            using var db = new AppDb();
            var user = await db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            // Verify password
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash) ? user : null;
        }

        // ✅ Create default Admin if not exists
        public async Task CreateDefaultAdminAsync()
        {
            using var db = new AppDb();

            if (!await db.Users.AnyAsync(u => u.Role == "Admin"))
            {
                var hash = BCrypt.Net.BCrypt.HashPassword("admin123");

                db.Users.Add(new User
                {
                    Username = "admin",
                    PasswordHash = hash,
                    Role = "Admin",
                    CreatedAt = DateTime.Now
                });

                await db.SaveChangesAsync();
                Console.WriteLine("✅ Default Admin created: username=admin, password=admin123");
            }
        }
    }
}

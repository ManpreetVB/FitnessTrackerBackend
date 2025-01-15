using Fitness_Tracker.Data;
using Fitness_Tracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Tracker.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        // Method to register a new user
        public async Task<bool> RegisterUser(string username, string email, string password)
        {
            // Check if the user already exists
            if (await _context.Users.AnyAsync(u => u.Username == username || u.Email == email))
            {
                return false; // Username or email already exists
            }

            // Hash the password
            var passwordHash = HashPassword(password);

            // Create a new User object
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = passwordHash
            };

            // Save the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // Method to validate a user's login credentials
        public async Task<User> ValidateUser(string username, string password)
        {
            // Find the user by username
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            // If user is not found or password does not match, return null
            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        // Helper method to hash a password
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        // Helper method to verify a password
        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }
    }
}


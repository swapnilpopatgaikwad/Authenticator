using Authenticator.Data;
using Authenticator.DTO;
using Authenticator.Interfaces;
using Authenticator.Model;
using Microsoft.EntityFrameworkCore;

namespace Authenticator.Repository
{
    public class AuthRepository: IAuthRepository
    {
        private readonly AuthenticatorContext _context;
        public AuthRepository(AuthenticatorContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<User> FindByIdAsync(int id)
        {
            var user = await _context.User.FindAsync(id);
            return user;
        }

        public async Task<int> UpdateUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddUser(User user)
        {
            _context.User.Add(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteUser(User user)
        {
            _context.User.Remove(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Username == username); //Get user from database.
            if (user == null)
                return null; // User does not exist.

            if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // Create hash using password salt.
                for (int i = 0; i < computedHash.Length; i++)
                { // Loop through the byte array
                    if (computedHash[i] != passwordHash[i]) return false; // if mismatch
                }
            }
            return true; //if no mismatches.
        }

        public async Task<User> Register(UserDto userDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var user = userDto.ToUser();

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.User.AddAsync(user); // Adding the user to context of users.
            await _context.SaveChangesAsync(); // Save changes to database.

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.User.AnyAsync(x => x.Username == username))
                return true;
            return false;
        }

        public async Task<bool> UserExistsById(int id)
        {
            if (await _context.User.AnyAsync(e => e.Id == id))
                return true;
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnifiedContract.Domain.Entities.Auth;
using UnifiedContract.Domain.Enums;
using UnifiedContract.Domain.Interfaces.Repositories;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace UnifiedContract.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(UnifiedContractDbContext dbContext) : base(dbContext) { }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UnifiedContract.Domain.Enums.UserRole role)
        {
            return await _dbContext.Set<User>()
                .Where(u => u.UserRoles.Any(ur => ur.Role.Name == role.ToString()))
                .ToListAsync();
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _dbContext.Set<User>().AnyAsync(u => u.Email == email);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
} 
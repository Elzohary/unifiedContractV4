using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.Auth;
using UnifiedContract.Domain.Enums;

namespace UnifiedContract.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetUsersByRoleAsync(UnifiedContract.Domain.Enums.UserRole role);
        Task<bool> UserExistsAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
} 
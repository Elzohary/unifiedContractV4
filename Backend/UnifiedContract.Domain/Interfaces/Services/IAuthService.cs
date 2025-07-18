using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.Auth;

namespace UnifiedContract.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(bool success, string token, User user)> LoginAsync(string username, string password);
        Task<bool> ValidateTokenAsync(string token);
        Task<User> GetUserFromTokenAsync(string token);
        Task<bool> ChangePasswordAsync(Guid userId, string oldPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> SetNewPasswordAsync(string resetToken, string newPassword);
        Task<bool> IsInRoleAsync(Guid userId, string role);
        Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);
    }
} 
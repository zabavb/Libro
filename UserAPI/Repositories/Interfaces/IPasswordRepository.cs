﻿using UserAPI.Models;

namespace UserAPI.Repositories
{
    public interface IPasswordRepository
    {
        Task<bool> VerifyAsync(Guid userId, string plainPassword);
        Task<bool> AddAsync(Guid id, string password, User user);
        Task<bool> UpdateAsync(Guid userId, string oldPassword, string newPassword);
        Task<bool> DeleteAsync(Guid passwordId);
        Task<Password> GetByIdAsync(Guid passwordId);
        Task<string> GetHashByIdAsync(Guid passwordId);
    }
}

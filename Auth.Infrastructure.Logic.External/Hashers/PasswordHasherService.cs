using Auth.Domain.Core.Logic.Models.DTOs.User;
using Auth.Domain.Interface.Logic.External.Auth;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure.Logic.External.Hashers
{
    internal class PasswordHasherService(IPasswordHasher<UserDTO> passwordHasher) : IPasswordHasherService
    {
        private readonly IPasswordHasher<UserDTO> _passwordHasher = passwordHasher;
        public string HashPassword(Guid id, string password)
        {
            return _passwordHasher.HashPassword(default, Converter(id, password));
        }
        public bool VerifyHashedPassword(Guid id, string hashedPassword, string password)
        {
            return _passwordHasher.VerifyHashedPassword(default, hashedPassword, Converter(id, password)) == PasswordVerificationResult.Success;
        }
        private string Converter(Guid id, string password)
        {
            return string.Format("{0}{1}", id, password);
        }
    }
}

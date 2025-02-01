namespace Auth.Domain.Interface.Logic.External.Auth
{
    public interface IPasswordHasherService
    {
        string HashPassword(Guid id, string password);
        bool VerifyHashedPassword(Guid id, string hashedPassword, string password);
    }
}

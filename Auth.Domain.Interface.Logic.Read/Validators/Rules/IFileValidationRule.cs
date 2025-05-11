namespace Auth.Domain.Interface.Logic.Read.Validators.Rules
{
    public interface IFileValidationRule : IValidationRuleBase
    {
        bool IsNotEmpty(Stream file);
        bool IsImage(Stream file, string contentType);
        bool IsVideo(Stream file, string contentType);
    }
}

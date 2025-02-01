using Auth.Domain.Core.Logic.Commands.Admin;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AdminValidators
{
    internal class RemoveUselessTokensValidator(IUnitOfWorkValidationRule rule) 
        : Validator<RemoveUselessTokensCommand>(rule)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(RemoveUselessTokensCommand command)
        {
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}

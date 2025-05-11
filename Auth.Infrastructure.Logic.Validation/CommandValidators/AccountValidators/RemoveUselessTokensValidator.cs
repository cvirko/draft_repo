using Auth.Domain.Core.Logic.Commands.Account;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators.AccountValidators
{
    internal class RemoveUselessTokensValidator(IValidationRuleService validate) 
        : Validator<RemoveUselessTokensCommand>(validate)
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync(RemoveUselessTokensCommand command)
        {
            return Task.FromResult<IEnumerable<ValidationError>>(GetErrors());
        }
    }
}

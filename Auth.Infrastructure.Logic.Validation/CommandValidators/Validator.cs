using Library.CommandMediator.Services;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators
{

    internal abstract class Validator<TCommand>(IValidationRuleService validate) 
        : BaseValidator<TCommand, IValidationRuleService, ValidationError, ErrorStatus>(validate)
        where TCommand : Command;
    internal abstract class Validator<TCommand, TResponse>(IValidationRuleService validate) 
        : BaseValidator<TCommand, TResponse, IValidationRuleService, ValidationError, ErrorStatus>(validate)
        where TCommand : Command<TResponse>;
}

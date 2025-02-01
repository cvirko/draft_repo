using Auth.Domain.Core.Logic.Commands;
using System.Linq.Expressions;

namespace Auth.Infrastructure.Logic.Validation.CommandValidators
{
    internal abstract class Validator<TCommand>(IUnitOfWorkValidationRule uowRule) : IValidator<TCommand> where TCommand : Command
    {
        private readonly IUnitOfWorkValidationRule _uowRule = uowRule;
        protected IUnitOfWorkValidationRule RuleFor()
        {
            _uowRule.SetFieldName(AppConsts.GLOBALERROR);
            return _uowRule;
        }
        protected IUnitOfWorkValidationRule RuleFor<TProperty>(Expression<Func<TCommand, TProperty>> expression)
        {
            _uowRule.SetFieldName(expression.GetPropertyInfo().Name);
            return _uowRule;
        }
        protected ValidationError[] GetErrors() => _uowRule.GetErrors();

        public abstract Task<IEnumerable<ValidationError>> ValidateAsync(TCommand command);

        protected void AddError(ErrorStatus status, params object[] values)
        {
            _uowRule.AddError(status, values);
        }
        public bool IsValid() => _uowRule.IsValid();
        public bool IsInvalid() => !_uowRule.IsValid();
    }
}

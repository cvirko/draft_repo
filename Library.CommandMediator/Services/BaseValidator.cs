using Library.CommandMediator.Consts;
using Library.CommandMediator.Extinsions;
using Library.CommandMediator.Interfaces;
using Library.CommandMediator.Models;
using System.Linq.Expressions;

namespace Library.CommandMediator.Services
{
    public abstract class BaseValidator<TValidationRule, TError, EStatus>(TValidationRule validate)
        where TValidationRule : IBaseValidationRuleService<TError, EStatus>
        where TError : BaseValidationError<EStatus>
        where EStatus : Enum
    {
        private readonly TValidationRule _validate = validate;
        protected TValidationRule RuleFor()
        {
            _validate.SetFieldName(ExceptionConst.GLOBALERROR);
            return _validate;
        }
        protected void AddError(EStatus status, params object[] values)
            => _validate.AddError(status, values);
        protected TError[] GetErrors() => _validate.GetErrors();
        public abstract Task<IEnumerable<TError>> ValidateAsync(object command);
        public void Throw(Type type, string message = null) => _validate.Throw(type, message);
        public bool IsValid => _validate.IsValid;
        public bool IsInvalid => !_validate?.IsValid ?? false;
    }
    public abstract class BaseValidator<TCommand, TValidationRule, TError, EStatus>(TValidationRule validate)
        : BaseValidator<TValidationRule, TError, EStatus>(validate), IValidator<TCommand, TError, EStatus> 
        where TCommand : CommandBase
        where TValidationRule : IBaseValidationRuleService<TError, EStatus>
        where TError : BaseValidationError<EStatus>
        where EStatus : Enum
    {
        private readonly TValidationRule _validate = validate;

        public override async Task<IEnumerable<TError>> ValidateAsync(object command)
            => await ValidateAsync((TCommand)command);
        public abstract Task<IEnumerable<TError>> ValidateAsync(TCommand command);

        protected TValidationRule RuleFor<TProperty>(Expression<Func<TCommand, TProperty>> expression)
        {
            _validate.SetFieldName(expression.GetPropertyInfo().Name);
            return _validate;
        }
    }
    public abstract class BaseValidator<TCommand, TResponse, TValidationRule, TError, EStatus>(TValidationRule validate)
        : BaseValidator<TValidationRule, TError, EStatus>(validate), IValidator<TCommand, TResponse, TError, EStatus> 
        where TCommand : CommandBase<TResponse>
        where TValidationRule : IBaseValidationRuleService<TError, EStatus>
        where TError : BaseValidationError<EStatus>
        where EStatus : Enum
    {
        private readonly TValidationRule _validate = validate;

        public override async Task<IEnumerable<TError>> ValidateAsync(object command)
            => await ValidateAsync((TCommand)command);
        public abstract Task<IEnumerable<TError>> ValidateAsync(TCommand command);
        protected TValidationRule RuleFor<TProperty>(Expression<Func<TCommand, TProperty>> expression)
        {
            _validate.SetFieldName(expression.GetPropertyInfo().Name);
            return _validate;
        }
    }
}

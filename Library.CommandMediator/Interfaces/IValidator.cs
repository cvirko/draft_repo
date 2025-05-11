using Library.CommandMediator.Models;

namespace Library.CommandMediator.Interfaces
{
    public interface IBaseValidator<TError,EStatus> 
        where TError : BaseValidationError<EStatus>
        where EStatus : Enum
    {
        public bool IsValid { get; }
        public bool IsInvalid { get; }
        public void Throw(Type type, string message = null);
        public Task<IEnumerable<TError>> ValidateAsync(object command);
    }
    public interface IValidator<in TCommand, TError, EStatus> : IBaseValidator<TError, EStatus>
        where TCommand : CommandBase
        where TError : BaseValidationError<EStatus> where EStatus : Enum
    {
        public Task<IEnumerable<TError>> ValidateAsync(TCommand command);
    }
    public interface IValidator<in TCommand, TResponse, TError, EStatus> : IBaseValidator<TError, EStatus>
        where TCommand : CommandBase<TResponse>
        where TError : BaseValidationError<EStatus> where EStatus : Enum
    {
        public Task<IEnumerable<TError>> ValidateAsync(TCommand command);
    }
}

using Library.CommandMediator.Consts;
using Library.CommandMediator.Models;

namespace Library.CommandMediator.Interfaces
{
    public interface IBaseValidationRuleService<TError, EStatus>
        where TError : BaseValidationError<EStatus>
        where EStatus : Enum
    {
        void SetFieldName(string field);

        void AddError(EStatus status, params object[] values);
        bool IsValid {  get; }
        TError[] GetErrors();
        void Throw(Type type, string message = ExceptionConst.MESSAGE);
        void Throw(string at, string message = ExceptionConst.MESSAGE);
    }
}

using Library.CommandMediator.Consts;
using Library.CommandMediator.Models;

namespace Library.CommandMediator.Interfaces
{
    public interface IValidationMessageService<TError, EStatus>
        where TError : BaseValidationError<EStatus>
        where EStatus : Enum
    {
        TError Get(EStatus status,
            string field = ExceptionConst.GLOBALERROR, params object[] values);
    }
}

using Auth.Domain.Core.Common.Consts;
using Auth.Domain.Core.Common.Exceptions;

namespace Auth.Domain.Interface.Logic.Read.ModelBuilder.ServiceBuilder
{
    public interface IValidationMessageService : IBuilder
    {
        ValidationError Get(ErrorStatus status,
            string field = AppConsts.GLOBALERROR, params object[] values);
    }
}

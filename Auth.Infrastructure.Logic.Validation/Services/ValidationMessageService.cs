using Auth.Domain.Core.Common.Tools.Configurations;
using Microsoft.Extensions.Options;
using System.Collections.Immutable;

namespace Auth.Infrastructure.Logic.Validation.Services
{
    internal class ValidationMessageService : IValidationMessageService
    {
        private static ImmutableSortedDictionary<ErrorStatus, string> ErrorsText;
        private readonly FilesOptions _options;
        private readonly IFileBuilder _fileBuilder;
        public ValidationMessageService(IOptions<FilesOptions> option, IFileBuilder file)
        {
            _options = option.Value;
            _fileBuilder = file;
            GetErrorsText();
        }
        public ValidationError Get(ErrorStatus status,
            string field = AppConsts.GLOBALERROR, params object[] values)
        {
            if (ErrorsText == null || !ErrorsText.TryGetValue(status, out string message))
                return new(field, default, status);

            if (values.Length == 0)
                return new(field, message, status);

            return new(field, Replace(message, values),
                        status, Join(values));
        }
        private void GetErrorsText()
        {
            if (ErrorsText != null) return;
            ErrorsText = _fileBuilder.GetFromJson<ErrorText[]>(_options.ErrorsTextPath).ToImmutableSortedDictionary(p => p.StatusName, p => p.Text);
        }
        private string Join(params object[] value)
        {
            return string.Join(AppConsts.SEPARATOR, value);
        }
        private string Replace(string message, params object[] replaces)
        {
            return string.Format(message, replaces);
        }
    }
}

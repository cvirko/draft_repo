namespace Auth.Infrastructure.Logic.Validation.ValidationRules
{
    internal class FileValidationRule(Action<ErrorStatus, object[]> action) 
        : ValidationRuleBase(action), IFileValidationRule
    {
        public bool IsNotEmpty(Stream file)
        {
            if (file != null && file.Length != 0)
                return true;
            AddError(ErrorStatus.NotFound);
            return false;
        }
        public bool IsImage(Stream file, string contentType)
        {
            if (!IsNotEmpty(file))
                return false;
            switch (contentType)
            {
                case MIMEType.Jpep:
                case MIMEType.Png:
                case MIMEType.Svg:
                    return true;
                default:
                    AddError(ErrorStatus.AccessDenied); return false;
            }
        }
        public bool IsVideo(Stream file, string contentType)
        {
            if (!IsNotEmpty(file))
                return false;
            switch (contentType)
            {
                case MIMEType.Mpeg:
                case MIMEType.Mp4:
                case MIMEType.Ogg:
                case MIMEType.Quicktime:
                    return true;
                default:
                    AddError(ErrorStatus.AccessDenied); return false;
            }
        }
    }
}

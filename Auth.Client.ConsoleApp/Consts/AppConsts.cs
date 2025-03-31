using System.Diagnostics;

namespace Auth.Client.ConsoleApp.Consts
{
    public class AppConsts
    {
        public const string AVATARS_PATH = "Avatars";
        public const int ERRORS_MAX_LENGTH = 5; 
        public const string ALLOWED_SPECIFIC_ORIGIN = "AllowedOrigins";
        public const string SEPARATOR = "_";
        public const string GLOBALERROR = "general";
        public const string HUBNAME = "hub";

        public const string TOKEN_SETTING_SECTION_NAME = "Token";
        public const string AUTH_SETTING_SECTION_NAME = "Authentication";
        public const string CONNECTION_SECTION_NAME = "ConnectionStrings";
        public const string FILE_SECTION_NAME = "File";
        public const string CACHE_SECTION_NAME = "Cache";
        public const string MAIL_SECTION_NAME = "Mail";
        public const string USER_FAILED_ACCESS_SECTION_NAME = "UserFailedAccess";
        public const string TIME_ZONE_SECTION_NAME = "TimeZoneOffsetInHours";
        public const string SENDGRID_SECTION_NAME = "SendGrid";
        public const string RABBITMQ_SECTION_NAME = "RabbitMQ";

        public static readonly string APP_NAME = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName).Split(".")[0];
        public static readonly string SWAGGER_DESCRIPTION = $"This is the Swagger documentation for the {APP_NAME} Management API.";
        public static readonly string SWAGGER_TITLE = $"{APP_NAME} API";
    }
}

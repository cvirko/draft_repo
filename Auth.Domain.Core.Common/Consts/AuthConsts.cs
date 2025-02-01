namespace Auth.Domain.Core.Common.Consts
{
    public class AuthConsts
    {
        public const string IS_SUPERADMIN = "IsSuperAdmin";
        public const string IS_ADMIN = "IsAdmin";
        public const string IS_USER = "IsUser";
        public const string IS_REFRESH_TOKEN = "IsRefreshToken";
        public const string IS_CONFIRMATION = "IsConfirmation";
        public const string IS_CONFIRMATION_MAIL = "IsConfirmationMail";
        public const string IS_CONFIRMATION_PASSWORD = "IsConfirmationPassword";
        public const string IS_RESET = "IsReset";

        public const string AUTHENTICATION_SCHEME = "Bearer";
        public const string BEARER_FORMAT = "JWT";
        public const string CLAIM_TYPE_NAME_USER_ID = "UserId";
        public const string CLAIM_TYPE_NAME_ACCESS = "ACCESS";
        public const string CLAIM_TYPE_NAME_LOGINID = "ID";
        public const string CLAIM_TYPE_NAME_LOGIN_DATE = "LoginDate";
        public const string CLAIM_TYPE_NAME_LOGIN_TOKENID = "LoginTokenID";
    }
}

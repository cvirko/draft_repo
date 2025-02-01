using Auth.Domain.Core.Common.Extensions;
using Auth.Domain.Core.Logic.Models.DTOs.User;
using Auth.Domain.Interface.Logic.External.Auth;
using Auth.Domain.Interface.Logic.External.Files;
using Auth.Domain.Interface.Logic.External.Mail;
using Auth.Domain.Interface.Logic.External.Randomiz;
using Auth.Domain.Interface.Logic.External.Socila;
using Auth.Infrastructure.Logic.External.Hashers;
using Auth.Infrastructure.Logic.External.Files;
using Auth.Infrastructure.Logic.External.Mails;
using Auth.Infrastructure.Logic.External.Randomiz;
using Auth.Infrastructure.Logic.External.Social;
using Auth.Infrastructure.Logic.External.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TokenOptions = Auth.Domain.Core.Common.Tools.Configurations.TokenOptions;

namespace Auth.Infrastructure.Logic.External
{
    public static class IoCExternalServices
    {
        public static void RegistrationExternalService(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<UserDTO>, PasswordHasher<UserDTO>>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddSingleton<IMailService, SMTPMailService>();
            services.AddScoped<IRandomService, RandomService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IFileService, FileService>();
            services.AddHttpClient();

            services.AddScoped<IAuthGoogle, AuthGoogle>();
            services.AddScoped<IAuthFacebook, AuthFacebook>();
        }
        public static void AddAuthentication(this IServiceCollection services, TokenOptions tokenO, AuthOptions auth)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = AuthConsts.AUTHENTICATION_SCHEME;
                options.DefaultChallengeScheme = AuthConsts.AUTHENTICATION_SCHEME;
            })
               .AddJwtBearer(AuthConsts.AUTHENTICATION_SCHEME, options =>
               {
                   options.RequireHttpsMetadata = true;
                   options.SaveToken = true;
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ClockSkew = TimeSpan.FromMinutes(5),
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       RequireExpirationTime = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,

                       ValidIssuer = tokenO.ValidIssuer,
                       ValidAudience = tokenO.ValidAudience,
                       IssuerSigningKey = new SymmetricSecurityKey(tokenO.TokenSecret.ToByteArray()),
                   };
               })
               ;
            services.AddAuthorization();
            services.AddScoped<ITokenService, TokenJwtService>();
        }
        private static void AddAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(auth =>
            {
                auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(AuthConsts.AUTHENTICATION_SCHEME)
                .RequireAuthenticatedUser()
                .Build();
                auth.AddPolicy(AuthConsts.AUTHENTICATION_SCHEME, new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(AuthConsts.AUTHENTICATION_SCHEME)
                .RequireAuthenticatedUser()
                .Build());
                auth.AddPolicy(AuthConsts.IS_SUPERADMIN, new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(AuthConsts.AUTHENTICATION_SCHEME)
                .RequireAuthenticatedUser()
                .RequireRole(RoleType.SuperAdmin.ToString())
                .Build());
                auth.AddPolicy(AuthConsts.IS_ADMIN, new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(AuthConsts.AUTHENTICATION_SCHEME)
                .RequireAuthenticatedUser()
                .RequireRole(RoleType.SuperAdmin.ToString(), RoleType.Admin.ToString())
                .Build());
                auth.AddPolicy(AuthConsts.IS_USER, new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(AuthConsts.AUTHENTICATION_SCHEME)
                .RequireAuthenticatedUser()
                .RequireRole(RoleType.SuperAdmin.ToString(), RoleType.Admin.ToString(), RoleType.User.ToString())
                .Build());
                auth.AddPolicy(AuthConsts.IS_REFRESH_TOKEN, new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(AuthConsts.AUTHENTICATION_SCHEME)
                .RequireAuthenticatedUser()
                .RequireRole(TokenType.Refresh.ToString())
                .Build());
                auth.AddPolicy(AuthConsts.IS_CONFIRMATION_MAIL, new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(AuthConsts.AUTHENTICATION_SCHEME)
                .RequireAuthenticatedUser()
                .RequireRole(TokenType.ConfirmMail.ToString())
                .Build());
                auth.AddPolicy(AuthConsts.IS_CONFIRMATION_PASSWORD, new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(AuthConsts.AUTHENTICATION_SCHEME)
                .RequireAuthenticatedUser()
                .RequireRole(TokenType.ConfirmPassword.ToString())
                .Build());
                auth.AddPolicy(AuthConsts.IS_RESET, new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(AuthConsts.AUTHENTICATION_SCHEME)
                .RequireAuthenticatedUser()
                .RequireRole(TokenType.Reset.ToString())
                .Build());

            });
        }
    }
}

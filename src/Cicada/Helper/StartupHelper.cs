using Cicada.Configuration;
using Cicada.Configuration.EmailSenderConfigs;
using Cicada.Data.Extensions;
using Cicada.EFCore.Mysql.Extensions;
using Cicada.EFCore.PostgreSQL.Extensions;
using Cicada.EFCore.Shared.Configuration;
using Cicada.EFCore.Shared.DBContexts;
using Cicada.EFCore.Shared.Enums;
using Cicada.EFCore.Sqlite.Extensions;
using Cicada.EFCore.SqlServer.Extensions;
using Cicada.Services;
using FluentEmail.Core.Interfaces;
using FluentEmail.Mailgun;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using System;

namespace Cicada.Helper
{
    public static class StartupHelper
    {
        /// <summary>
        /// Add email senders - configuration of sendgrid, smtp senders
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddEmailSenders(this IServiceCollection services, IConfiguration configuration)
        {
            var smtpConfiguration = configuration.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>();
            var sendGridConfiguration = configuration.GetSection(nameof(SendgridConfiguration)).Get<SendgridConfiguration>();
            var mailgunConfiguration = configuration.GetSection(nameof(MailgunConfiguration)).Get<MailgunConfiguration>();

            if (sendGridConfiguration != null && !string.IsNullOrWhiteSpace(sendGridConfiguration.ApiKey))
            {
                services.AddSingleton<ISendGridClient>(_ => new SendGridClient(sendGridConfiguration.ApiKey));
                services.AddSingleton(sendGridConfiguration);
                services.AddTransient<IEmailSender, SendgridEmailSender>();
            }
            else if (mailgunConfiguration != null && !string.IsNullOrWhiteSpace(mailgunConfiguration.ApiKey))
            {
                services.AddSingleton<ISender>(_ => new MailgunSender(mailgunConfiguration.Domain, mailgunConfiguration.ApiKey));
                services.AddSingleton(mailgunConfiguration);
                services.AddTransient<IEmailSender, MailgunEmailSender>();
            }
            else if (smtpConfiguration != null && !string.IsNullOrWhiteSpace(smtpConfiguration.Host))
            {
                services.AddSingleton(smtpConfiguration);
                services.AddTransient<IEmailSender, SmtpEmailSender>();
            }
            else
            {
                services.AddSingleton<IEmailSender, EmailSender>();
            }
        }

        public static void RegisterDbContexts<TIdentityDbContext, TCicadaDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TIdentityDbContext : DbContext
            where TCicadaDbContext : CicadaDbContext
        {
            var databaseProvider = configuration.GetSection(nameof(DbProviderTypeConfiguration)).Get<DbProviderTypeConfiguration>();

            var identityConnectionString = configuration.GetConnectionString(ConfigurationConsts.CicadaIdentityDbConnectionStringKey);
            var cicadaConnectionString = configuration.GetConnectionString(ConfigurationConsts.CicadaDbConnectionStringKey);

            switch (databaseProvider.ProviderType)
            {
                case DbTypeEnum.SqlServer:
                    services.RegisterSqlServerDbContexts<TIdentityDbContext, TCicadaDbContext>(cicadaConnectionString, identityConnectionString);
                    break;
                case DbTypeEnum.PostgreSQL:
                    services.RegisterNpgSqlDbContexts<TIdentityDbContext, TCicadaDbContext>(cicadaConnectionString, identityConnectionString);
                    break;
                case DbTypeEnum.MySql:
                    services.RegisterMySqlDbContexts<TIdentityDbContext, TCicadaDbContext>(cicadaConnectionString, identityConnectionString);
                    break;
                case DbTypeEnum.Sqlite:
                    services.RegisterSqliteDbContexts<TIdentityDbContext, TCicadaDbContext>(cicadaConnectionString, identityConnectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(databaseProvider.ProviderType), $@"The value needs to be one of {string.Join(", ", Enum.GetNames(typeof(DbTypeEnum)))}.");
            }
        }

        /// <summary>
        /// Add services for authentication, including Identity model, IdentityServer4 and external providers
        /// </summary>
        /// <typeparam name="TIdentityDbContext">DbContext for Identity</typeparam>
        /// <typeparam name="TUserIdentity">User Identity class</typeparam>
        /// <typeparam name="TUserIdentityRole">User Identity Role class</typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddAuthenticationServices<TIdentityDbContext, TUserIdentity, TUserIdentityRole>(this IServiceCollection services, IConfiguration configuration) where TIdentityDbContext : DbContext
            where TUserIdentity : class
            where TUserIdentityRole : class
        {
            var registrationConfiguration = GetRegistrationConfiguration(configuration);

            services
                .AddSingleton(registrationConfiguration)
                .AddIdentity<TUserIdentity, TUserIdentityRole>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                    options.Lockout.MaxFailedAccessAttempts = 10;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<TIdentityDbContext>()
                .AddErrorDescriber<LocalIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            var authenticationBuilder = services.AddAuthentication();

            AddExternalProviders(authenticationBuilder, configuration);
        }

        /// <summary>
        /// Get configuration for registration
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static RegisterConfiguration GetRegistrationConfiguration(IConfiguration configuration)
        {
            var registerConfiguration = configuration.GetSection(nameof(RegisterConfiguration)).Get<RegisterConfiguration>();

            // Cannot load configuration - use default configuration values
            if (registerConfiguration == null)
            {
                return new RegisterConfiguration();
            }

            return registerConfiguration;
        }

        /// <summary>
        /// Add external providers
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="configuration"></param>
        private static void AddExternalProviders(AuthenticationBuilder authenticationBuilder,
            IConfiguration configuration)
        {
            var externalProviderConfiguration = configuration.GetSection(nameof(ExternalProvidersConfiguration)).Get<ExternalProvidersConfiguration>();

            if (externalProviderConfiguration != null && externalProviderConfiguration.UseGitHubProvider)
            {
                authenticationBuilder.AddGitHub(options =>
                {
                    options.ClientId = externalProviderConfiguration.GitHubClientId;
                    options.ClientSecret = externalProviderConfiguration.GitHubClientSecret;
                    options.Scope.Add("user:email");
                });
            }
        }
    }
}

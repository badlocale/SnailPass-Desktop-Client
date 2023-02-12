using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using SnailPass_Desktop.Data.API;
using SnailPass_Desktop.Data.Repositories;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.Services;
using SnailPass_Desktop.ViewModel;
using SnailPass_Desktop.ViewModel.Factories;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SnailPass_Desktop
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>().SingleInstance();
            builder.RegisterType<CustomFieldRepository>().As<ICustomFieldRepository>().SingleInstance();

            builder.RegisterType<UserApi>().As<IUserRestApi>().SingleInstance();
            builder.RegisterType<AccountApi>().As<IAccountRestApi>().SingleInstance();
            builder.RegisterType<CustomFieldsApi>().As<ICustomFieldRestApi>().SingleInstance();

            builder.RegisterType<Pbkdf2Encryptor>().As<IKeyGenerator>().SingleInstance();
            builder.RegisterType<AesCbcCryptographer>().As<ISymmetricCryptographer>().InstancePerDependency();
            builder.RegisterType<NavigationStore>().As<INavigationStore>().InstancePerLifetimeScope();
            builder.RegisterType<UserIdentityStore>().As<IUserIdentityStore>().SingleInstance();
            builder.RegisterType<ApplicationModeStore>().As<IApplicationModeStore>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<SynchronizationService>().As<ISynchronizationService>().SingleInstance();
            builder.RegisterType<CryptographyService>().As<ICryptographyService>().InstancePerDependency();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().SingleInstance();
            builder.RegisterType<ViewModelFactory>().As<IViewModelFactory>();
            builder.Register<ILogger>((c, p) =>
            {
                return new LoggerConfiguration()
                    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Month)
                    .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
                    .MinimumLevel.Debug()
                    .CreateLogger();
            }).SingleInstance();

            builder.RegisterType<LoginViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<StartupViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<AccountsViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<ApplicationViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<AddNewAccountViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<AddCustomFieldViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<EditAccountViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<EditCustomFieldViewModel>().AsSelf().InstancePerDependency();
            builder.RegisterType<TokenExpiredViewModel>().AsSelf().InstancePerDependency();

            return builder.Build();
        }
    }
}
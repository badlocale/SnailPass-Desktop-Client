using Autofac;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using SnailPass_Desktop.Data;
using SnailPass_Desktop.Data.API;
using SnailPass_Desktop.Data.Repositories;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.Model.Interfaces;
using SnailPass_Desktop.ViewModel;
using SnailPass_Desktop.ViewModel.Factories;
using SnailPass_Desktop.ViewModel.Services;
using SnailPass_Desktop.ViewModel.Stores;

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
            builder.RegisterType<RestAPI>().As<IRestClient>().SingleInstance();
            builder.RegisterType<Pbkdf2Encryptor>().As<IMasterPasswordEncryptor>().SingleInstance();
            builder.RegisterType<AesCbcCryptographer>().As<ISymmetricCryptographer>();
            builder.RegisterType<NavigationStore>().As<INavigationStore>().InstancePerLifetimeScope();
            builder.RegisterType<UserIdentityStore>().As<IUserIdentityStore>().SingleInstance();
            builder.RegisterType<ApplicationModeStore>().As<IApplicationModeStore>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            builder.RegisterType<SynchronizationService>().As<ISynchronizationService>().InstancePerDependency();
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

            return builder.Build();
        }
    }
}
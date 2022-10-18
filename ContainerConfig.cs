using Autofac;
using SnailPass_Desktop.Data.API;
using SnailPass_Desktop.Model;
using SnailPass_Desktop.Model.Cryptography;
using SnailPass_Desktop.Repositories;
using SnailPass_Desktop.ViewModel;
using SnailPass_Desktop.ViewModel.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
            builder.RegisterType<AccountRepository>().As<IAccountRepository>().SingleInstance();
            builder.RegisterType<RestAPI>().As<IRestClient>().SingleInstance();
            builder.RegisterType<Pbkdf2Encryptor>().As<IMasterPasswordEncryptor>().SingleInstance();
            builder.RegisterType<AesCbcCryptographer>().As<ISymmetricCryptographer>();
            builder.RegisterType<NavigationStore>().As<INavigationStore>().InstancePerLifetimeScope();
            builder.RegisterType<UserIdentityStore>().As<IUserIdentityStore>().SingleInstance();
            builder.RegisterType<LoginViewModel>().AsSelf();
            builder.RegisterType<StartupViewModel>().AsSelf();
            builder.RegisterType<AccountsViewModel>().AsSelf();
            builder.RegisterType<ApplicationViewModel>().AsSelf();

            return builder.Build();
        }
    }
}

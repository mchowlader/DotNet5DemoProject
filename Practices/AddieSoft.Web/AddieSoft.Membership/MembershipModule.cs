using AddieSoft.Membership.Contexts;
using AddieSoft.Membership.Repositories;
using AddieSoft.Membership.Services;
using AddieSoft.Membership.UnitOfWorks;
using Autofac;
using System;

namespace AddieSoft.Membership
{
    public class MembershipModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public MembershipModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MembershipDbContext>().AsSelf()
               .WithParameter("connectionString", _connectionString)
               .WithParameter("migrationAssemblyName", _migrationAssemblyName)
               .InstancePerLifetimeScope();

            builder.RegisterType<MembershipDbContext>().As<IMembershipDbContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<UserInfoRepository>().As<IUserInfoRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MembershipUnitOfWork>().As<IMembershipUnitOfWork>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}

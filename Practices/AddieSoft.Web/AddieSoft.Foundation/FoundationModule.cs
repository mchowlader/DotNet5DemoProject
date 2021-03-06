using AddieSoft.Foundation.Services;
using Autofac;
using DevSkill.Core.Services;
using System;

namespace AddieSoft.Foundation
{
    public class FoundationModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public FoundationModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DirectoryAdapter>().As<IDirectoryAdapter>().InstancePerLifetimeScope();
            builder.RegisterType<FileStoreUtility>().As<IFileStoreUtility>().InstancePerLifetimeScope();
            builder.RegisterType<FileAdapter>().As<IFileAdapter>().InstancePerLifetimeScope();
            builder.RegisterType<PathService>().As<IPathService>().InstancePerLifetimeScope();
            builder.RegisterType<SystemImageResizer>().As<ISystemImageResizer>().InstancePerLifetimeScope();
            builder.RegisterType<ImageResizer>().As<IImageResizer>().InstancePerLifetimeScope();


            base.Load(builder);
        }
    }
}
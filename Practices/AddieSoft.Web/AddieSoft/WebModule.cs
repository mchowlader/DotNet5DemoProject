using AddieSoft.Models;
using Autofac;

namespace AddieSoft
{
    public class WebModule : Module
    {
      
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CreateModel>().AsSelf();
            builder.RegisterType<DataModel>().AsSelf();
            builder.RegisterType<EditModel>().AsSelf();

            base.Load(builder);
        }
    }
}
using Autofac;
using Services.Services.Abstractions;

namespace Services
{
    public class ModuleInit : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Business services
            builder.RegisterAssemblyTypes(GetType().Assembly)
                .Where(type => typeof(BaseService).IsAssignableFrom(type))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
using Autofac;
using lori.backend.Core.Interfaces;
using lori.backend.Core.Services;

namespace lori.backend.Core;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ToDoItemSearchService>()
        .As<IToDoItemSearchService>().InstancePerLifetimeScope();
  }
}

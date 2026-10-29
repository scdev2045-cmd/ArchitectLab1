using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using StarCatalog.DataAccessLayer;

namespace StarCatalog.Core
{
    /// <summary>
    /// Конфигурационный модуль Ninject для настройки сопоставления интерфейсов и реализаций.
    /// </summary>
    public class SimpleConfigModule : NinjectModule
    {
        /// <summary>
        /// Регистрирует зависимости в DI-контейнере и обеспечивает готовность схемы БД.
        /// </summary>
        public override void Load()
        {
            DatabaseInitializer.EnsureCreated();

            // Регистрируем репозиторий для Star как Singleton с передачей строки подключения
            // При необходимости переключения на EntityRepository достаточно изменить привязку здесь:
            // Bind<IRepository<Star>>().To<EntityRepository<Star>>().InSingletonScope()
            //     .WithConstructorArgument("connectionString", DatabaseSettings.ConnectionString);

            Bind<IRepository<Star>>()
                .To<DapperRepository>()
                .InSingletonScope()
                .WithConstructorArgument("connectionString", DatabaseSettings.ConnectionString);
        }
    }
}
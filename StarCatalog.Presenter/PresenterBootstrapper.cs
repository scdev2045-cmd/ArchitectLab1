using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using StarCatalog.Core;
using StarCatalog.Shared;

namespace StarCatalog.Presenter
{
    /// <summary>
    /// Точка сборки (Composition Root) приложения.
    /// Инкапсулирует настройку IoC-контейнера Ninject и создание презентера,
    /// благодаря чему слой View не зависит от Ninject, Model и DataAccessLayer.
    /// </summary>
    public static class PresenterBootstrapper
    {
        /// <summary>
        /// Единый экземпляр IoC-контейнера Ninject, настроенный модулем SimpleConfigModule.
        /// </summary>
        private static readonly IKernel Kernel = new StandardKernel(new SimpleConfigModule());

        /// <summary>
        /// Создает и связывает презентер с переданным представлением.
        /// Все зависимости модели и репозитория внедряются автоматически через Ninject.
        /// </summary>
        /// <param name="view">Экземпляр представления, реализующий IStarView.</param>
        /// <returns>Готовый к работе экземпляр StarCatalogPresenter.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если view равен null.</exception>
        public static StarCatalogPresenter CreatePresenter(IStarView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view), "Представление не может быть null.");
            }

            IStarModel model = Kernel.Get<IStarModel>();

            return new StarCatalogPresenter(view, model);
        }
    }
}
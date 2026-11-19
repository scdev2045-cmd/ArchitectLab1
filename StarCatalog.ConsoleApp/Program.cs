using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarCatalog.Presenter;

namespace StarCatalog.ConsoleApp
{
    /// <summary>
    /// Главный класс точки входа в консольное приложение управления каталогом звезд.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Точка входа в консольное приложение.
        /// Создает консольное представление и связывает его с презентером через точку сборки.
        /// </summary>
        /// <param name="args">Параметры командной строки.</param>
        static void Main(string[] args)
        {
            ConsoleStarView view = new ConsoleStarView();

            using (StarCatalogPresenter presenter = PresenterBootstrapper.CreatePresenter(view))
            {
                view.Run();
            }
        }
    }
}
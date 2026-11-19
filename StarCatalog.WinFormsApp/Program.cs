using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using StarCatalog.Presenter;

namespace StarCatalog.WinFormsApp
{
    /// <summary>
    /// Главный класс точки входа в графическое приложение Windows Forms.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// Создает представление и связывает его с презентером через точку сборки.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm mainForm = new MainForm();

            using (StarCatalogPresenter presenter = PresenterBootstrapper.CreatePresenter(mainForm))
            {
                Application.Run(mainForm);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StarCatalog.ConsoleApp;
using StarCatalog.WinFormsApp;

namespace StarCatalog.Presenter
{
    /// <summary>
    /// Запускает одно из представлений каталога и подключает к нему презентер.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Запускает WinForms без аргументов или консоль в режиме --console.
        /// </summary>
        /// <param name="args">
        /// Аргументы запуска. Для консольного режима передается --console.
        /// </param>
        [STAThread]
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                RunWinForms();
                return;
            }

            if (args.Length == 1 &&
                string.Equals(args[0], "--console", StringComparison.OrdinalIgnoreCase))
            {
                RunConsole();
                return;
            }

            Console.Error.WriteLine(
                "Запустите приложение без аргументов для WinForms " +
                "или с аргументом --console для консольного режима.");
            Environment.ExitCode = 1;
        }

        /// <summary>
        /// Подключает презентер к консольному представлению и запускает меню.
        /// </summary>
        private static void RunConsole()
        {
            var view = new ConsoleStarView();

            using (var presenter = PresenterBootstrapper.CreatePresenter(view))
            {
                view.Run();
            }
        }

        /// <summary>
        /// Подключает презентер к главной форме и запускает цикл Windows Forms.
        /// </summary>
        private static void RunWinForms()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var view = new MainForm())
            using (var presenter = PresenterBootstrapper.CreatePresenter(view))
            {
                Application.Run(view);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ninject;
using StarCatalog.Core;

namespace StarCatalog.WinFormsApp
{
    /// <summary>
    /// Главный класс точки входа в графическое приложение Windows Forms.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IKernel kernel = new StandardKernel(new SimpleConfigModule());
            ILogic logic = kernel.Get<ILogic>();

            Application.Run(new MainForm(logic));
        }
    }
}
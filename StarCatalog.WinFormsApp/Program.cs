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

            // Создаем IoC-контейнер и получаем бизнес-логику для слоя представления
            IKernel kernel = new StandardKernel(new SimpleConfigModule());
            Logic logic = kernel.Get<Logic>();

            // Передаем экземпляр Logic в форму через конструктор
            Application.Run(new MainForm(logic));
        }
    }
}
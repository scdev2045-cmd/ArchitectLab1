using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog.DataAccessLayer
{
    /// <summary>
    /// Содержит настройки подключения к базе данных каталога звезд.
    /// </summary>
    public static class DatabaseSettings
    {
        /// <summary>
        /// Возвращает строку подключения к общей базе данных приложения.
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                // Символ @ перед строкой позволяет писать
                // обратные слеши без удвоения.
                //
                // Оба интерфейса будут использовать один файл базы.
                //
                // Перед переносом на другой компьютер
                // этот путь потребуется настроить.
                return
                    @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    @"AttachDbFilename=C:\Users\egorb\source\repos\ArchitectureLab1\StarCatalog.ConsoleApp\StarDatabase.mdf;" +
                    @"Integrated Security=True;";
            }
        }
    }
}
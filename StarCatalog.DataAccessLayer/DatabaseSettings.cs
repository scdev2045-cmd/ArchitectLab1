using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace StarCatalog.DataAccessLayer
{
    /// <summary>
    /// Предоставляет настройки подключения к базе данных.
    /// </summary>
    public static class DatabaseSettings
    {
        /// <summary>
        /// Возвращает строку подключения из конфигурации
        /// запущенного приложения.
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                ConnectionStringSettings settings =
                    ConfigurationManager
                        .ConnectionStrings["StarDatabase"];

                if (settings == null)
                {
                    throw new InvalidOperationException(
                        "В App.config отсутствует строка подключения StarDatabase.");
                }

                if (string.IsNullOrWhiteSpace(
                    settings.ConnectionString))
                {
                    throw new InvalidOperationException(
                        "Строка подключения StarDatabase не заполнена.");
                }

                return settings.ConnectionString;
            }
        }
    }
}
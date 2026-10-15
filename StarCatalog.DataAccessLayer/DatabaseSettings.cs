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
                // Ищем настройку по имени StarDatabase.
                ConnectionStringSettings settings =
                    ConfigurationManager
                        .ConnectionStrings["StarDatabase"];

                // Если настройка отсутствует, сообщаем точную причину.
                if (settings == null)
                {
                    throw new InvalidOperationException(
                        "В App.config отсутствует строка подключения StarDatabase.");
                }

                // Настройка может существовать, но содержать пустую строку.
                if (string.IsNullOrWhiteSpace(
                    settings.ConnectionString))
                {
                    throw new InvalidOperationException(
                        "Строка подключения StarDatabase не заполнена.");
                }

                // Возвращаем найденное значение.
                return settings.ConnectionString;
            }
        }
    }
}
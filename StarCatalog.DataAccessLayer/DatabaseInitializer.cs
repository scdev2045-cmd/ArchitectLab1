using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace StarCatalog.DataAccessLayer
{
    /// <summary>
    /// Подготавливает базу данных каталога при запуске приложения.
    /// </summary>
    public static class DatabaseInitializer
    {
        /// <summary>
        /// Создает базу и таблицу звезд, если они еще отсутствуют.
        /// Существующие записи не удаляются.
        /// </summary>
        public static void EnsureCreated()
        {
            // Получаем строку подключения из App.config
            // запущенного приложения.
            string connectionString =
                DatabaseSettings.ConnectionString;

            // Разбираем строку подключения на отдельные настройки.
            // Это безопаснее, чем заменять части строки вручную.
            SqlConnectionStringBuilder builder =
                new SqlConnectionStringBuilder(connectionString);

            // Проверяем настройку, чтобы случайно не создавать
            // одну базу, а затем подключаться к другой.
            if (builder.InitialCatalog != "StarCatalogLab2")
            {
                throw new InvalidOperationException(
                    "В строке подключения должно быть указано " +
                    "Initial Catalog=StarCatalogLab2.");
            }

            // Старый способ подключения через файл здесь не используем.
            if (!string.IsNullOrWhiteSpace(builder.AttachDBFilename))
            {
                throw new InvalidOperationException(
                    "Удалите AttachDbFilename из строки подключения. " +
                    "Приложение создает базу автоматически.");
            }

            // Сначала подключаемся к служебной базе master.
            // К StarCatalogLab2 пока подключаться нельзя:
            // при первом запуске ее еще не существует.
            builder.InitialCatalog = "master";

            using (SqlConnection connection =
                new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                // DB_ID возвращает идентификатор существующей базы.
                // NULL означает, что базы с таким именем еще нет.
                //
                // Путь к файлам не указываем:
                // их размещение определит SQL Server.
                string createDatabaseSql = @"
                    IF DB_ID(N'StarCatalogLab2') IS NULL
                    BEGIN
                        CREATE DATABASE [StarCatalogLab2];
                    END;";

                using (SqlCommand command =
                    new SqlCommand(createDatabaseSql, connection))
                {
                    // Создание базы может занять некоторое время.
                    command.CommandTimeout = 60;

                    // Выполняем команду, которая не возвращает таблицу.
                    command.ExecuteNonQuery();
                }
            }

            // К этому моменту база должна существовать.
            // Подключаемся уже к ней по исходной строке.
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                connection.Open();

                // Проверяем наличие таблицы.
                // Если таблица уже есть, повторно ее не создаем.
                //
                // Существующие записи при повторном запуске
                // остаются на месте.
                string createTableSql = @"
                    IF OBJECT_ID(N'dbo.Stars', N'U') IS NULL
                    BEGIN
                        CREATE TABLE dbo.Stars
                        (
                            Id INT IDENTITY(1, 1) NOT NULL
                                CONSTRAINT PK_Stars PRIMARY KEY,

                            Name NVARCHAR(200) NOT NULL,

                            Discoverer NVARCHAR(200) NOT NULL,

                            StarType NVARCHAR(100) NOT NULL,

                            Radius FLOAT NOT NULL,

                            CONSTRAINT CK_Stars_Name
                                CHECK
                                (
                                    LEN(LTRIM(RTRIM(Name))) > 0
                                ),

                            CONSTRAINT CK_Stars_Discoverer
                                CHECK
                                (
                                    LEN(LTRIM(RTRIM(Discoverer))) > 0
                                ),

                            CONSTRAINT CK_Stars_Type
                                CHECK
                                (
                                    StarType IN
                                    (
                                        N'Красный гигант',
                                        N'Желтый карлик',
                                        N'Белый карлик',
                                        N'Нейтронная звезда'
                                    )
                                ),

                            CONSTRAINT CK_Stars_Radius
                                CHECK (Radius > 0)
                        );
                    END;";

                using (SqlCommand command =
                    new SqlCommand(createTableSql, connection))
                {
                    command.CommandTimeout = 60;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
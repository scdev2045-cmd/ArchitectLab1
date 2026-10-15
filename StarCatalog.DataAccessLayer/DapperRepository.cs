using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using StarCatalog.Core;

namespace StarCatalog.DataAccessLayer
{
    /// <summary>
    /// Выполняет CRUD-операции через Dapper.
    /// SQL-запросы этой реализации рассчитаны на сущность Star.
    /// </summary>
    /// <typeparam name="T">
    /// Тип сущности. В текущей реализации поддерживается только Star.
    /// </typeparam>
    public class DapperRepository<T> : IRepository<T>
        where T : class, IDomainObject
    {
        // Храним строку подключения для открытия соединений.
        private string connectionString;

        /// <summary>
        /// Создает репозиторий для работы с таблицей звезд.
        /// </summary>
        /// <param name="connectionString">
        /// Строка подключения к базе данных SQL Server.
        /// </param>
        public DapperRepository(string connectionString)
        {
            // Запрещаем создание репозитория без подключения.
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(
                    "Строка подключения не может быть пустой.",
                    "connectionString");
            }

            // Проверяем, что репозиторий создается именно для Star.
            // Запросы ниже используют поля таблицы звезд.
            if (typeof(T) != typeof(Star))
            {
                throw new NotSupportedException(
                    "Эта реализация DapperRepository поддерживает только Star.");
            }

            this.connectionString = connectionString;
        }

        /// <summary>
        /// Добавляет звезду в базу данных и записывает выданный Id в объект.
        /// </summary>
        /// <param name="item">Добавляемая звезда.</param>
        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            // Id не указываем в INSERT.
            // Его автоматически создаст SQL Server.
            //
            // OUTPUT INSERTED.Id возвращает номер созданной записи.
            string sql = @"
                INSERT INTO dbo.Stars
                (
                    Name,
                    Discoverer,
                    StarType,
                    Radius
                )
                OUTPUT INSERTED.Id
                VALUES
                (
                    @Name,
                    @Discoverer,
                    @StarType,
                    @Radius
                );";

            // using освобождает соединение после выполнения блока.
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                connection.Open();

                // Dapper берет значения параметров @Name,
                // @Discoverer, @StarType и @Radius
                // из свойств переданного объекта item.
                //
                // QuerySingle<int> ожидает одно возвращенное число:
                // идентификатор созданной записи.
                int newId = connection.QuerySingle<int>(sql, item);

                // Сохраняем полученный идентификатор в объекте.
                item.Id = newId;
            }
        }

        /// <summary>
        /// Удаляет звезду с указанным идентификатором.
        /// Если записи нет, база данных не изменяется.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой звезды.</param>
        public void Delete(int id)
        {
            string sql = @"
                DELETE FROM dbo.Stars
                WHERE Id = @Id;";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                connection.Open();

                // Создаем объект с параметром Id.
                // Dapper передаст его в SQL-параметр @Id.
                //
                // Значение не вставляется в текст SQL через склейку строк.
                connection.Execute(sql, new { Id = id });
            }
        }

        /// <summary>
        /// Возвращает все звезды из базы данных.
        /// </summary>
        /// <returns>Список звезд, упорядоченный по идентификатору.</returns>
        public List<T> ReadAll()
        {
            string sql = @"
                SELECT
                    Id,
                    Name,
                    Discoverer,
                    StarType,
                    Radius
                FROM dbo.Stars
                ORDER BY Id;";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                connection.Open();

                // Query<T> создает объекты из строк результата.
                // Названия столбцов совпадают со свойствами Star.
                //
                // ToList превращает результат в обычный List<T>.
                return connection.Query<T>(sql).ToList();
            }
        }

        /// <summary>
        /// Находит звезду по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор искомой звезды.</param>
        /// <returns>Найденная звезда или null, если записи нет.</returns>
        public T ReadById(int id)
        {
            string sql = @"
                SELECT
                    Id,
                    Name,
                    Discoverer,
                    StarType,
                    Radius
                FROM dbo.Stars
                WHERE Id = @Id;";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                connection.Open();

                // Первичный ключ уникален.
                // Поэтому результат содержит одну запись или ни одной.
                //
                // Если строки нет, возвращается null.
                return connection.QuerySingleOrDefault<T>(
                    sql,
                    new { Id = id });
            }
        }

        /// <summary>
        /// Обновляет характеристики существующей звезды.
        /// </summary>
        /// <param name="item">Звезда с новыми значениями свойств.</param>
        /// <exception cref="InvalidOperationException">
        /// Запись с указанным идентификатором не найдена.
        /// </exception>
        public void Update(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            string sql = @"
                UPDATE dbo.Stars
                SET
                    Name = @Name,
                    Discoverer = @Discoverer,
                    StarType = @StarType,
                    Radius = @Radius
                WHERE Id = @Id;";

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                connection.Open();

                // Execute возвращает количество затронутых строк.
                int changedRows = connection.Execute(sql, item);

                // Нельзя считать обновление успешным,
                // если записи с таким Id не было.
                if (changedRows == 0)
                {
                    throw new InvalidOperationException(
                        "Звезда для изменения не найдена.");
                }
            }
        }
    }
}

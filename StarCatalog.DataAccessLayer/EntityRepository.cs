using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using StarCatalog.Core;

namespace StarCatalog.DataAccessLayer
{
    /// <summary>
    /// Выполняет CRUD-операции с помощью Entity Framework 6.
    /// </summary>
    /// <typeparam name="T">
    /// Тип сущности, которая имеет свойство Id.
    /// Тип должен быть зарегистрирован в контексте базы данных.
    /// </typeparam>
    public class EntityRepository : IRepository
    {
        private string connectionString;

        /// <summary>
        /// Создает репозиторий для указанной базы данных.
        /// </summary>
        /// <param name="connectionString">
        /// Строка подключения к базе данных SQL Server.
        /// </param>
        public EntityRepository(string connectionString)
        {

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(
                    "Строка подключения не может быть пустой.",
                    "connectionString");
            }

            this.connectionString = connectionString;
        }

        /// <summary>
        /// Добавляет объект в базу данных.
        /// </summary>
        /// <param name="item">Добавляемый объект.</param>
        public void Add(Star item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            using (StarDbContext context =
                new StarDbContext(connectionString))
            {
                context.Stars.Add(item);

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Удаляет объект по идентификатору.
        /// Если объект отсутствует, ничего не изменяет.
        /// </summary>
        /// <param name="id">Идентификатор удаляемого объекта.</param>
        public void Delete(int id)
        {
            using (StarDbContext context =
                new StarDbContext(connectionString))
            {
                Star item = context.Stars.Find(id);

                if (item == null)
                {
                    return;
                }

                context.Stars.Remove(item);

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Возвращает все объекты из базы данных.
        /// </summary>
        /// <returns>
        /// Список объектов. Если записей нет, список будет пустым.
        /// </returns>
        public List<Star> ReadAll()
        {
            using (StarDbContext context =
                new StarDbContext(connectionString))
            {
                return context.Stars.AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// Возвращает объект с указанным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор искомого объекта.</param>
        /// <returns>
        /// Найденный объект или null, если запись отсутствует.
        /// </returns>
        public Star ReadById(int id)
        {
            using (StarDbContext context =
                new StarDbContext(connectionString))
            {
                return context.Stars.Find(id);
            }
        }

        /// <summary>
        /// Обновляет существующий объект в базе данных.
        /// </summary>
        /// <param name="item">
        /// Объект с идентификатором записи и новыми значениями.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Передан null вместо объекта.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Объект с указанным идентификатором не найден.
        /// </exception>
        public void Update(Star item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            using (StarDbContext context =
                new StarDbContext(connectionString))
            {
                Star existingItem = context.Stars.Find(item.Id);

                if (existingItem == null)
                {
                    throw new InvalidOperationException(
                        "Объект для изменения не найден.");
                }

                context.Entry(existingItem)
                    .CurrentValues
                    .SetValues(item);

                context.SaveChanges();
            }
        }
    }
}
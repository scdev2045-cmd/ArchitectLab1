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
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
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
                    nameof(connectionString));
            }

            this.connectionString = connectionString;
        }

        /// <summary>
        /// Добавляет объект в базу данных.
        /// </summary>
        /// <param name="item">Добавляемый объект.</param>
        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            using (StarDbContext context = new StarDbContext(connectionString))
            {
                context.Set<T>().Add(item);
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
            using (StarDbContext context = new StarDbContext(connectionString))
            {
                T item = context.Set<T>().Find(id);

                if (item == null)
                {
                    return;
                }

                context.Set<T>().Remove(item);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Возвращает все объекты из базы данных.
        /// </summary>
        /// <returns>
        /// Список объектов. Если записей нет, список будет пустым.
        /// </returns>
        public List<T> ReadAll()
        {
            using (StarDbContext context = new StarDbContext(connectionString))
            {
                return context.Set<T>()
                    .AsNoTracking()
                    .ToList();
            }
        }

        /// <summary>
        /// Возвращает объект с указанным идентификатором.
        /// </summary>
        /// <param name="id">Идентификатор искомого объекта.</param>
        /// <returns>
        /// Найденный объект или null, если запись отсутствует.
        /// </returns>
        public T ReadById(int id)
        {
            using (StarDbContext context = new StarDbContext(connectionString))
            {
                return context.Set<T>().Find(id);
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
        public void Update(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            using (StarDbContext context = new StarDbContext(connectionString))
            {
                T existingItem = context.Set<T>().Find(item.Id);

                if (existingItem == null)
                {
                    throw new InvalidOperationException("Объект для изменения не найден.");
                }

                context.Entry(existingItem).CurrentValues.SetValues(item);
                context.SaveChanges();
            }
        }
    }
}
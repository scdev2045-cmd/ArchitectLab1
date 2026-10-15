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
    public class EntityRepository<T> : IRepository<T>
        where T : class, IDomainObject
    {
        // Сохраняем строку подключения.
        // Она понадобится при создании каждого контекста.
        private string connectionString;

        /// <summary>
        /// Создает репозиторий для указанной базы данных.
        /// </summary>
        /// <param name="connectionString">
        /// Строка подключения к базе данных SQL Server.
        /// </param>
        public EntityRepository(string connectionString)
        {
            // Нельзя создать рабочий репозиторий
            // без строки подключения.
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(
                    "Строка подключения не может быть пустой.",
                    "connectionString");
            }

            // this.connectionString — поле нашего объекта.
            // connectionString справа — параметр конструктора.
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Добавляет объект в базу данных.
        /// </summary>
        /// <param name="item">Добавляемый объект.</param>
        public void Add(T item)
        {
            // null означает, что объект вообще не был передан.
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            // Создаем контекст для одной операции.
            // using освобождает контекст после выхода из блока,
            // в том числе если внутри возникнет исключение.
            using (StarDbContext context =
                new StarDbContext(connectionString))
            {
                // Set<T>() получает набор сущностей нужного типа.
                // Для T = Star это набор звезд.
                //
                // Add пока только сообщает контексту,
                // что объект нужно добавить.
                context.Set<T>().Add(item);

                // Отправляем изменение в базу данных.
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
                // Ищем объект по первичному ключу.
                T item = context.Set<T>().Find(id);

                // Если записи нет, удалять нечего.
                if (item == null)
                {
                    return;
                }

                // Помечаем найденную запись для удаления.
                context.Set<T>().Remove(item);

                // Выполняем удаление в базе данных.
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
            using (StarDbContext context =
                new StarDbContext(connectionString))
            {
                // AsNoTracking означает, что контексту
                // не нужно отслеживать изменения полученных объектов.
                //
                // ToList выполняет запрос и помещает результат
                // в обычный список до закрытия контекста.
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
            using (StarDbContext context =
                new StarDbContext(connectionString))
            {
                // Find ищет запись по первичному ключу.
                //
                // После выхода из using контекст будет освобожден.
                // Полученный объект можно будет читать,
                // но изменения его свойств сами в БД не сохранятся.
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
                throw new ArgumentNullException("item");
            }

            using (StarDbContext context =
                new StarDbContext(connectionString))
            {
                // Загружаем существующую запись.
                // Контекст отслеживает этот найденный объект.
                T existingItem =
                    context.Set<T>().Find(item.Id);

                // Не сообщаем об успешном обновлении,
                // если записи уже нет в базе.
                if (existingItem == null)
                {
                    throw new InvalidOperationException(
                        "Объект для изменения не найден.");
                }

                // Копируем значения свойств переданного объекта
                // в объект, который отслеживает текущий контекст.
                //
                // Для Star это Id, Name, Discoverer,
                // StarType и Radius.
                context.Entry(existingItem)
                    .CurrentValues
                    .SetValues(item);

                // Сохраняем изменения в базе данных.
                context.SaveChanges();
            }
        }
    }
}
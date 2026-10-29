using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarCatalog.Core;

namespace StarCatalog.DataAccessLayer
{
    /// <summary>
    /// Контракт для операций чтения сущностей из хранилища (ISP).
    /// </summary>
    /// <typeparam name="T">Тип сущности предметной области.</typeparam>
    public interface IReadOnlyRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Возвращает все сущности из хранилища данных.
        /// </summary>
        /// <returns>Список всех сущностей.</returns>
        List<T> ReadAll();

        /// <summary>
        /// Возвращает сущность по её уникальному идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор сущности.</param>
        /// <returns>Найденная сущность или null, если объект не найден.</returns>
        T ReadById(int id);
    }

    /// <summary>
    /// Контракт для операций модификации сущностей в хранилище (ISP).
    /// </summary>
    /// <typeparam name="T">Тип сущности предметной области.</typeparam>
    public interface IWriteOnlyRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Добавляет новую сущность в хранилище.
        /// </summary>
        /// <param name="item">Добавляемый объект сущности.</param>
        void Add(T item);

        /// <summary>
        /// Обновляет атрибуты существующей сущности в хранилище.
        /// </summary>
        /// <param name="item">Сущность с обновленными свойствами.</param>
        void Update(T item);

        /// <summary>
        /// Удаляет сущность из хранилища по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой сущности.</param>
        void Delete(int id);
    }

    /// <summary>
    /// Обобщенный интерфейс репозитория, выступающий фасадом CRUD-операций (ISP / Facade).
    /// </summary>
    /// <typeparam name="T">Тип сущности предметной области.</typeparam>
    public interface IRepository<T> : IReadOnlyRepository<T>, IWriteOnlyRepository<T> where T : class, IDomainObject
    {
    }
}
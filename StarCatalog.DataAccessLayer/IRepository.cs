using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarCatalog.Core;

namespace StarCatalog.DataAccessLayer
{
    /// <summary>
    /// Обобщенный интерфейс репозитория, задающий базовые CRUD-операции для работы с хранилищем данных.
    /// Ограничение where T : IDomainObject гарантирует, что у сущности всегда есть свойство Id.
    /// </summary>
    /// <typeparam name="T">Тип сущности предметной области.</typeparam>
    public interface IRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Добавляет новую сущность в базу данных.
        /// </summary>
        /// <param name="item">Добавляемый объект.</param>
        void Add(T item);

        /// <summary>
        /// Удаляет сущность из базы данных по ее уникальному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой записи.</param>
        void Delete(int id);

        /// <summary>
        /// Считывает и возвращает все записи сущностей из базы данных.
        /// </summary>
        /// <returns>Список всех объектов.</returns>
        List<T> ReadAll();

        /// <summary>
        /// Находит и возвращает одну запись по уникальному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор искомой записи.</param>
        /// <returns>Найденный объект или null, если запись отсутствует.</returns>
        T ReadById(int id);

        /// <summary>
        /// Обновляет данные существующей сущности в базе данных.
        /// </summary>
        /// <param name="item">Объект с обновленными данными.</param>
        void Update(T item);
    }
}

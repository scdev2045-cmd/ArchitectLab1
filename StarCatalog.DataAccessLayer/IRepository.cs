using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarCatalog.Core;

namespace StarCatalog.DataAccessLayer
{
    /// <summary>
    /// Обобщённый интерфейс репозитория для работы с сущностями, имеющими идентификатор.
    /// </summary>
    /// <typeparam name="T">Тип сущности предметной области.</typeparam>
    public interface IRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Добавляет сущность в хранилище.
        /// </summary>
        void Add(T item);

        /// <summary>
        /// Удаляет сущность по идентификатору.
        /// </summary>
        void Delete(int id);

        /// <summary>
        /// Возвращает все сущности.
        /// </summary>
        List<T> ReadAll();

        /// <summary>
        /// Возвращает сущность по идентификатору.
        /// </summary>
        T ReadById(int id);

        /// <summary>
        /// Изменяет сущность.
        /// </summary>
        void Update(T item);
    }
}
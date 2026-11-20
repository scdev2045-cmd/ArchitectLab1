using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarCatalog.Core;

namespace StarCatalog.DataAccessLayer
{
    /// <summary>
    /// Определяет операции чтения сущностей из хранилища.
    /// </summary>
    /// <typeparam name="T">Тип сущности с идентификатором.</typeparam>
    public interface IReadOnlyRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Возвращает все сохраненные сущности. Порядок элементов не гарантируется.
        /// </summary>
        /// <returns>
        /// Список сущностей; пустой список, если записей нет. Не возвращает null.
        /// </returns>
        List<T> ReadAll();

        /// <summary>
        /// Возвращает сохраненную сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор искомой записи.</param>
        /// <returns>Найденная сущность или null, если записи нет.</returns>
        T ReadById(int id);
    }

    /// <summary>
    /// Определяет операции добавления, изменения и удаления сущностей в хранилище.
    /// </summary>
    /// <typeparam name="T">Тип сущности с идентификатором.</typeparam>
    public interface IWriteOnlyRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Сохраняет новую сущность и записывает назначенный идентификатор в ее Id.
        /// </summary>
        /// <param name="item">Добавляемая сущность.</param>
        /// <exception cref="ArgumentNullException">Параметр item равен null.</exception>
        void Add(T item);

        /// <summary>
        /// Обновляет существующую запись по идентификатору.
        /// Не создает новую запись и не меняет идентификатор сохраненной записи.
        /// </summary>
        /// <param name="item">
        /// Сущность с идентификатором существующей записи и новыми значениями свойств.
        /// </param>
        /// <exception cref="ArgumentNullException">Параметр item равен null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Запись с идентификатором item.Id не найдена.
        /// </exception>
        void Update(T item);

        /// <summary>
        /// Удаляет запись по идентификатору.
        /// Если запись отсутствует, не изменяет хранилище.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой записи.</param>
        void Delete(int id);
    }

    /// <summary>
    /// Объединяет контракты чтения и записи в фасад CRUD-операций.
    /// </summary>
    /// <typeparam name="T">Тип сущности с идентификатором.</typeparam>
    public interface IRepository<T> : IReadOnlyRepository<T>, IWriteOnlyRepository<T>
        where T : class, IDomainObject
    {
    }
}
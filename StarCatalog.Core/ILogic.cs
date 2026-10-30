using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog.Core
{
    /// <summary>
    /// Интерфейс бизнес-логики управления каталогом звезд (DIP / OCP).
    /// </summary>
    public interface ILogic
    {
        /// <summary>
        /// Добавляет звезду в каталог после валидации входных характеристик.
        /// </summary>
        /// <param name="name">Название звезды.</param>
        /// <param name="discoverer">Сведения о первооткрывателе.</param>
        /// <param name="starType">Поддерживаемый тип звезды.</param>
        /// <param name="radius">Положительный радиус в радиусах Солнца.</param>
        /// <returns>true, если запись успешно валидирована и сохранена; иначе false.</returns>
        bool AddStar(string name, string discoverer, string starType, double radius);

        /// <summary>
        /// Возвращает все звезды из каталога, отсортированные по возрастанию идентификатора.
        /// </summary>
        /// <returns>Список звезд или пустой список, если записи отсутствуют.</returns>
        List<Star> GetAllStars();

        /// <summary>
        /// Находит звезду по ее уникальному идентификатору.
        /// </summary>
        /// <param name="id">Положительный идентификатор записи.</param>
        /// <returns>Найденный объект звезды или null, если запись отсутствует.</returns>
        Star FindById(int id);

        /// <summary>
        /// Удаляет существующую звезду по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой записи.</param>
        /// <returns>true, если запись найдена и удалена; иначе false.</returns>
        bool DeleteStar(int id);

        /// <summary>
        /// Изменяет характеристики существующей звезды в каталоге.
        /// </summary>
        /// <param name="id">Идентификатор изменяемой записи.</param>
        /// <param name="newName">Новое название звезды.</param>
        /// <param name="newDiscoverer">Новые сведения о первооткрывателе.</param>
        /// <param name="newStarType">Новый тип звезды.</param>
        /// <param name="newRadius">Новый радиус звезды.</param>
        /// <returns>true, если операция успешно завершена; иначе false.</returns>
        bool UpdateStar(int id, string newName, string newDiscoverer, string newStarType, double newRadius);

        /// <summary>
        /// Возвращает список звезд указанного спектрального типа.
        /// </summary>
        /// <param name="starType">Искомый тип звезды.</param>
        /// <returns>Список подходящих звезд или пустой список.</returns>
        List<Star> GetStarsByType(string starType);

        /// <summary>
        /// Рассчитывает средний радиус всех зарегистрированных звезд каталога.
        /// </summary>
        /// <returns>Средний радиус или 0, если каталог пуст.</returns>
        double CalculateAverageRadius();
    }
}
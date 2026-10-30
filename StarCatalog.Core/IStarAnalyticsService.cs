using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog.Core
{
    /// <summary>
    /// Сервис аналитических расчетов и выборки по каталогу звезд (SRP).
    /// </summary>
    public interface IStarAnalyticsService
    {
        /// <summary>
        /// Фильтрует список звезд по спектральному типу.
        /// </summary>
        /// <param name="stars">Исходный список звезд.</param>
        /// <param name="starType">Искомый спектральный тип.</param>
        /// <returns>Список звезд указанного типа.</returns>
        List<Star> FilterByType(IEnumerable<Star> stars, string starType);

        /// <summary>
        /// Рассчитывает средний радиус по переданной коллекции звезд.
        /// </summary>
        /// <param name="stars">Коллекция звезд для расчета.</param>
        /// <returns>Средний радиус или 0.0, если коллекция пуста.</returns>
        double CalculateAverageRadius(IEnumerable<Star> stars);
    }
}
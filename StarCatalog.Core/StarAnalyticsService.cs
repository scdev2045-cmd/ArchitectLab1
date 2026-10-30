using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog.Core
{
    /// <summary>
    /// Предоставляет реализацию аналитических вычислений над каталогом звезд (SRP).
    /// </summary>
    public class StarAnalyticsService : IStarAnalyticsService
    {
        /// <summary>
        /// Фильтрует список звезд по спектральному типу.
        /// </summary>
        /// <param name="stars">Исходный список звезд.</param>
        /// <param name="starType">Искомый спектральный тип.</param>
        /// <returns>Список звезд указанного типа.</returns>
        public List<Star> FilterByType(IEnumerable<Star> stars, string starType)
        {
            if (stars == null || string.IsNullOrWhiteSpace(starType))
            {
                return new List<Star>();
            }

            string targetType = starType.Trim();
            return stars
                .Where(s => string.Equals(s.StarType, targetType, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>
        /// Рассчитывает средний радиус по переданной коллекции звезд.
        /// </summary>
        /// <param name="stars">Коллекция звезд для расчета.</param>
        /// <returns>Средний радиус или 0.0, если коллекция пуста.</returns>
        public double CalculateAverageRadius(IEnumerable<Star> stars)
        {
            if (stars == null)
            {
                return 0.0;
            }

            List<Star> starList = stars.ToList();
            if (starList.Count == 0)
            {
                return 0.0;
            }

            double sum = 0.0;
            foreach (Star s in starList)
            {
                sum += s.Radius;
            }

            return sum / starList.Count;
        }
    }
}
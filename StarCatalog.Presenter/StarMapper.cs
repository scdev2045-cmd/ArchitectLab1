using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarCatalog.Core;
using StarCatalog.Shared;

namespace StarCatalog.Presenter
{
    /// <summary>
    /// Вспомогательный класс для преобразования доменных сущностей Star
    /// в объекты передачи данных StarDto (соблюдение SRP).
    /// </summary>
    public static class StarMapper
    {
        /// <summary>
        /// Преобразует один доменный объект звезды в объект передачи данных StarDto.
        /// </summary>
        /// <param name="star">Доменный объект звезды.</param>
        /// <returns>Новый экземпляр StarDto или null, если передан null.</returns>
        public static StarDto ToDto(Star star)
        {
            if (star == null)
            {
                return null;
            }

            return new StarDto(
                star.Id,
                star.Name,
                star.Discoverer,
                star.StarType,
                star.Radius);
        }

        /// <summary>
        /// Преобразует список доменных объектов звезд в список объектов StarDto.
        /// </summary>
        /// <param name="stars">Список доменных объектов звезд.</param>
        /// <returns>Список StarDto (пустой, если исходный список равен null или пуст).</returns>
        public static List<StarDto> ToDtoList(List<Star> stars)
        {
            List<StarDto> result = new List<StarDto>();

            if (stars == null)
            {
                return result;
            }

            for (int i = 0; i < stars.Count; i = i + 1)
            {
                StarDto dto = ToDto(stars[i]);
                if (dto != null)
                {
                    result.Add(dto);
                }
            }

            return result;
        }
    }
}
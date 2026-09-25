using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog.Core
{
    /// <summary>
    /// Представляет сущность астрономического объекта "Звезда".
    /// </summary>
    public class Star
    {
        /// <summary>
        /// Уникальный номер звезды в каталоге.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Астрономическое название звезды (например: Сириус, Бетельгейзе).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ФИО первооткрывателя или название обсерватории/каталога.
        /// </summary>
        public string Discoverer { get; set; }

        /// <summary>
        /// Спектральный тип звезды (например: Красный гигант, Желтый карлик, Белый карлик, Нейтронная звезда).
        /// </summary>
        public string StarType { get; set; }

        /// <summary>
        /// Радиус звезды относительно радиуса Солнца (в единицах R☉).
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр звезды со всеми обязательными характеристиками.
        /// </summary>
        /// <param name="id">Уникальный номер.</param>
        /// <param name="name">Название объекта.</param>
        /// <param name="discoverer">ФИО первооткрывателя.</param>
        /// <param name="starType">Спектральный тип звезды.</param>
        /// <param name="radius">Радиус в солнечных радиусах.</param>
        public Star(int id, string name, string discoverer, string starType, double radius)
        {
            Id = id;
            Name = name;
            Discoverer = discoverer;
            StarType = starType;
            Radius = radius;
        }
    }
}

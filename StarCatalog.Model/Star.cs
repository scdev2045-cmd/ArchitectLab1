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
    public class Star : IDomainObject
    {
        /// <summary>
        /// Уникальный номер звезды в каталоге и первичный ключ в БД.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Астрономическое название звезды.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ФИО первооткрывателя или название обсерватории.
        /// </summary>
        public string Discoverer { get; set; }

        /// <summary>
        /// Спектральный тип звезды.
        /// </summary>
        public string StarType { get; set; }

        /// <summary>
        /// Радиус звезды относительно Солнца.
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Пустой конструктор для создания объектов библиотеками EF и Dapper.
        /// </summary>
        public Star()
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр звезды со всеми характеристиками.
        /// </summary>
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

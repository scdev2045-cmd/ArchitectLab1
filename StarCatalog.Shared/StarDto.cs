using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog.Shared
{
    /// <summary>
    /// Объект передачи данных (DTO) для обмена сведениями о звезде
    /// между слоем представления (View) и презентером (Presenter).
    /// Не содержит бизнес-логики и не зависит от доменной модели.
    /// </summary>
    public class StarDto
    {
        /// <summary>
        /// Уникальный идентификатор звезды.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название звезды.
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
        /// Радиус звезды в радиусах Солнца.
        /// </summary>
        public double Radius { get; set; }

        /// <summary>
        /// Инициализирует пустой экземпляр DTO звезды.
        /// </summary>
        public StarDto()
        {
        }

        /// <summary>
        /// Инициализирует экземпляр DTO звезды с заданными значениями всех полей.
        /// </summary>
        /// <param name="id">Уникальный идентификатор звезды.</param>
        /// <param name="name">Название звезды.</param>
        /// <param name="discoverer">Первооткрыватель звезды.</param>
        /// <param name="starType">Спектральный тип звезды.</param>
        /// <param name="radius">Радиус звезды в радиусах Солнца.</param>
        public StarDto(int id, string name, string discoverer, string starType, double radius)
        {
            // Записываем переданные значения в свойства DTO.
            Id = id;
            Name = name;
            Discoverer = discoverer;
            StarType = starType;
            Radius = radius;
        }
    }
}
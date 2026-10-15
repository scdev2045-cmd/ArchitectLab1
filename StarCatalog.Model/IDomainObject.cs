using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog.Core
{
    /// <summary>
    /// Базовый интерфейс для всех сущностей предметной области, имеющих уникальный идентификатор.
    /// </summary>
    public interface IDomainObject
    {
        /// <summary>
        /// Уникальный идентификатор объекта в базе данных.
        /// </summary>
        int Id { get; set; }
    }
}

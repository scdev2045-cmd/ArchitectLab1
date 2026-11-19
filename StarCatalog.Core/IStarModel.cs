using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog.Core
{
    /// <summary>
    /// Интерфейс-фасад слоя модели и бизнес-логики (Model + BL) для архитектуры MVP.
    /// Позволяет презентеру запускать операции бизнес-логики и получать результаты через события.
    /// </summary>
    public interface IStarModel
    {
        /// <summary>
        /// Возникает после загрузки или обновления списка звезд.
        /// Передает актуальный список доменных объектов звезд.
        /// </summary>
        event Action<List<Star>> StarsListChanged;

        /// <summary>
        /// Возникает после успешного поиска звезды по идентификатору для редактирования.
        /// Передает найденный доменный объект звезды.
        /// </summary>
        event Action<Star> StarFoundForEdit;

        /// <summary>
        /// Возникает после завершения расчета среднего радиуса звезд каталога.
        /// Передает рассчитанное значение среднего радиуса.
        /// </summary>
        event Action<double> AverageRadiusCalculated;

        /// <summary>
        /// Возникает при успешном выполнении операции изменения данных (добавление, обновление, удаление).
        /// Передает текстовое сообщение о результате операции.
        /// </summary>
        event Action<string> OperationSucceeded;

        /// <summary>
        /// Возникает при ошибке валидации, отсутствии записи или сбое операции.
        /// Передает описание ошибки.
        /// </summary>
        event Action<string> OperationFailed;

        /// <summary>
        /// Запускает загрузку всех звезд каталога.
        /// Результат передается подписчикам через событие StarsListChanged.
        /// </summary>
        void LoadAllStars();

        /// <summary>
        /// Запускает добавление новой звезды в каталог.
        /// Результат передается через события OperationSucceeded / OperationFailed и StarsListChanged.
        /// </summary>
        /// <param name="name">Название звезды.</param>
        /// <param name="discoverer">Первооткрыватель звезды.</param>
        /// <param name="starType">Спектральный тип звезды.</param>
        /// <param name="radius">Радиус звезды в радиусах Солнца.</param>
        void AddStar(string name, string discoverer, string starType, double radius);

        /// <summary>
        /// Запускает поиск звезды по идентификатору для последующего редактирования.
        /// Результат передается через событие StarFoundForEdit или OperationFailed.
        /// </summary>
        /// <param name="id">Идентификатор искомой звезды.</param>
        void FindStarById(int id);

        /// <summary>
        /// Запускает обновление характеристик существующей звезды.
        /// Результат передается через события OperationSucceeded / OperationFailed и StarsListChanged.
        /// </summary>
        /// <param name="id">Идентификатор изменяемой звезды.</param>
        /// <param name="newName">Новое название звезды.</param>
        /// <param name="newDiscoverer">Новый первооткрыватель.</param>
        /// <param name="newStarType">Новый спектральный тип.</param>
        /// <param name="newRadius">Новый радиус звезды.</param>
        void UpdateStar(int id, string newName, string newDiscoverer, string newStarType, double newRadius);

        /// <summary>
        /// Запускает удаление звезды по идентификатору.
        /// Результат передается через события OperationSucceeded / OperationFailed и StarsListChanged.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой звезды.</param>
        void DeleteStar(int id);

        /// <summary>
        /// Запускает фильтрацию звезд по указанному спектральному типу.
        /// Результат передается через событие StarsListChanged.
        /// </summary>
        /// <param name="starType">Искомый спектральный тип звезды.</param>
        void FilterStarsByType(string starType);

        /// <summary>
        /// Запускает расчет среднего радиуса всех звезд каталога.
        /// Результат передается через событие AverageRadiusCalculated.
        /// </summary>
        void CalculateAverageRadius();
    }
}

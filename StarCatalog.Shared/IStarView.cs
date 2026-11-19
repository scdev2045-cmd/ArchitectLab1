using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog.Shared
{
    /// <summary>
    /// Контракт представления (View) каталога звезд в архитектуре MVP.
    /// Объединяет события пользовательских действий и методы отображения данных.
    /// Реализация может быть любой (WinForms, Console и т.д.).
    /// </summary>
    public interface IStarView
    {
        /// <summary>
        /// Возникает при первичной загрузке представления или запросе полного списка звезд.
        /// </summary>
        event Action LoadStarsRequested;

        /// <summary>
        /// Возникает при запросе добавления новой звезды.
        /// Передает DTO с введенными пользователем характеристиками.
        /// </summary>
        event Action<StarDto> AddStarRequested;

        /// <summary>
        /// Возникает при запросе поиска звезды по идентификатору перед редактированием.
        /// Передает идентификатор выбранной звезды.
        /// </summary>
        event Action<int> FindStarForEditRequested;

        /// <summary>
        /// Возникает при запросе сохранения измененных характеристик существующей звезды.
        /// Передает DTO с обновленными данными и идентификатором записи.
        /// </summary>
        event Action<StarDto> UpdateStarRequested;

        /// <summary>
        /// Возникает при запросе удаления звезды по ее идентификатору.
        /// Передает идентификатор удаляемой записи.
        /// </summary>
        event Action<int> DeleteStarRequested;

        /// <summary>
        /// Возникает при запросе фильтрации звезд по указанному спектральному типу.
        /// Передает название искомого типа звезды.
        /// </summary>
        event Action<string> FilterByTypeRequested;

        /// <summary>
        /// Возникает при запросе сброса фильтра и возврата к полному списку звезд.
        /// </summary>
        event Action ResetFilterRequested;

        /// <summary>
        /// Возникает при запросе расчета среднего радиуса всех звезд каталога.
        /// </summary>
        event Action CalculateAverageRadiusRequested;

        /// <summary>
        /// Отображает переданный список звезд в пользовательском интерфейсе.
        /// </summary>
        /// <param name="stars">Список DTO звезд для отображения.</param>
        void ShowStars(List<StarDto> stars);

        /// <summary>
        /// Передает в представление найденную звезду для начала процесса редактирования.
        /// </summary>
        /// <param name="star">DTO редактируемой звезды.</param>
        void ShowStarForEdit(StarDto star);

        /// <summary>
        /// Отображает рассчитанное значение среднего радиуса звезд.
        /// </summary>
        /// <param name="averageRadius">Средний радиус в радиусах Солнца.</param>
        void ShowAverageRadius(double averageRadius);

        /// <summary>
        /// Отображает информационное сообщение об успешном выполнении операции.
        /// </summary>
        /// <param name="message">Текст информационного сообщения.</param>
        void ShowMessage(string message);

        /// <summary>
        /// Отображает сообщение об ошибке или некорректном вводе.
        /// </summary>
        /// <param name="errorMessage">Текст сообщения об ошибке.</param>
        void ShowError(string errorMessage);
    }
}
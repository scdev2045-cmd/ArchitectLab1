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
    /// Презентер каталога звезд в архитектуре MVP.
    /// Работает исключительно через абстракции IStarView и IStarModel (DIP),
    /// связывая слои представления и бизнес-логики посредством событий.
    /// </summary>
    public class StarCatalogPresenter : IDisposable
    {
        /// <summary>
        /// Абстракция представления (WinForms, Console и др.).
        /// </summary>
        private readonly IStarView _view;

        /// <summary>
        /// Абстракция модели и бизнес-логики.
        /// </summary>
        private readonly IStarModel _model;

        /// <summary>
        /// Флаг, показывающий, были ли уже освобождены подписки на события.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// Инициализирует презентер и подписывает его на события представления и модели.
        /// </summary>
        /// <param name="view">Представление, реализующее IStarView.</param>
        /// <param name="model">Модель, реализующая IStarModel.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если view или model равны null.</exception>
        public StarCatalogPresenter(IStarView view, IStarModel model)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view), "Представление не может быть null.");
            }

            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Модель не может быть null.");
            }

            _view = view;
            _model = model;
            _isDisposed = false;

            SubscribeToViewEvents();
            SubscribeToModelEvents();
        }

        /// <summary>
        /// Подписывает методы презентера на все события представления.
        /// </summary>
        private void SubscribeToViewEvents()
        {
            _view.LoadStarsRequested += OnViewLoadStarsRequested;
            _view.AddStarRequested += OnViewAddStarRequested;
            _view.FindStarForEditRequested += OnViewFindStarForEditRequested;
            _view.UpdateStarRequested += OnViewUpdateStarRequested;
            _view.DeleteStarRequested += OnViewDeleteStarRequested;
            _view.FilterByTypeRequested += OnViewFilterByTypeRequested;
            _view.ResetFilterRequested += OnViewResetFilterRequested;
            _view.CalculateAverageRadiusRequested += OnViewCalculateAverageRadiusRequested;
        }

        /// <summary>
        /// Подписывает методы презентера на все события модели.
        /// </summary>
        private void SubscribeToModelEvents()
        {
            _model.StarsListChanged += OnModelStarsListChanged;
            _model.StarFoundForEdit += OnModelStarFoundForEdit;
            _model.AverageRadiusCalculated += OnModelAverageRadiusCalculated;
            _model.OperationSucceeded += OnModelOperationSucceeded;
            _model.OperationFailed += OnModelOperationFailed;
        }



        /// <summary>
        /// Обрабатывает запрос представления на загрузку всех звезд.
        /// </summary>
        private void OnViewLoadStarsRequested()
        {
            _model.LoadAllStars();
        }

        /// <summary>
        /// Обрабатывает запрос представления на добавление новой звезды.
        /// </summary>
        /// <param name="starDto">DTO новой звезды, полученный от представления.</param>
        private void OnViewAddStarRequested(StarDto starDto)
        {
            if (starDto == null)
            {
                _view.ShowError("Не переданы данные новой звезды.");
                return;
            }

            _model.AddStar(starDto.Name, starDto.Discoverer, starDto.StarType, starDto.Radius);
        }

        /// <summary>
        /// Обрабатывает запрос представления на поиск звезды по ID перед редактированием.
        /// </summary>
        /// <param name="id">Идентификатор редактируемой звезды.</param>
        private void OnViewFindStarForEditRequested(int id)
        {
            _model.FindStarById(id);
        }

        /// <summary>
        /// Обрабатывает запрос представления на сохранение изменений существующей звезды.
        /// </summary>
        /// <param name="starDto">DTO с измененными данными звезды.</param>
        private void OnViewUpdateStarRequested(StarDto starDto)
        {
            if (starDto == null)
            {
                _view.ShowError("Не переданы данные для обновления звезды.");
                return;
            }

            _model.UpdateStar(starDto.Id, starDto.Name, starDto.Discoverer, starDto.StarType, starDto.Radius);
        }

        /// <summary>
        /// Обрабатывает запрос представления на удаление звезды по ID.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой звезды.</param>
        private void OnViewDeleteStarRequested(int id)
        {
            _model.DeleteStar(id);
        }

        /// <summary>
        /// Обрабатывает запрос представления на фильтрацию звезд по спектральному типу.
        /// </summary>
        /// <param name="starType">Выбранный пользователем спектральный тип.</param>
        private void OnViewFilterByTypeRequested(string starType)
        {
            _model.FilterStarsByType(starType);
        }

        /// <summary>
        /// Обрабатывает запрос представления на сброс фильтра.
        /// </summary>
        private void OnViewResetFilterRequested()
        {
            _model.LoadAllStars();
        }

        /// <summary>
        /// Обрабатывает запрос представления на расчет среднего радиуса звезд.
        /// </summary>
        private void OnViewCalculateAverageRadiusRequested()
        {
            _model.CalculateAverageRadius();
        }



        /// <summary>
        /// Обрабатывает событие модели об обновлении списка звезд и перерисовывает представление.
        /// </summary>
        /// <param name="stars">Актуальный список доменных объектов звезд.</param>
        private void OnModelStarsListChanged(List<Star> stars)
        {
            List<StarDto> dtoList = StarMapper.ToDtoList(stars);

            _view.ShowStars(dtoList);
        }

        /// <summary>
        /// Обрабатывает событие модели о найденной для редактирования звезде.
        /// </summary>
        /// <param name="star">Найденный доменный объект звезды.</param>
        private void OnModelStarFoundForEdit(Star star)
        {
            StarDto dto = StarMapper.ToDto(star);

            _view.ShowStarForEdit(dto);
        }

        /// <summary>
        /// Обрабатывает событие модели о завершении расчета среднего радиуса.
        /// </summary>
        /// <param name="averageRadius">Вычисленный средний радиус.</param>
        private void OnModelAverageRadiusCalculated(double averageRadius)
        {
            _view.ShowAverageRadius(averageRadius);
        }

        /// <summary>
        /// Обрабатывает событие модели об успешном выполнении операции.
        /// </summary>
        /// <param name="message">Текст информационного сообщения.</param>
        private void OnModelOperationSucceeded(string message)
        {
            _view.ShowMessage(message);
        }

        /// <summary>
        /// Обрабатывает событие модели об ошибке при выполнении операции.
        /// </summary>
        /// <param name="errorMessage">Текст сообщения об ошибке.</param>
        private void OnModelOperationFailed(string errorMessage)
        {
            _view.ShowError(errorMessage);
        }

        /// <summary>
        /// Отписывает презентер от всех событий представления и модели,
        /// предотвращая утечки памяти после закрытия представления.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _view.LoadStarsRequested -= OnViewLoadStarsRequested;
            _view.AddStarRequested -= OnViewAddStarRequested;
            _view.FindStarForEditRequested -= OnViewFindStarForEditRequested;
            _view.UpdateStarRequested -= OnViewUpdateStarRequested;
            _view.DeleteStarRequested -= OnViewDeleteStarRequested;
            _view.FilterByTypeRequested -= OnViewFilterByTypeRequested;
            _view.ResetFilterRequested -= OnViewResetFilterRequested;
            _view.CalculateAverageRadiusRequested -= OnViewCalculateAverageRadiusRequested;


            _model.StarsListChanged -= OnModelStarsListChanged;
            _model.StarFoundForEdit -= OnModelStarFoundForEdit;
            _model.AverageRadiusCalculated -= OnModelAverageRadiusCalculated;
            _model.OperationSucceeded -= OnModelOperationSucceeded;
            _model.OperationFailed -= OnModelOperationFailed;

            _isDisposed = true;
        }
    }
}

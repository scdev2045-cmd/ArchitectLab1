using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog.Core
{
    /// <summary>
    /// Событийный фасад слоя Model + BL для архитектуры MVP.
    /// Делегирует выполнение бизнес-логики объекту ILogic (сохраняя OCP и SRP)
    /// и уведомляет подписчиков (Presenter) о проделанной работе через события.
    /// </summary>
    public class StarModel : IStarModel
    {
        /// <summary>
        /// Ссылка на интерфейс бизнес-логики, внедряемая через DI-контейнер.
        /// </summary>
        private readonly ILogic _logic;

        /// <summary>
        /// Событие обновления текущего списка звезд.
        /// </summary>
        public event Action<List<Star>> StarsListChanged;

        /// <summary>
        /// Событие успешного нахождения звезды для редактирования.
        /// </summary>
        public event Action<Star> StarFoundForEdit;

        /// <summary>
        /// Событие завершения расчета среднего радиуса.
        /// </summary>
        public event Action<double> AverageRadiusCalculated;

        /// <summary>
        /// Событие успешного завершения операции модификации данных.
        /// </summary>
        public event Action<string> OperationSucceeded;

        /// <summary>
        /// Событие возникновения ошибки валидации или выполнения операции.
        /// </summary>
        public event Action<string> OperationFailed;

        /// <summary>
        /// Инициализирует модель-фасад с внедрением бизнес-логики через интерфейс (DIP).
        /// </summary>
        /// <param name="logic">Реализация интерфейса бизнес-логики ILogic.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если передан null.</exception>
        public StarModel(ILogic logic)
        {
            if (logic == null)
            {
                throw new ArgumentNullException(nameof(logic), "Бизнес-логика не может быть null.");
            }

            _logic = logic;
        }

        /// <summary>
        /// Загружает полный список звезд из бизнес-логики и оповещает подписчиков.
        /// </summary>
        public void LoadAllStars()
        {
            try
            {
                List<Star> stars = _logic.GetAllStars();

                RaiseStarsListChanged(stars);
            }
            catch (Exception ex)
            {
                RaiseOperationFailed("Ошибка при загрузке списка звезд: " + ex.Message);
            }
        }

        
        /// <summary>
        /// Выполняет добавление новой звезды и уведомляет подписчиков о результате.
        /// </summary>
        /// <param name="name">Название звезды.</param>
        /// <param name="discoverer">Первооткрыватель звезды.</param>
        /// <param name="starType">Спектральный тип звезды.</param>
        /// <param name="radius">Радиус звезды в радиусах Солнца.</param>
        public void AddStar(string name, string discoverer, string starType, double radius)
        {
            try
            {
                bool added = _logic.AddStar(name, discoverer, starType, radius);

                if (added)
                {
                    RaiseOperationSucceeded("Звезда добавлена.");

                    List<Star> updatedList = _logic.GetAllStars();
                    RaiseStarsListChanged(updatedList);
                }
                else
                {
                    RaiseOperationFailed("Данные звезды заполнены неправильно.");
                }
            }
            catch (Exception ex)
            {
                RaiseOperationFailed("Ошибка при добавлении звезды: " + ex.Message);
            }
        }


        /// <summary>
        /// Ищет звезду по идентификатору и уведомляет подписчиков о результате поиска.
        /// </summary>
        /// <param name="id">Идентификатор искомой звезды.</param>
        public void FindStarById(int id)
        {
            try
            {
                Star star = _logic.FindById(id);

                if (star == null)
                {
                    RaiseOperationFailed("Звезда с ID " + id + " не найдена.");
                    return;
                }

                RaiseStarFoundForEdit(star);
            }
            catch (Exception ex)
            {
                RaiseOperationFailed("Ошибка при поиске звезды: " + ex.Message);
            }
        }

        /// <summary>
        /// Обновляет характеристики существующей звезды и уведомляет подписчиков.
        /// </summary>
        /// <param name="id">Идентификатор изменяемой звезды.</param>
        /// <param name="newName">Новое название звезды.</param>
        /// <param name="newDiscoverer">Новый первооткрыватель.</param>
        /// <param name="newStarType">Новый спектральный тип.</param>
        /// <param name="newRadius">Новый радиус звезды.</param>
        public void UpdateStar(int id, string newName, string newDiscoverer, string newStarType, double newRadius)
        {
            try
            {
                bool updated = _logic.UpdateStar(id, newName, newDiscoverer, newStarType, newRadius);

                if (updated)
                {
                    RaiseOperationSucceeded("Данные звезды успешно обновлены.");

                    List<Star> updatedList = _logic.GetAllStars();
                    RaiseStarsListChanged(updatedList);
                }
                else
                {
                    RaiseOperationFailed("Не удалось изменить данные звезды. Проверьте корректность полей.");
                }
            }
            catch (Exception ex)
            {
                RaiseOperationFailed("Ошибка при изменении звезды: " + ex.Message);
            }
        }

        /// <summary>
        /// Удаляет звезду по идентификатору и уведомляет подписчиков о результате.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой звезды.</param>
        public void DeleteStar(int id)
        {
            try
            {
                bool deleted = _logic.DeleteStar(id);

                if (deleted)
                {
                    RaiseOperationSucceeded("Звезда с ID " + id + " была удалена.");

                    List<Star> updatedList = _logic.GetAllStars();
                    RaiseStarsListChanged(updatedList);
                }
                else
                {
                    RaiseOperationFailed("Звезда с ID " + id + " не найдена.");
                }
            }
            catch (Exception ex)
            {
                RaiseOperationFailed("Ошибка при удалении звезды: " + ex.Message);
            }
        }


        /// <summary>
        /// Фильтрует звезды по спектральному типу и передает результат через событие.
        /// </summary>
        /// <param name="starType">Искомый спектральный тип звезды.</param>
        public void FilterStarsByType(string starType)
        {
            try
            {
                List<Star> filteredStars = _logic.GetStarsByType(starType);

                RaiseStarsListChanged(filteredStars);
            }
            catch (Exception ex)
            {
                RaiseOperationFailed("Ошибка при фильтрации звезд: " + ex.Message);
            }
        }


        /// <summary>
        /// Рассчитывает средний радиус звезд каталога и передает результат через событие.
        /// </summary>
        public void CalculateAverageRadius()
        {
            try
            {
                double averageRadius = _logic.CalculateAverageRadius();

                RaiseAverageRadiusCalculated(averageRadius);
            }
            catch (Exception ex)
            {
                RaiseOperationFailed("Ошибка при расчете среднего радиуса: " + ex.Message);
            }
        }


        /// <summary>
        /// Безопасно вызывает событие StarsListChanged, если на него есть подписчики.
        /// </summary>
        /// <param name="stars">Список звезд для передачи подписчикам.</param>
        private void RaiseStarsListChanged(List<Star> stars)
        {
            Action<List<Star>> handler = StarsListChanged;
            if (handler != null)
            {
                handler(stars);
            }
        }

        /// <summary>
        /// Безопасно вызывает событие StarFoundForEdit, если на него есть подписчики.
        /// </summary>
        /// <param name="star">Найденная звезда.</param>
        private void RaiseStarFoundForEdit(Star star)
        {
            Action<Star> handler = StarFoundForEdit;
            if (handler != null)
            {
                handler(star);
            }
        }

        /// <summary>
        /// Безопасно вызывает событие AverageRadiusCalculated, если на него есть подписчики.
        /// </summary>
        /// <param name="averageRadius">Рассчитанный средний радиус.</param>
        private void RaiseAverageRadiusCalculated(double averageRadius)
        {
            Action<double> handler = AverageRadiusCalculated;
            if (handler != null)
            {
                handler(averageRadius);
            }
        }

        /// <summary>
        /// Безопасно вызывает событие OperationSucceeded, если на него есть подписчики.
        /// </summary>
        /// <param name="message">Текст сообщения об успехе.</param>
        private void RaiseOperationSucceeded(string message)
        {
            Action<string> handler = OperationSucceeded;
            if (handler != null)
            {
                handler(message);
            }
        }

        /// <summary>
        /// Безопасно вызывает событие OperationFailed, если на него есть подписчики.
        /// </summary>
        /// <param name="errorMessage">Текст сообщения об ошибке.</param>
        private void RaiseOperationFailed(string errorMessage)
        {
            Action<string> handler = OperationFailed;
            if (handler != null)
            {
                handler(errorMessage);
            }
        }
    }
}
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
        /// Загружает полный список звезд и передает его подписчикам через событие.
        /// </summary>
        public void LoadAllStars()
        {
            List<Star> stars;

            try
            {
                stars = _logic.GetAllStars();
            }
            catch (Exception ex)
            {
                RaiseOperationFailed("Ошибка при загрузке списка звезд: " + ex.Message);
                return;
            }

            RaiseStarsListChanged(stars);
        }


        /// <summary>
        /// Добавляет новую звезду, сообщает результат операции
        /// и после успешного сохранения запрашивает обновление списка.
        /// </summary>
        /// <param name="name">Название звезды.</param>
        /// <param name="discoverer">Сведения о первооткрывателе.</param>
        /// <param name="starType">Поддерживаемый тип звезды.</param>
        /// <param name="radius">Положительный конечный радиус в радиусах Солнца.</param>
        public void AddStar(string name, string discoverer, string starType, double radius)
        {
            ExecuteChange(
                () => _logic.AddStar(name, discoverer, starType, radius),
                "Звезда добавлена.",
                "Данные звезды заполнены неправильно.",
                "Ошибка при добавлении звезды");
        }


        /// <summary>
        /// Ищет звезду для редактирования и сообщает результат через событие.
        /// </summary>
        /// <param name="id">Идентификатор искомой звезды.</param>
        public void FindStarById(int id)
        {
            Star star;

            try
            {
                star = _logic.FindById(id);
            }
            catch (Exception ex)
            {
                RaiseOperationFailed("Ошибка при поиске звезды: " + ex.Message);
                return;
            }

            if (star == null)
            {
                RaiseOperationFailed("Звезда с ID " + id + " не найдена.");
                return;
            }

            RaiseStarFoundForEdit(star);
        }

        /// <summary>
        /// Обновляет характеристики существующей звезды, сообщает результат
        /// операции и после успешного сохранения запрашивает обновление списка.
        /// </summary>
        /// <param name="id">Идентификатор изменяемой звезды.</param>
        /// <param name="newName">Новое название звезды.</param>
        /// <param name="newDiscoverer">Новые сведения о первооткрывателе.</param>
        /// <param name="newStarType">Новый поддерживаемый тип звезды.</param>
        /// <param name="newRadius">Новый положительный конечный радиус в радиусах Солнца.</param>
        public void UpdateStar(
            int id, string newName, string newDiscoverer, string newStarType, double newRadius)
        {
            ExecuteChange(
                () => _logic.UpdateStar(id, newName, newDiscoverer, newStarType, newRadius),
                "Данные звезды успешно обновлены.",
                "Не удалось изменить данные: проверьте поля и наличие звезды в каталоге.",
                "Ошибка при изменении звезды");
        }

        /// <summary>
        /// Удаляет звезду по идентификатору, сообщает результат операции
        /// и после успешного удаления запрашивает обновление списка.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой звезды.</param>
        public void DeleteStar(int id)
        {
            ExecuteChange(
                () => _logic.DeleteStar(id),
                "Звезда с ID " + id + " была удалена.",
                "Звезда с ID " + id + " не найдена.",
                "Ошибка при удалении звезды");
        }


        /// <summary>
        /// Фильтрует звезды по типу и передает полученный список через событие.
        /// </summary>
        /// <param name="starType">Искомый тип звезды.</param>
        public void FilterStarsByType(string starType)
        {
            List<Star> stars;

            try
            {
                stars = _logic.GetStarsByType(starType);
            }
            catch (Exception ex)
            {
                RaiseOperationFailed("Ошибка при фильтрации звезд: " + ex.Message);
                return;
            }

            RaiseStarsListChanged(stars);
        }


        /// <summary>
        /// Рассчитывает средний радиус звезд каталога и передает результат через событие.
        /// </summary>
        public void CalculateAverageRadius()
        {
            double averageRadius;

            try
            {
                averageRadius = _logic.CalculateAverageRadius();
            }
            catch (Exception ex)
            {
                RaiseOperationFailed("Ошибка при расчете среднего радиуса: " + ex.Message);
                return;
            }

            RaiseAverageRadiusCalculated(averageRadius);
        }



        /// <summary>
        /// Выполняет операцию изменения каталога и уведомляет подписчиков о её результате.
        /// После успеха отдельно загружает актуальный список.
        /// </summary>
        /// <param name="change">
        /// Операция бизнес-логики, возвращающая true при успехе и false при отказе.
        /// </param>
        /// <param name="successMessage">Сообщение об успешном изменении.</param>
        /// <param name="rejectedMessage">Сообщение для случая, когда операция вернула false.</param>
        /// <param name="errorPrefix">Начало сообщения об исключении при выполнении операции.</param>
        private void ExecuteChange(
            Func<bool> change, string successMessage, string rejectedMessage, string errorPrefix)
        {
            bool changed;

            try
            {
                changed = change();
            }
            catch (Exception ex)
            {
                RaiseOperationFailed(errorPrefix + ": " + ex.Message);
                return;
            }

            if (!changed)
            {
                RaiseOperationFailed(rejectedMessage);
                return;
            }

            RaiseOperationSucceeded(successMessage);

            RefreshStarsAfterChange();
        }


        /// <summary>
        /// Загружает актуальный список после успешного изменения каталога.
        /// При сбое чтения сообщает, что изменение уже выполнено.
        /// </summary>
        private void RefreshStarsAfterChange()
        {
            List<Star> stars;

            try
            {
                stars = _logic.GetAllStars();
            }
            catch (Exception ex)
            {
                RaiseOperationFailed(
                    "Изменение сохранено, но не удалось обновить список звезд. "
                    + "Запросите список заново. Причина: " + ex.Message);
                return;
            }

            RaiseStarsListChanged(stars);
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
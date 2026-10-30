using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarCatalog.DataAccessLayer;

namespace StarCatalog.Core
{
    /// <summary>
    /// Предоставляет бизнес-логику для работы с каталогом звезд.
    /// Реализует контракт ILogic и зависит от абстракции хранилища IRepository (DIP).
    /// </summary>
    public class Logic : ILogic
    {
        /// <summary>
        /// Абстрактное хранилище сущностей звезд.
        /// </summary>
        private readonly IRepository<Star> _repository;

        /// <summary>
        /// Сервис выполнения аналитических расчетов над звездами (SRP).
        /// </summary>
        private readonly IStarAnalyticsService _analyticsService;

        /// <summary>
        /// Инициализирует новый экземпляр класса бизнес-логики с внедрением зависимостей.
        /// </summary>
        /// <param name="repository">Экземпляр репозитория, реализующий IRepository.</param>
        /// <param name="analyticsService">Сервис аналитических вычислений.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если передан null.</exception>
        public Logic(IRepository<Star> repository, IStarAnalyticsService analyticsService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository), "Репозиторий не может быть null.");
            _analyticsService = analyticsService ?? throw new ArgumentNullException(nameof(analyticsService), "Сервис аналитики не может быть null.");
        }

        /// <summary>
        /// Перегруженный конструктор для обратной совместимости по OCP.
        /// </summary>
        /// <param name="repository">Экземпляр репозитория.</param>
        public Logic(IRepository<Star> repository) : this(repository, new StarAnalyticsService())
        {
        }

        /// <summary>
        /// Добавляет звезду в каталог после валидации входных характеристик.
        /// </summary>
        /// <param name="name">Название звезды.</param>
        /// <param name="discoverer">Сведения о первооткрывателе.</param>
        /// <param name="starType">Поддерживаемый тип звезды.</param>
        /// <param name="radius">Положительный радиус в радиусах Солнца.</param>
        /// <returns>true, если запись успешно валидирована и сохранена; иначе false.</returns>
        public bool AddStar(string name, string discoverer, string starType, double radius)
        {
            if (!IsStarDataCorrect(name, discoverer, starType, radius))
            {
                return false;
            }

            Star newStar = new Star(
                0,
                name.Trim(),
                discoverer.Trim(),
                starType.Trim(),
                radius);

            _repository.Add(newStar);
            return true;
        }

        /// <summary>
        /// Возвращает все звезды из каталога, отсортированные по возрастанию идентификатора.
        /// </summary>
        /// <returns>Список звезд или пустой список, если записи отсутствуют.</returns>
        public List<Star> GetAllStars()
        {
            List<Star> stars = _repository.ReadAll();
            if (stars == null)
            {
                return new List<Star>();
            }

            stars.Sort((a, b) => a.Id.CompareTo(b.Id));
            return stars;
        }

        /// <summary>
        /// Находит звезду по ее уникальному идентификатору.
        /// </summary>
        /// <param name="id">Положительный идентификатор записи.</param>
        /// <returns>Найденный объект звезды или null, если идентификатор некорректен или запись отсутствует.</returns>
        public Star FindById(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            return _repository.ReadById(id);
        }

        /// <summary>
        /// Удаляет существующую звезду по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой записи.</param>
        /// <returns>true, если запись найдена и удалена; иначе false.</returns>
        public bool DeleteStar(int id)
        {
            if (id <= 0)
            {
                return false;
            }

            Star existingStar = _repository.ReadById(id);
            if (existingStar == null)
            {
                return false;
            }

            _repository.Delete(id);
            return true;
        }

        /// <summary>
        /// Изменяет характеристики существующей звезды в каталоге.
        /// </summary>
        /// <param name="id">Идентификатор изменяемой записи.</param>
        /// <param name="newName">Новое название звезды.</param>
        /// <param name="newDiscoverer">Новые сведения о первооткрывателе.</param>
        /// <param name="newStarType">Новый тип звезды.</param>
        /// <param name="newRadius">Новый радиус звезды.</param>
        /// <returns>true, если операция успешно завершена; иначе false.</returns>
        public bool UpdateStar(int id, string newName, string newDiscoverer, string newStarType, double newRadius)
        {
            if (id <= 0)
            {
                return false;
            }

            if (!IsStarDataCorrect(newName, newDiscoverer, newStarType, newRadius))
            {
                return false;
            }

            Star existingStar = _repository.ReadById(id);
            if (existingStar == null)
            {
                return false;
            }

            Star updatedStar = new Star(
                id,
                newName.Trim(),
                newDiscoverer.Trim(),
                newStarType.Trim(),
                newRadius);

            _repository.Update(updatedStar);
            return true;
        }

        /// <summary>
        /// Возвращает список звезд указанного спектрального типа с делегированием сервису аналитики (SRP).
        /// </summary>
        /// <param name="starType">Искомый тип звезды.</param>
        /// <returns>Список найденных звезд или пустой список.</returns>
        public List<Star> GetStarsByType(string starType)
        {
            List<Star> stars = GetAllStars();
            return _analyticsService.FilterByType(stars, starType);
        }

        /// <summary>
        /// Рассчитывает средний радиус всех зарегистрированных звезд каталога с делегированием сервису аналитики (SRP).
        /// </summary>
        /// <returns>Средний радиус или 0, если каталог пуст.</returns>
        public double CalculateAverageRadius()
        {
            List<Star> stars = _repository.ReadAll();
            return _analyticsService.CalculateAverageRadius(stars);
        }

        /// <summary>
        /// Выполняет внутреннюю валидацию значений атрибутов звезды.
        /// </summary>
        /// <param name="name">Проверяемое название звезды.</param>
        /// <param name="discoverer">Проверяемые сведения о первооткрывателе.</param>
        /// <param name="starType">Проверяемый спектральный тип звезды.</param>
        /// <param name="radius">Проверяемый радиус звезды.</param>
        /// <returns>true, если все поля удовлетворяют критериям валидации; иначе false.</returns>
        private bool IsStarDataCorrect(string name, string discoverer, string starType, double radius)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Trim().Length > 200)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(discoverer) || discoverer.Trim().Length > 200)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(starType))
            {
                return false;
            }

            string checkedType = starType.Trim();
            if (checkedType != "Красный гигант"
                && checkedType != "Желтый карлик"
                && checkedType != "Белый карлик"
                && checkedType != "Нейтронная звезда")
            {
                return false;
            }

            if (radius <= 0 || double.IsNaN(radius) || double.IsInfinity(radius))
            {
                return false;
            }

            return true;
        }
    }
}
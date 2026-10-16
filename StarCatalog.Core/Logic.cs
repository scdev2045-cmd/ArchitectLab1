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
    /// Использует репозиторий для чтения и сохранения данных.
    /// </summary>
    public class Logic
    {
        private IRepository<Star> repository;

        /// <summary>
        /// Создает бизнес-логику с репозиторием Dapper\EF.
        /// </summary>
        public Logic()
        {
            DatabaseInitializer.EnsureCreated();
            repository = new DapperRepository(DatabaseSettings.ConnectionString);
            // repository = new EntityRepository(DatabaseSettings.ConnectionString);
        }

        /// <summary>
        /// Добавляет звезду в базу данных после проверки характеристик.
        /// </summary>
        /// <param name="name">Название звезды.</param>
        /// <param name="discoverer">Сведения о первооткрывателе.</param>
        /// <param name="starType">Один из поддерживаемых типов звезды.</param>
        /// <param name="radius">
        /// Положительный конечный радиус в радиусах Солнца.
        /// </param>
        /// <returns>
        /// true, если запись добавлена;
        /// false, если введенные характеристики некорректны.
        /// </returns>
        public bool AddStar(
            string name,
            string discoverer,
            string starType,
            double radius)
        {
            if (!IsStarDataCorrect(
                name,
                discoverer,
                starType,
                radius))
            {
                return false;
            }

            Star newStar = new Star(
                0,
                name.Trim(),
                discoverer.Trim(),
                starType.Trim(),
                radius);

            repository.Add(newStar);

            return true;
        }

        /// <summary>
        /// Возвращает все звезды по возрастанию идентификатора.
        /// </summary>
        /// <returns>Список звезд или пустой список, если записей нет</returns>
        public List<Star> GetAllStars()
        {
            List<Star> stars = repository.ReadAll();

            stars.Sort((a, b) => a.Id.CompareTo(b.Id));

            return stars;
        }

        /// <summary>
        /// Находит звезду по идентификатору.
        /// </summary>
        /// <param name="id">Положительный идентификатор записи.</param>
        /// <returns>
        /// Найденная звезда или null, если идентификатор некорректен
        /// либо запись отсутствует.
        /// </returns>
        public Star FindById(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            return repository.ReadById(id);
        }

        /// <summary>
        /// Удаляет существующую звезду по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой записи.</param>
        /// <returns>
        /// true, если запись найдена и операция удаления завершена;
        /// false, если идентификатор некорректен или запись не найдена.
        /// </returns>
        public bool DeleteStar(int id)
        {
            if (id <= 0)
            {
                return false;
            }

            Star existingStar = repository.ReadById(id);

            if (existingStar == null)
            {
                return false;
            }

            repository.Delete(id);

            return true;
        }

        private bool IsStarDataCorrect(
            string name,
            string discoverer,
            string starType,
            double radius)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(discoverer))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(starType))
            {
                return false;
            }

            if (name.Trim().Length > 200)
            {
                return false;
            }

            if (discoverer.Trim().Length > 200)
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

            if (radius <= 0)
            {
                return false;
            }

            if (double.IsNaN(radius))
            {
                return false;
            }

            if (double.IsInfinity(radius))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Изменяет характеристики существующей звезды.
        /// </summary>
        /// <param name="id">Идентификатор изменяемой записи.</param>
        /// <param name="newName">Новое название звезды.</param>
        /// <param name="newDiscoverer">
        /// Новые сведения о первооткрывателе.
        /// </param>
        /// <param name="newStarType">Новый допустимый тип звезды.</param>
        /// <param name="newRadius">Новый положительный конечный радиус.</param>
        /// <returns>
        /// true, если операция сохранения завершена;
        /// false, если данные некорректны или запись отсутствует.
        /// </returns>
        public bool UpdateStar(
            int id,
            string newName,
            string newDiscoverer,
            string newStarType,
            double newRadius)
        {

            if (id <= 0)
            {
                return false;
            }

            if (!IsStarDataCorrect(
                newName,
                newDiscoverer,
                newStarType,
                newRadius))
            {
                return false;
            }

            Star existingStar = repository.ReadById(id);

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

            repository.Update(updatedStar);

            return true;
        }

        /// <summary>
        /// Возвращает звезды указанного типа по возрастанию Id.
        /// </summary>
        /// <param name="starType">Искомый тип звезды.</param>
        /// <returns>
        /// Список подходящих звезд или пустой список.
        /// </returns>
        public List<Star> GetStarsByType(string starType)
        {
            List<Star> result = new List<Star>();

            if (string.IsNullOrWhiteSpace(starType))
            {
                return result;
            }

            string searchedType = starType.Trim();

            List<Star> stars = GetAllStars();

            for (int i = 0; i < stars.Count; i = i + 1)
            {
                if (string.Equals(
                    stars[i].StarType,
                    searchedType,
                    StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(stars[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// Рассчитывает средний радиус всех звезд каталога.
        /// </summary>
        /// <returns>
        /// Средний радиус или 0, если каталог пуст.
        /// </returns>
        public double CalculateAverageRadius()
        {
            List<Star> stars = repository.ReadAll();

            if (stars.Count == 0)
            {
                return 0;
            }

            double sum = 0;

            for (int i = 0; i < stars.Count; i = i + 1)
            {
                sum = sum + stars[i].Radius;
            }

            return sum / stars.Count;
        }
    }
}
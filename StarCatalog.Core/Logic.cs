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
        // Поле хранит ссылку на объект, реализующий IRepository<Star>.
        //
        // Методы Logic обращаются к общему интерфейсу.
        // Поэтому им не нужно знать SQL-запросы
        // или особенности Entity Framework.
        private IRepository<Star> repository;

        /// <summary>
        /// Создает бизнес-логику с репозиторием Entity Framework.
        /// </summary>
        public Logic()
        {

            // При первом запуске создаем базу и таблицу.
            // При следующих запусках проверяем их наличие,
            // не удаляя существующие записи.
            DatabaseInitializer.EnsureCreated();
            // Создаем реализацию репозитория через Dapper.
            // Строка подключения берется из общего класса настроек.
            //
            // Конструктор без параметров сохраняем,
            // чтобы существующий код new Logic() продолжал работать.
            repository = new DapperRepository<Star>(DatabaseSettings.ConnectionString);
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
            // Сначала проверяем данные.
            // При ошибке до обращения к базе дело не дойдет.
            if (!IsStarDataCorrect(
                name,
                discoverer,
                starType,
                radius))
            {
                return false;
            }

            // Создаем объект новой звезды.
            //
            // Значение Id = 0 означает, что объект новый.
            // Настоящий идентификатор выдаст база при добавлении.
            Star newStar = new Star(
                0,
                name.Trim(),
                discoverer.Trim(),
                starType.Trim(),
                radius);

            // Репозиторий выполняет сохранение в базе.
            repository.Add(newStar);

            // До этой строки дойдем только после завершения Add.
            return true;
        }

        /// <summary>
        /// Возвращает все звезды по возрастанию идентификатора.
        /// </summary>
        /// <returns>Список звезд или пустой список, если записей нет</returns>
        public List<Star> GetAllStars()
        {
            // Получаем записи из базы данных.
            List<Star> stars = repository.ReadAll();

            // Явно сортируем результат на уровне Logic.
            //
            // Это обеспечивает одинаковый порядок независимо от того,
            // какой репозиторий используется: Dapper или EF.
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
            // В нашей таблице идентификаторы положительные.
            if (id <= 0)
            {
                return null;
            }

            // Сам поиск выполняет репозиторий.
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

            // Проверяем наличие записи перед удалением.
            Star existingStar = repository.ReadById(id);

            if (existingStar == null)
            {
                return false;
            }

            // Удаляем запись через выбранную реализацию репозитория.
            repository.Delete(id);

            return true;
        }

        // Общая проверка используется при добавлении и изменении.
        // Это приватный вспомогательный метод,
        // поэтому обязательный summary здесь не нужен.
        private bool IsStarDataCorrect(
            string name,
            string discoverer,
            string starType,
            double radius)
        {
            // Запрещаем null, пустую строку и строку только из пробелов.
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

            // Ограничения длины совпадают с колонками нашей таблицы.
            // Проверяем длину после удаления крайних пробелов.
            if (name.Trim().Length > 200)
            {
                return false;
            }

            if (discoverer.Trim().Length > 200)
            {
                return false;
            }

            // Убираем пробелы по краям перед проверкой типа.
            string checkedType = starType.Trim();

            // Разрешаем только те значения, которые используются
            // в меню, выпадающем списке и ограничении таблицы.
            if (checkedType != "Красный гигант"
                && checkedType != "Желтый карлик"
                && checkedType != "Белый карлик"
                && checkedType != "Нейтронная звезда")
            {
                return false;
            }

            // Радиус должен быть больше нуля.
            if (radius <= 0)
            {
                return false;
            }

            // NaN — специальное значение "не число".
            // Одна проверка radius <= 0 его не отсекает.
            if (double.IsNaN(radius))
            {
                return false;
            }

            // Бесконечность также не является допустимым радиусом
            // для сохранения в нашей таблице.
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
            // Проверяем идентификатор до обращения к базе.
            if (id <= 0)
            {
                return false;
            }

            // Проверяем все новые характеристики.
            // Старые данные пока не изменяются.
            if (!IsStarDataCorrect(
                newName,
                newDiscoverer,
                newStarType,
                newRadius))
            {
                return false;
            }

            // Проверяем, существует ли изменяемая запись.
            Star existingStar = repository.ReadById(id);

            if (existingStar == null)
            {
                return false;
            }

            // Формируем объект с новым состоянием записи.
            // Id сохраняем прежний: обновляем существующую звезду.
            Star updatedStar = new Star(
                id,
                newName.Trim(),
                newDiscoverer.Trim(),
                newStarType.Trim(),
                newRadius);

            // В отличие от версии с List, одного изменения
            // свойств объекта недостаточно.
            // Явно передаем новые данные репозиторию для сохранения.
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
            // Создаем отдельный список результата.
            List<Star> result = new List<Star>();

            // Пустой запрос не должен приводить к ошибке.
            if (string.IsNullOrWhiteSpace(starType))
            {
                return result;
            }

            string searchedType = starType.Trim();

            // Получаем актуальные данные из базы.
            // GetAllStars уже сортирует их по Id.
            List<Star> stars = GetAllStars();

            for (int i = 0; i < stars.Count; i = i + 1)
            {
                // Сравниваем строки без учета регистра букв.
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
            // Для расчета каждый раз читаем актуальный каталог.
            List<Star> stars = repository.ReadAll();

            // Не допускаем деление на ноль.
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
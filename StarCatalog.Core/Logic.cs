using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarCatalog.Core
{
    /// <summary>
    /// Предоставляет бизнес-логику для работы с каталогом звезд.
    /// </summary>
    public class Logic
    {
        /// <summary>
        /// Внутренний список всех звезд в каталоге.
        /// </summary>
        private List<Star> stars;

        /// <summary>
        /// Инициализирует каталог и заполняет его стартовыми астрономическими данными.
        /// </summary>
        public Logic()
        {
            stars = new List<Star>();

            AddStar("Сириус", "Клавдий Птолемей", "Белый карлик", 1.71);
            AddStar("Бетельгейзе", "Джон Гершель", "Красный гигант", 764.0);
            AddStar("Солнце", "Человечество", "Желтый карлик", 1.0);
            AddStar("Альдебаран", "Аль-Суфи", "Красный гигант", 44.2);
        }

        /// <summary>
        /// Находит наименьший положительный свободный номер Id для новой записи.
        /// </summary>
        /// <returns>Первый незанятый целочисленный идентификатор, начиная с 1.</returns>
        private int GetNextAvailableId()
        {
            int candidateId = 1;
            while (true)
            {
                bool isOccupied = false;
                for (int i = 0; i < stars.Count; i = i + 1)
                {
                    if (stars[i].Id == candidateId)
                    {
                        isOccupied = true;
                        break;
                    }
                }

                if (!isOccupied)
                {
                    return candidateId;
                }

                candidateId = candidateId + 1;
            }
        }

        /// <summary>
        /// Создает и регистрирует новую звезду в каталоге.
        /// </summary>
        /// <param name="name">Название звезды.</param>
        /// <param name="discoverer">ФИО первооткрывателя.</param>
        /// <param name="starType">Спектральный тип звезды.</param>
        /// <param name="radius">Радиус звезды в радиусах Солнца.</param>
        public void AddStar(string name, string discoverer, string starType, double radius)
        {
            int freeId = GetNextAvailableId();
            Star newStar = new Star(freeId, name, discoverer, starType, radius);
            stars.Add(newStar);
        }

        /// <summary>
        /// Возвращает список всех звезд, отсортированный по возрастанию Id.
        /// </summary>
        /// <returns>Упорядоченный список объектов Star.</returns>
        public List<Star> GetAllStars()
        {
            // Создаем копию списка
            List<Star> sortedList = new List<Star>(stars);

            // Стандартный метод Sort: сравнивает Id двух звезд (a и b) и сортирует по возрастанию
            sortedList.Sort((a, b) => a.Id.CompareTo(b.Id));

            return sortedList;
        }

        /// <summary>
        /// Осуществляет поиск звезды в каталоге по ее уникальному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор звезды.</param>
        /// <returns>Найденный объект Star или null, если объект не найден.</returns>
        public Star FindById(int id)
        {
            for (int i = 0; i < stars.Count; i = i + 1)
            {
                if (stars[i].Id == id)
                {
                    return stars[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Удаляет звезду из каталога по указанному идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой звезды.</param>
        /// <returns>True, если удаление прошло успешно; иначе False.</returns>
        public bool DeleteStar(int id)
        {
            Star starToDelete = FindById(id);
            if (starToDelete != null)
            {
                stars.Remove(starToDelete);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Обновляет параметры уже существующей звезды.
        /// </summary>
        /// <param name="id">Идентификатор изменяемой звезды.</param>
        /// <param name="newName">Новое название звезды.</param>
        /// <param name="newDiscoverer">Новое ФИО первооткрывателя.</param>
        /// <param name="newStarType">Новый тип звезды.</param>
        /// <param name="newRadius">Новый радиус звезды.</param>
        /// <returns>True, если параметры обновлены; иначе False.</returns>
        public bool UpdateStar(int id, string newName, string newDiscoverer, string newStarType, double newRadius)
        {
            Star starToUpdate = FindById(id);
            if (starToUpdate == null)
            {
                return false;
            }

            starToUpdate.Name = newName;
            starToUpdate.Discoverer = newDiscoverer;
            starToUpdate.StarType = newStarType;
            starToUpdate.Radius = newRadius;
            return true;
        }

        /// <summary>
        /// Бизнес-функция 1: Выполняет фильтрацию каталога по типу звезды.
        /// </summary>
        /// <param name="starType">Искомый тип звезды.</param>
        /// <returns>Список звезд, удовлетворяющих заданному типу.</returns>
        public List<Star> GetStarsByType(string starType)
        {
            List<Star> result = new List<Star>();
            for (int i = 0; i < stars.Count; i = i + 1)
            {
                if (stars[i].StarType.ToLower() == starType.ToLower())
                {
                    result.Add(stars[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// Бизнес-функция 2: Вычисляет средний радиус всех зарегистрированных звезд каталога.
        /// </summary>
        /// <returns>Среднее значение радиуса в единицах R☉ (0, если каталог пуст).</returns>
        public double CalculateAverageRadius()
        {
            if (stars.Count == 0)
            {
                return 0.0;
            }

            double sum = 0.0;
            for (int i = 0; i < stars.Count; i = i + 1)
            {
                sum = sum + stars[i].Radius;
            }

            return sum / stars.Count;
        }
    }
}
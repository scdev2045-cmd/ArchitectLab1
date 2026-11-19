using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarCatalog.Shared;

namespace StarCatalog.ConsoleApp
{
    /// <summary>
    /// Консольное представление каталога звезд в архитектуре MVP.
    /// Реализует тот же интерфейс IStarView, что и Windows Forms (LSP),
    /// взаимодействуя с презентером исключительно через события и DTO.
    /// </summary>
    public class ConsoleStarView : IStarView
    {
        /// <summary>
        /// Событие запроса загрузки полного списка звезд.
        /// </summary>
        public event Action LoadStarsRequested;

        /// <summary>
        /// Событие запроса добавления новой звезды.
        /// </summary>
        public event Action<StarDto> AddStarRequested;

        /// <summary>
        /// Событие запроса поиска звезды по идентификатору для редактирования.
        /// </summary>
        public event Action<int> FindStarForEditRequested;

        /// <summary>
        /// Событие запроса сохранения изменений существующей звезды.
        /// </summary>
        public event Action<StarDto> UpdateStarRequested;

        /// <summary>
        /// Событие запроса удаления звезды по идентификатору.
        /// </summary>
        public event Action<int> DeleteStarRequested;

        /// <summary>
        /// Событие запроса фильтрации звезд по спектральному типу.
        /// </summary>
        public event Action<string> FilterByTypeRequested;

        /// <summary>
        /// Событие запроса сброса фильтра.
        /// </summary>
        public event Action ResetFilterRequested;

        /// <summary>
        /// Событие запроса расчета среднего радиуса звезд.
        /// </summary>
        public event Action CalculateAverageRadiusRequested;

        /// <summary>
        /// Последний список звезд, полученный от презентера.
        /// </summary>
        private List<StarDto> _currentStars = new List<StarDto>();

        /// <summary>
        /// Управляет тем, нужно ли сразу печатать список звезд в консоль при вызове ShowStars.
        /// Это предотвращает лишний вывод таблицы после операций добавления, изменения и удаления.
        /// </summary>
        private bool _printStarsOnUpdate = false;

        /// <summary>
        /// Запускает главный цикл консольного меню.
        /// </summary>
        public void Run()
        {
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine("          КАТАЛОГ АСТРОНОМИЧЕСКИХ ЗВЕЗД           ");
                Console.WriteLine("==================================================");
                Console.WriteLine("  1. Вывести список всех звезд");
                Console.WriteLine("  2. Зарегистрировать новую звезду");
                Console.WriteLine("  3. Удалить звезду по ID");
                Console.WriteLine("  4. Редактировать характеристики звезды");
                Console.WriteLine("  5. Найти звезды по типу");
                Console.WriteLine("  6. Рассчитать средний радиус");
                Console.WriteLine("  0. Выход из программы");
                Console.WriteLine("==================================================");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    ShowAllStars();
                }
                else if (choice == "2")
                {
                    AddNewStar();
                }
                else if (choice == "3")
                {
                    RemoveStar();
                }
                else if (choice == "4")
                {
                    EditStar();
                }
                else if (choice == "5")
                {
                    FilterByType();
                }
                else if (choice == "6")
                {
                    RequestAverageRadius();
                }
                else if (choice == "0")
                {
                    isRunning = false;
                    Console.WriteLine("\nЗавершение работы программы.");
                }
                else
                {
                    Console.WriteLine("\nОшибка: Неизвестная команда!");
                    PauseScreen();
                }
            }
        }


        /// <summary>
        /// Сохраняет переданный презентером список звезд и при необходимости выводит его на экран.
        /// </summary>
        /// <param name="stars">Список DTO звезд.</param>
        public void ShowStars(List<StarDto> stars)
        {
            _currentStars = stars ?? new List<StarDto>();

            if (_printStarsOnUpdate)
            {
                PrintStarsList(_currentStars);
            }
        }

        /// <summary>
        /// Запрашивает у пользователя новые характеристики для найденной звезды
        /// и генерирует событие UpdateStarRequested.
        /// </summary>
        /// <param name="star">DTO найденной звезды.</param>
        public void ShowStarForEdit(StarDto star)
        {
            if (star == null)
            {
                ShowError("Не удалось получить данные выбранной звезды.");
                return;
            }

            Console.WriteLine("\nВыбрана звезда: " + star.Name);
            Console.Write("Новое название: ");
            string name = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine();
                Console.WriteLine("Название не может быть пустым или состоять только из пробелов.");
                Console.Write("Введите новое название еще раз: ");
                name = Console.ReadLine();
            }

            Console.Write("Новый первооткрыватель: ");
            string discoverer = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(discoverer))
            {
                Console.WriteLine();
                Console.WriteLine("Первооткрыватель не может быть пустым или состоять только из пробелов.");
                Console.Write("Введите нового первооткрывателя еще раз: ");
                discoverer = Console.ReadLine();
            }

            string newStarType = ReadStarTypeFromConsole(
                "--- РЕДАКТИРОВАНИЕ ЗВЕЗДЫ ---",
                "Выбрана звезда: " + star.Name);

            Console.Write("Новый радиус (R.sun): ");
            double radius;
            while (!double.TryParse(Console.ReadLine(), out radius) || radius <= 0)
            {
                Console.Write("Некорректное значение. Введите число: ");
            }

            StarDto updatedDto = new StarDto(
                star.Id,
                name.Trim(),
                discoverer.Trim(),
                newStarType,
                radius);

            Action<StarDto> handler = UpdateStarRequested;
            if (handler != null)
            {
                handler(updatedDto);
            }
        }

        /// <summary>
        /// Выводит рассчитанный средний радиус звезд в консоль.
        /// </summary>
        /// <param name="averageRadius">Средний радиус в радиусах Солнца.</param>
        public void ShowAverageRadius(double averageRadius)
        {
            Console.WriteLine("Средний радиус зарегистрированных звезд: " + averageRadius.ToString("F2") + " R.sun");
        }

        /// <summary>
        /// Выводит информационное сообщение в консоль.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        public void ShowMessage(string message)
        {
            Console.WriteLine("\n[Успех] " + message);
        }

        /// <summary>
        /// Выводит сообщение об ошибке в консоль.
        /// </summary>
        /// <param name="errorMessage">Текст ошибки.</param>
        public void ShowError(string errorMessage)
        {
            Console.WriteLine("\n[Ошибка] " + errorMessage);
        }


        /// <summary>
        /// Пункт 1: запрашивает и отображает полный список звезд.
        /// </summary>
        private void ShowAllStars()
        {
            Console.Clear();
            Console.WriteLine("=== СПИСОК ВСЕХ ЗВЕЗД В КАТАЛОГЕ ===\n");

            RequestAndPrintAllStars();

            PauseScreen();
        }

        /// <summary>
        /// Пункт 2: запрашивает данные новой звезды и генерирует событие добавления.
        /// </summary>
        private void AddNewStar()
        {
            Console.Clear();
            Console.WriteLine("--- ДОБАВЛЕНИЕ НОВОЙ ЗВЕЗДЫ ---");

            Console.Write("Введите название звезды: ");
            string name = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("Название не может быть пустым. Повторите ввод: ");
                name = Console.ReadLine();
            }

            Console.Write("Введите ФИО первооткрывателя: ");
            string discoverer = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(discoverer))
            {
                Console.WriteLine();
                Console.WriteLine("ФИО первооткрывателя не может быть пустым.");
                Console.Write("Введите ФИО первооткрывателя еще раз: ");
                discoverer = Console.ReadLine();
            }

            string starType = ReadStarTypeFromConsole(
                "--- ДОБАВЛЕНИЕ НОВОЙ ЗВЕЗДЫ ---",
                "Название: " + name + "\nПервооткрыватель: " + discoverer);

            Console.Clear();
            Console.WriteLine("--- ДОБАВЛЕНИЕ НОВОЙ ЗВЕЗДЫ ---");
            Console.WriteLine("Название: " + name);
            Console.WriteLine("Первооткрыватель: " + discoverer);
            Console.WriteLine("Тип: " + starType);
            Console.WriteLine();

            Console.Write("Введите радиус звезды: ");
            double radius;
            while (!double.TryParse(Console.ReadLine(), out radius) || radius <= 0)
            {
                Console.WriteLine();
                Console.WriteLine("Ошибка: радиус должен быть положительным числом.");
                Console.Write("Введите радиус еще раз: ");
            }

            StarDto newStar = new StarDto(0, name.Trim(), discoverer.Trim(), starType, radius);

            Action<StarDto> handler = AddStarRequested;
            if (handler != null)
            {
                handler(newStar);
            }

            PauseScreen();
        }

        /// <summary>
        /// Пункт 3: выводит список звезд, запрашивает ID и генерирует событие удаления.
        /// </summary>
        private void RemoveStar()
        {
            Console.Clear();
            Console.WriteLine("=== УДАЛЕНИЕ ЗВЕЗДЫ ===");
            Console.WriteLine("Текущий список доступных объектов:\n");

            RequestAndPrintAllStars();

            Console.Write("Введите ID звезды, которую хотите удалить: ");

            int id;
            if (int.TryParse(Console.ReadLine(), out id))
            {
                Action<int> handler = DeleteStarRequested;
                if (handler != null)
                {
                    handler(id);
                }
            }
            else
            {
                ShowError("Введен некорректный номер.");
            }

            PauseScreen();
        }

        /// <summary>
        /// Пункт 4: выводит список звезд, запрашивает ID и запускает поиск для редактирования.
        /// </summary>
        private void EditStar()
        {
            Console.Clear();
            Console.WriteLine("=== РЕДАКТИРОВАНИЕ ЗВЕЗДЫ ===");
            Console.WriteLine("Текущий список доступных объектов:\n");

            RequestAndPrintAllStars();

            Console.Write("Введите ID звезды для изменения: ");

            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                ShowError("Некорректный номер ID.");
                PauseScreen();
                return;
            }

            Action<int> handler = FindStarForEditRequested;
            if (handler != null)
            {
                handler(id);
            }

            PauseScreen();
        }

        /// <summary>
        /// Пункт 5: запрашивает тип звезды и генерирует событие фильтрации.
        /// </summary>
        private void FilterByType()
        {
            Console.Clear();
            Console.WriteLine("--- ПОИСК ЗВЕЗД ПО ТИПУ ---");

            _printStarsOnUpdate = false;
            Action loadHandler = LoadStarsRequested;
            if (loadHandler != null)
            {
                loadHandler();
            }

            if (_currentStars.Count == 0)
            {
                Console.WriteLine("Список звезд пуст.");
                PauseScreen();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Доступные звезды:");
            PrintStarsList(_currentStars);

            string selectedType = ReadStarTypeFromConsole("--- ПОИСК ЗВЕЗД ПО ТИПУ ---", null);

            Console.Clear();
            Console.WriteLine("--- РЕЗУЛЬТАТЫ ПОИСКА ---");
            Console.WriteLine("Выбранный тип: " + selectedType);
            Console.WriteLine();


            _printStarsOnUpdate = true;
            Action<string> filterHandler = FilterByTypeRequested;
            if (filterHandler != null)
            {
                filterHandler(selectedType);
            }
            _printStarsOnUpdate = false;


            Action resetHandler = ResetFilterRequested;
            if (resetHandler != null)
            {
                resetHandler();
            }

            PauseScreen();
        }

        /// <summary>
        /// Пункт 6: генерирует событие расчета среднего радиуса.
        /// </summary>
        private void RequestAverageRadius()
        {
            Console.Clear();
            Console.WriteLine("--- СРЕДНИЙ РАДИУС ЗВЕЗД КАТАЛОГА ---");

            Action handler = CalculateAverageRadiusRequested;
            if (handler != null)
            {
                handler();
            }

            PauseScreen();
        }


        /// <summary>
        /// Запрашивает полный список звезд через событие LoadStarsRequested и печатает его.
        /// </summary>
        private void RequestAndPrintAllStars()
        {
            _printStarsOnUpdate = true;

            Action handler = LoadStarsRequested;
            if (handler != null)
            {
                handler();
            }

            _printStarsOnUpdate = false;
        }

        /// <summary>
        /// Печатает переданный список DTO звезд в консоль.
        /// </summary>
        /// <param name="list">Список DTO звезд.</param>
        private void PrintStarsList(List<StarDto> list)
        {
            if (list == null || list.Count == 0)
            {
                Console.WriteLine("Звезд не найдено (список пуст).");
                return;
            }

            for (int i = 0; i < list.Count; i = i + 1)
            {
                StarDto s = list[i];
                Console.WriteLine(
                    "ID: " + s.Id +
                    " | " + s.Name +
                    " | Открыл: " + s.Discoverer +
                    " | Тип: " + s.StarType +
                    " | Радиус: " + s.Radius + " R.sun");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Интерактивно запрашивает у пользователя выбор одного из четырех типов звезды.
        /// </summary>
        /// <param name="header">Заголовок экрана выбора.</param>
        /// <param name="contextInfo">Дополнительная информация для отображения под заголовком (может быть null).</param>
        /// <returns>Строковое название выбранного типа звезды.</returns>
        private string ReadStarTypeFromConsole(string header, string contextInfo)
        {
            string selectedType = "";

            while (selectedType == "")
            {
                Console.Clear();
                Console.WriteLine(header);

                if (!string.IsNullOrWhiteSpace(contextInfo))
                {
                    Console.WriteLine(contextInfo);
                }

                Console.WriteLine();
                Console.WriteLine("Выберите тип звезды:");
                Console.WriteLine("1. Красный гигант");
                Console.WriteLine("2. Желтый карлик");
                Console.WriteLine("3. Белый карлик");
                Console.WriteLine("4. Нейтронная звезда");
                Console.Write("Введите номер типа (1-4): ");

                int typeNumber;
                if (!int.TryParse(Console.ReadLine(), out typeNumber))
                {
                    Console.WriteLine("\nОшибка: необходимо ввести целое число от 1 до 4.");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    continue;
                }

                if (typeNumber == 1)
                {
                    selectedType = "Красный гигант";
                }
                else if (typeNumber == 2)
                {
                    selectedType = "Желтый карлик";
                }
                else if (typeNumber == 3)
                {
                    selectedType = "Белый карлик";
                }
                else if (typeNumber == 4)
                {
                    selectedType = "Нейтронная звезда";
                }
                else
                {
                    Console.WriteLine("\nОшибка: введите число от 1 до 4.");
                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }

            return selectedType;
        }

        /// <summary>
        /// Приостанавливает выполнение экрана до нажатия любой клавиши.
        /// </summary>
        private void PauseScreen()
        {
            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
    }
}
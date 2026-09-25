using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarCatalog.Core;

namespace StarCatalog.ConsoleApp
{
    internal class Program
    {
        private static Logic logic = new Logic();

        static void Main(string[] args)
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
                    ShowAverageRadius();
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

        private static void PauseScreen()
        {
            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }

        private static void PrintStarsTable()
        {
            List<Star> list = logic.GetAllStars();

            if (list.Count == 0)
            {
                Console.WriteLine("Список звезд пуст.");
                return;
            }

            for (int i = 0; i < list.Count; i = i + 1)
            {
                Star s = list[i];
                Console.WriteLine("ID: " + s.Id + " | " + s.Name + " | Открыл: " + s.Discoverer + " | Тип: " + s.StarType + " | Радиус: " + s.Radius + " R.sun");
            }
            Console.WriteLine();
        }

        private static void ShowAllStars()
        {
            Console.Clear();
            Console.WriteLine("=== СПИСОК ВСЕХ ЗВЕЗД В КАТАЛОГЕ ===\n");
            PrintStarsTable();
            PauseScreen();
        }

        private static void AddNewStar()
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

            string starType = "";
            while (starType == "")
            {
                Console.WriteLine("1. Красный гигант");
                Console.WriteLine("2. Желтый карлик");
                Console.WriteLine("3. Белый карлик");
                Console.WriteLine("4. Нейтронная звезда");
                Console.Write("Введите номер типа: ");

                int typeNumber;

                if (int.TryParse(Console.ReadLine(), out typeNumber))
                {
                    if (typeNumber == 1)
                    {
                        starType = "Красный гигант";
                    }
                    else if (typeNumber == 2)
                    {
                        starType = "Желтый карлик";
                    }
                    else if (typeNumber == 3)
                    {
                        starType = "Белый карлик";
                    }
                    else if (typeNumber == 4)
                    {
                        starType = "Нейтронная звезда";
                    }
                    else
                    {
                        Console.WriteLine("Введите число от 1 до 4.\n");
                    }
                }
                else
                {
                    Console.WriteLine("Нужно ввести число.\n");
                }
            }

            Console.Write("Введите радиус звезды: ");

            double radius;

            while (!double.TryParse(Console.ReadLine(), out radius)
                   || radius <= 0)
            {
                Console.Write(
                    "Ошибка. Введите положительное число: ");
            }

            bool added = logic.AddStar(name, discoverer, starType, radius);

            if (added)
            {
                Console.WriteLine("Звезда добавлена.");
            }
            else
            {
                Console.WriteLine(
                    "Не удалось добавить звезду.");
            }
            PauseScreen();
        }

        private static void RemoveStar()
        {
            Console.Clear();
            Console.WriteLine("=== УДАЛЕНИЕ ЗВЕЗДЫ ===");
            Console.WriteLine("Текущий список доступных объектов:\n");

            PrintStarsTable();

            Console.Write("Введите ID звезды, которую хотите удалить: ");

            int id;
            if (int.TryParse(Console.ReadLine(), out id))
            {
                bool deleted = logic.DeleteStar(id);
                if (deleted)
                {
                    Console.WriteLine("\n[Успех] Звезда с ID " + id + " была удалена.");
                }
                else
                {
                    Console.WriteLine("\n[Ошибка] Звезда с ID " + id + " не найдена.");
                }
            }
            else
            {
                Console.WriteLine("\n[Ошибка] Введен некорректный номер.");
            }

            PauseScreen();
        }

        private static void EditStar()
        {
            Console.Clear();
            Console.WriteLine("=== РЕДАКТИРОВАНИЕ ЗВЕЗДЫ ===");
            Console.WriteLine("Текущий список доступных объектов:\n");

            PrintStarsTable();

            Console.Write("Введите ID звезды для изменения: ");

            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("\n[Ошибка] Некорректный номер ID.");
                PauseScreen();
                return;
            }

            Star star = logic.FindById(id);
            if (star == null)
            {
                Console.WriteLine("\n[Ошибка] Звезда с ID " + id + " не найдена.");
                PauseScreen();
                return;
            }

            Console.WriteLine("\nВыбрана звезда: " + star.Name);
            Console.Write("Новое название: ");
            string name = Console.ReadLine();

            Console.Write("Новый первооткрыватель: ");
            string discoverer = Console.ReadLine();

            string newStarType = "";

            while (newStarType == "")
            {
                Console.Clear();

                Console.WriteLine("--- РЕДАКТИРОВАНИЕ ЗВЕЗДЫ ---");
                Console.WriteLine("Выбрана звезда: " + star.Name);
                Console.WriteLine();

                Console.WriteLine("Выберите новый тип звезды:");
                Console.WriteLine("1. Красный гигант");
                Console.WriteLine("2. Желтый карлик");
                Console.WriteLine("3. Белый карлик");
                Console.WriteLine("4. Нейтронная звезда");
                Console.Write("Введите номер нового типа: ");

                int typeNumber;

                if (!int.TryParse(Console.ReadLine(), out typeNumber))
                {
                    Console.WriteLine();
                    Console.WriteLine("Ошибка: нужно ввести число от 1 до 4.");
                    Console.WriteLine("Нажмите любую клавишу и попробуйте снова.");
                    Console.ReadKey();
                    continue;
                }

                if (typeNumber == 1)
                {
                    newStarType = "Красный гигант";
                }
                else if (typeNumber == 2)
                {
                    newStarType = "Желтый карлик";
                }
                else if (typeNumber == 3)
                {
                    newStarType = "Белый карлик";
                }
                else if (typeNumber == 4)
                {
                    newStarType = "Нейтронная звезда";
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Ошибка: введите число от 1 до 4.");
                    Console.WriteLine("Нажмите любую клавишу и попробуйте снова.");
                    Console.ReadKey();
                }
            }

            Console.Write("Новый радиус (R.sun): ");
            double radius;
            while (!double.TryParse(Console.ReadLine(), out radius) || radius <= 0)
            {
                Console.Write("Некорректное значение. Введите число: ");
            }

            logic.UpdateStar(id, name, discoverer, newStarType, radius);
            Console.WriteLine("\n[Успех] Данные звезды обновлены!");
            PauseScreen();
        }

        private static void FilterByType()
        {
            Console.Clear();
            Console.WriteLine("--- ПОИСК ЗВЕЗД ПО ТИПУ ---");
            Console.WriteLine("1. Красный гигант");
            Console.WriteLine("2. Желтый карлик");
            Console.WriteLine("3. Белый карлик");
            Console.WriteLine("4. Нейтронная звезда");
            Console.Write("Выберите искомый тип (1-4): ");
            string choice = Console.ReadLine();

            string starType = "Желтый карлик";
            if (choice == "1") starType = "Красный гигант";
            else if (choice == "2") starType = "Желтый карлик";
            else if (choice == "3") starType = "Белый карлик";
            else if (choice == "4") starType = "Нейтронная звезда";

            List<Star> found = logic.GetStarsByType(starType);
            Console.WriteLine("\nРезультаты поиска для типа '" + starType + "':");

            if (found.Count == 0)
            {
                Console.WriteLine("Звезд такого типа в каталоге не обнаружено.");
            }
            else
            {
                for (int i = 0; i < found.Count; i = i + 1)
                {
                    Star s = found[i];
                    Console.WriteLine("ID: " + s.Id + " | Название: " + s.Name + " | Радиус: " + s.Radius + " R☉");
                }
            }

            PauseScreen();
        }

        private static void ShowAverageRadius()
        {
            Console.Clear();
            Console.WriteLine("--- СРЕДНИЙ РАДИУС ЗВЕЗД КАТАЛОГА ---");
            double avg = logic.CalculateAverageRadius();
            Console.WriteLine("Средний радиус зарегистрированных звезд: " + avg.ToString("F2") + " R☉");
            PauseScreen();
        }
    }
}
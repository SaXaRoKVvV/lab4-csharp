using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab4.Collections
{
    internal static class Program
    {
        private const string GasolineFilePath = "gasoline.txt";

        private static void Main()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            bool isRunning = true;
            while (isRunning)
            {
                PrintMenu();
                int choice = ReadInt("Ваш выбор: ", 0, 5);
                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        RunTask1();
                        break;
                    case 2:
                        RunTask2();
                        break;
                    case 3:
                        RunTask3();
                        break;
                    case 4:
                        RunTask4();
                        break;
                    case 5:
                        RunTask5();
                        break;
                    default:
                        isRunning = false;
                        break;
                }
                Console.WriteLine();
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("Лабораторная работа №4, вариант 10");
            Console.WriteLine("1 - List: " +
                              "убрать подряд идущие дубликаты");
            Console.WriteLine("2 - LinkedList: " +
                              "двунаправленный список");
            Console.WriteLine("3 - HashSet: факультативы");
            Console.WriteLine("4 - HashSet: " +
                              "символы слов с нечетными номерами");
            Console.WriteLine("5 - Dictionary: " +
                              "самый дешевый бензин");
            Console.WriteLine("0 - Выход");
        }

        private static void RunTask1()
        {
            List<int> list = ReadIntList
                ("Элементы списка через пробел: ");
            CollectionTasks.RemoveConsecutiveDuplicates(list);
            Console.WriteLine("Результат: " + Join(list));
        }

        private static void RunTask2()
        {
            List<int> numbers = ReadIntList("Элементы" +
                " однонаправленного списка через пробел: ");
            SinglyLinkedNode head = 
                BuildSinglyLinkedList(numbers);
            LinkedList<int> result = 
                CollectionTasks.BuildDoublyLinkedList(head);

            List<int> backward = new List<int>();
            LinkedListNode<int> node = result.Last;
            while (node != null)
            {
                backward.Add(node.Value);
                node = node.Previous;
            }

            Console.WriteLine("Слева направо: " + Join(result));
            Console.WriteLine("Справа налево: " + Join(backward));
        }

        private static void RunTask3()
        {
            HashSet<string> electives = 
                ReadNames("Перечень факультативов (через запятую): "
                    , null);
            int studentCount = ReadInt
                ("Количество студентов: ", 1, 100);
            List<HashSet<string>> students = 
                new List<HashSet<string>>();

            for (int i = 1; i <= studentCount; i++)
            {
                students.Add(ReadNames
                    ("Факультативы студента " + i + 
                     " (через запятую, можно пусто): ", electives));
            }

            HashSet<string> all;
            HashSet<string> any;
            HashSet<string> none;
            CollectionTasks.AnalyzeElectives(electives, students, out all, out any, out none);

            Console.WriteLine("Ходят все: " + Join(all));
            Console.WriteLine("Ходит хотя бы один: " + Join(any));
            Console.WriteLine("Не ходит никто: " + Join(none));
        }

        private static void RunTask4()
        {
            Console.Write("Путь к текстовому файлу (например, text.txt): ");
            string path = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                Console.WriteLine("Ошибка: файл не найден.");
                return;
            }

            try
            {
                List<char> chars = CollectionTasks.GetCharsOfOddWords(path);
                Console.WriteLine("Символы: " + Join(chars));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        private static void RunTask5()
        {
            bool needFill = !File.Exists(GasolineFilePath) || ReadYesNo("Заполнить файл " + GasolineFilePath + " заново? (д/н): ");

            try
            {
                if (needFill)
                {
                    CollectionTasks.FillGasolineFile(GasolineFilePath);
                }

                Dictionary<int, int> counts = CollectionTasks.CountCheapestStations(GasolineFilePath);
                Console.WriteLine(counts[92] + " " + counts[95] + " " + counts[98]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        private static SinglyLinkedNode BuildSinglyLinkedList(List<int> numbers)
        {
            SinglyLinkedNode head = null;
            SinglyLinkedNode tail = null;

            foreach (int number in numbers)
            {
                SinglyLinkedNode node = new SinglyLinkedNode(number);
                if (head == null)
                {
                    head = node;
                }
                else
                {
                    tail.Next = node;
                }
                tail = node;
            }
            return head;
        }

        private static string Join<T>(IEnumerable<T> items)
        {
            string text = string.Join(" ", items);
            return text.Length == 0 ? "(пусто)" : text;
        }

        private static int ReadInt(string prompt, int min, int max)
        {
            int value;
            bool isValid;
            do
            {
                Console.Write(prompt);
                isValid = int.TryParse(Console.ReadLine(), out value) && value >= min && value <= max;
                if (!isValid) Console.WriteLine("Ошибка: введите целое число от " + min + " до " + max + ".");
            }
            while (!isValid);
            return value;
        }

        private static bool ReadYesNo(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string answer = Console.ReadLine();
                if (answer != null) answer = answer.Trim().ToLower();

                if (answer == "д" || answer == "y") return true;
                if (answer == "н" || answer == "n") return false;

                Console.WriteLine("Ошибка: введите д или н.");
            }
        }

        private static List<int> ReadIntList(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string line = Console.ReadLine();
                string[] parts = line == null ? new string[0] : line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                List<int> result = new List<int>();
                bool isValid = true;

                foreach (string part in parts)
                {
                    int number;
                    if (int.TryParse(part, out number))
                    {
                        result.Add(number);
                    }
                    else
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid) return result;
                Console.WriteLine("Ошибка: вводите только целые числа через пробел.");
            }
        }

        private static HashSet<string> ReadNames(string prompt, HashSet<string> allowed)
        {
            while (true)
            {
                Console.Write(prompt);
                string line = Console.ReadLine();
                string[] parts = line == null ? new string[0] : 
                    line.Split(new char[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries);

                HashSet<string> names = new HashSet<string>();
                foreach (string part in parts)
                {
                    string name = part.Trim();
                    if (name.Length > 0)
                    {
                        names.Add(name);
                    }
                }

                if (allowed == null && names.Count == 0)
                {
                    Console.WriteLine("Ошибка: перечень не может быть пустым.");
                }
                else if (allowed != null && !names.IsSubsetOf(allowed))
                {
                    Console.WriteLine("Ошибка: все названия должны входить в перечень факультативов.");
                }
                else
                {
                    return names;
                }
            }
        }
    }
}
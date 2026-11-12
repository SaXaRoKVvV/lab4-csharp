using System;
using System.Collections.Generic;
using System.IO;

namespace Lab4.Collections
{
    /// <summary>
    /// Решения заданий 1-5 лабораторной работы №4 (вариант 10).
    /// </summary>
    public static class CollectionTasks
    {
        private const int MaxNameLength = 20;
        private const int MinPrice = 1000;
        private const int MaxPrice = 3000;
        private static readonly int[] brands = { 92, 95, 98 };

        /// <summary>
        /// Задание 1.10 (List). Оставляет в списке только один элемент
        /// из каждой группы подряд идущих одинаковых элементов.
        /// </summary>
        public static void RemoveConsecutiveDuplicates
            (List<int> list)
        {
            if (list == null) throw 
                new ArgumentNullException(nameof(list));
            if (list.Count == 0) return;

            int writeIndex = 1;
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] != list[writeIndex - 1])
                {
                    list[writeIndex] = list[i];
                    writeIndex++;
                }
            }
            list.RemoveRange
                (writeIndex, list.Count - writeIndex);
        }

        /// <summary>
        /// Задание 2.10 (LinkedList). Строит двунаправленный список
        /// по однонаправленному списку, заданному головным узлом.
        /// </summary>
        public static LinkedList<int> BuildDoublyLinkedList
            (SinglyLinkedNode head)
        {
            LinkedList<int> result = new LinkedList<int>();
            SinglyLinkedNode current = head;

            while (current != null)
            {
                result.AddLast(current.Value);
                current = current.Next;
            }
            return result;
        }

        /// <summary>
        /// Задание 3.10 (HashSet).
        /// Определяет факультативы, которые посещают все студенты,
        /// хотя бы один студент и ни один студент.
        /// </summary>
        public static void AnalyzeElectives(
            HashSet<string> electives,
            List<HashSet<string>> students,
            out HashSet<string> attendedByAll,
            out HashSet<string> attendedByAny,
            out HashSet<string> attendedByNone)
        {
            if (electives == null) throw new
                ArgumentNullException(nameof(electives));
            if (students == null) throw new
                ArgumentNullException(nameof(students));
            if (students.Count == 0)
            {
                throw new ArgumentException
                    ("Список студентов пуст", nameof(students));
            }

            attendedByAll = new HashSet<string>(electives);
            attendedByAny = new HashSet<string>();

            foreach (HashSet<string> student in students)
            {
                attendedByAll.IntersectWith(student);
                attendedByAny.UnionWith(student);
            }

            attendedByAny.IntersectWith(electives);

            attendedByNone = new HashSet<string>(electives);
            attendedByNone.ExceptWith(attendedByAny);
        }

        /// <summary>
        /// Задание 4.10 (HashSet). Возвращает в алфавитном
        /// порядке символы, которые
        /// встречаются хотя бы в одном слове с нечетным номером.
        /// </summary>
        public static List<char> GetCharsOfOddWords(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException
                    ("Путь к файлу не задан.", nameof(path));
            }
            if (!File.Exists(path))
            {
                throw new FileNotFoundException
                    ("Файл не найден: " + path);
            }

            string text = File.ReadAllText(path);
            HashSet<char> chars = new HashSet<char>();
            int wordNumber = 0;
            bool isInsideWord = false;

            foreach (char symbol in text)
            {
                if (char.IsLetterOrDigit(symbol))
                {
                    if (!isInsideWord)
                    {
                        isInsideWord = true;
                        wordNumber++;
                    }

                    if (wordNumber % 2 == 1)
                    {
                        chars.Add(char.ToLower(symbol));
                    }
                }
                else
                {
                    isInsideWord = false;
                }
            }

            List<char> sorted = new List<char>(chars);
            sorted.Sort();
            return sorted;
        }

        /// <summary>
        /// Задание 5.10 (Dictionary).
        /// Заполняет исходный файл с данными о ценах на бензин.
        /// </summary>
        public static void FillGasolineFile(string path)
        {
            int count = ReadPositiveInt
                ("Количество записей N: ");

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(count);

                for (int i = 1; i <= count; i++)
                {
                    string line;
                    bool isValid;
                    int brand;
                    int price;

                    do
                    {
                        Console.Write("Запись " + i + " (<Компания>" +
                                      " <Улица> <Марка> <Цена>): ");
                        line = Console.ReadLine();
                        isValid = TryParseStationLine(line, 
                            out brand, out price);

                        if (!isValid)
                        {
                            Console.WriteLine("Ошибка формата." +
                                " Компания и улица - до 20 " +
                                "символов, марка - 92/95/98," +
                                " цена 1000-3000, один пробел.");
                        }
                    }
                    while (!isValid);

                    writer.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Задание 5.10 (Dictionary). Для каждой марки бензина определяет, сколько АЗС
        /// продают ее дешевле всего.
        /// </summary>
        public static Dictionary<int, int> CountCheapestStations(string path)
        {
            Dictionary<int, int> minPrices = new Dictionary<int, int>();
            Dictionary<int, int> counts = new Dictionary<int, int>();

            foreach (int fuel in brands)
            {
                minPrices[fuel] = int.MaxValue;
                counts[fuel] = 0;
            }

            using (StreamReader reader = new StreamReader(path))
            {
                string firstLine = reader.ReadLine();
                int total;
                if (!int.TryParse(firstLine, out total) || total < 0)
                {
                    throw new FormatException("Первая строка " +
                        "файла должна содержать количество записей N.");
                }

                for (int i = 1; i <= total; i++)
                {
                    string line = reader.ReadLine();
                    int brand;
                    int price;

                    if (!TryParseStationLine(line, out brand, out price))
                    {
                        throw new FormatException("Некорректная " +
                            "запись в строке " + (i + 1) + " файла.");
                    }

                    if (price < minPrices[brand])
                    {
                        minPrices[brand] = price;
                        counts[brand] = 1;
                    }
                    else if (price == minPrices[brand])
                    {
                        counts[brand]++;
                    }
                }
            }
            return counts;
        }

        private static int ReadPositiveInt(string prompt)
        {
            int value;
            bool isValid;
            do
            {
                Console.Write(prompt);
                isValid = int.TryParse(Console.ReadLine(), 
                    out value) && value > 0;
                if (!isValid)
                {
                    Console.WriteLine("Ошибка: введите целое " +
                                      "положительное число.");
                }
            }
            while (!isValid);
            return value;
        }

        private static bool TryParseStationLine(string line,
            out int brand, out int price)
        {
            brand = 0;
            price = 0;
            if (line == null) return false;

            string[] parts = line.Split(' ');
            if (parts.Length != 4) return false;

            if (parts[0].Length == 0 || parts[0].Length >
                MaxNameLength ||
                parts[1].Length == 0 || parts[1].Length >
                MaxNameLength) return false;

            if (!int.TryParse(parts[2], out brand) ||
                Array.IndexOf(brands, brand) < 0) return false;

            return int.TryParse(parts[3], out price) && price >=
                MinPrice && price <= MaxPrice;
        }
    }
}
using System;
using System.Text;

namespace Lab4.Operators
{
    internal static class Program
    {
        private static void Main()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            Triangle first = new Triangle(3, 4, 5);
            Triangle second = new Triangle(6, 8, 10);

            bool isRunning = true;
            while (isRunning)
            {
                PrintMenu();
                int choice = ReadInt("Ваш выбор: ", 0, 5);
                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        first = ReadTriangle("первого");
                        second = ReadTriangle("второго");
                        break;
                    case 2:
                        ShowInfo("Первый", first);
                        ShowInfo("Второй", second);
                        break;
                    case 3:
                        CompareAreas(first, second);
                        break;
                    case 4:
                        ChangeSide(first);
                        break;
                    case 5:
                        TestConstructors(first);
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
            Console.WriteLine("Лабораторная работа №4," +
                              " задания 6-7, вариант 10 (Triangle)");
            Console.WriteLine("1 - Ввести два треугольника");
            Console.WriteLine("2 - Показать информацию");
            Console.WriteLine("3 - Сравнить площади");
            Console.WriteLine("4 - Изменить сторону " +
                              "первого треугольника");
            Console.WriteLine("5 - Тест конструкторов");
            Console.WriteLine("0 - Выход");
        }

        private static void ShowInfo(string title, Triangle triangle)
        {
            double area = -triangle;
            double perimeter = triangle;
            bool exists = (bool)triangle;

            Console.WriteLine(title + ": " + triangle.ToString());
            Console.WriteLine("  Метод Exists(): "
                              + triangle.Exists());
            Console.WriteLine("  (bool) - существует: " + exists);
            Console.WriteLine("  -t - площадь: "
                              + area.ToString("F2"));
            Console.WriteLine("  (double) - периметр: "
                              + perimeter.ToString("F2"));
        }

        private static void CompareAreas(Triangle first,
            Triangle second)
        {
            Console.WriteLine("first < second: " + (first < second));
            Console.WriteLine("first > second: " + (first > second));
        }

        private static void ChangeSide(Triangle triangle)
        {
            int side = ReadInt("Какую сторону изменить" +
                               " (1 - a, 2 - b, 3 - c): ", 1, 3);
            double value = ReadPositiveDouble("Новое значение: ");

            if (side == 1) triangle.A = value;
            else if (side == 2) triangle.B = value;
            else triangle.C = value;

            Console.WriteLine("Теперь: " + triangle.ToString());
        }

        private static void TestConstructors(Triangle first)
        {
            Triangle byDefault = new Triangle();
            Console.WriteLine("По умолчанию: " +
                              byDefault.ToString());

            Triangle withParameters = new Triangle(3, 4, 5);
            Console.WriteLine("С параметрами (3, 4, 5): "
                              + withParameters.ToString());

            Triangle copy = new Triangle(first);
            copy.A = copy.A + 1;
            Console.WriteLine("Копия первого с измененной" +
                              " стороной a: " + copy.ToString());
            Console.WriteLine("Оригинал не изменился: " +
                              first.ToString());

            try
            {
                Triangle invalid = new Triangle(-1, 2, 3);
                Console.WriteLine(invalid.ToString());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Triangle(-1, 2, 3) -> ошибка:" +
                                  " " + ex.Message);
            }
        }

        private static Triangle ReadTriangle(string ordinal)
        {
            Console.WriteLine("Введите стороны " + ordinal +
                              " треугольника:");
            double a = ReadPositiveDouble("  a = ");
            double b = ReadPositiveDouble("  b = ");
            double c = ReadPositiveDouble("  c = ");
            return new Triangle(a, b, c);
        }

        private static double ReadPositiveDouble(string prompt)
        {
            double value;
            bool isValid;
            do
            {
                Console.Write(prompt);
                isValid = double.TryParse(Console.ReadLine(),
                    out value) && value > 0 && 
                          !double.IsInfinity(value);
                if (!isValid) Console.WriteLine
                    ("Ошибка: введите положительное число.");
            }
            while (!isValid);
            return value;
        }

        private static int ReadInt(string prompt, int min, int max)
        {
            int value;
            bool isValid;
            do
            {
                Console.Write(prompt);
                isValid = int.TryParse(Console.ReadLine(),
                    out value) && value >= min && value <= max;
                if (!isValid) Console.WriteLine
                    ("Ошибка: введите целое число от " + min +
                     " до " + max + ".");
            }
            while (!isValid);
            return value;
        }
    }
}
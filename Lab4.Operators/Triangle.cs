using System;

namespace Lab4.Operators
{
    /// <summary>
    /// Треугольник, заданный длинами сторон.
    /// </summary>
    public class Triangle
    {
        private double _a;
        private double _b;
        private double _c;

        public Triangle() : this(1, 1, 1)
        {
        }

        public Triangle(double a, double b, double c)
        {
            _a = CheckSide(a, "a");
            _b = CheckSide(b, "b");
            _c = CheckSide(c, "c");
        }

        public Triangle(Triangle other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            _a = other._a;
            _b = other._b;
            _c = other._c;
        }

        public double A
        {
            get { return _a; }
            set { _a = CheckSide(value, "value"); }
        }

        public double B
        {
            get { return _b; }
            set { _b = CheckSide(value, "value"); }
        }

        public double C
        {
            get { return _c; }
            set { _c = CheckSide(value, "value"); }
        }

        public static double operator -(Triangle triangle)
        {
            if (triangle == null) throw
                new ArgumentNullException("triangle");
            return triangle.GetArea();
        }

        public static implicit operator double(Triangle triangle)
        {
            if (triangle == null) throw
                new ArgumentNullException("triangle");
            return triangle._a + triangle._b + triangle._c;
        }

        public static explicit operator bool(Triangle triangle)
        {
            if (triangle == null) throw
                new ArgumentNullException("triangle");
            return triangle.Exists();
        }

        public static bool operator <(Triangle left, Triangle right)
        {
            if (left == null || right == null) throw
                new ArgumentNullException();
            return left.GetArea() < right.GetArea();
        }

        public static bool operator >(Triangle left, Triangle right)
        {
            if (left == null || right == null) throw
                new ArgumentNullException();
            return left.GetArea() > right.GetArea();
        }

        public bool Exists()
        {
            return _a + _b > _c && _a + _c > _b && _b + _c > _a;
        }

        public double GetArea()
        {
            if (!Exists()) return 0;
            double p = (_a + _b + _c) / 2;
            double product = p * (p - _a) * (p - _b) * (p - _c);
            return Math.Sqrt(Math.Max(0, product));
        }

        public override string ToString()
        {
            return string.Format("Triangle: a = {0}, b = {1}," +
                                 " c = {2}", _a, _b, _c);
        }

        private static double CheckSide
            (double side, string paramName)
        {
            if (double.IsNaN(side) || double.IsInfinity(side) ||
                side <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName,
                    side, "Сторона должна быть положительным" +
                          " конечным числом.");
            }
            return side;
        }
    }
}
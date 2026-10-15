using System;
using System.Collections.Generic;

namespace GeometryFiguresApp
{
    // Власний тип винятку для некоректних розмірів фігури
    public class InvalidFigureSizeException : Exception
    {
        public InvalidFigureSizeException() { }
        public InvalidFigureSizeException(string message) : base(message) { }
        public InvalidFigureSizeException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    // Абстрактний базовий клас
    public abstract class Shape : IComparable<Shape>
    {
        private string _name;

        public string Name
        {
            get => _name;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Назва фігури не може бути порожньою.");
                _name = value;
            }
        }

        protected Shape(string name) => Name = name;

        public abstract double GetArea();
        public abstract double GetPerimeter();

        public virtual void PrintInfo()
        {
            Console.WriteLine($"{Name,-15} Площа: {GetArea(),10:F2} | Периметр: {GetPerimeter(),10:F2}");
        }

        public int CompareTo(Shape? other)
        {
            if (other is null) return 1;
            return GetArea().CompareTo(other.GetArea());
        }
    }

    public class Circle : Shape
    {
        private double _radius;

        public double Radius
        {
            get => _radius;
            set
            {
                if (value <= 0)
                    throw new InvalidFigureSizeException("Радіус кола має бути більшим за нуль.");
                _radius = value;
            }
        }

        public Circle(double radius) : base("Коло") => Radius = radius;

        public override double GetArea() => Math.PI * Radius * Radius;
        public override double GetPerimeter() => 2 * Math.PI * Radius;

        public override void PrintInfo()
        {
            Console.Write($"Радіус: {Radius:F2} | ");
            base.PrintInfo();
        }
    }

    public class Rectangle : Shape
    {
        private double _width;
        private double _height;

        public double Width
        {
            get => _width;
            set
            {
                if (value <= 0)
                    throw new InvalidFigureSizeException("Ширина прямокутника має бути більшою за нуль.");
                _width = value;
            }
        }

        public double Height
        {
            get => _height;
            set
            {
                if (value <= 0)
                    throw new InvalidFigureSizeException("Висота прямокутника має бути більшою за нуль.");
                _height = value;
            }
        }

        public Rectangle(double width, double height) : base("Прямокутник")
        {
            Width = width;
            Height = height;
        }

        public override double GetArea() => Width * Height;
        public override double GetPerimeter() => 2 * (Width + Height);

        public override void PrintInfo()
        {
            Console.Write($"Ширина: {Width:F2} | Висота: {Height:F2} | ");
            base.PrintInfo();
        }
    }

    public class Program
    {
        private static void Main()
        {
            List<Shape> shapes = new()
            {
                new Circle(5),
                new Rectangle(4, 6),
                new Circle(2.5),
                new Rectangle(3, 8)
            };

            bool running = true;
            while (running)
            {
                Console.WriteLine("\n========== МЕНЮ ==========");
                Console.WriteLine("0. Вихід");
                Console.WriteLine("1. Додати фігуру");
                Console.WriteLine("2. Показати всі фігури");
                Console.WriteLine("3. Відсортувати фігури за площею");
                Console.WriteLine("4. Показати сумарну площу та периметр");
                int choice = ReadInt("Ваш вибір: ", 0, 4);

                try
                {
                    switch (choice)
                    {
                        case 0:
                            running = false;
                            break;
                        case 1:
                            AddShape(shapes);
                            break;
                        case 2:
                            ShowShapes(shapes);
                            break;
                        case 3:
                            SortShapes(shapes);
                            break;
                        case 4:
                            ShowTotals(shapes);
                            break;
                    }
                }
                catch (InvalidFigureSizeException ex)
                {
                    Console.WriteLine($"Помилка розміру фігури: {ex.Message}");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"Помилка діапазону: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Непередбачена помилка: {ex.Message}");
                }
            }

            Console.WriteLine("Роботу завершено.");
        }

        private static void AddShape(List<Shape> shapes)
        {
            Console.WriteLine("\nОберіть тип фігури:");
            Console.WriteLine("1. Коло");
            Console.WriteLine("2. Прямокутник");
            int type = ReadInt("Ваш вибір: ", 1, 2);

            try
            {
                if (type == 1)
                {
                    double radius = ReadDouble("Введіть радіус: ");
                    shapes.Add(new Circle(radius));
                }
                else
                {
                    double width = ReadDouble("Введіть ширину: ");
                    double height = ReadDouble("Введіть висоту: ");
                    shapes.Add(new Rectangle(width, height));
                }

                Console.WriteLine("Фігуру успішно додано.");
            }
            catch (InvalidFigureSizeException ex)
            {
                Console.WriteLine($"Не вдалося додати фігуру: {ex.Message}");
            }
        }

        private static void ShowShapes(List<Shape> shapes)
        {
            if (shapes.Count == 0)
            {
                Console.WriteLine("Колекція порожня.");
                return;
            }

            Console.WriteLine("\n--- Список фігур ---");
            foreach (Shape shape in shapes)
                shape.PrintInfo();
        }

        private static void SortShapes(List<Shape> shapes)
        {
            shapes.Sort();
            Console.WriteLine("Фігури відсортовано за зростанням площі.");
            ShowShapes(shapes);
        }

        private static void ShowTotals(List<Shape> shapes)
        {
            double totalArea = 0;
            double totalPerimeter = 0;

            foreach (Shape shape in shapes)
            {
                totalArea += shape.GetArea();
                totalPerimeter += shape.GetPerimeter();
            }

            Console.WriteLine($"Сумарна площа колекції: {totalArea:F2}");
            Console.WriteLine($"Сумарний периметр колекції: {totalPerimeter:F2}");
        }

        private static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (double.TryParse(input, out double value))
                    return value;

                Console.WriteLine("Некоректне введення. Введіть число (наприклад, 3,5).");
            }
        }

        private static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int value) && value >= min && value <= max)
                    return value;

                Console.WriteLine($"Введіть ціле число від {min} до {max}.");
            }
        }
    }
}

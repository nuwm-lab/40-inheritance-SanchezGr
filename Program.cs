using System;
using System.Globalization;
using System.Linq;

namespace LabWork
{
    class Praktykant
    {
        public string LastName { get; protected set; }
        public string FirstName { get; protected set; }
        public string University { get; protected set; }

        public Praktykant() { }

        public Praktykant(string lastName, string firstName, string university)
        {
            LastName = lastName;
            FirstName = firstName;
            University = university;
        }

        protected static string ReadOnlyLetters(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine()?.Trim() ?? "";
                if (s.Length == 0) { Console.WriteLine("Введіть дані"); continue; }

                bool ok = s.All(c => char.IsLetter(c) || c == ' ' || c == '-' || c == '\'' || c == '’');
                if (ok) return s;

                Console.WriteLine("Помилка, неправильний ввід");
            }
        }

        public static Praktykant ReadFromConsole()
        {
            var ln = ReadOnlyLetters("Прізвище практиканта: ");
            var fn = ReadOnlyLetters("Ім'я практиканта: ");
            var u  = ReadOnlyLetters("ВНЗ: ");
            return new Praktykant(ln, fn, u);
        }

        public bool IsSurnamePalindrome()
        {
            if (string.IsNullOrWhiteSpace(LastName)) return false;
            var letters = new string(LastName
                .ToLower(CultureInfo.CurrentCulture)
                .Where(char.IsLetter).ToArray());
            if (letters.Length == 0) return false;
            int l = 0, r = letters.Length - 1;
            while (l < r)
            {
                if (letters[l] != letters[r]) return false;
                l++; r--;
            }
            return true;
        }

        public virtual void Print()
        {
            Console.WriteLine($"\nПрактикант: {FirstName} {LastName}");
            Console.WriteLine($"ВНЗ: {University}");
            Console.WriteLine($"Симетричне прізвище: {(IsSurnamePalindrome() ? "Так" : "Ні")}");
        }
    }

    class PracivnykFirmy : Praktykant
    {
        public DateTime HireDate { get; private set; }
        public string GraduatedSchool { get; private set; }
        public string Position { get; private set; }

        public PracivnykFirmy() { }

        public PracivnykFirmy(string lastName, string firstName, string university,
                              string school, string position, DateTime hireDate)
            : base(lastName, firstName, university)
        {
            GraduatedSchool = school;
            Position = position;
            HireDate = hireDate.Date;
        }

        static DateTime ReadDateOneLine(string prompt)
        {
            string[] formats = { "yyyy-MM-dd", "dd.MM.yyyy" };
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine();
                if (DateTime.TryParseExact(
                        s, formats, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var dt))
                    return dt.Date;
                Console.WriteLine("Неправильний формат. Приклади: 2023-09-01 або 01.09.2023");
            }
        }

        public static PracivnykFirmy ReadFromConsole()
        {
            var p = new PracivnykFirmy();
            var baseObj = Praktykant.ReadFromConsole();
            p.LastName = baseObj.LastName;
            p.FirstName = baseObj.FirstName;
            p.University = baseObj.University;

            p.GraduatedSchool = ReadOnlyLetters("Заклад, який закінчив: ");
            p.Position = ReadOnlyLetters("Посада: ");
            p.HireDate = ReadDateOneLine("Дата прийому (yyyy-MM-dd або dd.MM.yyyy): ");
            return p;
        }

        public void Experience(out int years, out int months, out int days)
        {
            var now = DateTime.Today;
            if (now < HireDate) { years = months = days = 0; return; }
            years = now.Year - HireDate.Year;
            months = now.Month - HireDate.Month;
            days = now.Day - HireDate.Day;
            if (days < 0)
            {
                var prev = now.AddMonths(-1);
                days += DateTime.DaysInMonth(prev.Year, prev.Month);
                months--;
            }
            if (months < 0)
            {
                months += 12;
                years--;
            }
        }

        public override void Print()
        {
            Console.WriteLine($"\nПрацівник фірми: {FirstName} {LastName}");
            Console.WriteLine($"Посада: {Position}");
            Console.WriteLine($"ВНЗ: {University}");
            Console.WriteLine($"Закінчив: {GraduatedSchool}");
            Console.WriteLine($"Дата прийому: {HireDate:yyyy-MM-dd}");
            Experience(out int y, out int m, out int d);
            Console.WriteLine($"Стаж роботи: {y} р. {m} міс. {d} дн.");
            Console.WriteLine($"Симетричне прізвище: {(IsSurnamePalindrome() ? "Так" : "Ні")}");
        }
    }

    class Program
    {
        static void Main()
        {
            var praktykant = Praktykant.ReadFromConsole();
            praktykant.Print();

            var pracivnyk = PracivnykFirmy.ReadFromConsole();
            pracivnyk.Print();
        }
    }
}

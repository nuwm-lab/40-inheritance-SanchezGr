
using System;

namespace LabWork
{
    class Praktykant
    {
        protected string prizv;
        protected string imya;
        protected string vuz;

        protected static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
                Console.WriteLine("Введіть дані");
            }
        }


        protected static string ReadOnlyLetters(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine()?.Trim() ?? "";

                bool ok = true;
                foreach (char c in s)
                    if (!char.IsLetter(c)) ok = false;

                if (s.Length > 0 && ok)
                    return s;

                Console.WriteLine("Помилка: вводьте лише літери.");
            }
        }

        public virtual void Vvesty()
        {
            prizv = ReadOnlyLetters("Прізвище практиканта: ");
            imya = ReadOnlyLetters("Ім'я практиканта: ");
            vuz = ReadOnlyLetters("ВНЗ: ");
        }

        public bool ChySymPrizv()
        {
            if (string.IsNullOrEmpty(prizv)) return false;
            string s = prizv.ToLower();
            string t = "";
            for (int i = 0; i < s.Length; i++)
                if (char.IsLetter(s[i])) t += s[i];
            int l = 0, r = t.Length - 1;
            while (l < r)
            {
                if (t[l] != t[r]) return false;
                l++; r--;
            }
            return t.Length > 0;
        }

        public virtual void Vyvesty()
        {
            Console.WriteLine($"\nПрактикант: {imya} {prizv}");
            Console.WriteLine($"ВНЗ: {vuz}");
            Console.WriteLine($"Симетричне прізвище: {(ChySymPrizv() ? "Так" : "Ні")}");
        }
    }

    class PracivnykFirmy : Praktykant
    {
        private DateTime dataPrijomu;
        private string zaklad;
        private string posada;

        private static DateTime ReadDateOneLine(string prompt)
        {
            string[] formats = { "yyyy-MM-dd", "dd.MM.yyyy" };
            while (true)
            {
                Console.Write(prompt);
                string s = Console.ReadLine();
                if (DateTime.TryParseExact(s, formats,
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out var dt))
                    return dt.Date;
                Console.WriteLine("Неправильний ввід даних, спробуйте ще раз:");
            }
        }

        public override void Vvesty()
        {
            base.Vvesty();
            zaklad = ReadOnlyLetters("Заклад, який закінчив: ");
            posada = ReadOnlyLetters("Посада: ");
            dataPrijomu = ReadDateOneLine("Дата прийому (yyyy-MM-dd): ");
        }

        public void RozrahuvatyStaj(out int r, out int m, out int d)
        {
            DateTime now = DateTime.Today;
            if (now < dataPrijomu) { r = m = d = 0; return; }
            r = now.Year - dataPrijomu.Year;
            m = now.Month - dataPrijomu.Month;
            d = now.Day - dataPrijomu.Day;
            if (d < 0)
            {
                var prev = now.AddMonths(-1);
                d += DateTime.DaysInMonth(prev.Year, prev.Month);
                m--;
            }
            if (m < 0)
            {
                m += 12;
                r--;
            }
        }

        public override void Vyvesty()
        {
            Console.WriteLine($"\nПрацівник фірми: {imya} {prizv}");
            Console.WriteLine($"Посада: {posada}");


            Console.WriteLine($"ВНЗ: {vuz}");
            Console.WriteLine($"Закінчив: {zaklad}");
            Console.WriteLine($"Дата прийому: {dataPrijomu:yyyy-MM-dd}");
            RozrahuvatyStaj(out int ry, out int my, out int dy);
            Console.WriteLine($"Стаж роботи: {ry} р. {my} міс. {dy} дн.");
            Console.WriteLine($"Симетричне прізвище: {(ChySymPrizv() ? "Так" : "Ні")}");
        }
    }

    class Program
    {
        static void Main()
        {
            var praktykant = new Praktykant();
            Console.WriteLine(" Введення даних для практиканта ");
            praktykant.Vvesty();
            praktykant.Vyvesty();

            var pracivnyk = new PracivnykFirmy();
            Console.WriteLine("\n Введення даних для працівника ");
            pracivnyk.Vvesty();
            pracivnyk.Vyvesty();
        }
    }



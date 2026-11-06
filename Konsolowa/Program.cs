using System;

namespace WynagrodzeniaKonsola
{
    public enum TypUmowy
    {
        UmowaOPrace,
        UmowaZlecenie
    }

    public class KalkulatorWynagrodzen
    {
        public double KwotaBrutto { get; }
        public TypUmowy TypUmowyZatrudnienia { get; }
        public bool UlgaDlaMlodych { get; }

        public KalkulatorWynagrodzen(double kwotaBrutto, TypUmowy typUmowyZatrudnienia, bool ulgaDlaMlodych)
        {
            if (!SprawdzPoprawnoscBrutto(kwotaBrutto))
            {
                throw new ArgumentException("Kwota brutto musi być większa od 0.");
            }
            KwotaBrutto = kwotaBrutto;
            TypUmowyZatrudnienia = typUmowyZatrudnienia;
            UlgaDlaMlodych = ulgaDlaMlodych;
        }

        public static bool SprawdzPoprawnoscBrutto(double kwota)
        {
            return kwota > 0;
        }

        public double ObliczZusSpoleczne()
        {
            return KwotaBrutto * 0.1371;
        }

        public double ObliczSkladkeZdrowotna()
        {
            double podstawa = KwotaBrutto - ObliczZusSpoleczne();
            return podstawa * 0.09;
        }

        public double ObliczPit()
        {
            if (UlgaDlaMlodych)
            {
                return 0.0;
            }
            double podstawa = KwotaBrutto - ObliczZusSpoleczne();
            return podstawa * 0.12;
        }

        public double ObliczNetto()
        {
            double zus = ObliczZusSpoleczne();
            double zdrowotna = ObliczSkladkeZdrowotna();
            double pit = ObliczPit();
            return KwotaBrutto - zus - zdrowotna - pit;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SYSTEM KALKULACJI WYNAGRODZEN PRACOWNIKÓW ===\n");

            var pracownikStandard = new KalkulatorWynagrodzen(
                5500.00,
                TypUmowy.UmowaOPrace,
                false
            );

            var pracownikMlody = new KalkulatorWynagrodzen(
                5500.00,
                TypUmowy.UmowaZlecenie,
                true
            );

            WyswietlPodsumowanie("Wariant 1: Umowa o pracę (Standard)", pracownikStandard);
            WyswietlPodsumowanie("Wariant 2: Umowa zlecenie (Ulga dla młodych)", pracownikMlody);
        }

        private static void WyswietlPodsumowanie(string tytul, KalkulatorWynagrodzen kalkulator)
        {
            string nazwaUmowy = kalkulator.TypUmowyZatrudnienia == TypUmowy.UmowaOPrace
                ? "Umowa o pracę"
                : "Umowa zlecenie";

            Console.WriteLine(tytul);
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine($"Forma zatrudnienia:    {nazwaUmowy}");
            Console.WriteLine($"Ulga dla młodych:      {(kalkulator.UlgaDlaMlodych ? "Tak" : "Nie")}");
            Console.WriteLine($"Wynagrodzenie brutto:  {kalkulator.KwotaBrutto:F2} zł");
            Console.WriteLine($"Składki ZUS (13.71%):  {kalkulator.ObliczZusSpoleczne():F2} zł");
            Console.WriteLine($"Składka zdrowotna (9%): {kalkulator.ObliczSkladkeZdrowotna():F2} zł");
            Console.WriteLine($"Zaliczka PIT:          {kalkulator.ObliczPit():F2} zł");
            Console.WriteLine($"Wynagrodzenie netto:   {kalkulator.ObliczNetto():F2} zł");
            Console.WriteLine("==================================================\n");
        }
    }
}
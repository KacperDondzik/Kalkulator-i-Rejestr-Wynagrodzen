/********************************************************
- nazwa klasy: KalkulatorWynagrodzen
- opis klasy: Klasa odpowiedzialna za obliczanie wynagrodzenia netto, składek ZUS, składki zdrowotnej oraz zaliczki na podatek PIT na podstawie kwoty brutto, typu umowy oraz ulgi dla młodych.
- pola: KwotaBrutto, TypUmowyZatrudnienia, UlgaDlaMlodych
- autor: Kacper Dondzik
********************************************************/

using System;

namespace KalkulatorWynagrodzenApp
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
}
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Samochody
{
    class Program
    {
        static void Main(string[] args)
        {
            var samochody = WczytywaniePliku2("paliwo.csv");

            var zapytanie = samochody.OrderByDescending(s => s.SpalanieAutostrada).ThenBy(s => s.Producent);

            var zapytanie2 = from samochod in samochody
                             orderby samochod.SpalanieAutostrada ascending, samochod.Producent ascending
                             select samochod;

            foreach (var samochod in zapytanie2.Take(10))
            {
                Console.WriteLine(samochod.Producent + " " + samochod.Model + " : " + samochod.SpalanieAutostrada);
            }
        }

        private static List<Samochod> WczytywaniePliku2(string sciezka)
        {
            var zapytanie = from linia in File.ReadAllLines(sciezka).Skip(1)
                            where linia.Length > 1
                            select Samochod.ParsujCSV(linia);
            return zapytanie.ToList();
        }

        private static List<Samochod> WczytywaniePliku(string sciezka)
        {
            return File.ReadAllLines(sciezka)
                       .Skip(1)
                       .Where(linia => linia.Length > 1)
                       .Select(Samochod.ParsujCSV).ToList();
        }
    }
}

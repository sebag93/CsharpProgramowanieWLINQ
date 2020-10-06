﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Samochody
{
    class Program
    {
        static void Main(string[] args)
        {
            TworzenieXML();
        }

        private static void TworzenieXML()
        {
            var rekordy = WczytywanieSamochodu("paliwo.csv");

            var dokument = new XDocument();
            var samochody = new XElement("Samochody",
                                         from rekord in rekordy
                                         select new XElement("Samochod",
                                                             new XAttribute("Rok", rekord.Rok),
                                                             new XAttribute("Producent", rekord.Producent),
                                                             new XAttribute("Model", rekord.Model),
                                                             new XAttribute("SpalanieAutostrada", rekord.SpalanieAutostrada),
                                                             new XAttribute("SpalanieMiasto", rekord.SpalanieMiasto),
                                                             new XAttribute("SpalanieMieszane", rekord.SpalanieMieszane)));

            dokument.Add(samochody);
            dokument.Save("paliwo.xml");
        }

        private static List<Samochod> WczytywanieSamochodu(string sciezka)
        {
            var zapytanie = File.ReadAllLines(sciezka)
                                .Skip(1)
                                .Where(l => l.Length > 1)
                                .WSamochod();

            return zapytanie.ToList();
        }

        private static List<Producent> WczytywanieProducenci(string sciezka)
        {
            var zapytanie = File.ReadAllLines(sciezka)
                                .Where(l => l.Length > 1)
                                .Select(l =>
                                {
                                    var kolumny = l.Split(',');
                                    return new Producent
                                    {
                                        Nazwa = kolumny[0],
                                        Siedziba = kolumny[1],
                                        Rok = int.Parse(kolumny[2])
                                    };
                                });

            return zapytanie.ToList();
        }
    }

    public static class SamochodRozszerzenie
        {
            public static IEnumerable<Samochod> WSamochod(this IEnumerable<string> zrodlo)
            {
                foreach (var linia in zrodlo)
                {
                    var kolumny = linia.Split(',');

                    yield return new Samochod
                    {
                        Rok = int.Parse(kolumny[0]),
                        Producent = kolumny[1],
                        Model = kolumny[2],
                        Pojemnosc = double.Parse(kolumny[3], CultureInfo.InvariantCulture),
                        IloscCylindrow = int.Parse(kolumny[4]),
                        SpalanieMiasto = int.Parse(kolumny[5]),
                        SpalanieAutostrada = int.Parse(kolumny[6]),
                        SpalanieMieszane = int.Parse(kolumny[7])
                    };
                }
            }
        }
}

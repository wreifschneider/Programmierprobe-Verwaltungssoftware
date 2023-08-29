using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalenderUndTerminverwaltung
{
    internal class KalenderUndTermineOberflaeche
    {
        // Felder

        private static bool falscheEingabe = false;
        private static string blue = "\u001b[34m", red = "\u001b[31m", green = "\u001b[32m", yellow = "\u001b[33m",
            magenta = "\u001b[35m", cyan = "\u001b[36m", darkGray = "\u001b[90m", reset = "\u001b[0m";

        // Kalender des ausgewählten Monats wird generiert 
        public static void KalenderAusdruck()
        {
            Kalender.KalenderMonatGenerierung();
            Termine.TermineTageZurListAuslesung();

            Console.OutputEncoding = Encoding.UTF8; // UTF-8 wird gebraucht um ein Zeichen anzeigen zu können.

            Console.SetCursorPosition(18, 1);
            Console.Write("Mo Di Mi Do Fr Sa So\n\t          ____________________\n");
            for (int i = 0; i < Kalender.KalendertageTabelle.GetLength(0); i++)
            {
                Console.Write("\t         |");
                for (int j = 0; j < Kalender.KalendertageTabelle.GetLength(1); j++)
                {
                    int tag;
                    try
                    {
                        tag = Convert.ToInt32(Kalender.KalendertageTabelle[i, j]);
                    }
                    catch 
                    {
                        tag = 0;
                    }

                    for (int k = 0; k < Termine.TageVergangernerTermine.Count; k++)
                    {
                        if (tag == Termine.TageVergangernerTermine[k])
                        {
                            Console.Write(darkGray);
                            break;
                        }
                    }
                    for (int k = 0; k < Termine.TageZukuenftigerTermine.Count; k++)
                    {
                        if (tag == Termine.TageZukuenftigerTermine[k])
                        {
                            Console.Write(yellow);
                            break;
                        }
                    }

                    if (tag == Termine.TagHeutigerTermine && Kalender.MonatUndJahrAktuell())
                        Console.Write(cyan);
                    else if (tag == DateTime.Today.Day && Kalender.MonatUndJahrAktuell())
                        Console.Write(red);

                    if (tag < 10)
                        Console.Write($" {Kalender.KalendertageTabelle[i, j]} ");
                    else
                        Console.Write($"{Kalender.KalendertageTabelle[i, j]} ");

                    Console.Write(reset);
                }
                Console.WriteLine("\b|");
            }
            Console.WriteLine("\t          ‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾");

            Console.SetCursorPosition(0, 8);
            Console.WriteLine($" {Kalender.Jahr} {Kalender.MonatAusgabe()}");
        }

        // Menüs:
        // Menü für die Auswahl des Monats und Jahres
        static void DatumEingabeMenue()
        {
            int eingabeMonat;
            int eingabeJahr;
            falscheEingabe = false;
            do
            {
                falscheEingabe = true;
                Console.Clear();
                KalenderAusdruck();

                Console.WriteLine("\n\n Monat  (mm):\n\n Jahr (jjjj):");

                Console.SetCursorPosition(14, 11);
                eingabeMonat = IntPruefer();

                if (eingabeMonat > 0 && eingabeMonat <= 12)
                {
                    Kalender.Monat = eingabeMonat;

                    eingabeJahr = IntPruefer();

                    if (eingabeJahr > 0)
                    {
                        Kalender.Jahr = eingabeJahr;

                        falscheEingabe = false;
                    }
                }

                if (falscheEingabe)
                {
                    FalscheEingabe(18, 15, 1);
                }

            } while (falscheEingabe);
        }

        // Das ist das eigentlcihe Hauptmenü aus der aus man in verschiedene andere Menüs navigieren kann.
        public static void KalenderMenue()
        {
            do
            {
                Console.Clear();
                Console.CursorVisible = false;
                KalenderAusdruck();
                Console.Write($"{blue}\n      (V)Vorheriger   (A)Aktueller   (N)Nächster{reset}");
                Console.Write($"\n\n\t :\n\n ({yellow}T{reset})Termine:\n\n");
                Termine.TermineListeAusgabe(false);

                Console.SetCursorPosition(1, 1);
                Console.Write($"{blue}({green}D{blue})Datum");

                Console.SetCursorPosition(55, 1);
                Console.Write($"{blue}({red}X{blue})Beenden");
                Console.Write(reset);

                Console.SetCursorPosition(11, 13); 

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.N:
                        Kalender.NaechsterMonat();
                        break;
                    case ConsoleKey.V:
                        Kalender.VorherigerMonat();
                        break;
                    case ConsoleKey.A:
                        Kalender.AktuellerMonat();
                        break;
                    case ConsoleKey.D:
                        Console.CursorVisible = true;
                        DatumEingabeMenue();
                        break;
                    case ConsoleKey.T:
                        TerminMenue();
                        break;
                    case ConsoleKey.X:
                        return;
                    default:
                        Console.WriteLine("404 somethimhg get wrong");
                        break;
                }
            } while (true);
        }
        private static int IntPruefer()
        {
            return (int.TryParse(Console.ReadLine(), out int i) ? i : -1);
        }


        // Das Hauptmenü für Termine aus der man verschiedene Menüs für Termine ansteurn kann.
        static void TerminMenue()
        {
            do
            {
                Console.Clear();
                KalenderAusdruck();

                Console.Write($"{yellow}\n          (H)Hinzufügen (B)Bearbeiten (L)Löschen{reset}");

                Console.Write("\n\n\t : \n\n Termine:\n\n");
                Termine.TermineListeAusgabe(false);

                Console.SetCursorPosition(55, 1);
                Console.Write($"{blue}(Z)Zurück{reset}");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.H:
                        Console.CursorVisible = true;
                        TermineEingabeMenue(-1, 'h');
                        break;
                    case ConsoleKey.L:
                        TermineIDMenue(false);
                        break;
                    case ConsoleKey.B:
                        TermineIDMenue(true);
                        break;
                    case ConsoleKey.Z:
                        return;
                    default:
                        Console.WriteLine("404 something get wrong");
                        break;
                }

            } while (true);
        }

        static void IDBearbeitung(bool bearbeiten, bool falscheEingabe)
        {
            int eingabeID;
            try
            {
                eingabeID = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                eingabeID = -1;
                falscheEingabe = true;
            }

            if (!falscheEingabe && eingabeID >= 0 && eingabeID < Termine.ListenLaenge())
            {
                if (bearbeiten)
                {
                    Termine.TerminIDBestaetigung(eingabeID, 'b');
                }
                else
                {
                    Termine.TerminIDBestaetigung(eingabeID, 'l');
                }
            }
            else
            {
                Console.CursorVisible = false;
                HilfeText(5, 3);
                FalscheEingabe(22, 8, 3);
            }
        }

        // Menü mit ID-Ausgabe wird jeweils gebraucht für die Löschung oder Bearbeitung von Terminen (Bearbeitung hat 2 Menüs vereint)
        static void TermineIDMenue(bool bearbeiten)
        {
            do
            {
                falscheEingabe = false;
                Console.Clear();
                Console.CursorVisible = false;
                Console.Write($"{green}\n (I)ID{reset}");
                
                Console.WriteLine("\n\n\n\n\n     ID: \n\n\n Termine:\n");
                Termine.TermineListeAusgabe(true);

                Console.SetCursorPosition(55, 1);
                Console.Write($"{blue}(Z)Zurück{reset}");

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Z:
                        return;
                    case ConsoleKey.I:
                        Console.CursorVisible = true;
                        Console.SetCursorPosition(9, 6);
                        IDBearbeitung(bearbeiten, falscheEingabe);
                        break;

                }

            } while (true);
        }

        private static string UhrzeitEingabe()
        {
            string eingabe = String.Empty;
            falscheEingabe = true;
            int stunden;
            int minuten;
            
            stunden = IntPruefer();

            if (stunden >= 0 && stunden <= 23)
            {
                minuten = IntPruefer();

                if (minuten >= 0 && minuten < 60)
                {
                    eingabe = $" {stunden:00}:{minuten:00}";
                    falscheEingabe = false;
                }  
                
            }
            return eingabe;
        }

        // Menü für die Erstellung oder Bearbeitung eines Termines (der zweite Teil der Bearbeitung)
        public static void TermineEingabeMenue(int id, char auswahl)
        {
            string? eingabe = null;
            do
            {
                if (falscheEingabe)
                {
                    FalscheEingabe(18, 14, 1);
                }
                falscheEingabe = false;
                Console.Clear();
                KalenderAusdruck();

                if (eingabe == null)
                {
                    Console.Write("\nTag: ");

                    eingabe = Console.ReadLine();

                    eingabe ??= string.Empty;

                    if (Termine.TermineEingabeDatumIstZahl(eingabe) && eingabe.Length == 1)
                    {
                        eingabe = $"0{eingabe}{Kalender.MonatJahrAlsString()}";
                    }
                    else if (Termine.TermineEingabeDatumIstZahl(eingabe) && eingabe.Length == 2 &&
                        Convert.ToInt32($"{eingabe[0]}{eingabe[1]}") <= DateTime.DaysInMonth(Kalender.Jahr, Kalender.Monat) ||
                        Termine.TermineEingabeDatumIstZahl(eingabe) && eingabe.Length == 2 && eingabe[0] == '0')
                    {
                        eingabe += Kalender.MonatJahrAlsString();
                    }

                    else
                    {
                        eingabe = null;
                        falscheEingabe = true;
                    }
                }
                else
                {
                    Console.WriteLine($"\n{eingabe}");
                }
                if (!falscheEingabe)
                {
                    Console.Write("\nUhrzeit:   :  ");

                    Console.SetCursorPosition(9, 13);
                    eingabe += UhrzeitEingabe();
                }

            } while (falscheEingabe);

            Console.Write("Terminname: ");
            eingabe += $" {Console.ReadLine()}";
            switch (auswahl)
            {
                case 'b':
                    Termine.TerminBearbeiten(id, eingabe);
                    break;
                case 'h':
                    Termine.TerminHinzufuegen(eingabe);
                    break;
                default:
                    break;
            }
        }

        // Hilfetext wird angezeigt bei Falscheingabe an bestimmter Stelle (wird zusammen mit FalscheEingabe genutzt)
        static void HilfeText(int cursor, int zeile)
        {
            Console.SetCursorPosition(cursor, zeile);
            Console.WriteLine("Tippe den richtigen Buchstaben aus den Klammern ein.");
        }

        // Warnung, dass eine flasche Eingabe getätigt wurde
        static void FalscheEingabe(int cursor, int zeile, int secunden)
        {
            Console.SetCursorPosition(cursor, zeile);
            Console.WriteLine($"{red}Falsche Eingabe{reset}");
            Thread.Sleep(secunden * 1000);
        }
    }
}

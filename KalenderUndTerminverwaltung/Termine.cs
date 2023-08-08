using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KalenderUndTerminverwaltung
{
    internal class Termine
    {
        // Eine List für alle Termine
        static List<string> termine = new();

        // In den Listen werden die ausgelesenen Tage für den jeweiligen Monat eingespeichert.
        // Falls ein Termin am aktuellen Tag statfindet, wird er im Integer abgespeichert.
        static List<int> tageVergangernerTermine = new();
        static List<int> tageZukuenftigerTermine = new();
        static int tagHeutigerTermine;

        // Pfad für die Datei, die die "termine" List sichert.
        static readonly string pfad = @".\Termine.txt";

        public static List<int> TageVergangernerTermine { get => tageVergangernerTermine; set => tageVergangernerTermine = value; }
        public static List<int> TageZukuenftigerTermine { get => tageZukuenftigerTermine; set => tageZukuenftigerTermine = value; }
        public static int TagHeutigerTermine { get => tagHeutigerTermine; set => tagHeutigerTermine = value; }

        // Prüft ob die Eingabe eine Zahl ist.
        public static bool TermineEingabeDatumIstZahl(string? eingabe)
        {
            if (eingabe == null) return false;
            bool eingabeRichtig = false;
            for (int i = 0; i < eingabe.Length; i++)
            {
                if (eingabe[i] == '.')
                {
                    break;
                }
                if (Char.IsDigit(eingabe[i]))
                {
                    eingabeRichtig = true;
                }
            }
            return eingabeRichtig;
        }

        // String wird in die "termine" List hinzugefügt.
        public static void TerminHinzufuegen(string eingabe)
        {
            termine.Add(eingabe);
            TermineSortierung();
        }

        // Ein Element der "termine" List wird überschrieben und in der Datei abgespeichert.
        public static void TerminBearbeiten(int index, string ausgabeMenue)
        {
            termine[index] = ausgabeMenue;
            TermineSortierung();
        }

        static void TerminLoeschung(int index)
        {
            termine.RemoveAt(index);
            TermineSortierung();
        }

        static void TermineSortierung()
        {
            termine = termine.OrderBy(x => int.Parse($"{x[6]}{x[7]}{x[8]}{x[9]}"))
            .ThenBy(x => int.Parse($"{x[3]}{x[4]}"))
            .ThenBy(x => int.Parse($"{x[0]}{x[1]}")).ToList();
            File.WriteAllLines(pfad, termine);
        }

        // Ein bestimmtes Element aus der "termine" List wird entfernt.
        public static void TerminIDBestaetigung(int index, char auswahl)
        {
            do
            {
                Console.Clear();
                Console.SetCursorPosition(20, 1);

                string aktion;
                switch (auswahl)
                {
                    case 'b':
                        aktion = "bearbeitet";
                        break;
                    case 'l':
                        aktion = "gelöscht";
                        break;
                    default:
                        aktion = string.Empty;
                        break;
                }
                Console.Write($"Soll wirklich dieser Termin {aktion} werden? (j/n)");
                Console.SetCursorPosition(30, 3);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"ID: {index:00} | {termine[index]}");
                Console.ResetColor();
                Console.SetCursorPosition(11, 4);
                Console.Write(":");
                Console.SetCursorPosition(13, 4);
                string? eingabe = Console.ReadLine();

                if (eingabe != null) eingabe = eingabe.ToLower().Trim();
                switch (eingabe)
                {
                    case "j":
                        switch (auswahl)
                        {
                            case 'b':
                                KalenderUndTermineOberflaeche.TermineEingabeMenue(index, 'b');
                                break;
                            case 'l':
                                TerminLoeschung(index);
                                break;
                            default:
                                break;
                        }
                        return;
                    case "n":
                        return;
                    default:
                        Console.SetCursorPosition(22, 8);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Falsche Eingabe");
                        Console.ResetColor();
                        Thread.Sleep(1000);
                        break;
                }
            } while (true);
        }

        // Beschreibt die "termine" List mit Daten aus der Datei. 
        public static void DateiZuListe()
        {
            if (File.Exists(pfad))
            {
                string[] tmp = File.ReadAllLines(pfad);
                termine = tmp.ToList();
            }
            else
            {
                return;
            }
        }

        // Gibt die anzahl der Elemente in der "termine" List wieder.
        public static int ListenLaenge()
        {
            return termine.Count;
        }

        // Gibt nur die Termine aus, die am ausgewählten Monat stattfinden.
        public static void TermineListeAusgabe(bool mitID)
        {
            int index = 0;
            foreach (string termin in termine)
            {
                // int jahr = 10 * Convert.ToInt32(termin[8] - '0') + Convert.ToInt32(termin[9] - '0') + 2000;
                int jahr = Convert.ToInt32($"{termin[6]}{termin[7]}{termin[8]}{termin[9]}");
                // int monat = 10 * Convert.ToInt32(termin[3] - '0') + Convert.ToInt32(termin[4] - '0');
                int monat = Convert.ToInt32($"{termin[3]}{termin[4]}");
                if (monat == Kalender.Monat && jahr == Kalender.Jahr)
                {
                    string id;
                    if (mitID)
                    {
                        id = $"ID: {index:00} | ";
                    }
                    else
                    {
                        id = "";
                    }
                    int tag;

                    try
                    {
                        tag = Convert.ToInt32($"{termin[0]}{termin[1]}");
                    }
                    catch
                    {
                        tag = 0;
                    }

                    DateTime dateTime = new DateTime(jahr, monat, tag);

                    if (dateTime == DateTime.Today)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else if (dateTime < DateTime.Today)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else if (dateTime > DateTime.Today)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.WriteLine($" {id}{termin}");
                    Console.ResetColor();
                }
                
                index++;
            }
        }

        // Nur Termine des ausgewählten Monats werden rausgefiltert und in die verschiedenen Listen reingeschrieben und falls es ein Termin am aktuellen Tag
        // stattfindet fird die Integer-Variable beschrieben.

        // Habe einen Fehler entdeckt, den ich in der kurzen Zeit nicht ausbessern kann.

        public static void TermineTageZurListAuslesung()
        {
            // Fehler gefunden die Listen hatten alte Daten drin. Mit Clear werden sie auf den Anfangsstand gesetzt
            tagHeutigerTermine = 0;
            tageVergangernerTermine.Clear();
            tageZukuenftigerTermine.Clear();
            foreach (string termin in termine)
            {
                //int jahr = 10 * Convert.ToInt32(termin[8] - '0') + Convert.ToInt32(termin[9] - '0') + 2000;
                // $"{termin[6]}{termin[7]}{termin[8]}{termin[9]}") // Zuspät bemerkt, dass es auch so gehen könnte (hatte keine Zeit zum realisierren und testen)
                int jahr = Convert.ToInt32($"{termin[6]}{termin[7]}{termin[8]}{termin[9]}");
                //int monat = 10 * Convert.ToInt32(termin[3] - '0') + Convert.ToInt32(termin[4] - '0');
                // Convert.ToInt32($"{termin[3]}{termin[4]}") // Wie auch hier
                int monat = Convert.ToInt32($"{termin[3]}{termin[4]}");
                if (monat == Kalender.Monat && jahr == Kalender.Jahr)
                {
                    int tag;
                    try
                    {
                        tag = Convert.ToInt32($"{termin[0]}{termin[1]}");
                    }
                    catch
                    {
                        tag = 0;
                    }
                    DateTime dateTime = new DateTime(jahr, monat, tag);
                    if (dateTime == DateTime.Today)
                    {
                        tagHeutigerTermine = tag;
                    }
                    else if (dateTime < DateTime.Today)
                    {
                        tageVergangernerTermine.Add(tag);
                    }
                    else if (dateTime > DateTime.Today)
                    {
                        tageZukuenftigerTermine.Add(tag);
                    }
                }
            }
        }
    }
}

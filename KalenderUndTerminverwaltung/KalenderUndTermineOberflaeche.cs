using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalenderUndTerminverwaltung
{
    internal class KalenderUndTermineOberflaeche
    {
        // Kalender des ausgewählten Monats wird generiert 
        public static void KalenderAusdruck()
        {
            Kalender.KalenderMonatGenerierung();
            Termine.TermineTageZurListAuslesung();

            Console.OutputEncoding = System.Text.Encoding.UTF8; // UTF-8 wird gebraucht um ein Zeichen anzeigen zu können.

            Console.WriteLine($"\n {Kalender.Jahr} {Kalender.MonatAusgabe()}");
            //Console.SetCursorPosition(11, 1);
            //Console.Write($"{Kalender.Jahr}");
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
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                        }
                    }
                    for (int k = 0; k < Termine.TageZukuenftigerTermine.Count; k++)
                    {
                        if (tag == Termine.TageZukuenftigerTermine[k])
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        }
                    }

                    if (Kalender.MonatUndJahrAktuell() && tag == DateTime.Today.Day)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    if (tag == Termine.TagHeutigerTermine && Kalender.MonatUndJahrAktuell())
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }

                    if (tag < 10)
                    {
                        Console.Write($" {Kalender.KalendertageTabelle[i, j]} ");
                    }
                    else
                    {
                        Console.Write($"{Kalender.KalendertageTabelle[i, j]} ");
                    }

                    Console.ResetColor();
                }
                Console.WriteLine("\b|");
            }
            Console.WriteLine("\t          ‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾");
        }

        // Menüs:
        // Das ist das eigentlcihe Hauptmenü aus der aus man in verschiedene andere Menüs navigieren kann.
        public static void KalenderMenue()
        {
            string? eingabe;
            do
            {
                Console.Clear();
                KalenderAusdruck();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n        (V)orheriger  (A)ktueller  (N)ächster");
                Console.ResetColor();
                Console.Write("\n\n\t :\n\n ( )ermine:\n\n");
                Termine.TermineListeAusgabe(false);

                Console.SetCursorPosition(2, 15);
                Console.ForegroundColor= ConsoleColor.Yellow;
                Console.Write("T");

                Console.SetCursorPosition(1, 3);
                Console.ForegroundColor= ConsoleColor.Green;
                Console.Write("(K)alender");

                Console.SetCursorPosition(55, 1);
                Console.ForegroundColor= ConsoleColor.Blue;
                Console.Write("( )Beenden");

                Console.SetCursorPosition(56, 1);
                Console.ForegroundColor= ConsoleColor.Red;
                Console.Write("X");
                Console.ResetColor();

                Console.SetCursorPosition(11, 13);
                eingabe = Console.ReadLine();

                if (eingabe != null) eingabe = eingabe.ToLower().Trim();

                switch (eingabe)
                {
                    case "n":
                        Kalender.NaechsterMonat();
                        break;
                    case "v":
                        Kalender.VorherigerMonat();
                        break;
                    case "a":
                        Kalender.AktuellerMonat();
                        break;
                    case "k":
                        MonatUndJahrEingabeMenue();
                        break;
                    case "t":
                        TerminMenue();
                        break;
                    case "x":
                        Console.SetCursorPosition(0, 30);
                        return;
                    default:
                        HilfeText(3, 10);
                        FalscheEingabe(18, 15, 3);
                        break;
                }
            } while (true);
        }

        // Menü für die Auswahl des Monats und Jahres
        static void MonatUndJahrEingabeMenue()
        {
            int eingabeMonat;
            int eingabeJahr;
            bool falscheiEingabe = false;
            do
            {
                if (falscheiEingabe)
                {
                    FalscheEingabe(18, 13, 1);
                }
                falscheiEingabe = false;
                Console.Clear();
                KalenderAusdruck();

                Console.WriteLine("\n Monat  (mm):\n\n Jahr (jjjj):");


                try
                {
                    Console.SetCursorPosition(14, 11);
                    eingabeMonat = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    eingabeMonat = -1;
                    falscheiEingabe = true;
                }


                if (!falscheiEingabe && eingabeMonat > 0 && eingabeMonat <= 12)
                {
                    Kalender.Monat = eingabeMonat;
                }
                else
                {
                    falscheiEingabe = true;
                }

                if (falscheiEingabe) continue;
                try
                {
                    Console.SetCursorPosition(16, 13);
                    eingabeJahr = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    eingabeJahr = -1;
                    falscheiEingabe = true;
                }

                if (!falscheiEingabe && eingabeJahr > 0)
                {
                    Kalender.Jahr = eingabeJahr;
                }
                else
                {
                    falscheiEingabe = true;
                }

            } while (falscheiEingabe);
        }

        // Das Hauptmenü für Termine aus der man verschiedene Menüs für Termine ansteurn kann.
        static void TerminMenue()
        {
            string? eingabe;
            do
            {
                Console.Clear();
                KalenderAusdruck();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n          (H)inzufügen (B)earbeiten (L)öschen ");
                Console.ResetColor();

                Console.Write("\n\n\t : \n\n Termine:\n\n");
                Termine.TermineListeAusgabe(false);

                Console.SetCursorPosition(55, 1);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("(Z)urück");
                Console.ResetColor();

                Console.SetCursorPosition(11, 13);
                eingabe = Console.ReadLine();

                if (eingabe != null) eingabe = eingabe.ToLower().Trim();

                switch (eingabe)
                {
                    case "h":
                        TermineEingabeMenue(-1, 'h');
                        break;
                    case "l":
                        TermineIDMenue(false);
                        break;
                    case "b":
                        TermineIDMenue(true);
                        break;
                    case "z":
                        return;
                    default:
                        HilfeText(3, 10);
                        FalscheEingabe(18, 15, 3);
                        break;
                }

            } while (true);
        }

        // Menü mit ID-Ausgabe wird jeweils gebraucht für die Löschung oder Bearbeitung von Terminen (Bearbeitung hat 2 Menüs vereint)
        static void TermineIDMenue(bool bearbeiten)
        {
            string? eingabe;
            int eingabeID;
            bool falscheEingabe;
            do
            {
                falscheEingabe = false;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\n (I)D");
                Console.ResetColor();
                Console.WriteLine("\n\n\n           :\n\n     ID: \n\n\n Termine:\n");
                Termine.TermineListeAusgabe(true);

                Console.SetCursorPosition(55, 1);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("(Z)urück");
                Console.ResetColor();

                Console.SetCursorPosition(13, 4);
                eingabe = Console.ReadLine();

                if (eingabe != null) eingabe = eingabe.ToLower().Trim();

                if (eingabe == "z")
                {
                    return;
                }
                else if (eingabe == "i")
                {
                    Console.SetCursorPosition(9, 6);
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
                            //TermineEingabeMenue(true, eingabeID);
                            Termine.TerminIDBestaetigung(eingabeID, 'b');
                        }
                        else
                        {
                            Termine.TerminIDBestaetigung(eingabeID, 'l');
                        }
                    }
                    else
                    {
                        falscheEingabe = true;
                    }
                }
                else
                {
                    falscheEingabe = true;
                }

                if (falscheEingabe)
                {
                    HilfeText(5, 3);
                    FalscheEingabe(22, 8, 3);
                }

            } while (true);
        }

        // Menü für die Erstellung oder Bearbeitung eines Termines (der zweite Teil der Bearbeitung)
        public static void TermineEingabeMenue(int id, char auswahl)
        {
            string? eingabe = null;
            bool falscheEingabe = false;
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
                    int stunden;
                    int minuten;
                    try
                    {
                        Console.SetCursorPosition(9, 13);
                        stunden = Convert.ToInt32(Console.ReadLine());

                    }
                    catch
                    {
                        falscheEingabe = true;
                        stunden = -1;
                    }

                    if (falscheEingabe || !falscheEingabe && stunden < 0 && stunden > 23 )
                    {
                        falscheEingabe = true;
                        continue;
                    }

                    try
                    {
                        Console.SetCursorPosition(12, 13);
                        minuten = Convert.ToInt32(Console.ReadLine());
                    }
                    catch
                    {
                        minuten = -1;
                        falscheEingabe = true;
                    }

                    if (!falscheEingabe &&  minuten >= 0 && minuten < 60)
                    {
                        eingabe += $" {stunden:00}:{minuten:00}";
                    }
                    else
                    {
                        falscheEingabe = true;
                    }
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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Falsche Eingabe");
            Console.ResetColor();
            Thread.Sleep(secunden * 1000);
        }
    }
}

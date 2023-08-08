using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KalenderUndTerminverwaltung
{
    internal class Kalender
    {
        // Zweidimensionales Array für die Ausgeabe des Kalenders in der richtigen Form (Rechteck).
        static string[,] kalendertageTabelle = new string[6, 7];

        // Jahr und Monat für die ausgabe des Kalenders im Terminal
        static int jahr = DateTime.Now.Year;
        static int monat = DateTime.Now.Month;

        public static string[,] KalendertageTabelle { get => kalendertageTabelle; set => kalendertageTabelle = value; }
        public static int Jahr { get => jahr; set => jahr = value; }
        public static int Monat { get => monat; set => monat = value; }

        // Die Tage des Monats werden ab der richtigen Position ausgegeben (damit die Übereinstimmung mit den Wochentagen übereinstimmt).
        public static void KalenderMonatGenerierung()
        {
            DateTime ersterTagDesMonats = new(jahr, monat, 1);

            int tage = DateTime.DaysInMonth(ersterTagDesMonats.Year, ersterTagDesMonats.Month);
            int wochentagDesErstenTagDesMonats = (int)ersterTagDesMonats.DayOfWeek - 1;
            int aufzaehlung = 1;

            for (int i = 0; i < kalendertageTabelle.GetLength(0); i++)
            {
                for (int j = 0; j < kalendertageTabelle.GetLength(1); j++)
                {
                    if (i == 0 && j < wochentagDesErstenTagDesMonats)
                    {
                        kalendertageTabelle[i, j] = " ";
                    }
                    else if (tage > 0)
                    {
                        kalendertageTabelle[i, j] = Convert.ToString(aufzaehlung);
                        aufzaehlung++;
                        tage--;
                    }
                    else
                    {
                        kalendertageTabelle[i, j] = " ";
                    }
                }
            }
        }

        // Führt zur Ausgabe des nächsten Monats
        public static void NaechsterMonat()
        {
            if (monat == 12)
            {
                monat = 1;
                jahr++;
            }
            else
            {
                monat++;
            }
        }

        // Führt zur Ausgabe des vorherigen Monats
        public static void VorherigerMonat()
        {
            if (monat == 1)
            {
                monat = 12;
                jahr--;
            }
            else
            {
                monat--;
            }
        }

        // Führt zur Ausgabe des aktuellen Monats
        public static void AktuellerMonat()
        {
            monat = DateTime.Now.Month;
            jahr = DateTime.Now.Year;
        }

        // Bool-Variable gibt "true" zurück, wenn der angezeigte Monat gleichzeitig der aktuelle ist
        public static bool MonatUndJahrAktuell() 
        {
            return monat == DateTime.Now.Month && jahr == DateTime.Now.Year;
        }

        // Monate als String-Ausgabe
        public static string MonatAusgabe()
        {
            return monat switch
            {
                1 => "Januar",
                2 => "Februar",
                3 => "März",
                4 => "April",
                5 => "Mai",
                6 => "Juni",
                7 => "Juli",
                8 => "August",
                9 => "September",
                10 => "Oktober",
                11 => "November",
                12 => "Dezember",
                _ => "",
            };
        }

        // Monats- und Jahreszahl als String-Ausgabe
        public static string MonatJahrAlsString()
        {
            return $".{monat:00}.{jahr}";
        }

        // Korrekte String-Beschreibung des Tages (mit führender Null)
        public static string TagKorrekteBeschreibung(string eingabe)
        {
            if (eingabe.Length == 1)
            {
                eingabe = $"0{eingabe}{Kalender.MonatJahrAlsString()}";
            }
            else if (eingabe.Length == 2 &&
                10 * Convert.ToInt32(eingabe[0] - '0') + Convert.ToInt32(eingabe[1] - '0') <= DateTime.DaysInMonth(jahr, monat) ||
                eingabe.Length == 2 && eingabe[0] == '0')
            {
                eingabe += Kalender.MonatJahrAlsString();
            }
            return eingabe;
        }
    }
}

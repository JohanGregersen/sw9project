using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using CarDataProject.DataManipulators;

namespace CarDataProject {
    class Program {

        static string path = @"\data\";
        static string filename = "t01c01";
        static string filetype = ".txt";


        static void Main(string[] args) {

            //InsertCarDataIntoDB();

            /*Int64 utmX = 556425;
            Int64 utmY = 6321387;
            string utmZone = "32N";

            GeoCoordinate position = Utility.UtmToLatLng(utmX, utmY, utmZone);*/

            /*
            DBController dbc = new DBController();
            dbc.GetAllLogEntriesWithJSONPoint();
            dbc.Close();
            */
            Int64 carId = 1;
            Dictionary<DayOfWeek, int> result = WeekdayCalculator.CalculateWeekdays(carId);

            Car firstCar = new Car(1);
            DBController dbc = new DBController();
            List<SatHdop> myList = dbc.GetSatHdopForTrip(1, 1);
            dbc.Close();

            //WriteQualityPlotsToFile(myList);


            List<Int64> Hdop = new List<Int64>();
            List<Int64> Sat = new List<Int64>();

            for (int i = 0; i < myList.Count; i++) {
                Hdop.Add(myList[i].Hdop);
                Sat.Add(myList[i].Sat);
            }

            WriteQualityHdopSatToFile(myList);
            Gnuplot();

            myList = myList;

            List<SatHdop> lowQualityEntries = new List<SatHdop>();
            foreach (SatHdop qual in myList) {
                if (qual.Sat <= 3 || qual.Hdop >= 3) {
                    lowQualityEntries.Add(qual);
                }

            }


            /*
            Vejrdata
                Kvalitet af data
                Driving in bad weather
            Dato
                Kørsel på forskellige ugedage
                Forskellige tidspunkter
                    
            */
            lowQualityEntries = lowQualityEntries;
        }

        public static void Gnuplot() {

            string Pgm = @"C:\Program Files (x86)\gnuplot\bin\gnuplot.exe";
            Process extPro = new Process();
            extPro.StartInfo.FileName = Pgm;
            extPro.StartInfo.UseShellExecute = false;
            extPro.StartInfo.RedirectStandardInput = true;
            extPro.Start();

            StreamWriter gnupStWr = extPro.StandardInput;
            
            gnupStWr.WriteLine("plot 'C:\\data\\qualityHdopSat.dat' using 1:2 with lines t \"HDOP\", 'C:\\data\\qualityHdopSat.dat' using 1:3 with lines t \"Sat\"");
            //gnupStWr.WriteLine("plot 'C:\\data\\qualityhdop.dat' with linespoints ls 1");
            //gnupStWr.WriteLine("plot 'C:\\data\\qualitysat.dat' with linespoints ls 1");
            gnupStWr.Flush();


        }

        public static void WriteQualityHdopSatToFile(List<SatHdop> qualityPlots) {
            using (StreamWriter writer = new StreamWriter(path + "qualityHdopSat.dat")) {
                writer.WriteLine("#X, Hdop, Sat");
                for (int i = 0; i < qualityPlots.Count; i++) {
                    writer.WriteLine(i + " " + qualityPlots[i].Hdop + " " + qualityPlots[i].Sat);
                }
            }
        }
        /*
        public static void WriteQualitySatToFile(List<SatHdop> qualityPlots) {
            using (StreamWriter writer = new StreamWriter(path + "qualitysat.dat")) {
                for (int i = 0; i < qualityPlots.Count; i++) {
                    writer.WriteLine(i + " " + qualityPlots[i].Sat);
                }
            }
        }
        */
        public static List<CarLogEntry> ReadCarDataFromFile() {
            return CarLogEntryReader.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), path + filename + filetype));
        }

        public static void InsertCarDataIntoDB() {
            List<CarLogEntry> entries = ReadCarDataFromFile();

            DBController dbc = new DBController();

            foreach (CarLogEntry entry in entries) {
                dbc.AddCarLogEntry(entry);
            }
            dbc.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    class Program {

        static string path = @"\data\";
        static string filename = "t01c01";
        static string filetype = ".txt";

        static void Main(string[] args) {

            Int64 utmX = 556425;
            Int64 utmY = 6321387;
            string utmZone = "32N";

            GeoCoordinate position = Utility.UtmToLatLng(utmX, utmY, utmZone);
            
            List<CarLogEntry> fetchedData = ReadCarDataFromFile();

            /*
            DBController dbc = new DBController();
            for (int i = 0; i < 10; i++) {
                dbc.AddCarLogEntry(fetchedData[i]);
            }

            dbc.Close();*/

        }

        public static List<CarLogEntry> ReadCarDataFromFile() {
            return CarLogEntryReader.ReadFile(Path.Combine(Directory.GetCurrentDirectory(), path + filename + filetype));
        }

        public static void InsertCarDataIntoBD() {
            List<CarLogEntry> entries = ReadCarDataFromFile();

            DBController dbc = new DBController();

            foreach (CarLogEntry entry in entries) {
                dbc.AddCarLogEntry(entry);
            }

            List<CarLogEntry> list = dbc.GetAllLogEntries();
            dbc.Close();
            list.Count();
        }
    }
}

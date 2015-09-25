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

            /*Int64 utmX = 556425;
            Int64 utmY = 6321387;
            string utmZone = "32N";

            GeoCoordinate position = Utility.UtmToLatLng(utmX, utmY, utmZone);*/

            /*
            DBController dbc = new DBController();
            dbc.GetAllLogEntriesWithJSONPoint();
            dbc.Close();
            */

            Car firstCar = new Car(1);
            DBController dbc = new DBController();
            List<SatHdop> myList = dbc.GetSatHdopForTrip(1, 1);
            dbc.Close();

            //WriteQualityPlotsToFile(myList);


            List<Int64> Hdop = new List<Int64>();
            List<Int64> Sat = new List<Int64>();

            for(int i = 0; i < myList.Count; i++) {
                Hdop.Add(myList[i].Hdop);
                Sat.Add(myList[i].Sat);
            }
            
            
            

            myList = myList;

            List<SatHdop> lowQualityEntries = new List<SatHdop>();
            foreach(SatHdop qual in myList) {
                if(qual.Sat <= 3 || qual.Hdop >= 3) {
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
            


            

            //InsertCarDataIntoDB();

        }

        public static void WriteQualityPlotsToFile(List<SatHdop> qualityPlots) {
            using (StreamWriter writer = new StreamWriter(path + "qualityplots.txt")) {
                foreach(SatHdop plots in qualityPlots) {
                    writer.WriteLine(plots.Id + "," + plots.Sat + "," + plots.Hdop);
                }
            }
        }

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

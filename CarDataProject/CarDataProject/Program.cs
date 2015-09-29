using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CarDataProject {
    class Program {

        static string path = @"\data\";
        static string filename = "t01c01";
        static string filetype = ".txt";

        static void Main(string[] args) {

            //InsertCarDataIntoDB();

            //Car car = new Car(1);
            //car.UpdateCarWithTripIdsOptimized(1);

            DefaultStatistic.TripsTaken(1);
            DefaultStatistic.KilometersDriven(1, 1);

            //ReadCarDataFromFile();
            // ValidationPlots.GetAllPlots(1, 5);
            int quahha = 1;

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

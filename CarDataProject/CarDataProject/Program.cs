using System.Collections.Generic;

namespace CarDataProject {
    class Program {
        static string CarLogFile = "t01c01";

        static void Main(string[] args) {

            PerCarCalculator.SaveAllCardata(1);

            int breakpoint = 0;
        }

        private static void InsertCarLogIntoDB() {
            List<CarLogEntry> entries = FileReader.CarLog(CarLogFile);

            DBController dbc = new DBController();

            foreach (CarLogEntry entry in entries) {
                dbc.AddCarLogEntry(entry);
            }

            Car car = new Car(1);
            car.UpdateCarWithTripIdsOptimized(1);
            
            dbc.UpdateEntryWithPointAndMpoint(1);
            dbc.Close();
        }
    }
}

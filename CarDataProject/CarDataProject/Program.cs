using System;
using System.Collections.Generic;

namespace CarDataProject {
    class Program {
        static string CarLogFile = "t01c01";

        static void Main(string[] args) {

            int breakpoint = 0;
        }

        private static void InsertCarLogIntoDB() {
            List<Fact> facts = FileReader.CarLog(CarLogFile);

            DBController dbc = new DBController();

            foreach (Fact fact in facts) {
                dbc.AddCarLogEntry(fact);
            }

            OldCar car = new OldCar(1);
            car.UpdateCarWithTripIdsOptimized(1);
            
            dbc.UpdateEntryWithPointAndMpoint(1);
            dbc.Close();
        }
    }
}

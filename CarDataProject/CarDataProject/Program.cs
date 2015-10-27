using System;
using System.Collections.Generic;

namespace CarDataProject {
    class Program {
        static void Main(string[] args) {
            int breakpoint = 0;
        }

        static void InsertCarLogIntoDB() {
            List<Fact> facts = INFATI.ReadLog(1, 1);
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

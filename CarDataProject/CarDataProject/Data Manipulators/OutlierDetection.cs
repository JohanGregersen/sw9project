using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    public static class OutlierDetection {
        public static void OutlierHandling(Int16 carId) {
            List<Int64> tripIds = new List<long>();
            DBController dbc = new DBController();
            tripIds = dbc.GetTripIdsByCarId(carId);

            Int64 outliersCount = 0;

            foreach (Int64 tripId in tripIds) {
                List<Int64> outliers = OutlierDetection.RemoveOutliers(carId, tripId);
                if (outliers.Count != 0) {
                    dbc.UpdateEntriesWithNoTrip(outliers);
                }
                outliersCount += OutlierDetection.RemoveOutliers(carId, tripId).Count();
            }
            dbc.Close();
            Console.WriteLine("Car " + carId + " removed " + outliersCount + " outliers.");
        }

        public static List<Int64> RemoveOutliers(Int16 carId, Int64 tripId) {
            List<Fact> facts = new List<Fact>();

            DBController dbc = new DBController();
            facts = dbc.GetFactsByCarIdAndTripId(carId, tripId);
            dbc.Close();

            List<Int64> OutlierIds = new List<Int64>();

            for (int i = 1; i < facts.Count(); i++) {
                double distanceTravelled = facts[i].Spatial.MPoint.GetDistanceTo(facts[i - 1].Spatial.MPoint);
                double currentSpeed = Math.Max(facts[i].Measure.Speed, facts[i - 1].Measure.Speed) + 15;
                currentSpeed = currentSpeed * 3.6;

                if (distanceTravelled / (facts[i].Temporal.Timestamp - facts[i - 1].Temporal.Timestamp).TotalSeconds > currentSpeed || facts[i].Temporal.Timestamp == facts[i - 1].Temporal.Timestamp) {
                    OutlierIds.Add(facts[i].EntryId);
                }
            }


            if (OutlierIds.Count > facts.Count * 0.1) {
                //Console.WriteLine(tripId + " is invalid");
            }

            return OutlierIds;
        }
    }
}

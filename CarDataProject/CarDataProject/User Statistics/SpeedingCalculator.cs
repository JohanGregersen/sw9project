using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject.User_Statistics {
    public static class SpeedingCalculator {

        public static TimeSpan Time(Int16 carId, int tripId, int minimumSpeedingAmount) {
            TimeSpan timeSped = new TimeSpan();

            DBController dbc = new DBController();
            List<Tuple<Timestamp, int, int>> speedData = dbc.GetSpeedDataByTrip(carId, tripId);
            dbc.Close();

            bool speeding = false;

            DateTime speedingStartPoint = new DateTime();

            //For the entire trip
            foreach (Tuple<Timestamp, int, int> entry in speedData) {

                //If entry indicates speeding
                if (entry.Item3 + minimumSpeedingAmount <= entry.Item2) {

                    //If not previously speeding, remember the time where driver began speeding
                    if (!speeding) {
                        speeding = true;
                        speedingStartPoint = entry.Item1.timestamp;
                    }

                //If driver is not speeding
                } else {
                    //If previously speeding, add the speeding-time to the total
                    if (speeding) {
                        speeding = false;
                        timeSped += entry.Item1.timestamp - speedingStartPoint;
                    }
                }
            }

            return timeSped;

        }
    }
}

using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class SpeedingStatistics {
        public static TimeSpan Time(Int16 carId, Int64 tripId, Int16 minimumSpeedingAmount, TimeSpan ignorableSpeedingTime) {
            TimeSpan timeSped = new TimeSpan();

            DBController dbc = new DBController();
            List<Fact> speedData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            bool speeding = false;
            DateTime speedingStartPoint = new DateTime();

            //For the entire trip
            foreach (Fact fact in speedData) {
                //If entry indicates speeding
                if (fact.Measure.Speed >= fact.Segment.MaxSpeed + minimumSpeedingAmount) {

                    //If not previously speeding, remember the time where driver began speeding
                    if (!speeding) {
                        speeding = true;
                        speedingStartPoint = fact.Temporal.Timestamp;
                    }

                //If driver is not speeding
                } else {
                    //If previously speeding, add the speeding-time to the total
                    if (speeding) {
                        speeding = false;

                        if (fact.Temporal.Timestamp - speedingStartPoint >= ignorableSpeedingTime) {
                            timeSped += fact.Temporal.Timestamp - speedingStartPoint;
                        }
                    }
                }
            }

            return timeSped;
        }
    }
}

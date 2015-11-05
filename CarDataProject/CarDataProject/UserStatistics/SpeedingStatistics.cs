using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class SpeedingStatistics {
        public static TimeSpan Time(Int16 carId, Int64 tripId, Int16 ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            TimeSpan timeSped = new TimeSpan();

            DBController dbc = new DBController();
            List<Fact> speedData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            bool speeding = false;
            DateTime speedingStartPoint = new DateTime();
            double speedingDistance = 0;

            //For the entire trip
            foreach (Fact fact in speedData) {
                //If entry indicates speeding
                if (fact.Measure.Speed >= fact.Segment.MaxSpeed + ignorableSpeed) {
                    speedingDistance += fact.Spatial.DistanceToLag;

                    //If not previously speeding, remember the time where driver began speeding
                    if (!speeding) {
                        speedingStartPoint = fact.Temporal.Timestamp;
                        speeding = true;
                    }

                //If driver is not speeding
                } else {
                    //If previously speeding, add the speeding-time to the total
                    if (speeding) {
                        if (fact.Temporal.Timestamp - speedingStartPoint >= ignorableTime && speedingDistance > ignorableDistance) {
                            timeSped += fact.Temporal.Timestamp - speedingStartPoint;
                        }
                        speedingDistance = 0;
                        speeding = false;
                    }
                }
            }

            //Handle case where last entry in trip is still speeding
            if (speeding) {
                if (speedData[speedData.Count - 1].Temporal.Timestamp - speedingStartPoint >= ignorableTime && speedingDistance > ignorableDistance) {
                    timeSped += speedData[speedData.Count - 1].Temporal.Timestamp - speedingStartPoint;
                }
            }

            return timeSped;
        }

        public static double Distance(Int16 carId, Int64 tripId, Int16 ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            double distanceSped = 0;

            DBController dbc = new DBController();
            List<Fact> speedData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            bool speeding = false;
            DateTime speedingStartPoint = new DateTime();
            double speedingDistance = 0;

            //For the entire trip
            foreach (Fact fact in speedData) {
                //If entry indicates speeding
                if (fact.Measure.Speed >= fact.Segment.MaxSpeed + ignorableSpeed) {
                    speedingDistance += fact.Spatial.DistanceToLag;

                    //If not previously speeding, remember the time where driver began speeding
                    if (!speeding) {
                        speedingStartPoint = fact.Temporal.Timestamp;
                        speeding = true;
                    }

                    //If driver is not speeding
                } else {
                    //If previously speeding, add the speeding-time to the total
                    if (speeding) {
                        if (fact.Temporal.Timestamp - speedingStartPoint >= ignorableTime && speedingDistance > ignorableDistance) {
                            distanceSped += speedingDistance;
                        }

                        speedingDistance = 0;
                        speeding = false;
                    }
                }
            }

            //Handle case where last entry in trip is still speeding
            if (speeding) {
                if (speedData[speedData.Count - 1].Temporal.Timestamp - speedingStartPoint >= ignorableTime && speedingDistance > ignorableDistance) {
                    distanceSped += speedingDistance;
                }
            }

                return distanceSped;
        }
    }
}

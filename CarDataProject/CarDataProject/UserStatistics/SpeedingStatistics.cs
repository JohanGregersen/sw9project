using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class SpeedingStatistics {
        public static TimeSpan TimeAbove(Int16 carId, Int64 tripId, Int16 ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            TimeSpan timeSped = new TimeSpan();

            DBController dbc = new DBController();
            List<Fact> speedData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            bool speeding = false;
            DateTime speedingStartPoint = speedData[0].Temporal.Timestamp;
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

        public static TimeSpan TimeBelow(Int16 carId, Int64 tripId, Int16 ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            TimeSpan totalTimeBelow = new TimeSpan();

            DBController dbc = new DBController();
            List<Fact> speedData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            bool belowLimit = false;
            DateTime belowLimitStartPoint = speedData[0].Temporal.Timestamp;
            double distanceBelowLimit = 0;

            //For the entire trip
            foreach (Fact fact in speedData) {
                //If entry indicates not speeding
                if (fact.Measure.Speed <= fact.Segment.MaxSpeed + ignorableSpeed) {
                    distanceBelowLimit += fact.Spatial.DistanceToLag;

                    //If not previously below limit, remember the time where driver got below limit
                    if (!belowLimit) {
                        belowLimitStartPoint = fact.Temporal.Timestamp;
                        belowLimit = true;
                    }

                    //If driver is speeding
                } else {
                    //If previously below limit, add the time below limit to the total
                    if (belowLimit) {
                        if (fact.Temporal.Timestamp - belowLimitStartPoint >= ignorableTime && distanceBelowLimit > ignorableDistance) {
                            totalTimeBelow += fact.Temporal.Timestamp - belowLimitStartPoint;
                        }
                        distanceBelowLimit = 0;
                        belowLimit = false;
                    }
                }
            }

            //Handle case where last entry in trip is below limit
            if (belowLimit) {
                if (speedData[speedData.Count - 1].Temporal.Timestamp - belowLimitStartPoint >= ignorableTime && distanceBelowLimit > ignorableDistance) {
                    totalTimeBelow += speedData[speedData.Count - 1].Temporal.Timestamp - belowLimitStartPoint;
                }
            }

            return totalTimeBelow;
        }

        public static double DistanceAbove(Int16 carId, Int64 tripId, Int16 ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            double distanceSped = 0;

            DBController dbc = new DBController();
            List<Fact> speedData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            bool speeding = false;
            DateTime speedingStartPoint = speedData[0].Temporal.Timestamp;
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

        public static double DistanceBelow(Int16 carId, Int64 tripId, Int16 ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            double totalDistanceBelow = 0;

            DBController dbc = new DBController();
            List<Fact> speedData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            bool belowLimit = false;
            DateTime belowLimitStartPoint = speedData[0].Temporal.Timestamp;
            double distanceBelowLimit = 0;

            //For the entire trip
            foreach (Fact fact in speedData) {
                //If entry indicates not speeding
                if (fact.Measure.Speed <= fact.Segment.MaxSpeed + ignorableSpeed) {
                    distanceBelowLimit += fact.Spatial.DistanceToLag;

                    //If not previously below limit, remember the time where driver got below limit
                    if (!belowLimit) {
                        belowLimitStartPoint = fact.Temporal.Timestamp;
                        belowLimit = true;
                    }

                    //If driver is speeding
                } else {
                    //If previously below limit, add the time below limit to the total
                    if (belowLimit) {
                        if (fact.Temporal.Timestamp - belowLimitStartPoint >= ignorableTime && distanceBelowLimit > ignorableDistance) {
                            totalDistanceBelow += distanceBelowLimit;
                        }
                        distanceBelowLimit = 0;
                        belowLimit = false;
                    }
                }
            }

            //Handle case where last entry in trip is below limit
            if (belowLimit) {
                if (speedData[speedData.Count - 1].Temporal.Timestamp - belowLimitStartPoint >= ignorableTime && distanceBelowLimit > ignorableDistance) {
                    totalDistanceBelow += distanceBelowLimit;
                }
            }

            return totalDistanceBelow;
        }

        public static double PercentageTimeAbove(Int16 carId, Int64 tripId, Int16 ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            TimeSpan timeAbove = TimeAbove(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
            TimeSpan timeBelow = TimeBelow(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);

            return timeAbove.Ticks / (timeAbove.Ticks + timeBelow.Ticks) * 100;
        }

        public static double PercentageTimeBelow(Int16 carId, Int64 tripId, Int16 ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            TimeSpan timeAbove = TimeAbove(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
            TimeSpan timeBelow = TimeBelow(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);

            return timeBelow.Ticks / (timeAbove.Ticks + timeBelow.Ticks) * 100;
        }

        public static double PercentageDistanceAbove(Int16 carId, Int64 tripId, Int16 ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            double distanceAbove = DistanceAbove(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
            double distanceBelow = DistanceBelow(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);

            return distanceAbove / (distanceAbove + distanceBelow) * 100;
        }

        public static double PercentageDistanceBelow(Int16 carId, Int64 tripId, Int16 ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            double distanceAbove = DistanceAbove(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
            double distanceBelow = DistanceBelow(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);

            return distanceBelow / (distanceAbove + distanceBelow) * 100;
        }
    }
}

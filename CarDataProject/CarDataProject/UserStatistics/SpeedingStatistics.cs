using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class SpeedingStatistics {
        public static TimeSpan TimeAbove(Int16 carId, Int64 tripId, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            TimeSpan timeSped = new TimeSpan();

            DBController dbc = new DBController();
            List<Fact> speedData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            DateTime speedingStartPoint = speedData[0].Temporal.Timestamp;
            double speedingDistance = 0;
            bool speeding = false;

            //For the entire trip
            foreach (Fact fact in speedData) {
                //If entry indicates speeding
                if (fact.Measure.Speed > fact.Segment.MaxSpeed + ignorableSpeed) {
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

        public static TimeSpan TimeBelow(Int16 carId, Int64 tripId, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
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

        public static double DistanceAbove(Int16 carId, Int64 tripId, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
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
                if (fact.Measure.Speed > fact.Segment.MaxSpeed + ignorableSpeed) {
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

        public static double DistanceBelow(Int16 carId, Int64 tripId, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
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

        public static double PercentageTimeAbove(Int16 carId, Int64 tripId, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            TimeSpan timeAbove = TimeAbove(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
            TimeSpan timeBelow = TimeBelow(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);

            return timeAbove.Ticks / (timeAbove.Ticks + timeBelow.Ticks) * 100;
        }

        public static double PercentageTimeBelow(Int16 carId, Int64 tripId, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            TimeSpan timeAbove = TimeAbove(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
            TimeSpan timeBelow = TimeBelow(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);

            return timeBelow.Ticks / (timeAbove.Ticks + timeBelow.Ticks) * 100;
        }

        public static double PercentageDistanceAbove(Int16 carId, Int64 tripId, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            double distanceAbove = DistanceAbove(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
            double distanceBelow = DistanceBelow(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);

            return distanceAbove / (distanceAbove + distanceBelow) * 100;
        }

        public static double PercentageDistanceBelow(Int16 carId, Int64 tripId, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            double distanceAbove = DistanceAbove(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);
            double distanceBelow = DistanceBelow(carId, tripId, ignorableSpeed, ignorableTime, ignorableDistance);

            return distanceBelow / (distanceAbove + distanceBelow) * 100;
        }

        public static double PercentageTimeAboveInThreshold(Int16 carId, Int64 tripId, double lowerPercentage, double upperPercentage, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            lowerPercentage += 100;
            upperPercentage += 100;
            TimeSpan totalTimeInThreshold = new TimeSpan(0, 0, 0);
            TimeSpan totalTimeOutsideThreshold = new TimeSpan(0, 0, 0);

            DBController dbc = new DBController();
            List<Fact> speedData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            DateTime speedingStartPoint = speedData[0].Temporal.Timestamp;
            double speedingDistance = 0;
            TimeSpan timeInThreshold = new TimeSpan(0, 0, 0);
            TimeSpan timeOutsideThreshold = new TimeSpan(0, 0, 0);
            bool speeding = false;

            //For the entire trip
            for (int i = 1; i < speedData.Count; i++) {
                //If entry indicates speeding
                double percentageOfMaxSpeed = ((speedData[i].Measure.Speed - ignorableSpeed) / speedData[i].Segment.MaxSpeed) * 100;

                if (percentageOfMaxSpeed > 100) {
                    speedingDistance += speedData[i].Spatial.DistanceToLag;

                    if (percentageOfMaxSpeed >= lowerPercentage && percentageOfMaxSpeed <= upperPercentage) {
                        timeInThreshold += speedData[i].Temporal.SecondsToLag;
                    } else {
                        timeOutsideThreshold += speedData[i].Temporal.SecondsToLag;
                    }
                    //If not previously speeding, remember the time where driver began speeding
                    if (!speeding) {
                        speedingStartPoint = speedData[i].Temporal.Timestamp;
                        speeding = true;
                    }

                    //If driver is not speeding
                } else {
                    //If previously speeding, add the speeding-time to the total
                    if (speeding) {
                        if (speedData[i].Temporal.Timestamp - speedingStartPoint >= ignorableTime && speedingDistance > ignorableDistance) {
                            totalTimeInThreshold += timeInThreshold;
                            totalTimeOutsideThreshold += timeOutsideThreshold;
                        }
                        timeInThreshold = new TimeSpan(0, 0, 0);
                        timeOutsideThreshold = new TimeSpan(0, 0, 0);
                        speedingDistance = 0;
                        speeding = false;
                    }
                }
            }

            //Handle case where last entry in trip is still speeding
            if (speeding) {
                if (speedData[speedData.Count - 1].Temporal.Timestamp - speedingStartPoint >= ignorableTime && speedingDistance > ignorableDistance) {
                    totalTimeInThreshold += timeInThreshold;
                    totalTimeOutsideThreshold += timeOutsideThreshold;
                }
            }

            return totalTimeInThreshold.Ticks / (totalTimeInThreshold.Ticks + totalTimeOutsideThreshold.Ticks) * 100;
        }

        public static double PercentageDistanceAboveInThreshold(Int16 carId, Int64 tripId, double lowerPercentage, double upperPercentage, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            lowerPercentage += 100;
            upperPercentage += 100;
            double totalDistanceInThreshold = 0;
            double totalDistanceOutsideThreshold = 0;

            DBController dbc = new DBController();
            List<Fact> speedData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            DateTime speedingStartPoint = speedData[0].Temporal.Timestamp;
            double speedingDistance = 0;
            double distanceInThreshold = 0;
            double distanceOutsideThreshold = 0;
            bool speeding = false;

            //For the entire trip
            for (int i = 1; i < speedData.Count; i++) {
                //If entry indicates speeding
                double percentageOfMaxSpeed = ((speedData[i].Measure.Speed - ignorableSpeed) / speedData[i].Segment.MaxSpeed) * 100;

                if (percentageOfMaxSpeed > 100) {
                    speedingDistance += speedData[i].Spatial.DistanceToLag;

                    if (percentageOfMaxSpeed >= lowerPercentage && percentageOfMaxSpeed <= upperPercentage) {
                        distanceInThreshold += speedData[i].Spatial.DistanceToLag;
                    } else {
                        distanceOutsideThreshold += speedData[i].Spatial.DistanceToLag;
                    }
                    //If not previously speeding, remember the time where driver began speeding
                    if (!speeding) {
                        speedingStartPoint = speedData[i].Temporal.Timestamp;
                        speeding = true;
                    }

                    //If driver is not speeding
                } else {
                    //If previously speeding, add the speeding-distance to the total
                    if (speeding) {
                        if (speedData[i].Temporal.Timestamp - speedingStartPoint >= ignorableTime && speedingDistance > ignorableDistance) {
                            totalDistanceInThreshold += distanceInThreshold;
                            totalDistanceOutsideThreshold += distanceOutsideThreshold;
                        }
                        distanceInThreshold = 0;
                        distanceOutsideThreshold = 0;
                        speedingDistance = 0;
                        speeding = false;
                    }
                }
            }

            //Handle case where last entry in trip is still speeding
            if (speeding) {
                if (speedData[speedData.Count - 1].Temporal.Timestamp - speedingStartPoint >= ignorableTime && speedingDistance > ignorableDistance) {
                    totalDistanceInThreshold += distanceInThreshold;
                    totalDistanceOutsideThreshold += distanceOutsideThreshold;
                }
            }

            return totalDistanceInThreshold / (totalDistanceInThreshold + totalDistanceOutsideThreshold) * 100;
        }
    }
}

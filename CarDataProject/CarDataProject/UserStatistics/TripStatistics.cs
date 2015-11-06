using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace CarDataProject {
    class TripStatistics {
        public static void WriteAll(Int16 carId, Int64 tripId) {
            FileWriter.DefaultTripStatistics(carId, tripId);
        }

        public static TimeSpan Duration(Int16 carId, Int64 tripId) {
            DBController dbc = new DBController();
            List<TemporalInformation> timestamps = dbc.GetTimestampsByCarIdAndTripId(carId, tripId);
            dbc.Close();

            return timestamps[timestamps.Count - 1].Timestamp - timestamps[0].Timestamp;
        }

        public static double Distance(Int16 carId, Int64 tripId) {
            DBController dbc = new DBController();
            List<SpatialInformation> entries = dbc.GetMPointsByCarIdAndTripId(carId, tripId);
            dbc.Close();

            double distance = 0;

            for (int i = 1; i < entries.Count - 1; i++) {
                distance += entries[i].MPoint.GetDistanceTo(entries[i - 1].MPoint);
            }

            return distance /= 1000;
        }

        public static List<Fact> Accelerations(Int16 carId, Int64 tripId) {
            DBController dbc = new DBController();
            List<Fact> accelerationData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            for (int i = 1; i < accelerationData.Count(); i++) {
                //Acceleration = Velocity change / Time
                double velocityChange = accelerationData[i].Measure.Speed - accelerationData[i - 1].Measure.Speed;
                TimeSpan time = accelerationData[i].Temporal.Timestamp - accelerationData[i - 1].Temporal.Timestamp;
                double acceleration = velocityChange / time.Seconds;

                accelerationData[i].Measure.Acceleration = acceleration;
            }

            return accelerationData;
        }

        public static Dictionary<int, List<Fact>> PositiveAccelerationsAboveThreshold(Int16 carId, Int64 tripId, double threshold, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            Dictionary<int, List<Fact>> positiveAccelerationsAboveThreshold = new Dictionary<int, List<Fact>>();
            int accelerationCount = 0;

            DBController dbc = new DBController();
            List<Fact> speedData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            DateTime accelerationStartPoint = speedData[0].Temporal.Timestamp;
            double accelerationDistance = 0;
            bool aboveThreshold = false;
            List<Fact> accelerationPoints = new List<Fact>();

            //For the entire trip
            foreach (Fact fact in speedData) {
                //If entry indicates acceleration above threshold, and above ignorable speeds
                if (fact.Measure.Speed > ignorableSpeed && fact.Measure.Acceleration > threshold) {
                    accelerationDistance += fact.Spatial.DistanceToLag;

                    //If not previously above threshold, remember the time where driver began accelerating
                    if (!aboveThreshold) {
                        accelerationStartPoint = fact.Temporal.Timestamp;
                        accelerationPoints.Add(fact);
                        aboveThreshold = true;
                    }

                    //If driver is not above threshold
                } else {
                    //If previously above threshold, add the facts result
                    if (aboveThreshold) {
                        if (fact.Temporal.Timestamp - accelerationStartPoint >= ignorableTime && accelerationDistance > ignorableDistance) {
                            positiveAccelerationsAboveThreshold.Add(accelerationCount, accelerationPoints);
                            accelerationCount++;
                        }
                        accelerationPoints = new List<Fact>();
                        accelerationDistance = 0;
                        aboveThreshold = false;
                    }
                }
            }

            //Handle case where last entry in trip is still speeding
            if (aboveThreshold) {
                if (speedData[speedData.Count - 1].Temporal.Timestamp - accelerationStartPoint >= ignorableTime && accelerationDistance > ignorableDistance) {
                    positiveAccelerationsAboveThreshold.Add(accelerationCount, accelerationPoints);
                }
            }

            return positiveAccelerationsAboveThreshold;
        }

        public static Dictionary<int, List<Fact>> NegativeAccelerationsBelowThreshold(Int16 carId, Int64 tripId, double threshold, double ignorableSpeed, TimeSpan ignorableTime, double ignorableDistance) {
            Dictionary<int, List<Fact>> negativeAccelerationsBelowThreshold = new Dictionary<int, List<Fact>>();
            int accelerationCount = 0;

            DBController dbc = new DBController();
            List<Fact> speedData = dbc.GetSpeedInformationByCarIdAndTripId(carId, tripId);
            dbc.Close();

            DateTime accelerationStartPoint = speedData[0].Temporal.Timestamp;
            double accelerationDistance = 0;
            bool belowThreshold = false;
            List<Fact> accelerationPoints = new List<Fact>();

            //For the entire trip
            foreach (Fact fact in speedData) {
                //If entry indicates acceleration below threshold, and above ignorable speeds
                if (fact.Measure.Speed > ignorableSpeed && fact.Measure.Acceleration < threshold) {
                    accelerationDistance += fact.Spatial.DistanceToLag;

                    //If not previously below threshold, remember the time where driver began accelerating
                    if (!belowThreshold) {
                        accelerationStartPoint = fact.Temporal.Timestamp;
                        accelerationPoints.Add(fact);
                        belowThreshold = true;
                    }

                    //If driver is not below threshold
                } else {
                    //If previously below threshold, add the facts result
                    if (belowThreshold) {
                        if (fact.Temporal.Timestamp - accelerationStartPoint >= ignorableTime && accelerationDistance > ignorableDistance) {
                            negativeAccelerationsBelowThreshold.Add(accelerationCount, accelerationPoints);
                            accelerationCount++;
                        }

                        accelerationPoints = new List<Fact>();
                        accelerationDistance = 0;
                        belowThreshold = false;
                    }
                }
            }

            //Handle case where last entry in trip is still speeding
            if (belowThreshold) {
                if (speedData[speedData.Count - 1].Temporal.Timestamp - accelerationStartPoint >= ignorableTime && accelerationDistance > ignorableDistance) {
                    negativeAccelerationsBelowThreshold.Add(accelerationCount, accelerationPoints);
                }
            }

            return negativeAccelerationsBelowThreshold;
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                 
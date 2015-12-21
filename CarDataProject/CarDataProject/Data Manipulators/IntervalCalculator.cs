using System;
using System.Collections.Generic;
using System.Linq;

namespace CarDataProject {
    public static class IntervalCalculator {
        public static List<double> RoadType(Trip trip, List<Fact> facts) {
            SortedDictionary<Global.Enums.RoadType, double> metersDistribution = new SortedDictionary<Global.Enums.RoadType, double>();

            DBController dbc = new DBController();
            List<Global.Enums.RoadType> roadTypes = dbc.GetRoadTypesByTripId(trip.TripId);
            dbc.Close();

            //Populate dictionary with all policy-required roadtypes
            foreach (Global.Enums.RoadType roadType in DefaultPolicy.RoadTypes) {
                metersDistribution.Add(roadType, 0);
            }

            //Calculate meters driven on each roadtype
            for (int i = 1; i < facts.Count; i++) {
                if (DefaultPolicy.RoadTypes.Contains(roadTypes[i])) {
                    metersDistribution[roadTypes[i]] += facts[i].Spatial.DistanceToLag;
                }
            }

            List<double> resultList = metersDistribution.Values.ToList();

            //Calculate the distribution in percentages of whole trip
            for (int i = 0; i < resultList.Count; i++) {
                resultList[i] = (resultList[i] / trip.MetersDriven) * 100;
            }

            return resultList;
        }

        public static List<double> CriticalTime(Trip trip, List<Fact> facts) {
            SortedDictionary<TimeInterval, double> criticalTimes = new SortedDictionary<TimeInterval, double>();

            //Populate dictionary with all policy-required time-intervals
            foreach (TimeInterval interval in DefaultPolicy.TimeIntervals) {
                criticalTimes.Add(interval, 0);
            }

            //Calculate meters driven in every interval
            for (int i = 1; i < facts.Count; i++) {
                foreach(TimeInterval interval in DefaultPolicy.TimeIntervals) {
                    if(interval.ActiveDays.Contains(facts[i].Temporal.Timestamp.DayOfWeek) && interval.StartTime <= facts[i].Temporal.Timestamp.TimeOfDay && interval.EndTime >= facts[i].Temporal.Timestamp.TimeOfDay) {
                        criticalTimes[interval] += facts[i].Spatial.DistanceToLag;
                    }
                }
            }

            //Calculate the distribution in percentages of whole trip
            for(int i = 0; i < DefaultPolicy.TimeIntervals.Count; i++) {
                criticalTimes[DefaultPolicy.TimeIntervals[i]] = (criticalTimes[DefaultPolicy.TimeIntervals[i]] / trip.MetersDriven) * 100;
            }

            return criticalTimes.Values.ToList();
        }

        public static List<double> Speeding(Trip trip, List<Fact> facts) {
            SortedDictionary<Interval, double> speedingIntervals = new SortedDictionary<Interval, double>();

            //Populate dictionary with all policy-required speeding-intervals
            foreach (Interval interval in DefaultPolicy.SpeedingIntervals) {
                speedingIntervals.Add(interval, 0);
            }

            //Calculate meters sped in every interval
            for (int i = 1; i < facts.Count; i++) {
                if (facts[i].Flag.Speeding) {
                    double percentage = facts[i].Measure.Speed / facts[i].Segment.MaxSpeed * 100 - 100;
                    
                    foreach (Interval interval in DefaultPolicy.SpeedingIntervals) {
                        if (interval.Contains(percentage)) {
                            speedingIntervals[interval] += facts[i].Spatial.DistanceToLag;
                        }
                    }
                }
            }

            //Calculate the distribution in percentages of total meters sped
            foreach (Interval interval in DefaultPolicy.SpeedingIntervals) {
                speedingIntervals[interval] = (speedingIntervals[interval] / trip.MetersSped) * 100;
            }

            return speedingIntervals.Values.ToList();
        }

        //Mulige forbedrninger: Acceleration, brake og jerk er per gps point, men ignorerer tid - interpoler?
        public static List<double> Acceleration(Trip trip, List<Fact> facts) {
            SortedDictionary<Interval, double> accelerationIntervals = new SortedDictionary<Interval, double>();

            //Populate dictionary with all policy-required acceleration-intervals
            foreach (Interval interval in DefaultPolicy.AccelerationIntervals) {
                accelerationIntervals.Add(interval, 0);
            }

            //Add an acceleration in the fitting interval per point measured
            for (int i = 1; i < facts.Count; i++) {
                if (facts[i].Flag.Braking) {
                    foreach (Interval interval in accelerationIntervals.Keys) {
                        if (interval.Contains(facts[i].Measure.Acceleration)) {
                            accelerationIntervals[interval]++;
                        }
                    }
                }
            }

            //Calculate the distribution of points in percentages
            foreach (Interval interval in DefaultPolicy.AccelerationIntervals) {
                accelerationIntervals[interval] = (accelerationIntervals[interval] / trip.AccelerationCount) * 100;
            }

            return accelerationIntervals.Values.ToList();
        }

        public static List<double> Brake(Trip trip, List<Fact> facts) {
            SortedDictionary<Interval, double> brakeIntervals = new SortedDictionary<Interval, double>();
            double brakeValue;

            //Populate dictionary with all policy-required brake-intervals
            foreach (Interval interval in DefaultPolicy.BrakeIntervals) {
                brakeIntervals.Add(interval, 0);
            }

            //Add a brake in the fitting interval per point measured
            for (int i = 1; i < facts.Count; i++) {
                if (facts[i].Flag.Braking) {

                    //Convert all brakes to positive numbers, to make them comparable with policy variables
                    brakeValue = facts[i].Measure.Acceleration * -1;

                    foreach (Interval interval in DefaultPolicy.BrakeIntervals) {
                        if (interval.Contains(brakeValue)) {
                            brakeIntervals[interval]++;
                        }
                    }
                }
            }

            //Calculate the distribution of points in percentages
            foreach (Interval interval in DefaultPolicy.BrakeIntervals) {
                brakeIntervals[interval] = (brakeIntervals[interval] / trip.BrakeCount) * 100;
            }

            return brakeIntervals.Values.ToList();
        }

        public static List<double> Jerk(Trip trip, List<Fact> facts) {
            SortedDictionary<Interval, double> jerkIntervals = new SortedDictionary<Interval, double>();
            double jerkValue;

            //Populate dictionary with all policy-required jerk-intervals
            foreach (Interval interval in DefaultPolicy.JerkIntervals) {
                jerkIntervals.Add(interval, 0);
            }

            //Add a jerk in the fitting interval per point measured
            for (int i = 1; i < facts.Count; i++) {
                if (facts[i].Flag.Braking) {

                    //Convert all jerks to positive numbers - makes life easier
                    if (facts[i].Measure.Jerk > 0) {
                        jerkValue = facts[i].Measure.Jerk;
                    } else {
                        jerkValue = facts[i].Measure.Jerk * -1;
                    }

                    foreach (Interval interval in DefaultPolicy.JerkIntervals) {
                        if (interval.Contains(facts[i].Measure.Jerk)) {
                            jerkIntervals[interval]++;
                        }
                    }
                }
            }

            //Calculate the distribution of points in percentages
            foreach (Interval interval in DefaultPolicy.JerkIntervals) {
                jerkIntervals[interval] = (jerkIntervals[interval] / trip.JerkCount) * 100;
            }

            return jerkIntervals.Values.ToList();
        }
    }
}
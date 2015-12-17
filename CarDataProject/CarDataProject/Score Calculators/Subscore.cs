using System;
using System.Collections.Generic;

namespace CarDataProject {
    /*
    * Subscore calculates several different scores for a trip using a variety of parameters
    */
    public static class Subscore {     
        /*
        * RoadTypes score is based on the risk associated with driving on certain types of roads
        */
        public static double RoadTypes (double metersDriven, List<double> intervals, List<double> weights) {
            Dictionary<double, double> weightedIntervals = new Dictionary<double, double>();

            for(int i = 0; i < weights.Count - 1; i++) {
                weightedIntervals.Add(weights[i], intervals[i]);
            }

            //Return the amount of meters added by the calculated multiplier
            return metersDriven * AccumulatedMultiplier(weightedIntervals);
        }

        /*
        * CriticalTimePeriod score is based on the risk associated with driving during certain times of the day
        */
        public static double CriticalTimePeriod(double metersDriven, List<double> intervals, List<double> weights) {
            Dictionary<double, double> weightedIntervals = new Dictionary<double, double>();

            for (int i = 0; i < weights.Count - 1; i++) {
                weightedIntervals.Add(weights[i], intervals[i]);
            }

            //Return the amount of meters added by the calculated multiplier
            return metersDriven * AccumulatedMultiplier(weightedIntervals);
        }
        
        /*
        * Speeding score is based on the amount and severity of speeding during a trip
        */
        public static double Speeding(double metersSped, List<double> intervals, List<double> weights, double a, double b, double c, double poly) {
            Dictionary<double, double> weightedIntervals = new Dictionary<double, double>();

            for (int i = 0; i < weights.Count - 1; i++) {
                weightedIntervals.Add(weights[i], intervals[i]);
            }


            //Find a multiplier on the severity of speeding
            double x = AccumulatedMultiplier(weightedIntervals);

            //Provide polynomial functionality to make bad distributions count for more
            double totalMultiplier = Math.Pow(a * x, poly) + b * x + c;

            //x becomes the amount of meters that should be added to the trip
            return totalMultiplier * metersSped;
        }

        /*
        * Acceleration score is based on the amount and severity of accelerations during a trip
        */
        public static double Accelerations(double accelerations, List<double> intervals, double accelerationPrice, List<double> weights, double a, double b, double c, double poly) {
            Dictionary<double, double> weightedIntervals = new Dictionary<double, double>();

            for (int i = 0; i < weights.Count - 1; i++) {
                weightedIntervals.Add(weights[i], intervals[i]);
            }

            //Find a multiplier on the severity of accelerations
            double x = AccumulatedMultiplier(weightedIntervals);

            //Provide polynomial functionality to make bad distributions count for more
            double totalMultiplier = Math.Pow(a * x, poly) + b * x + c;

            //x becomes the amount of meters that should be added to the trip
            return totalMultiplier * accelerations * accelerationPrice;

        }

        /*
        * Brakes score is based on the amount and severity of brakes during a trip
        */
        public static double Brakes(double brakes, List<double> intervals, double brakePrice, List<double> weights, double a, double b, double c, double poly) {
            Dictionary<double, double> weightedIntervals = new Dictionary<double, double>();

            for (int i = 0; i < weights.Count - 1; i++) {
                weightedIntervals.Add(weights[i], intervals[i]);
            }


            //Find a multiplier on the severity of brakes
            double x = AccumulatedMultiplier(weightedIntervals);

            //Provide polynomial functionality to make bad distributions count for more
            double totalMultiplier = Math.Pow(a * x, poly) + b * x + c;

            //x becomes the amount of meters that should be added to the trip
            return totalMultiplier * brakes * brakePrice;
        }

        /*
        * Jerks score is based on the amount and severity of jerks during a trip
        */
        public static double Jerks(double jerks, List<double> intervals, double jerkPrice, List<double> weights, double a, double b, double c, double poly) {
            Dictionary<double, double> weightedIntervals = new Dictionary<double, double>();

            for (int i = 0; i < weights.Count - 1; i++) {
                weightedIntervals.Add(weights[i], intervals[i]);
            }

            //Find a multiplier on the severity of jerks
            double x = AccumulatedMultiplier(weightedIntervals);

            //Provide polynomial functionality to make bad distributions count for more
            double totalMultiplier = Math.Pow(a * x, poly) + b * x + c;

            //x becomes the amount of meters that should be added to the trip
            return totalMultiplier * jerks * jerkPrice;
        }

        /*
        * Helper method to calculate the combined weight for all intervals
        * The result returned is a multiplier for a trip distance
        */
        private static double AccumulatedMultiplier(Dictionary<double, double> weightedIntervals) {
            double result = 0;

            foreach (KeyValuePair<double, double> weightedInterval in weightedIntervals) {
                result += weightedInterval.Key * weightedInterval.Value;
            }

            return result / 100 - 1;
        }
    }
}

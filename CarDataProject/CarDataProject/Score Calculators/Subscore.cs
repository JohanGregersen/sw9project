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
            List<KeyValuePair<double, double>> weightedIntervals = new List<KeyValuePair<double, double>>();

            for(int i = 0; i < weights.Count; i++) {
                weightedIntervals.Add(new KeyValuePair<double, double>(weights[i], intervals[i]));
            }

            //Find the amount of meters added by the calculated multiplier
            double result = metersDriven * AccumulatedMultiplier(weightedIntervals);

            //Handle result if NaN
            if (double.IsNaN(result)) {
                return 0;
            } else {
                return result;
            }
        }

        /*
        * CriticalTimePeriod score is based on the risk associated with driving during certain times of the day
        */
        public static double CriticalTimePeriod(double metersDriven, List<double> intervals, List<double> weights) {
            List<KeyValuePair<double, double>> weightedIntervals = new List<KeyValuePair<double, double>>();

            for (int i = 0; i < weights.Count; i++) {
                weightedIntervals.Add(new KeyValuePair<double, double>(weights[i], intervals[i]));
            }

            //Find the amount of meters added by the calculated multiplier
            double result = metersDriven * AccumulatedMultiplier(weightedIntervals);

            //Handle result if NaN
            if (double.IsNaN(result)) {
                return 0;
            } else {
                return result;
            }
        }
        
        /*
        * Speeding score is based on the amount and severity of speeding during a trip
        */
        public static double Speeding(double metersSped, List<double> intervals, List<double> weights, double a, double b, double c, double poly) {
            List<KeyValuePair<double, double>> weightedIntervals = new List<KeyValuePair<double, double>>();

            for (int i = 0; i < weights.Count; i++) {
                weightedIntervals.Add(new KeyValuePair<double, double>(weights[i], intervals[i]));
            }

            //Find a multiplier on the severity of speeding
            double x = AccumulatedMultiplier(weightedIntervals);

            //Provide polynomial functionality to make bad distributions count for more
            double totalMultiplier = Math.Pow(a * x, poly) + b * x + c;

            //Amount of meters that should be added to the trip
            double result =  totalMultiplier * metersSped;

            //Handle result if NaN
            if (double.IsNaN(result)) {
                return 0;
            } else {
                return result;
            }
        }

        /*
        * Acceleration score is based on the amount and severity of accelerations during a trip
        */
        public static double Accelerations(double accelerations, List<double> intervals, double accelerationPrice, List<double> weights, double a, double b, double c, double poly) {
            List<KeyValuePair<double, double>> weightedIntervals = new List<KeyValuePair<double, double>>();

            for (int i = 0; i < weights.Count; i++) {
                weightedIntervals.Add(new KeyValuePair<double, double>(weights[i], intervals[i]));
            }

            //Find a multiplier on the severity of accelerations
            double x = AccumulatedMultiplier(weightedIntervals);

            //Provide polynomial functionality to make bad distributions count for more
            double totalMultiplier = Math.Pow(a * x, poly) + b * x + c;

            //Amount of meters that should be added to the trip
            double result = totalMultiplier * accelerations * accelerationPrice;

            //Handle result if NaN
            if (double.IsNaN(result)) {
                return 0;
            } else {
                return result;
            }
        }

        /*
        * Brakes score is based on the amount and severity of brakes during a trip
        */
        public static double Brakes(double brakes, List<double> intervals, double brakePrice, List<double> weights, double a, double b, double c, double poly) {
            List<KeyValuePair<double, double>> weightedIntervals = new List<KeyValuePair<double, double>>();

            for (int i = 0; i < weights.Count; i++) {
                weightedIntervals.Add(new KeyValuePair<double, double>(weights[i], intervals[i]));
            }

            //Find a multiplier on the severity of brakes
            double x = AccumulatedMultiplier(weightedIntervals);

            //Provide polynomial functionality to make bad distributions count for more
            double totalMultiplier = Math.Pow(a * x, poly) + b * x + c;

            //Amount of meters that should be added to the trip
            double result = totalMultiplier * brakes * brakePrice;

            //Handle result if NaN
            if (double.IsNaN(result)) {
                return 0;
            } else {
                return result;
            }
        }

        /*
        * Jerks score is based on the amount and severity of jerks during a trip
        */
        public static double Jerks(double jerks, List<double> intervals, double jerkPrice, List<double> weights, double a, double b, double c, double poly) {
            List<KeyValuePair<double, double>> weightedIntervals = new List<KeyValuePair<double, double>>();

            for (int i = 0; i < weights.Count; i++) {
                weightedIntervals.Add(new KeyValuePair<double, double>(weights[i], intervals[i]));
            }

            //Find a multiplier on the severity of jerks
            double x = AccumulatedMultiplier(weightedIntervals);

            //Provide polynomial functionality to make bad distributions count for more
            double totalMultiplier = Math.Pow(a * x, poly) + b * x + c;

            //Amount of meters that should be added to the trip
            double result = totalMultiplier * jerks * jerkPrice;

            //Handle result if NaN
            if (double.IsNaN(result)) {
                return 0;
            } else {
                return result;
            }
        }

        /*
        * Helper method to calculate the combined weight for all intervals
        * The result returned is a multiplier for a trip distance
        */
        private static double AccumulatedMultiplier(List<KeyValuePair<double, double>> weightedIntervals) {
            double result = 0;
            double intervalTotal = 0;

            foreach (KeyValuePair<double, double> weightedInterval in weightedIntervals) {
                intervalTotal += weightedInterval.Value;
                result += weightedInterval.Key * weightedInterval.Value;
            }

            //Fix missing rounding data as well as possible
            if (intervalTotal < 100) {
                result += (100 % intervalTotal);
            } else if (intervalTotal > 100) {
                result -= (intervalTotal - 100);
            }

            return result / 100 - 1;
        }
    }
}

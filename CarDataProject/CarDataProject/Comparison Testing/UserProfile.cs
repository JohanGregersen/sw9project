using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    class UserProfile {

        public static double AverageTripPercentage(List<Trip> trips) {

            int count = 0;
            double percentage = 0;

            foreach(Trip trip in trips) {
                percentage = +TripStatistics.tripPercentage(trip.TripId);
                count++;
            }

            return percentage / count;
        }

        public static Dictionary<string, double> AverageMetricPercentage(List<Trip> trips) {

            Dictionary<string, double> result = new Dictionary<string, double>();
            Dictionary<string, double> tempDict = new Dictionary<string, double>();
            int count = 0;

            foreach(Trip trip in trips) {
                tempDict = MetricPercentage(trip);
                foreach(KeyValuePair<string, double> kvp in tempDict) {
                    if(!result.ContainsKey(kvp.Key)) {
                        result.Add(kvp.Key, kvp.Value);
                    } else {
                        result[kvp.Key] = +kvp.Value;
                    }       
                }
                count++;
            }

            foreach(KeyValuePair<string, double> kvp in result) {
                result[kvp.Key] = kvp.Value / count;
            }

            return result;
        }

        public static Dictionary<string, double> MetricPercentage(Trip trip) {

            Dictionary<string, double> result = new Dictionary<string, double>();

            result.Add("Roadtypes", (Subscore.RoadTypes(trip.MetersDriven, IntervalHelper.Decode(trip.IntervalInformation.RoadTypesInterval), DefaultPolicy.RoadTypeWeights) / trip.MetersDriven));
            result.Add("CriticalTimePeriod", (Subscore.CriticalTimePeriod(trip.MetersDriven, IntervalHelper.Decode(trip.IntervalInformation.CriticalTimeInterval), DefaultPolicy.CriticalTimeWeights) / trip.MetersDriven));
            result.Add("Speeding", (Subscore.Speeding(trip.MetersSped, IntervalHelper.Decode(trip.IntervalInformation.SpeedInterval),DefaultPolicy.SpeedingWeights, DefaultPolicy.A, DefaultPolicy.B, DefaultPolicy.C, DefaultPolicy.Poly) / trip.MetersDriven));
            result.Add("Accelerations", (Subscore.Accelerations(trip.AccelerationCount, IntervalHelper.Decode(trip.IntervalInformation.AccelerationInterval), DefaultPolicy.AccelerationPrice, DefaultPolicy.AccelerationWeights, DefaultPolicy.A, DefaultPolicy.B, DefaultPolicy.C, DefaultPolicy.Poly) / trip.MetersDriven));
            result.Add("Brakes",(Subscore.Brakes(trip.BrakeCount,IntervalHelper.Decode(trip.IntervalInformation.BrakingInterval), DefaultPolicy.BrakePrice, DefaultPolicy.BrakeWeights, DefaultPolicy.A, DefaultPolicy.B, DefaultPolicy.C, DefaultPolicy.Poly) / trip.MetersDriven));
            result.Add("Jerks",(Subscore.Jerks(trip.JerkCount, IntervalHelper.Decode(trip.IntervalInformation.JerkInterval), DefaultPolicy.JerkPrice, DefaultPolicy.JerkWeights, DefaultPolicy.A, DefaultPolicy.B, DefaultPolicy.C, DefaultPolicy.Poly) / trip.MetersDriven));

            return result;
        }
    }
}

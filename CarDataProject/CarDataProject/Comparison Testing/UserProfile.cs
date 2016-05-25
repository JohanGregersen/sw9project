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

        public static Dictionary<string, double> AverageMetricNormalized(List<Trip> trips) {

            Dictionary<string, double> result = new Dictionary<string, double>();
            Dictionary<string, double> tempDict = new Dictionary<string, double>();
            int count = 0;

            foreach (Trip trip in trips) {
                tempDict = MetricNormalized(trip);
                foreach (KeyValuePair<string, double> kvp in tempDict) {
                    if (!result.ContainsKey(kvp.Key)) {
                        result.Add(kvp.Key, kvp.Value);
                    } else {
                        result[kvp.Key] = +kvp.Value;
                    }
                }
                count++;
            }

            foreach (KeyValuePair<string, double> kvp in result) {
                result[kvp.Key] = kvp.Value / count;
            }

            return result;

        }

        public static Dictionary<string, List<double>> AverageMetricDegree(List<Trip> trips) {
            
            //It is not relational to the lenght of the trip, maybe it should be?

            Dictionary<string, List<double>> result = new Dictionary<string, List<double>>();

            List<double> temproadtypelist = new List<double>();
            List<double> tempcriticaltimelist = new List<double>();
            List<double> tempspeedinglist = new List<double>();
            List<double> tempacclist = new List<double>();
            List<double> tempbrakelist = new List<double>();
            List<double> tempjerklist = new List<double>();

            foreach (Trip trip in trips) {

                if (trip == trips.First()) {
                    result["Roadtypes"] = IntervalHelper.Decode(trip.IntervalInformation.RoadTypesInterval);
                    result["Criticaltime"] = IntervalHelper.Decode(trip.IntervalInformation.CriticalTimeInterval);
                    result["Speeding"] = IntervalHelper.Decode(trip.IntervalInformation.SpeedInterval);
                    result["Accelerations"] = IntervalHelper.Decode(trip.IntervalInformation.AccelerationInterval);
                    result["Brakes"] = IntervalHelper.Decode(trip.IntervalInformation.BrakingInterval);
                    result["Jerks"] = IntervalHelper.Decode(trip.IntervalInformation.JerkInterval);
                } else {
                    temproadtypelist = IntervalHelper.Decode(trip.IntervalInformation.RoadTypesInterval);
                    tempcriticaltimelist = IntervalHelper.Decode(trip.IntervalInformation.CriticalTimeInterval);
                    tempspeedinglist = IntervalHelper.Decode(trip.IntervalInformation.SpeedInterval);
                    tempacclist = IntervalHelper.Decode(trip.IntervalInformation.AccelerationInterval);
                    tempbrakelist = IntervalHelper.Decode(trip.IntervalInformation.BrakingInterval);
                    tempjerklist = IntervalHelper.Decode(trip.IntervalInformation.JerkInterval);

                    for (int i = 0; i < 8; i++) {
                        result["Roadtypes"][i] = +temproadtypelist[i];
                        result["Criticaltime"][i] = +tempcriticaltimelist[i];
                        result["Speeding"][i] = +tempspeedinglist[i];
                        result["Accelerations"][i] = +tempacclist[i];
                        result["Brakes"][i] = +tempbrakelist[i];
                        result["Jerks"][i] = +tempjerklist[i];
                    }
                }
            }

            for (int i = 0; i < 8; i++) {
                result["Roadtypes"][i] = result["Roadtypes"][i] / trips.Count;
                result["Criticaltime"][i] = result["Criticaltime"][i] / trips.Count;
                result["Speeding"][i] = result["Speeding"][i] / trips.Count;
                result["Accelerations"][i] = result["Accelerations"][i] / trips.Count;
                result["Brakes"][i] = result["Brakes"][i] / trips.Count;
                result["Jerks"][i] = result["Jerks"][i] / trips.Count;
            }
            
            return result;
        }

        public static Dictionary<string, double> MetricPercentage(Trip trip) {

            Dictionary<string, double> result = new Dictionary<string, double>();

            result.Add("Roadtypes", trip.RoadTypeScore / trip.MetersDriven * 100);
            result.Add("CriticalTimePeriod", trip.CriticalTimeScore / trip.MetersDriven * 100);
            result.Add("Speeding", trip.SpeedingScore / trip.MetersDriven * 100);
            result.Add("Accelerations", trip.AccelerationScore / trip.MetersDriven * 100);
            result.Add("Brakes", trip.BrakeScore / trip.MetersDriven * 100);
            result.Add("Jerks", trip.JerkScore / trip.MetersDriven * 100);

            return result;
        }

        public static Dictionary<string, double> MetricNormalized(Trip trip) {
        
            Dictionary<string, double> result = new Dictionary<string, double>();
            DBController dbc = new DBController();

            List<Fact> facts = dbc.GetFactsByTripId(trip.TripId);

            result.Add("Speeding", 0);
            result.Add("Accelerations", 0);
            result.Add("Brakes", 0);
            result.Add("Jerks", 0);

            foreach (Fact fact in facts) {
                if (fact.Flag.Speeding) {
                    result["Speeding"]++;
                }
            }
            if (result["Speeding"] != 0) {
                result["Speeding"] = result["Speeding"] / facts.Count * 100;
            }
            result["Accelerations"] = trip.AccelerationCount / facts.Count * 100;
            result["Brakes"] = trip.BrakeCount / facts.Count * 100;
            result["Jerks"] = trip.JerkCount / facts.Count * 100;
            
            return result;
        }
    }
}

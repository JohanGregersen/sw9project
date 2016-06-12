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
                percentage += TripStatistics.tripPercentage(trip.TripId);
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
                        result[kvp.Key] = result[kvp.Key] + kvp.Value;
                    }       
                }
                count++;
            }

            foreach(KeyValuePair<string, double> kvp in result.ToList()) {
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
                        result[kvp.Key] += kvp.Value;
                    }
                }
                count++;
            }

            foreach (KeyValuePair<string, double> kvp in result.ToList()) {
                result[kvp.Key] = kvp.Value / count;
            }

            return result;

        }

        public static Dictionary<string, List<double>> AverageMetricDegree(List<Trip> trips) {
            
            //It is not relational to the lenght of the trip, maybe it should be?

            Dictionary<string, List<double>> result = new Dictionary<string, List<double>>();

            double temproadcount = 0;
            double tempcriticalcount = 0;
            double tempspeedingcount = 0;
            double tempacccount = 0;
            double tempbrakecount = 0;
            double tempjerkcount = 0;


            foreach (Trip trip in trips) {

                List<double> templist = new List<double>();

                if (trip.IntervalInformation.RoadTypesInterval != 0 && !result.ContainsKey("Roadtypes")) {
                    result["Roadtypes"] = IntervalHelper.Decode(trip.IntervalInformation.RoadTypesInterval);
                    temproadcount++;
                } else if (trip.IntervalInformation.RoadTypesInterval != 0 && result.ContainsKey("Roadtypes")) {
                    templist = IntervalHelper.Decode(trip.IntervalInformation.RoadTypesInterval);
                    temproadcount++;
                    for (int i = 0; i < 8; i++) {
                        result["Roadtypes"][i] += templist[i];
                    }
                }

                if (trip.IntervalInformation.CriticalTimeInterval != 0 && !result.ContainsKey("Criticaltime")) {
                    result["Criticaltime"] = IntervalHelper.Decode(trip.IntervalInformation.CriticalTimeInterval);
                    tempcriticalcount++;
                } else if (trip.IntervalInformation.CriticalTimeInterval != 0 && result.ContainsKey("Criticaltime")) {
                    templist = IntervalHelper.Decode(trip.IntervalInformation.CriticalTimeInterval);
                    tempcriticalcount++;
                    for (int i = 0; i < 8; i++) {
                        result["Criticaltime"][i] += templist[i];
                    }
                }

                if (trip.IntervalInformation.SpeedInterval != 0 && !result.ContainsKey("Speeding")) {
                    result["Speeding"] = IntervalHelper.Decode(trip.IntervalInformation.SpeedInterval);
                    tempspeedingcount++;
                } else if (trip.IntervalInformation.SpeedInterval != 0 && result.ContainsKey("Speeding")) {
                    templist = IntervalHelper.Decode(trip.IntervalInformation.SpeedInterval);
                    tempspeedingcount++;
                    for (int i = 0; i < 8; i++) {
                        result["Speeding"][i] += templist[i];
                    }
                }

                if (trip.IntervalInformation.AccelerationInterval != 0 && !result.ContainsKey("Accelerations")) {
                    result["Accelerations"] = IntervalHelper.Decode(trip.IntervalInformation.AccelerationInterval);
                    tempacccount++;
                } else if (trip.IntervalInformation.AccelerationInterval != 0 && result.ContainsKey("Accelerations")) {
                    templist = IntervalHelper.Decode(trip.IntervalInformation.AccelerationInterval);
                    tempacccount++;
                    for (int i = 0; i < 8; i++) {
                        result["Accelerations"][i] += templist[i];
                    }
                }

                if (trip.IntervalInformation.BrakingInterval != 0 && !result.ContainsKey("Brakes")) {
                    result["Brakes"] = IntervalHelper.Decode(trip.IntervalInformation.BrakingInterval);
                    tempbrakecount++;
                } else if (trip.IntervalInformation.BrakingInterval != 0 && result.ContainsKey("Brakes")) {
                    templist = IntervalHelper.Decode(trip.IntervalInformation.BrakingInterval);
                    tempbrakecount++;
                    for (int i = 0; i < 8; i++) {
                        result["Brakes"][i] += templist[i];
                    }
                }

                if (trip.IntervalInformation.JerkInterval != 0 && !result.ContainsKey("Jerks")) {
                    result["Jerks"] = IntervalHelper.Decode(trip.IntervalInformation.JerkInterval);
                    tempjerkcount++;
                } else if (trip.IntervalInformation.JerkInterval != 0 && result.ContainsKey("Jerks")) {
                    templist = IntervalHelper.Decode(trip.IntervalInformation.JerkInterval);
                    tempjerkcount++;
                    for (int i = 0; i < 8; i++) {
                        result["Jerks"][i] += templist[i];
                    }
                }
            }

            List<double> nulliste = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 8; i++) {

                if (result.ContainsKey("Roadtypes") && temproadcount != 0) {
                    result["Roadtypes"][i] = result["Roadtypes"][i] / temproadcount;
                } else if(!result.ContainsKey("Roadtypes")) {
                    result["Roadtypes"] = nulliste;
                    result["Roadtypes"][i] = 0;
                } else if(result.ContainsKey("Roadtypes") && temproadcount == 0) {
                    result["Roadtypes"][i] = 0;
                }

                if (result.ContainsKey("Criticaltime") && tempcriticalcount != 0) {
                    result["Criticaltime"][i] = result["Criticaltime"][i] / tempcriticalcount;
                } else if(!result.ContainsKey("Criticaltime")){
                    result["Criticaltime"] = nulliste;
                    result["Criticaltime"][i] = 0;
                } else if (result.ContainsKey("Criticaltime") && tempcriticalcount == 0) {
                    result["Criticaltime"][i] = 0;
                }

                if (result.ContainsKey("Speeding") && tempspeedingcount != 0) {
                    result["Speeding"][i] = result["Speeding"][i] / tempspeedingcount;
                } else if (!result.ContainsKey("Speeding")) {
                    result["Speeding"] = nulliste;
                    result["Speeding"][i] = 0;
                } else if (result.ContainsKey("Speeding") && tempspeedingcount == 0) {
                    result["Speeding"][i] = 0;
                }

                if (result.ContainsKey("Accelerations") && tempacccount != 0) {
                    result["Accelerations"][i] = result["Accelerations"][i] / tempacccount;
                } else if (!result.ContainsKey("Accelerations")) {
                    result["Accelerations"] = nulliste;
                    result["Accelerations"][i] = 0;
                } else if (result.ContainsKey("Accelerations") && tempacccount == 0) {
                    result["Accelerations"][i] = 0;
                }

                if (result.ContainsKey("Brakes") && tempbrakecount != 0) {
                    result["Brakes"][i] = result["Brakes"][i] / tempbrakecount;
                } else if(!result.ContainsKey("Brakes")) {
                    result["Brakes"] = nulliste;
                    result["Brakes"][i] = 0;
                } else if (result.ContainsKey("Brakes") && tempbrakecount == 0) {
                    result["Brakes"][i] = 0;
                }

                if (result.ContainsKey("Jerks") && tempjerkcount != 0) {
                    result["Jerks"][i] = result["Jerks"][i] / tempjerkcount;
                } else if (!result.ContainsKey("Jerks")) {
                    result["Jerks"] = nulliste;
                    result["Jerks"][i] = 0;
                } else if (result.ContainsKey("Jerks") && tempjerkcount == 0) {
                    result["Jerks"][i] = 0;
                }
            }
            return result;
        }

        public static Dictionary<string, double> MetricPercentage(Trip trip) {

            Dictionary<string, double> result = new Dictionary<string, double>();
            if (trip.RoadTypeScore != 0 || trip.MetersDriven != 0) {
                result.Add("Roadtypes", trip.RoadTypeScore / trip.MetersDriven * 100);
            } else {
                result.Add("Roadtypes", 0);
            }

            if (trip.CriticalTimeScore != 0 || trip.MetersDriven != 0) {
                result.Add("CriticalTimePeriod", trip.CriticalTimeScore / trip.MetersDriven * 100);
            } else {
                result.Add("CriticalTimePeriod", 0);
            }

            if (trip.SpeedingScore != 0 || trip.MetersDriven != 0) {
                result.Add("Speeding", trip.SpeedingScore / trip.MetersDriven * 100);
            } else {
                result.Add("Speeding", 0);
            }

            if (trip.AccelerationScore != 0 || trip.MetersDriven != 0) {
                result.Add("Accelerations", trip.AccelerationScore / trip.MetersDriven * 100);
            } else {
                result.Add("Accelerations", 0);
            }

            if (trip.BrakeScore != 0 || trip.MetersDriven != 0) {
                result.Add("Brakes", trip.BrakeScore / trip.MetersDriven * 100);
            } else {
                result.Add("Brakes", 0);
            }

            if (trip.JerkScore != 0 || trip.MetersDriven != 0) {
                result.Add("Jerks", trip.JerkScore / trip.MetersDriven * 100);
            } else {
                result.Add("Jerks", 0);
            }

            return result;
        }

        public static Dictionary<string, double> MetricNormalized(Trip trip) {
        
            Dictionary<string, double> result = new Dictionary<string, double>();
            DBController dbc = new DBController();

            List<Fact> facts = dbc.GetFactsByTripIdNoQuality(trip.TripId);

            double tempint = trip.MetersDriven / 1000;


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
                result["Speeding"] = result["Speeding"] / tempint;
            }

            result["Accelerations"] = ((double)trip.AccelerationCount / tempint);
            result["Brakes"] = ((double)trip.BrakeCount / tempint);
            result["Jerks"] = ((double)trip.JerkCount / tempint);

            dbc.Close();

            return result;
        }

        public static string print(Dictionary<string, double> dict) {
            StringBuilder builder = new StringBuilder();

            foreach (KeyValuePair<string, double> kvp in dict) {
                Console.WriteLine(kvp.Key + " : " + kvp.Value.ToString());
                string tempstring = kvp.Key + " : " + kvp.Value.ToString();
                builder.Append(tempstring);
                builder.Append("\n");
            }

            builder.Append("\n");
            return builder.ToString();
        }

        public static string print(Dictionary<string, List<double>> dict) {
            StringBuilder builder = new StringBuilder();

            foreach(KeyValuePair<string, List<double>> kvp in dict) {

                StringBuilder tempbuilder = new StringBuilder();

                for(int i = 0; i < kvp.Value.Count; i++) {
                    tempbuilder.Append(kvp.Value.ElementAt(i).ToString()).Append(" | ");
                    builder.Append(kvp.Value.ElementAt(i).ToString()).Append(" | ");
                }
                
                Console.WriteLine(kvp.Key + " : " + tempbuilder.ToString());
                builder.Append("\n");
            }
            builder.Append("\n");
            return builder.ToString();
        }
    }
}

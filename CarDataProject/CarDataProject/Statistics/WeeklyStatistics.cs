using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CarDataProject {
    class WeeklyStatistics {
        public static void WriteAll(Int16 carId) {
            FileWriter.WeeklyAverageTripDistance(carId, AverageTripDistance(carId));
        }

        public static void PlotAll(Int16 carId) {
            GnuplotHelper.Plot(Global.CarStatistics.WeeklyAverageTripDistanceFile(carId), Global.CarStatistics.WeeklyAverageTripDistanceGraph(carId), true, 1, 3, "Kilometers Per Week");
        }

        //Returns average kilometers per trip for each week number
        public static Dictionary<KeyValuePair<int, int>, double> AverageTripDistance(Int16 carId) {
            //Get calendar for week number reference
            DateTimeFormatInfo formatInformation = DateTimeFormatInfo.CurrentInfo;
            Calendar calendar = formatInformation.Calendar;

            //Dictionary of <Year, Week>, <TripCount, Distance>
            Dictionary<KeyValuePair<int, int>, KeyValuePair<int, double>> results = new Dictionary<KeyValuePair<int, int>, KeyValuePair<int, double>();

            //Get trips to get distances from
            DBController dbc = new DBController();
            List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);


            foreach (Int64 tripId in tripIds) {
                List<Fact> facts = dbc.GetSpatioTemporalByCarIdAndTripId(carId, tripId);
                //Save starting points of trip, in case trip is split up later
                int startIndex = 0;
                double distanceForWeek = 0;
                int startWeek = calendar.GetWeekOfYear(facts[0].Temporal.Timestamp, formatInformation.CalendarWeekRule, formatInformation.FirstDayOfWeek);
                int startYear = facts[0].Temporal.Timestamp.Year;

                for (int i = 1; i < facts.Count; i++) {

                    //If week or year changes in the trip, save that part from startIndex to current index
                    int week = calendar.GetWeekOfYear(facts[i].Temporal.Timestamp, formatInformation.CalendarWeekRule, formatInformation.FirstDayOfWeek);
                    int year = facts[i].Temporal.Timestamp.Year;
                    if (week != startWeek || year != startYear) {
                        for (int j = startIndex; j <= i; j++) {
                            distanceForWeek += facts[j].Spatial.DistanceToLag;
                        }

                        KeyValuePair<int, int> tripPartKey = new KeyValuePair<int, int>(startYear, startWeek);

                        if (results.ContainsKey(tripPartKey)) {
                            results[tripPartKey] = new KeyValuePair<int, double>(results[tripPartKey].Key + 1, results[tripPartKey].Value + distanceForWeek);
                        } else {
                            results.Add(new KeyValuePair<int, int>(startYear, startWeek), new KeyValuePair<int, double>(1, distanceForWeek));
                        }

                        //Update starting points for new part of trip
                        startIndex = i;
                        startWeek = week;
                        startYear = year;
                        distanceForWeek = 0;
                    }
                }

                //At the end of the trip, save current part from startIndex to current index.
                for (int j = startIndex; j < facts.Count; j++) {
                    distanceForWeek += facts[j].Spatial.DistanceToLag;
                }

                KeyValuePair<int, int> key = new KeyValuePair<int, int>(startYear, startWeek);

                if (results.ContainsKey(key)) {
                    results[key] = new KeyValuePair<int, double>(results[key].Key + 1, results[key].Value + distanceForWeek);
                } else {
                    results.Add(new KeyValuePair<int, int>(startYear, startWeek), new KeyValuePair<int, double>(1, distanceForWeek));
                }
            }

            dbc.Close();

            //Calculate average distance per week, and return result
            Dictionary<KeyValuePair<int, int>, double> weeklyAverageTripDistance = new Dictionary<KeyValuePair<int, int>, double>();

            foreach (KeyValuePair<KeyValuePair<int, int>, KeyValuePair<int, double>> kvp in results) {
                weeklyAverageTripDistance.Add(kvp.Key, (kvp.Value.Value / kvp.Value.Key));
            }

            return weeklyAverageTripDistance;
        }
    }
}

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
        public static Dictionary<int, double> AverageTripDistance(Int16 carId) {
            //Get calendar for week number reference
            DateTimeFormatInfo formatInformation = DateTimeFormatInfo.CurrentInfo;
            Calendar calendar = formatInformation.Calendar;


            Dictionary<int, double> weeklyDistance = new Dictionary<int, double>();
            Dictionary<int, int> weeklyTrips = new Dictionary<int, int>();
            Dictionary<int, double> weeklyAverageTripDistance = new Dictionary<int, double>();
            int weekno;
            
            DBController dbc = new DBController();
            List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);

            foreach (Int64 tripId in tripIds) {
                List<TemporalInformation> temporals = dbc.GetTimestampsByCarIdAndTripId(carId, tripId);

            }

            for (int i = 1; i <= tripCount; i++) {
                List<Timestamp> time = dbc.GetTimestampsByCarIdAndTripId(carId, i);
                weekno = cal.GetWeekOfYear(time[0].timestamp, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                if (!weeklykm.ContainsKey(weekno)) {
                    weeklykm.Add(weekno, TripStatistics.GetKilometersDriven(carId, i));
                    tripsofweek.Add(weekno, 1);
                } else {
                    weeklykm[weekno] = weeklykm[weekno] + TripStatistics.GetKilometersDriven(carId, i);
                    tripsofweek[weekno] = tripsofweek[weekno] + 1;
                }
            }

            dbc.Close();

            foreach (KeyValuePair<int, double> kvp in weeklykm) {
                finaldict.Add(kvp.Key, (kvp.Value / tripsofweek[kvp.Key]));
            }

            return finaldict;
        }

        private void GroupTemporalInformationByWeek(List<TemporalInformation> temporal) {
            var dfi = DateTimeFormatInfo.CurrentInfo;
            var ordered = temporal.Where(x => x.Timestamp).OrderBy(x => dfi.Calendar.GetWeekOfYear(x.Value, CalendarWeekRule.FirstDay, DayOfWeek.Monday));
        }
}

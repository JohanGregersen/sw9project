using System;
using System.Collections.Generic;
using System.Linq;

namespace CarDataProject {
    public static class TripCalculator {
        public static List<Trip> CalculateTripsByCarId(Int16 carId) {
            //Fetch all dates a given car is being used.
            List<int> allDates = FetchAllDatesByCarId(carId);

            //Instantiate the containers for trips and timestamps fetched by a single date at a time
            List<Trip> allTrips = new List<Trip>();
            Trip trip = new Trip();
            allTrips.Add(trip);
            List<Tuple<int, DateTime>> timestampsByDate;

            //Used to handle the first case of the algorithm because it needs to be there to ensure the first comparison goes well
            bool firstCaseFlag = true;
            
            //Starting to iterate over all dates
            for (int x = 0; x < allDates.Count(); x++) {
                //Fetch all id+timestamps given by a date, id is item1, datetime is item2
                timestampsByDate = FetchTimestampsByDate(allDates[x]);

                //Iterate over all timestampsByDate
                for (int i = 0; i < timestampsByDate.Count(); i++) {
                    //1st case handler
                    if (firstCaseFlag) {
                        allTrips.Last().allTimestamps.Add(timestampsByDate.ElementAt(0));
                        i++;
                        firstCaseFlag = false;
                    }

                    //(optimization with calculating unixtime twice for the lasst element in alltrips.)

                    //Compare the last seen timestamp to the current, if more than 300 seconds has past, create a new trip and store the current timestamp in it
                    if (Math.Abs(ToUnixTime(allTrips.Last().allTimestamps.Last().Item2) - ToUnixTime(timestampsByDate.ElementAt(i).Item2)) <= 180) {
                        allTrips.Last().allTimestamps.Add(timestampsByDate.ElementAt(i));
                    } else {
                        allTrips.Add(new Trip());
                        allTrips.Last().allTimestamps.Add(timestampsByDate.ElementAt(i));
                    }
                }
            }

            return allTrips;
        }

        private static List<int> FetchAllDatesByCarId(Int16 carId) {
            DBController dbc = new DBController();
            List<int> allDates = dbc.GetAllDatesByCarId(carId, true, true);
            dbc.Close();
            return allDates;
        }

        private static List<Tuple<int, DateTime>> FetchTimestampsByDate(int date) {
            DBController dbc = new DBController();
            List<Tuple<int, DateTime>> timestampsByDate = dbc.GetTimeByDate(date);
            dbc.Close();
            return timestampsByDate;
        }

        private static long ToUnixTime(DateTime date) {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}

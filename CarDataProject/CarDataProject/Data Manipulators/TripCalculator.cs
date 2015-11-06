using System;
using System.Collections.Generic;
using System.Linq;

namespace CarDataProject {
    public static class TripCalculator {
        
        public static List<INFATITrip> CalculateTripsByCarId(Int16 carId) {

            DBController dbc = new DBController();

            //Fetch all temporalinformation for a cardId
            List<TemporalInformation> datapoints = dbc.GetTimestampsByCarId(carId);
            dbc.Close();
            Console.WriteLine("data fetched");
            //Instantiate the containers for trips and timestamps fetched by a single date at a time
            List<INFATITrip> allTrips = new List<INFATITrip>();
            INFATITrip trip = new INFATITrip(carId);
            allTrips.Add(trip);

            //Used to handle the first case of the algorithm because it needs to be there to ensure the first comparison goes well
            bool firstCaseFlag = true;

            //Starting to iterate over all timestamps
            for(int i = 0; i < datapoints.Count(); i++) {
                if (firstCaseFlag) {
                    allTrips.Last().Timestamps.Add(new TemporalInformation(datapoints.ElementAt(0).EntryId, datapoints.ElementAt(0).Timestamp));
                    i++;
                    firstCaseFlag = false;
                }

                //Compare the last seen timestamp to the current, if more than 300 seconds has past, create a new trip and store the current timestamp in it
                if (Math.Abs(ToUnixTime(allTrips.Last().Timestamps.Last().Timestamp) - ToUnixTime(datapoints.ElementAt(i).Timestamp)) <= 180) {
                    allTrips.Last().Timestamps.Add(new TemporalInformation(datapoints.ElementAt(i).EntryId, datapoints.ElementAt(i).Timestamp));
                } else {
                    allTrips.Add(new INFATITrip(carId));
                    allTrips.Last().Timestamps.Add(new TemporalInformation(datapoints.ElementAt(i).EntryId, datapoints.ElementAt(i).Timestamp));
                }
            }

            return allTrips;
        }

        private static long ToUnixTime(DateTime date) {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}

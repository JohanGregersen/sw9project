using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    public static class TripFactUpdater {
        public static void Update(Int16 carId) {
            DBController dbc = new DBController();
            List<Int64> tripList = dbc.GetTripIdsByCarId(carId);

            List<Fact> facts = null;
            Trip trip = null;
            Int64 prevTripId = 0;
            DateTime prevEndTimestamp = new DateTime();

            for (int i = 0; i < tripList.Count; i++) {
                trip = dbc.GetTripByCarIdAndTripId(carId, tripList[i]);
                facts = dbc.GetFactsByCarIdAndTripId(carId, tripList[i]);

                //Previous Trip Calculations
                //Dont do this on the first trip.
                if (i != 0) {
                    trip.PreviousTripId = prevTripId;
                    trip.SecondsToLag = MeasureCalculator.SecondsToLag(facts.First().Temporal.Timestamp, prevEndTimestamp);
                }

                trip = UpdateTrip(trip, facts);

                Console.WriteLine("Updating trip {0} with FactsCount = {1} and prevTripId = {2}", trip.TripId, facts.Count, trip.PreviousTripId);

                dbc.UpdateTripFactWithMeasures(trip);

                //Variables for previous Calculations
                prevTripId = trip.TripId;
                prevEndTimestamp = trip.EndTemporal.Timestamp;

            }

            dbc.Close();

        }

        private static Trip UpdateTrip(Trip trip, List<Fact> facts) {

            //Temporal
            trip.StartTemporal = facts.First().Temporal;
            trip.EndTemporal = facts.Last().Temporal;
            trip.SecondsDriven = MeasureCalculator.SecondsToLag(facts.Last().Temporal.Timestamp, facts.First().Temporal.Timestamp);

            trip.MetersDriven = 0;
            trip.JerkCount = 0;
            trip.BrakeCount = 0;
            trip.AccelerationCount = 0;
            trip.MetersSped = 0;
            trip.TimeSped = new TimeSpan();
            trip.SteadySpeedDistance = 0;
            trip.SteadySpeedTime = new TimeSpan();
            for (int i = 1; i < facts.Count; i++) {
                trip.MetersDriven += MeasureCalculator.DistanceToLag(facts[i].Spatial.MPoint, facts[i - 1].Spatial.MPoint);

                //JerkCounting
                if (MeasureCalculator.Jerking(facts[i].Measure) == true) {
                    trip.JerkCount++;
                }

                //BrakeCounting
                if (facts[i].Flag.Braking == true) {
                    trip.BrakeCount++;
                }

                //AccelerateCounting
                if (MeasureCalculator.Accelerating(facts[i].Measure) == true) {
                    trip.AccelerationCount++;
                }

                //Meters Sped
                if (facts[i].Flag.Speeding == true) {
                    trip.MetersSped += facts[i].Spatial.DistanceToLag;
                }

                //Time Sped
                if (facts[i].Flag.Speeding == true) {
                    trip.TimeSped += facts[i].Temporal.SecondsToLag;
                }

                //SteadySpeed Distance
                if (facts[i].Flag.SteadySpeed == true) {
                    trip.SteadySpeedDistance += facts[i].Spatial.DistanceToLag;
                }

                //SteadySpeed Time
                if (facts[i].Flag.SteadySpeed == true) {
                    trip.SteadySpeedTime += facts[i].Temporal.SecondsToLag;
                }
            }

            //Price?
            //Optimal Score?
            //Trip Score?



            /*
            previoustripid bigint,
            carid integer,
            startdate integer,
            enddate integer,
            starttime integer,
            endtime integer,
            secondsdriven integer,
            metersdriven real,
            price real,
            optimalscore real,
            tripscore real,
            jerkcount smallint,
            brakecount smallint,
            accelerationcount smallint,
            meterssped real,
            timesped real,
            steadyspeeddistance real,
            steadyspeedtime real,
            secondstolag integer,
            dataquality real,
            */






            return trip;
        }
    }
}

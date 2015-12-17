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

                Console.WriteLine("Updating Car {0} - Trip {1} with prevTripId = {2} and FactsCount = {3}", carId, trip.TripId, trip.PreviousTripId, facts.Count);

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

            //Fact related calculations
            trip.MetersDriven = 0;
            trip.JerkCount = 0;
            trip.BrakeCount = 0;
            trip.AccelerationCount = 0;
            trip.MetersSped = 0;
            trip.TimeSped = new TimeSpan();
            trip.SteadySpeedDistance = 0;
            trip.SteadySpeedTime = new TimeSpan();
            trip.DataQuality = 0;

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
                if (facts[i].Flag.Speeding == true && facts[i].Segment.MaxSpeed != 0) {
                    trip.MetersSped += facts[i].Spatial.DistanceToLag;
                }

                //Time Sped
                if (facts[i].Flag.Speeding == true && facts[i].Segment.MaxSpeed != 0) {
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

                trip.DataQuality += facts[i].Quality.Hdop;
            }

            //Data Quality
            trip.DataQuality = trip.DataQuality / facts.Count;

            //Interval Information
            

            //Price?
            //Optimal Score?
            //Trip Score?


            return trip;
        }
    }
}

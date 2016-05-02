using System;
using System.Collections.Generic;
using System.Linq;

namespace CarDataProject {
    public static class TripFactUpdater {

        public static void UpdateTrip(Int16 carId, Int64 tripId) {
            DBController dbc = new DBController();

            Trip trip = new Trip(tripId, carId);
            List<Fact> facts = dbc.GetFactsByTripIdNoQuality(tripId);

            //Getting the previous tripid and seconds to previous trip
            List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
            //In case this is the first trip - ignore computing measures for previous trip
            if (tripIds.Count > 1) {
                Int64 latestTrip = tripIds[tripIds.Count() - 2];
                Trip previousTrip = dbc.GetTripByCarIdAndTripId(carId, latestTrip);
                trip.PreviousTripId = previousTrip.TripId;
                trip.LocalTripId = previousTrip.LocalTripId + 1;
                trip.SecondsToLag = MeasureCalculator.SecondsToLag(facts[0].Temporal.Timestamp, previousTrip.EndTemporal.Timestamp);
            } else {
                trip.SecondsToLag = new TimeSpan(0, 0, -1);
                trip.LocalTripId = 1;
            }
            //Calc the trip updates
            trip = UpdateTrip(trip, facts, dbc);

            //Compute the scores
            trip.OptimalScore = FinalScore.CalculateOptimalScore(trip);
            List<double> fullscores = FinalScore.CalculateTripScores(trip);

            trip.RoadTypeScore = fullscores[0];
            trip.CriticalTimeScore = fullscores[1];
            trip.SpeedingScore = fullscores[2];
            trip.AccelerationScore = fullscores[3];
            trip.BrakeScore = fullscores[4];
            trip.JerkScore = fullscores[5];
            trip.TripScore = fullscores[6];

            //Update the trip in the database
            dbc.UpdateTripFactWithMeasures(trip);
            dbc.Close();
        }

        public static void Update(Int16 carId) {
            DBController dbc = new DBController();
            List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);
            List<Fact> facts;
            Trip trip;
            Int64 previousTripId = 0;
            DateTime previousEndTimestamp = new DateTime();

            for (int i = 0; i < tripIds.Count; i++) {
                trip = dbc.GetTripByTripId(tripIds[i]);
                facts = dbc.GetFactsByTripId(tripIds[i]);

                //If a previous trip exists, save ID and the time passed between them
                if (i > 0) {
                    trip.PreviousTripId = previousTripId;
                    trip.SecondsToLag = MeasureCalculator.SecondsToLag(facts[0].Temporal.Timestamp, previousEndTimestamp);
                }

                //Remember variables for next iteration
                previousTripId = trip.TripId;
                previousEndTimestamp = trip.EndTemporal.Timestamp;

                Console.WriteLine("Updating car {0}, trip {1} ({2} facts)", carId, trip.TripId, facts.Count);
                trip = UpdateTrip(trip, facts, dbc);
                trip.OptimalScore = FinalScore.CalculateOptimalScore(trip);
                List<double> fullscores = FinalScore.CalculateTripScores(trip);

                trip.RoadTypeScore = fullscores[0];
                trip.CriticalTimeScore = fullscores[1];
                trip.SpeedingScore = fullscores[2];
                trip.AccelerationScore = fullscores[3];
                trip.BrakeScore = fullscores[4];
                trip.JerkScore = fullscores[5];
                trip.TripScore = fullscores[6];
                dbc.UpdateTripFactWithMeasures(trip);
            }

            dbc.Close();
        }

        public static Trip UpdateTrip(Trip trip, List<Fact> facts, DBController dbc) {
            //Temporal information
            trip.StartTemporal = facts.First().Temporal;
            trip.EndTemporal = facts.Last().Temporal;
            trip.SecondsDriven = MeasureCalculator.SecondsToLag(facts.Last().Temporal.Timestamp, facts.First().Temporal.Timestamp);

            //Counts
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
                //Meters driven
                trip.MetersDriven += MeasureCalculator.DistanceToLag(facts[i].Spatial.MPoint, facts[i - 1].Spatial.MPoint);

                //Meters sped
                if (facts[i].Flag.Speeding && facts[i].Segment.MaxSpeed != 0) {
                    trip.MetersSped += facts[i].Spatial.DistanceToLag;
                }

                //Time sped
                if (facts[i].Flag.Speeding && facts[i].Segment.MaxSpeed != 0) {
                    trip.TimeSped += facts[i].Temporal.SecondsToLag;
                }

                //Acceleration count
                if (facts[i].Flag.Accelerating) {
                    trip.AccelerationCount++;
                }

                //Brake count
                if (facts[i].Flag.Braking) {
                    trip.BrakeCount++;
                }

                //Jerk count
                if (facts[i].Flag.Jerking) {
                    trip.JerkCount++;
                }

                //Steady speed distance
                if (facts[i].Flag.SteadySpeed) {
                    trip.SteadySpeedDistance += facts[i].Spatial.DistanceToLag;
                }

                //Steady speed time
                if (facts[i].Flag.SteadySpeed) {
                    trip.SteadySpeedTime += facts[i].Temporal.SecondsToLag;
                }

                trip.DataQuality += facts[i].Quality.Hdop;
            }

            //Interval Information
            trip = UpdateTripWithIntervals(trip, facts, dbc);

            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // Needs to be calculated right
            ////////////////////////////////
            //Data Quality
            trip.DataQuality = trip.DataQuality / facts.Count;

            return trip;
        }

        private static Trip UpdateTripWithIntervals(Trip trip, List<Fact> facts, DBController dbc) {
            List<double> intervals;

            trip.IntervalInformation = new IntervalInformation(trip.CarId, trip.TripId);

            intervals = IntervalCalculator.RoadType(trip, facts, dbc);
            trip.IntervalInformation.RoadTypesInterval = IntervalHelper.Encode(intervals);

            intervals = IntervalCalculator.CriticalTime(trip, facts);
            trip.IntervalInformation.CriticalTimeInterval = IntervalHelper.Encode(intervals);

            intervals = IntervalCalculator.Speeding(trip, facts);
            trip.IntervalInformation.SpeedInterval = IntervalHelper.Encode(intervals);

            intervals = IntervalCalculator.Acceleration(trip, facts);
            trip.IntervalInformation.AccelerationInterval = IntervalHelper.Encode(intervals);

            intervals = IntervalCalculator.Brake(trip, facts);
            trip.IntervalInformation.BrakingInterval = IntervalHelper.Encode(intervals);

            intervals = IntervalCalculator.Jerk(trip, facts);
            trip.IntervalInformation.JerkInterval = IntervalHelper.Encode(intervals);

            return trip;
        }
    }
}

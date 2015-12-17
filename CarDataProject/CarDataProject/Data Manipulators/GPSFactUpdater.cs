using System;
using System.Collections.Generic;
using System.Linq;

namespace CarDataProject {
    public static class GPSFactUpdater {
        private static double speedLock = 0;
        private static double speedThreshold = 3;
        private static double steadySpeedCounter = 0;

        public static void Update(Int16 CarId) {
            DBController dbc = new DBController();
            List<Int64> tripIds = dbc.GetTripIdsByCarId(CarId);
            foreach (Int64 tripId in tripIds) {
                List<Fact> facts = dbc.GetFactsByCarIdAndTripId(CarId, tripId);
                dbc.UpdateGPSFactWithMeasures(UpdatedFacts(facts));
            }

            dbc.Close();
        }

        private static List<Fact> UpdatedFacts(List<Fact> facts) {
            //First-cases
            //Speeding
            bool firstSpeeding = MeasureCalculator.Speeding(facts.First().Measure.Speed, facts.First().Segment.MaxSpeed);
            facts.First().Flag = new FlagInformation(facts.First().TripId, facts.First().EntryId, firstSpeeding, false);

            //steady-speed
            speedLock = facts.First().Measure.Speed;

            for (int i = 1; i < facts.Count; i++) {
                //PathLine
                //Handled in DBController because it has to use PostGis command ST_MakeLine

                //Measures
                //Acceleration
                double Acceleration = MeasureCalculator.Acceleration(facts[i].Measure, facts[i - 1].Measure, facts[i].Temporal, facts[i - 1].Temporal);
                //Jerk
                double Jerk = MeasureCalculator.Jerk(facts[i].Measure, facts[i - 1].Measure, facts[i].Temporal, facts[i - 1].Temporal);

                facts[i].Measure = new MeasureInformation(facts[i].Measure.Speed, Acceleration, Jerk);

                //Spatial
                //DistanceToLag
                double DistanceToLag = MeasureCalculator.DistanceToLag(facts[i].Spatial.MPoint, facts[i - 1].Spatial.MPoint);

                facts[i].Spatial = new SpatialInformation(facts[i].Spatial.MPoint, DistanceToLag);

                //Temporal
                //SecondsToLag
                TimeSpan SecondsToLag = MeasureCalculator.SecondsToLag(facts[i].Temporal.Timestamp, facts[i - 1].Temporal.Timestamp);

                facts[i].Temporal = new TemporalInformation(facts[i].Temporal.Timestamp, SecondsToLag);

                //FlagInformation
                //Speeding
                bool speeding = MeasureCalculator.Speeding(facts[i].Measure.Speed, facts[i].Segment.MaxSpeed);

                //Braking
                bool braking = MeasureCalculator.Braking(facts[i].Measure);

                //SteadySpeed
                bool steadySpeed = false;

                //Speed-change above speedThreshold?
                if (Math.Abs(speedLock - facts[i].Measure.Speed) > speedThreshold) {
                    speedLock = facts[i].Measure.Speed;
                    steadySpeedCounter = 0;
                }

                steadySpeedCounter++;

                if (steadySpeedCounter >= 5) {
                    steadySpeed = true; 
                }

                facts[i].Flag = new FlagInformation(speeding, braking, steadySpeed);
            }

            return facts;
        }
    }
}

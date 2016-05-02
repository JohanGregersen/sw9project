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
                List<Fact> facts = dbc.GetFactsByTripId(tripId);
                dbc.UpdateGPSFactWithMeasures(UpdatedFacts(facts));
            }

            dbc.Close();
        }

        public static List<Fact> UpdatedFacts(List<Fact> facts) {
            //First-cases
            //Speeding
            bool firstSpeeding = MeasureCalculator.Speeding(facts.First().Measure.Speed, facts.First().Segment.MaxSpeed);
            facts.First().Flag = new FlagInformation(facts.First().TripId, facts.First().EntryId, firstSpeeding, false, false, false);

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

                //Accelerating
                bool accelerating = MeasureCalculator.Accelerating(facts[i].Measure);
                //Jerking
                bool jerking = MeasureCalculator.Jerking(facts[i].Measure);
                
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

                facts[i].Flag = new FlagInformation(speeding, accelerating, jerking, braking, steadySpeed);
            }

            return facts;
        }

        public static void UpdateRawGPS(Int16 CarId, Int64 TripId) {
            DBController dbc = new DBController();
            List<Fact> facts = dbc.GetFactsByCarIdAndTripIdNoQuality(CarId, TripId);
            dbc.Close();

            if (facts.Count() < 2) {
                return;
            }

            //First case - Set speed equal to second fact and set flags to false. This is essentially a freebie
            double speedFrom2ndFact = MeasureCalculator.Speed(facts[1].Spatial, facts[0].Spatial, facts[1].Temporal, facts[0].Temporal);
            facts[0].Measure = new MeasureInformation(speedFrom2ndFact, 0, 0);
            facts[0].Flag = new FlagInformation(false, false, false, false);

            for (int i = 1; i < facts.Count(); i++) {
                //Spatial
                facts[i].Spatial.DistanceToLag = MeasureCalculator.DistanceToLag(facts[i].Spatial.MPoint, facts[i - 1].Spatial.MPoint);

                //Temporal
                facts[i].Temporal.SecondsToLag = MeasureCalculator.SecondsToLag(facts[i].Temporal.Timestamp, facts[i - 1].Temporal.Timestamp);

                //MeasureInformation
                double speed = MeasureCalculator.Speed(facts[i].Spatial, facts[i - 1].Spatial, facts[i].Temporal, facts[i - 1].Temporal);
                facts[i].Measure = new MeasureInformation(speed, 0, 0);
                facts[i].Measure.Acceleration = MeasureCalculator.Acceleration(facts[i].Measure, facts[i - 1].Measure, facts[i].Temporal, facts[i - 1].Temporal);
                facts[i].Measure.Jerk = MeasureCalculator.Jerk(facts[i].Measure, facts[i - 1].Measure, facts[i].Temporal, facts[i - 1].Temporal);

                //FlagInformation
                Boolean accelerating = MeasureCalculator.Accelerating(facts[i].Measure);
                Boolean braking = MeasureCalculator.Braking(facts[i].Measure);
                Boolean jerking = MeasureCalculator.Jerking(facts[i].Measure);
                facts[i].Flag = new FlagInformation(false, accelerating, braking, jerking);
            }
            dbc = new DBController();
            try {
                dbc.UpdateGPSFactWithMeasures(facts);
            }
            catch(Exception e) {
                Console.WriteLine(e.ToString());
            }
            dbc.Close();
        }
    }
}

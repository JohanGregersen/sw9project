using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class GPSFactUpdater {
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

                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //SteadySpeed - NEEDS TO BE DONE CORRECTLY - ACCORDING TO THE WAY WE WANT THIS TO WORK!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                bool steadySpeed = false;
                if (i >= 4) {
                    steadySpeed = MeasureCalculator.SteadySpeed(facts.GetRange(i - 4, 5));
                }

                facts[i].Flag = new FlagInformation(speeding, braking, steadySpeed);
            }

            return facts;
        }
    }
}
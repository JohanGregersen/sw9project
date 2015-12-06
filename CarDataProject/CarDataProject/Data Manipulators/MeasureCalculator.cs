using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace CarDataProject {
    public static class MeasureCalculator {

        public static double DistanceToLag(GeoCoordinate MPoint, GeoCoordinate LagMPoint) {
            return MPoint.GetDistanceTo(LagMPoint);
        }

        public static TimeSpan SecondsToLag(DateTime Timestamp, DateTime LagTimestamp) {
            return Timestamp - LagTimestamp;
        }

        public static double Acceleration(MeasureInformation CurrentMI, MeasureInformation PrevMI, TemporalInformation CurrentTI, TemporalInformation PrevTI) {
            //Acceleration = Velocity change / Time
            double velocityChange = CurrentMI.Speed - PrevMI.Speed;
            TimeSpan time = CurrentTI.Timestamp - PrevTI.Timestamp;

            return velocityChange / time.Seconds;
        }

        public static double Jerk(MeasureInformation CurrentMi, MeasureInformation PrevMI, TemporalInformation CurrentTI, TemporalInformation PrevTI) {
            //Jerk = Acceleration change / time
            double accelerationChange = CurrentMi.Acceleration - PrevMI.Acceleration;
            TimeSpan time = CurrentTI.Timestamp - PrevTI.Timestamp;

            return accelerationChange / time.Seconds;
        }

        public static bool Speeding(double Speed, Int16 MaxSpeed) {
            if (Speed >= MaxSpeed + 3) {
                return true;
            }
            return false;
        }

        public static bool Braking(MeasureInformation MI) {
            if(MI.Acceleration <= -5) {
                return true;
            }
            return false;
        }

        public static bool Jerking (MeasureInformation MI) {
            if (Math.Abs(MI.Jerk) >= 5) {
                return true;
            }
            return false;
        }

        public static bool Accelerating(MeasureInformation MI) {
            if (Math.Abs(MI.Acceleration) >= 5) {
                return true;
            }
            return false;
        }

        public static bool SteadySpeed(List<Fact> facts) {
            if(facts.Count != 5) {
                throw new ArgumentException("Steady Speed must be calculated with exactly 5 elements");
            }

            double totalSpeedChange = 0;

            //Average change in speed does not exceed 3, and no peak change above 5
            for(int i = 1; i < facts.Count; i++) {
                double speedChange = Math.Abs(facts[i].Measure.Speed - facts[i - 1].Measure.Speed);
                if (speedChange >= 5) {
                    return false;
                }
                totalSpeedChange += speedChange;
            }

            //Average Speed computation
            
            if ((totalSpeedChange / 5) >= 3) {
                return false;
            }

            return true;
        }

    }
}

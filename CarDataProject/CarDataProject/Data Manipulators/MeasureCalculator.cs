using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Device.Location;
using GeoCoordinatePortable;

namespace CarDataProject {
    public static class MeasureCalculator {

        public static double DistanceToLag(GeoCoordinate MPoint, GeoCoordinate LagMPoint) {
            return MPoint.GetDistanceTo(LagMPoint);
        }

        public static TimeSpan SecondsToLag(DateTime Timestamp, DateTime LagTimestamp) {
            return Timestamp - LagTimestamp;
        }

        public static double Speed(SpatialInformation CurrentSI, SpatialInformation PrevSI, TemporalInformation CurrentTI, TemporalInformation PrevTI) {
            //Speed = Distance / Time
            double distance = CurrentSI.MPoint.GetDistanceTo(PrevSI.MPoint);

            TimeSpan time = CurrentTI.Timestamp - PrevTI.Timestamp;

            if (time.Seconds == 0) {
                return 0;
            }
            return (distance / time.Seconds) * 3.6;
        }

        public static double Acceleration(MeasureInformation CurrentMI, MeasureInformation PrevMI, TemporalInformation CurrentTI, TemporalInformation PrevTI) {
            //Acceleration = Velocity change / Time
            double velocityChange = CurrentMI.Speed - PrevMI.Speed;
            TimeSpan time = CurrentTI.Timestamp - PrevTI.Timestamp;

            if(time.Seconds == 0) {
                return 0;
            }

            return velocityChange / time.Seconds;
        }

        public static double Jerk(MeasureInformation CurrentMi, MeasureInformation PrevMI, TemporalInformation CurrentTI, TemporalInformation PrevTI) {
            //Jerk = Acceleration change / Time
            double accelerationChange = CurrentMi.Acceleration - PrevMI.Acceleration;
            TimeSpan time = CurrentTI.Timestamp - PrevTI.Timestamp;

            if (time.Seconds == 0) {
                return 0;
            }

            return accelerationChange / time.Seconds;
        }

        public static bool Speeding(double Speed, Int16 MaxSpeed) {
            if (Speed >= MaxSpeed + 3) {
                return true;
            }

            return false;
        }

        public static bool Accelerating(MeasureInformation MI) {
            if (MI.Acceleration >= 6) {
                return true;
            }

            return false;
        }

        public static bool Jerking (MeasureInformation MI) {
            if (Math.Abs(MI.Jerk) >= 8) {
                return true;
            }

            return false;
        }
        
        public static bool Braking(MeasureInformation MI) {
            if (MI.Acceleration <= -8) {
                return true;
            }

            return false;
        }

    }
}

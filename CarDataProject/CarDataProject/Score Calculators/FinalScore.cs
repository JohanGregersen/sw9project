using System;
using System.Collections.Generic;

namespace CarDataProject {
    /*
     * FinalScore calculates a total score for a trip
     * The score uses the trip distance as base, adding distance for each of the different weight functions
     */
    public class FinalScore {
        //Testvariabler for fiktiv tur
        static double metersDriven = 23700;
        static double metersSped = 10300;
        static List<double> weightSet = new List<double> { 1, 1.2, 1.4, 1, 1, 1.8, 1, 1.1 };
        static List<double> intervals = new List<double> { 12, 13, 12, 13, 12, 13, 12, 13 };
        static Int64 tripId = 1;

        //Forsikringsselskabet skal bestemme alt det her
        List<double> optimalSpeedingProfile = new List<double> { 7, 3, 0, 0, 0, 0, 0, 0 };
        List<double> optimalAccelrationProfile = new List<double> { 7, 3, 0, 0, 0, 0, 0, 0 };
        List<double> optimalBrakesProfile = new List<double> { 7, 3, 0, 0, 0, 0, 0, 0 };
        List<double> optimalJerksProfile = new List<double> { 7, 3, 0, 0, 0, 0, 0, 0 };
        double a = 1.2;
        double b = 1.3;
        double c = 0;
        double poly = 1.08;


        public double CalculateReal(Int64 tripId) {
            //double metersDriven = DBController.MetersDriven(Int64 tripId);
            //double metersSped = DBController.MetersSped(Int64 tripId);
            //List<double> roadTypeIntervals = DBController.RoadTypeIntervals(Int64 tripId);
            //List<double> criticalTimePeriodIntervals = DBController.CriticalTimePeriodIntervals(Int64 tripId);
            //List<double> speedingIntervals = DBController.SpeedingIntervals(Int64 tripId);
            //List<double> accelerationIntervals = DBController.AccelerationIntervals(Int64 tripId);
            //List<double> brakeIntervals = DBController.BrakeIntervals(Int64 tripId);
            //List<double> jerkIntervals = DBController.JerkIntervals(Int64 tripId);

            return metersDriven + Subscore.RoadTypes(metersDriven, intervals, weightSet) +
                                  Subscore.CriticalTimePeriod(metersDriven, intervals, weightSet) +
                                  Subscore.Speeding(metersDriven, metersSped, intervals, weightSet, a, b, c, poly) +
                                  Subscore.Accelerations(metersDriven, intervals, weightSet, a, b, c, poly) +
                                  Subscore.Brakes(metersDriven, intervals, weightSet, a, b, c, poly) +
                                  Subscore.Jerks(metersDriven, intervals, weightSet, a, b, c, poly);
        }

        public double CalculateOptimal(Int64 tripId, List<double> optimalSpeedingProfile,
                                                     List<double> optimalAccelrationProfile,
                                                     List<double> optimalBrakesProfile,
                                                     List<double> optimalJerksProfile) {
            //double metersDriven = DBController.MetersDriven(Int64 tripId);
            //List<double> roadTypeIntervals = DBController.RoadTypeIntervals(Int64 tripId);
            //List<double> criticalTimePeriodIntervals = DBController.CriticalTimePeriodIntervals(Int64 tripId);
            double metersSped = 0;
            double accelerations = 0;
            double brakes = 0;
            double jerks = 0;

            for (int i = 0; i < optimalSpeedingProfile.Count; i++) {
                //Optimal Profiles are per hundred kilometers - change the values to match the trip length instead
                optimalSpeedingProfile[i] *= (metersDriven / 100000);
                optimalAccelrationProfile[i] *= (metersDriven / 100000);
                optimalBrakesProfile[i] *= (metersDriven / 100000);
                optimalJerksProfile[i] *= (metersDriven / 100000);
                metersSped += optimalSpeedingProfile[i];
                accelerations += optimalAccelrationProfile[i];
                brakes += optimalBrakesProfile[i];
                jerks += optimalJerksProfile[i];
            }

            for (int i = 0; i < optimalSpeedingProfile.Count; i++) {
                optimalSpeedingProfile[i] /= metersSped * 100;
                optimalAccelrationProfile[i] /= accelerations * 100;
                optimalBrakesProfile[i] /= brakes * 100;
                optimalJerksProfile[i] /= jerks * 100;
            }

            return metersDriven + Subscore.RoadTypes(metersDriven, intervals, weightSet) +
                                  Subscore.CriticalTimePeriod(metersDriven, intervals, weightSet) +
                                  Subscore.Speeding(metersDriven, metersSped, optimalSpeedingProfile, weightSet, a, b, c, poly) +
                                  Subscore.Accelerations(metersDriven, optimalAccelrationProfile, weightSet, a, b, c, poly) +
                                  Subscore.Brakes(metersDriven, optimalBrakesProfile, weightSet, a, b, c, poly) +
                                  Subscore.Jerks(metersDriven, optimalJerksProfile, weightSet, a, b, c, poly);
        }
    }
}

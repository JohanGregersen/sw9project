using System;
using System.Collections.Generic;

namespace CarDataProject {
    /*
     * FinalScore calculates a total score for a trip
     * The score uses the trip distance as base, adding distance for each of the different weight functions
     */
    public static class FinalScore {
        public static List<double> CalculateTripScores(Trip trip) {
            List<double> roadTypeIntervals = IntervalHelper.Decode(trip.IntervalInformation.RoadTypesInterval);
            List<double> criticalTimeIntervals = IntervalHelper.Decode(trip.IntervalInformation.CriticalTimeInterval);
            List<double> speedingIntervals = IntervalHelper.Decode(trip.IntervalInformation.SpeedInterval);
            List<double> accelerationIntervals = IntervalHelper.Decode(trip.IntervalInformation.AccelerationInterval);
            List<double> brakingIntervals = IntervalHelper.Decode(trip.IntervalInformation.BrakingInterval);
            List<double> jerkIntervals = IntervalHelper.Decode(trip.IntervalInformation.JerkInterval);

            List<double> fullscores = new List<double>();
            double finalScore = trip.MetersDriven;

            double roadtypescore = Subscore.RoadTypes(trip.MetersDriven, roadTypeIntervals, DefaultPolicy.RoadTypeWeights);
            finalScore += roadtypescore;

            double criticaltimescore = Subscore.CriticalTimePeriod(trip.MetersDriven, criticalTimeIntervals, DefaultPolicy.CriticalTimeWeights);
            finalScore += criticaltimescore;

            double speedingscore = Subscore.Speeding(trip.MetersSped,
                                            speedingIntervals,
                                            DefaultPolicy.SpeedingWeights,
                                            DefaultPolicy.A,
                                            DefaultPolicy.B,
                                            DefaultPolicy.C,
                                            DefaultPolicy.Poly);
            finalScore += speedingscore;

            double accelerationscore = Subscore.Accelerations(trip.AccelerationCount,
                                                 accelerationIntervals,
                                                 DefaultPolicy.AccelerationPrice,
                                                 DefaultPolicy.AccelerationWeights,
                                                 DefaultPolicy.A,
                                                 DefaultPolicy.B,
                                                 DefaultPolicy.C,
                                                 DefaultPolicy.Poly);
            finalScore += accelerationscore;

            double brakescore = Subscore.Brakes(trip.BrakeCount,
                                          brakingIntervals,
                                          DefaultPolicy.BrakePrice,
                                          DefaultPolicy.BrakeWeights,
                                          DefaultPolicy.A,
                                          DefaultPolicy.B,
                                          DefaultPolicy.C,
                                          DefaultPolicy.Poly);
            finalScore += brakescore;

            double jerkscore = Subscore.Jerks(trip.JerkCount,
                                        jerkIntervals,
                                        DefaultPolicy.JerkPrice,
                                        DefaultPolicy.JerkWeights,
                                        DefaultPolicy.A,
                                        DefaultPolicy.B,
                                        DefaultPolicy.C,
                                        DefaultPolicy.Poly);
            finalScore += jerkscore;
            fullscores.Add(roadtypescore);
            fullscores.Add(criticaltimescore);
            fullscores.Add(speedingscore);
            fullscores.Add(accelerationscore);
            fullscores.Add(brakescore);
            fullscores.Add(jerkscore);
            fullscores.Add(finalScore);

            return fullscores;
        }

        public static double CalculateOptimalScore(Trip trip) {
            List<double> roadTypeIntervals = IntervalHelper.Decode(trip.IntervalInformation.RoadTypesInterval);
            List<double> criticalTimeIntervals = IntervalHelper.Decode(trip.IntervalInformation.CriticalTimeInterval);

            double optimalScore = trip.MetersDriven;
            optimalScore += Subscore.RoadTypes(trip.MetersDriven, roadTypeIntervals, DefaultPolicy.RoadTypeWeights);
            optimalScore += Subscore.CriticalTimePeriod(trip.MetersDriven, criticalTimeIntervals, DefaultPolicy.CriticalTimeWeights);

            return optimalScore;
        }

        public static double CalculateOptimal(Int64 tripId, List<double> optimalSpeedingProfile,
                                                     List<double> optimalAccelrationProfile,
                                                     List<double> optimalBrakesProfile,
                                                     List<double> optimalJerksProfile) {
            DBController dbc = new DBController();
            Trip trip = dbc.GetTripByTripId(tripId);
            dbc.Close();

            List<double> roadTypeIntervals = IntervalHelper.Decode(trip.IntervalInformation.RoadTypesInterval);
            List<double> criticalTimeIntervals = IntervalHelper.Decode(trip.IntervalInformation.CriticalTimeInterval);

            double metersSped = 0;
            double accelerations = 0;
            double brakes = 0;
            double jerks = 0;

            for (int i = 0; i < optimalSpeedingProfile.Count; i++) {
                //Optimal Profiles are per hundred kilometers - change the values to match the trip length instead
                optimalSpeedingProfile[i] *= (trip.MetersDriven / 100000);
                optimalAccelrationProfile[i] *= (trip.MetersDriven / 100000);
                optimalBrakesProfile[i] *= (trip.MetersDriven / 100000);
                optimalJerksProfile[i] *= (trip.MetersDriven / 100000);
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

            double finalScore = trip.MetersDriven;
            finalScore += Subscore.RoadTypes(trip.MetersDriven, roadTypeIntervals, DefaultPolicy.RoadTypeWeights);
            finalScore += Subscore.CriticalTimePeriod(trip.MetersDriven, criticalTimeIntervals, DefaultPolicy.CriticalTimeWeights);
            finalScore += Subscore.Speeding(metersSped,
                                            optimalSpeedingProfile,
                                            DefaultPolicy.SpeedingWeights,
                                            DefaultPolicy.A,
                                            DefaultPolicy.B,
                                            DefaultPolicy.C,
                                            DefaultPolicy.Poly);
            finalScore += Subscore.Accelerations(accelerations,
                                                 optimalAccelrationProfile,
                                                 DefaultPolicy.AccelerationPrice,
                                                 DefaultPolicy.AccelerationWeights,
                                                 DefaultPolicy.A,
                                                 DefaultPolicy.B,
                                                 DefaultPolicy.C,
                                                 DefaultPolicy.Poly);
            finalScore += Subscore.Brakes(brakes,
                                          optimalBrakesProfile,
                                          DefaultPolicy.BrakePrice,
                                          DefaultPolicy.BrakeWeights,
                                          DefaultPolicy.A,
                                          DefaultPolicy.B,
                                          DefaultPolicy.C,
                                          DefaultPolicy.Poly);
            finalScore += Subscore.Jerks(jerks,
                                         optimalJerksProfile,
                                         DefaultPolicy.JerkPrice,
                                         DefaultPolicy.JerkWeights,
                                         DefaultPolicy.A,
                                         DefaultPolicy.B,
                                         DefaultPolicy.C,
                                         DefaultPolicy.Poly);
            return finalScore;
        }
    }

}

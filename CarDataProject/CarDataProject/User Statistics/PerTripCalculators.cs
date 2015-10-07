﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace CarDataProject {
    class PerTripCalculator {

        static string path = @"C:\data\";

        public static TimeSpan GetTime (Int16 carid, int tripid) {

            DBController dbc = new DBController();
            List<Timestamp> timestamps = dbc.GetTimestampsByCarAndTripId(carid, tripid);

            TimeSpan triptime = timestamps[timestamps.Count - 1].timestamp - timestamps[0].timestamp;
            dbc.Close();

            return triptime;
        }

        public static double GetKilometersDriven(Int16 carid, int tripid) {

            double kmdriven = 0;

            List<Point> MPointData = ValidationPlots.GetMPointData(carid, tripid);

            for (int i = 0; i < MPointData.Count - 1; i++) {
                kmdriven += MPointData[i].Mpoint.GetDistanceTo(MPointData[i + 1].Mpoint);
            }

            kmdriven = kmdriven / 1000;
            return kmdriven;
        }



        public static List<double> GetAccelerationCalcultions(Int16 carId, int tripId) {
            DBController dbc = new DBController();
            List<Tuple<Timestamp, int>> accelerationData = dbc.GetAccelerationDataByTrip(carId, tripId);
            dbc.Close();

            List<double> accelerationCalculations = new List<double>();

            for(int i = 0; i < accelerationData.Count() - 1; i++) {
                //Item1 = TimeStamp (DateTime object)
                //Item2 = speed
                // a = VELCOCITY CHANGE / TIME TAKEN;
                double velocityChange = accelerationData[i + 1].Item2 - accelerationData[i].Item2;
                    
                double timeTaken = Convert.ToDouble((DateTimeHelper.ToUnixTime(accelerationData[i + 1].Item1.timestamp) - DateTimeHelper.ToUnixTime(accelerationData[i].Item1.timestamp)));

                double acceleration = velocityChange / timeTaken;

                accelerationCalculations.Add(acceleration);

            }

            FileWriter.Acceleration(accelerationCalculations);
            GnuplotHelper.PlotGraph(10);

            return accelerationCalculations;
        }

    }
}

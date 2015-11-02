using System;
using System.Collections.Generic;
using System.Device.Location;

namespace CarDataProject {
    class ValidationPlots {
        public static void GetAllPlots(Int16 carId, Int64 tripId) {
            GetMpointPlot(carId, tripId);
            GetTimePlot(carId, tripId);
            GetSatHdopPlot(carId, tripId);
        }

        public static void GetTimePlot(Int16 carId, Int64 tripId) {
            DBController dbc = new DBController();
            List<TemporalInformation> temporalData = dbc.GetTimestampsByCarIdAndTripId(carId, tripId);
            dbc.Close();

            List<TimeSpan> intervals = new List<TimeSpan>();

            for (int i = 1; i < temporalData.Count; i++) {
                TimeSpan interval = temporalData[i].Timestamp - temporalData[i - 1].Timestamp;
                intervals.Add(interval);
            }
        }

        public static void GetMpointPlot(Int16 carId, Int64 tripId) {
            DBController dbc = new DBController();
            List<SpatialInformation> spatialData = dbc.GetMPointsByCarIdAndTripId(carId, tripId);
            dbc.Close();

            List<double> distances = new List<double>();
            Dictionary<GeoCoordinate, GeoCoordinate> outliers = new Dictionary<GeoCoordinate, GeoCoordinate>();

            for (int i = 1; i < spatialData.Count; i++) {
                distances.Add(spatialData[i].MPoint.GetDistanceTo(spatialData[i - 1].MPoint));
                
                //TODO: Hvor kommer 250 fra???
                if (spatialData[i].MPoint.GetDistanceTo(spatialData[i - 1].MPoint) > 250) {
                    outliers.Add(spatialData[i].MPoint, spatialData[i - 1].MPoint);
                }
            }
        }

        public static void GetSatHdopPlot(Int16 carId, Int64 tripId) {
            DBController dbc = new DBController();
            List<QualityInformation> qualityData = dbc.GetQualityByCarIdAndTripId(carId, tripId);
            dbc.Close();

            List<Int16> Sat = new List<Int16>();
            List<double> Hdop = new List<double>();

            foreach(QualityInformation quality in qualityData) {
                Sat.Add(quality.Sat);
                Hdop.Add(quality.Hdop);
            }
        }
    }
}

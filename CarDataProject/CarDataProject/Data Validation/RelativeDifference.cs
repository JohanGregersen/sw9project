using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject.Data_Validation {
    public static class RelativeDifference {

        public static Dictionary<Point, double> Speed(Int16 carId, int tripId, TimeSpan measurementRangeSeconds) {

            Dictionary<Point, double> pointMeasures = new Dictionary<Point, double>();
            Dictionary<Point, double> relativePointMeasures = new Dictionary<Point, double>();

            DBController dbc = new DBController();
            List<Point> mpoints = dbc.GetMPointByCarAndTripId(carId, tripId);
            List<Tuple<int, Timestamp, int>> timestampsAndSpeed = new List<Tuple<int, Timestamp, int>>();
            timestampsAndSpeed = dbc.GetAccelerationDataByTrip(carId, tripId);
            dbc.Close();

            //For each plot in a trip
            for (int i = 0; i < timestampsAndSpeed.Count; i++) {
                List<Point> range = new List<Point>();

                //Find all plots within measurement range
                for (int j = i - 1; j >= 0; j--) {
                    if (timestampsAndSpeed[i].Item2.timestamp - timestampsAndSpeed[j].Item2.timestamp <= measurementRangeSeconds) {
                        range.Add(mpoints.Find(x => x.Id.Equals(timestampsAndSpeed[j].Item2.Id)));
                    } else {
                        break;
                    }
                }

                for (int j = i + 1; j < timestampsAndSpeed.Count; j++) {
                    if (timestampsAndSpeed[j].Item2.timestamp - timestampsAndSpeed[i].Item2.timestamp <= measurementRangeSeconds) {
                        range.Add(mpoints.Find(x => x.Id.Equals(timestampsAndSpeed[j].Item2.Id)));
                    } else {
                        break;
                    }
                }

                //Use the measurement range to calculate measure
                double measure = 0;

                foreach (Point point in range) {
                    measure += timestampsAndSpeed.Find(x => x.Item2.Id.Equals(point.Id)).Item3;
                }

                measure /= range.Count;
                pointMeasures.Add(mpoints.Find(x => x.Id.Equals(timestampsAndSpeed[i].Item2.Id)), measure);

            }

            //For each plot in a trip
            for (int i = 0; i < timestampsAndSpeed.Count; i++) {
                List<Point> range = new List<Point>();

                //Find all plots within measurement range
                for (int j = i - 1; j >= 0; j--) {
                    if (timestampsAndSpeed[i].Item2.timestamp - timestampsAndSpeed[j].Item2.timestamp <= measurementRangeSeconds) {
                        range.Add(mpoints.Find(x => x.Id.Equals(timestampsAndSpeed[j].Item2.Id)));
                    } else {
                        break;
                    }
                }

                for (int j = i + 1; j < timestampsAndSpeed.Count; j++) {
                    if (timestampsAndSpeed[j].Item2.timestamp - timestampsAndSpeed[i].Item2.timestamp <= measurementRangeSeconds) {
                        range.Add(mpoints.Find(x => x.Id.Equals(timestampsAndSpeed[j].Item2.Id)));
                    } else {
                        break;
                    }
                }

                //Use the measurement range to calculate relative measure
                double relativeMeasure = 0;

                for(int j = 0; j < range.Count; j++) {
                    relativeMeasure += pointMeasures[range[0]];
                }

                relativeMeasure /= range.Count;
                relativeMeasure = timestampsAndSpeed[i].Item3 - relativeMeasure;
                relativePointMeasures.Add(mpoints.Find(x => x.Id.Equals(timestampsAndSpeed[i].Item2.Id)), relativeMeasure);
            }

            return relativePointMeasures;
        }
    }
}

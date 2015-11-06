using System;
using System.Collections.Generic;
using Npgsql;
using System.Data;
using System.Text;
using System.Device.Location;

namespace CarDataProject {
    public class DBController {
        NpgsqlConnection Connection;
        DataSet DataSet = new DataSet();
        DataTable DataTable = new DataTable();

        public DBController() {
            string connectionSettings = String.Format("Server={0};User Id={1};Password={2};Database={3};",
                Global.Database.Host, Global.Database.User, Global.Database.Password, Global.Database.Name);

            Connection = new NpgsqlConnection(connectionSettings);
            Connection.Open();
        }

        public void Close() {
            Connection.Close();
        }

        private DataRowCollection Query(string sql) {
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sql, Connection);
            DataSet.Reset();
            dataAdapter.Fill(DataSet);
            DataTable = DataSet.Tables[0];

            return DataTable.Rows;
        }

        private int NonQuery(NpgsqlCommand command, string table) {
            int affectedRows = command.ExecuteNonQuery();
            if (affectedRows == 0) {
                return -1;
            } else {
                return affectedRows;
            }
        }

        private int NonQueryWithReturnValue(NpgsqlCommand command, string table) {
            NpgsqlDataReader reader = command.ExecuteReader();

            int returnValue = -1;

            while (reader.Read()) {
                returnValue = reader.GetInt32(0);
            }
            reader.Close();

            return returnValue;
        }


        #region Creators
        public int AddCarInformation() {
            string sql = @"INSERT INTO carinformation 
                        DEFAULT VALUES RETURNING carid";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);

            return NonQueryWithReturnValue(command, "carinformation");
        }

        public Int64 AddTripInformation(int CarId) {
            string sql = @"INSERT INTO tripinformation(carid) VALUES (@carid) RETURNING tripid";

            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@carid", CarId);
            try {
                return NonQueryWithReturnValue(command, "tripinformation");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }

        public int AddSegment(SegmentInformation segment, int speedlimitForward, int speedlimitBackward, string lineString) {
            string sql = @"INSERT INTO segmentinformation(segmentid, osmid, roadname, roadtype, oneway, speedbackward, speedforward, segmentline) 
                            VALUES (@segmentid, @osmid, @roadname, @roadtype, @oneway, @speedbackward, @speedforward, ST_GeomFromText(@lineString, 4326))";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            command.Parameters.AddWithValue("@segmentid", segment.SegmentId);
            command.Parameters.AddWithValue("@osmid", segment.OSMId);
            command.Parameters.AddWithValue("@roadname", segment.RoadName);
            command.Parameters.AddWithValue("@roadtype", segment.RoadType);
            command.Parameters.AddWithValue("@oneway", segment.Oneway);
            command.Parameters.AddWithValue("@speedforward", (Int16)speedlimitForward);
            command.Parameters.AddWithValue("@speedbackward", (Int16)speedlimitBackward);
            command.Parameters.AddWithValue("@lineString", lineString);

            //Im am not entirely sure that lineString will be added this way, but it is possible
            //http://www.bostongis.com/postgis_geomfromtext.snippet
            //referring to this link.
            try {
                return NonQuery(command, "segmentinformation");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }

        public int AddQualityInformation(QualityInformation QI) {
            string sql = @"INSERT INTO qualityinformation(satellites, hdop) VALUES (@satellites, @hdop)";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            command.Parameters.AddWithValue("@satellites", QI.Sat);
            command.Parameters.AddWithValue("@hdop", QI.Hdop);

            return NonQuery(command, "qualityinformation");
        }

        public int AddINFATIEntry(INFATIEntry entry) {
            string sql = @"INSERT INTO facttable(carid, segmentid, qualityid, dayofweek, idate, itime, point, mpoint, speed) 
                        VALUES (@carid, @segmentid, @qualityid, @dayofweek, @idate, @itime, 
                        ST_Transform(ST_SetSrid(ST_MakePoint(@UTMx, @UTMy), 32632), 4326), ST_Transform(ST_SetSrid(ST_MakePoint(@UTMmx, @UTMmy), 32632), 4326), @speed)";


            // ST_Transform(ST_SetSrid(ST_MakePoint(xcoord, ycoord), 32632), 4326)

            NpgsqlCommand command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@carid", entry.CarId);

            if (entry.SegmentId != 0) {
                command.Parameters.AddWithValue("@segmentid", entry.SegmentId);
            } else {
                command.Parameters.AddWithValue("@segmentid", DBNull.Value);
            }
            command.Parameters.AddWithValue("@qualityid", entry.QualityId);
            command.Parameters.AddWithValue("@dayofweek", (int)entry.Timestamp.DayOfWeek);
            command.Parameters.AddWithValue("@idate", Convert.ToInt32(entry.Timestamp.ToString("ddMMyy")));
            command.Parameters.AddWithValue("@itime", Convert.ToInt32(entry.Timestamp.ToString("HHmmss")));
            command.Parameters.AddWithValue("@UTMx", entry.UTMx);
            command.Parameters.AddWithValue("@UTMy", entry.UTMy);
            command.Parameters.AddWithValue("@UTMmx", entry.UTMmx);
            command.Parameters.AddWithValue("@UTMmy", entry.UTMmy);
            command.Parameters.AddWithValue("@speed", entry.Speed);


            try {
                return NonQuery(command, "facttable");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            return 0;
        }
        #endregion Creators

        #region Getters

        public List<int> GetDatesByCarId(Int16 carid, bool uniqueOnly, bool sortAscending) {
            string unique = "DISTINCT ";
            string ascending = " ORDER BY date ASC";

            string sql = "SELECT ";
            if (uniqueOnly) {
                sql += unique;
            }
            sql += "idate FROM facttable WHERE carid = " + carid;
            if (sortAscending) {
                sql += ascending;
            }

            DataRowCollection result = Query(sql);
            List<int> dates = new List<int>();
            if (result.Count >= 1) {
                foreach (DataRow date in result) {
                    dates.Add(date.Field<int>("idate"));
                }
            }

            return dates;
        }

        public List<TemporalInformation> GetTimesByCarIdAndDate(Int16 carId, int date) {
            string sql = String.Format(@"SELECT entryid, itime
                                         FROM facttable
                                         WHERE carid = '{0}' AND idate = '{1}'
                                         ORDER BY itime ASC", carId, date);
            DataRowCollection result = Query(sql);

            List<TemporalInformation> timesByDate = new List<TemporalInformation>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    Int64 entryId = row.Field<Int64>("entryid");
                    int time = row.Field<int>("itime");
                    timesByDate.Add(new TemporalInformation(entryId, DateTimeHelper.ConvertToDateTime(date, time)));
                }
            }

            return timesByDate;
        }

        public List<QualityInformation> GetQualityByCarIdAndTripId(Int16 carId, Int64 tripId) {
            string sql = String.Format(@"SELECT entryid, satellites, hdop 
                                         FROM facttable LEFT JOIN qualityinformation
                                         ON(facttable.qualityid = qualityinformation.qualityid)
                                         WHERE carid = {'0'} && tripid = {'1'}", carId, tripId);
            DataRowCollection result = Query(sql);

            List<QualityInformation> qualities = new List<QualityInformation>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    Int64 entryId = row.Field<Int64>("entryid");
                    Int16 satellites = row.Field<Int16>("satellites");
                    double hdop = row.Field<double>("hdop");
                    qualities.Add(new QualityInformation(entryId, satellites, hdop));
                }
            }

            return qualities;
        }

        public List<SpatialInformation> GetMPointsByCarIdAndTripId(Int16 carId, Int64 tripId) {
            string sql = String.Format(@"SELECT entryid, idate, itime, ST_Y(mpoint) AS latitude, ST_X(mpoint) AS longitude
                                         FROM facttable
                                         WHERE carid = '{0}' AND tripid = '{1}'", carId, tripId);
            DataRowCollection result = Query(sql);

            List<Fact> facts = new List<Fact>();
            List<SpatialInformation> mPoints = new List<SpatialInformation>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    Int64 entryId = row.Field<Int64>("entryid");
                    int date = row.Field<int>("idate");
                    int time = row.Field<int>("itime");
                    double latitude = row.Field<double>("latitude");
                    double longitude = row.Field<double>("longitude");

                    facts.Add(new Fact(entryId,
                                       new TemporalInformation(DateTimeHelper.ConvertToDateTime(date, time)),
                                       new SpatialInformation(entryId, new GeoCoordinate(latitude, longitude))));
                }

                SortingHelper.FactsByDateTime(facts);

                foreach (Fact fact in facts) {
                    mPoints.Add(fact.Spatial);
                }
            }
            return mPoints;
        }

        public List<TemporalInformation> GetTimestampsByCarIdAndTripId(Int16 carId, Int64 tripId) {
            string sql = String.Format(@"SELECT entryid, idate, itime
                                         FROM facttable
                                         WHERE carid = '{0}' AND tripid = '{1}'", carId, tripId);
            DataRowCollection result = Query(sql);

            List<TemporalInformation> timestamps = new List<TemporalInformation>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    Int64 entryId = row.Field<Int64>("entryid");
                    int date = row.Field<int>("idate");
                    int time = row.Field<int>("itime");

                    timestamps.Add(new TemporalInformation(entryId, DateTimeHelper.ConvertToDateTime(date, time)));
                }

                SortingHelper.TemporalInformationByDateTime(timestamps);
            }

            return timestamps;
        }


        public List<TemporalInformation> GetTimestampsByCarId(Int16 carId) {
            string sql = String.Format(@"SELECT entryid, idate, itime
                                         FROM facttable
                                         WHERE carid = '{0}'", carId);
            DataRowCollection result = Query(sql);

            List<TemporalInformation> timestamps = new List<TemporalInformation>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    Int64 entryId = row.Field<Int64>("entryid");
                    int date = row.Field<int>("idate");
                    int time = row.Field<int>("itime");

                    timestamps.Add(new TemporalInformation(entryId, DateTimeHelper.ConvertToDateTime(date, time)));

                }
                SortingHelper.TemporalInformationByDateTime(timestamps);
            }

            return timestamps;
        }

        public List<Fact> GetSpatioTemporalByCarIdAndTripId(Int16 carId, Int64 tripId) {
            string sql = String.Format(@"SELECT entryid, idate, itime, ST_Y(mpoint) AS latitude, ST_X(mpoint) AS longitude, distancetolag
                                         FROM facttable
                                         WHERE carid = '{0}' AND tripid = '{1}'", carId, tripId);
            DataRowCollection result = Query(sql);

            List<Fact> facts = new List<Fact>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    Int64 entryId = row.Field<Int64>("entryid");
                    int date = row.Field<int>("idate");
                    int time = row.Field<int>("itime");
                    double latitude = row.Field<double>("latitude");
                    double longitude = row.Field<double>("longitude");
                    double distanceToLag = row.Field<double>("distancetolag");

                    facts.Add(new Fact(entryId,
                                       new TemporalInformation(DateTimeHelper.ConvertToDateTime(date, time)),
                                       new SpatialInformation(entryId, new GeoCoordinate(latitude, longitude), distanceToLag)));
                }

                SortingHelper.FactsByDateTime(facts);
            }

            return facts;
        }

        public List<Fact> GetSpeedInformationByCarIdAndTripId(Int16 carId, Int64 tripId) {
            string sql = String.Format(@"SELECT entryid, itime, idate, speed, distancetolag, timetolag, acceleration,  CASE 
                                            WHEN direction = TRUE THEN speedforward
                                            ELSE speedbackward
                                            END AS maxspeed
                                         FROM facttable LEFT JOIN segmentinformation
                                         ON(facttable.segmentid = segmentinformation.segmentid)
                                         WHERE carid = '{0}' AND tripid = '{1}'", carId, tripId);
            DataRowCollection res = Query(sql);

            List<Fact> speedInformation = new List<Fact>();
            if (res.Count > 0) {
                foreach (DataRow row in res) {
                    Int64 entryId = row.Field<Int64>("entryid");
                    int date = row.Field<int>("idate");
                    int time = row.Field<int>("itime");
                    double speed = row.Field<double>("speed");
                    Int16 maxSpeed = row.Field<Int16>("maxspeed");
                    double distanceToLag = row.Field<double>("distancetolag");
                    Int16 timeToLag = row.Field<Int16>("timetolag");
                    double acceleration = row.Field<double>("acceleration");

                    speedInformation.Add(new Fact(entryId, SegmentInformation.CreateWithMaxSpeed(maxSpeed),
                                       new TemporalInformation(DateTimeHelper.ConvertToDateTime(date, time), new TimeSpan(0, 0, timeToLag)),
                                       new SpatialInformation(distanceToLag),
                                       new MeasureInformation(speed, acceleration)));
                }

                SortingHelper.FactsByDateTime(speedInformation);
            }

            return speedInformation;
        }

        public Int64 GetTripCountByCarId(Int16 carId) {
            string sql = String.Format(@"SELECT COUNT(tripid) AS tripcount
                                         FROM tripinformation
                                         WHERE carid = {'0'}", carId);
            DataRowCollection result = Query(sql);

            return result[0].Field<Int64>("tripcount");
        }

        public Int64 GetTripCount() {
            string sql = String.Format(@"SELECT COUNT(tripid) AS tripcount
                                         FROM tripinformation");
            DataRowCollection result = Query(sql);

            return result[0].Field<Int64>("tripcount");
        }

        public List<Int64> GetTripIdsByCarId(Int16 carId) {
            string sql = String.Format(@"SELECT tripid
                                         FROM tripinformation
                                         WHERE carid = {'0'}", carId);
            DataRowCollection result = Query(sql);
            List<Int64> tripIds = new List<Int64>();
            foreach (DataRow row in result) {
                tripIds.Add(row.Field<Int64>("tripid"));
            }

            return tripIds;
        }

        public List<Int16> GetCarIds() {
            string sql = String.Format(@"SELECT carid
                                         FROM carinformation");
            DataRowCollection result = Query(sql);
            List<Int16> carIds = new List<Int16>();
            foreach (DataRow row in result) {
                carIds.Add(row.Field<Int16>("carid"));
            }

            return carIds;
        }

        //INFATI Loading
        public int GetQualityInformationIdBySatHdop(Int16 sat, double hdop) {
            string sql = String.Format(@"SELECT qualityid
                                         FROM qualityinformation
                                         WHERE satellites = '{0}' AND hdop = '{1}'", sat, hdop);
            DataRowCollection result = Query(sql);

            return result[0].Field<int>("qualityid");
        }


        #endregion Getters

        #region Updaters

        /*public int AddCarLogEntry(CarLogEntry entry) {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO cardata(id, entryid, carid, driverid, rdate, rtime, xcoord, ycoord, mpx, mpy, sat, hdop, maxspd, spd, strtcod, segmentkey, tripid, tripsegmentno)");
            sql.Append(" VALUES (@id, @entryid, @carid, @driverid, @rdate, @rtime, @xcoord, @ycoord, @mpx, @mpy, @sat, @hdop, @maxspd, @spd, @strtcod, @segmentkey, @tripid, @tripsegmentno)");

            NpgsqlCommand command = new NpgsqlCommand(sql.ToString(), connection);
            command.Parameters.AddWithValue("@id", entry.id);
            command.Parameters.AddWithValue("@entryid", entry.entryid);
            command.Parameters.AddWithValue("@carid", entry.carid);
            command.Parameters.AddWithValue("@driverid", entry.driverid);
            command.Parameters.AddWithValue("@rdate", entry.rdate);
            command.Parameters.AddWithValue("@rtime", entry.rtime);
            command.Parameters.AddWithValue("@xcoord", entry.xcoord);
            command.Parameters.AddWithValue("@ycoord", entry.ycoord);
            command.Parameters.AddWithValue("@mpx", entry.mpx);
            command.Parameters.AddWithValue("@mpy", entry.mpy);
            command.Parameters.AddWithValue("@sat", entry.sat);
            command.Parameters.AddWithValue("@hdop", entry.hdop);
            command.Parameters.AddWithValue("@maxspd", entry.maxspd);
            command.Parameters.AddWithValue("@spd", entry.spd);
            command.Parameters.AddWithValue("@strtcod", entry.strtcod);
            command.Parameters.AddWithValue("@segmentkey", entry.segmentkey);
            command.Parameters.AddWithValue("@tripid", entry.tripid);
            command.Parameters.AddWithValue("@tripsegmentno", entry.tripsegmentno);

            return NonQuery(command, "cardata");
        }*/

        public int InsertTripAndUpdateFactTable(INFATITrip trip) {
            Int64 tripId = AddTripInformation(trip.CarId);

            string sql = String.Format("UPDATE facttable SET tripid = '{0}' WHERE entryid = '{1}'", tripId, trip.Timestamps[0].EntryId);

            StringBuilder sb = new StringBuilder(sql);

            for (int i = 1; i < trip.Timestamps.Count; i++) {
                sb.Append(String.Format(" OR entryid = '{0}'", trip.Timestamps[i].EntryId));
            }

            NpgsqlCommand command = new NpgsqlCommand(sb.ToString(), connection);
            try {
                return NonQuery(command, "facttable");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            return 0;

        }

        public void UpdateEntryWithPointAndMpoint(Int16 carId) {


            string sql = String.Format("SELECT id AS entryids FROM cardata where carid = '{0}' ORDER BY id ASC", carId);
            DataRowCollection res = Query(sql);
            List<int> entryIds = new List<int>();
            if (res.Count >= 1) {
                foreach (DataRow logEntry in res) {
                    entryIds.Add(logEntry.Field<int>("entryids"));
                }
            }


            foreach (int entryId in entryIds) {
                string sql2 = String.Format("UPDATE cardata SET point = ST_Transform(ST_SetSrid(ST_MakePoint(xcoord, ycoord), 32632), 4326), mpoint = ST_Transform(ST_SetSrid(ST_MakePoint(mpx, mpy), 32632), 4326) WHERE id = '{0}'", entryId);
                NpgsqlCommand command = new NpgsqlCommand(sql2, Connection);
                NonQuery(command, "cardata");
            }
        }

        public void UpdateAllMlines(Int16 carId) {
            Int64 tripsTaken = CarStatistics.TripCount(carId);
            for (int i = 1; i < tripsTaken; i++) {
                string sql = String.Format("UPDATE cardata SET mline = myline FROM(SELECT newtripid, id, ST_MakeLine(mpoint, next_mpoint) AS myline FROM(SELECT newtripid, id, mpoint, lead(mpoint) OVER w as next_mpoint, lead(newtripid) OVER w as next_newtripid FROM cardata WINDOW w AS(ORDER BY id)) AS res WHERE newtripid = '{0}' AND next_newtripid = newtripid) AS calclines WHERE cardata.id = calclines.id", i);
                NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
                NonQuery(command, "cardata");
            }
        }

        #endregion Updaters

        /*public List<CarLogEntry> GetAllLogEntries() {
            string sql = String.Format("SELECT * FROM cardata");
            DataRowCollection res = Query(sql);
            List<CarLogEntry> allLogEntries = new List<CarLogEntry>();
            if (res.Count >= 1) {
                foreach (DataRow logEntry in res) {
                    allLogEntries.Add(new CarLogEntry(logEntry));
                }
                return allLogEntries;
            } else {
                return allLogEntries;
            }
        }

        public List<CarLogEntry> GetAllLogEntriesWithJSONPoint() {
            string sql = String.Format("SELECT id, entryid, carid, driverid, rdate, rtime, sat, hdop, maxspd, spd, strtcod, segmentkey, tripid, tripsegmentno, ST_X(point) AS xcoord, ST_Y(point) AS ycoord, ST_X(mpoint) AS mpx, ST_Y(mpoint) AS mpy FROM cardata");
            DataRowCollection res = Query(sql);
            List<CarLogEntry> allLogEntries = new List<CarLogEntry>();
            if (res.Count >= 1) {
                foreach (DataRow logEntry in res) {
                    allLogEntries.Add(new CarLogEntry(logEntry));
                }
                return allLogEntries;
            } else {
                return allLogEntries;
            }
        }
        
        //Lau, giver den her mening??
        public List<CarLogEntry> GetEntriesByIds(List<int> ids) {
            string sql = String.Format("SELECT * FROM cardata WHERE id = '{0}'", ids[0]);

            StringBuilder sb = new StringBuilder(sql);

            for (int i = 1; i < ids.Count; i++) {
                sb.Append(String.Format("OR id = '{0}'", ids[i]));
            }

            DataRowCollection res = Query(sql);
            List<CarLogEntry> allLogEntries = new List<CarLogEntry>();
            if (res.Count >= 1) {
                foreach (DataRow logEntry in res) {
                    allLogEntries.Add(new CarLogEntry(logEntry));
                }
                return allLogEntries;
            } else {
                return allLogEntries;
            }
        }
        */
    }
}

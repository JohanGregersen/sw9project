using System;
using System.Collections.Generic;
using Npgsql;
using System.Data;
using System.Text;
using System.Device.Location;

namespace CarDataProject {
    public class DBController {
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
        private NpgsqlConnection conn;

        private static string dbHost = "localhost";
        private static string dbName = "CarDB";
        private static string dbUser = "casper";
        private static string dbPass = "1234";

        public DBController() {
            string connstring = String.Format(
                "Server={0};User Id={1};Password={2};Database={3};Encoding=Unicode;",
                dbHost, dbUser, dbPass, dbName);
            conn = new NpgsqlConnection(connstring);
            //
            conn.Open();
        }

        public void Close() {
            conn.Close();
        }

        private DataRowCollection Query(string sql) {
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];

            return dt.Rows;
        }
        
        private int NonQuery(NpgsqlCommand command, string table) {
            int affectedRows = command.ExecuteNonQuery();
            if (affectedRows == 0) {
                return -1;
            } else {
                return affectedRows;
            }
        }

        public List<CarLogEntry> GetAllLogEntries() {
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

        public int AddCarLogEntry(CarLogEntry entry) {
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO cardata(id, entryid, carid, driverid, rdate, rtime, xcoord, ycoord, mpx, mpy, sat, hdop, maxspd, spd, strtcod, segmentkey, tripid, tripsegmentno)");
            sql.Append(" VALUES (@id, @entryid, @carid, @driverid, @rdate, @rtime, @xcoord, @ycoord, @mpx, @mpy, @sat, @hdop, @maxspd, @spd, @strtcod, @segmentkey, @tripid, @tripsegmentno)");

            NpgsqlCommand command = new NpgsqlCommand(sql.ToString(), conn);
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
        }

        public List<Int64> GetAllDatesByCarId(Int64 carid, bool uniqueOnly, bool sortAscending) {

            string unique = "DISTINCT ";
            string ascending = " ORDER BY rdate ASC";

            string sql = "SELECT ";
            if (uniqueOnly) {
                sql += unique;
            }
            sql += "rdate FROM cardata WHERE carid = " + carid;
            if (sortAscending) {
                sql += ascending;
            }

            DataRowCollection res = Query(sql);
            List<Int64> allDates = new List<Int64>();
            if (res.Count >= 1) {
                foreach (DataRow date in res) {
                    allDates.Add(date.Field<Int64>("rdate"));
                }
                return allDates;
            } else {
                return allDates;
            }
        }

        public List<Tuple<Int64, DateTime>> GetTimeByDate(Int64 rdate) {

            string sql = String.Format("SELECT id, rtime FROM cardata WHERE rdate = '{0}' ORDER BY rtime ASC", rdate);
            DataRowCollection res = Query(sql);
            List<Tuple<Int64, DateTime>> timesByDate = new List<Tuple<Int64, DateTime>>();
            if (res.Count >= 1) {
                foreach (DataRow time in res) {
                    Int64 rtime = time.Field<Int64>("rtime");

                    Int64 id = time.Field<Int64>("id");
                    timesByDate.Add(new Tuple<Int64, DateTime>(id, DateTimeHelper.ConvertToDateTime(rdate, rtime)));
                }
                return timesByDate;
            } else {
                return timesByDate;
            }
        }

        public List<SatHdop> GetSatHdopForTrip(int carId, int tripId) {
            string sql = String.Format("SELECT id, sat, hdop FROM cardata where carid = '{0}' AND newtripid = '{1}'", carId, tripId);
            DataRowCollection res = Query(sql);
            List<SatHdop> allSatHdopForCar = new List<SatHdop>();
            if (res.Count >= 1) {
                foreach (DataRow row in res) {
                    allSatHdopForCar.Add(new SatHdop(row));
                }
                return allSatHdopForCar;
            } else {
                return allSatHdopForCar;
            }
        }

        public List<Point> GetMPointByCarAndTripId(int carId, int tripId) {
            string sql = String.Format("SELECT id, ST_Y(mpoint) AS lat, ST_X(mpoint) AS lng FROM cardata where carid = '{0}' AND newtripid = '{1}' ORDER BY id ASC", carId, tripId);
            DataRowCollection res = Query(sql);
            List<Point> allLogEntries = new List<Point>();
            if (res.Count >= 1) {
                foreach (DataRow logEntry in res) {
                    allLogEntries.Add(new Point(logEntry.Field<Int64>("id"), new GeoCoordinate(logEntry.Field<double>("lat"), logEntry.Field<double>("lng"))));
                }
                return allLogEntries;
            } else {
                return allLogEntries;
            }
        }

        public List<Timestamp> GetTimestampsByCarAndTripId(int carId, int tripId) {
            string sql = String.Format("SELECT id, rdate, rtime FROM cardata where carid = '{0}' AND newtripid = '{1}' ORDER BY id ASC", carId, tripId);
            DataRowCollection res = Query(sql);
            List<Timestamp> allLogEntries = new List<Timestamp>();
            if (res.Count >= 1) {
                foreach (DataRow ts in res) {
                    allLogEntries.Add(new Timestamp(ts));
                }
                return allLogEntries;
            } else {
                return allLogEntries;
            }
        }

        public Int64 GetAmountOfTrips(int carid) {
            string sql = String.Format("SELECT COUNT(DISTINCT newtripid) AS tripamount FROM cardata");
            DataRowCollection res = Query(sql);
            return res[0].Field<Int64>("tripamount");
        }

        //UPDATE with newTripId

        public int UpdateWithNewId(int newId, Int64 currentId) {
            string sql = String.Format("UPDATE cardata SET newtripid = '{0}' WHERE id = '{1}'", newId, currentId);
            NpgsqlCommand command = new NpgsqlCommand(sql, conn);
            return NonQuery(command, "cardata");
        }

        public int UpdateWithNewIdWithMultipleEntries(int newId, List<Tuple<Int64, DateTime>> currentIds) {
            string sql = String.Format("UPDATE cardata SET newtripid = '{0}' WHERE id = '{1}'", newId, currentIds[0].Item1);

            StringBuilder sb = new StringBuilder(sql);
            
            for(int i = 1; i < currentIds.Count; i++) {
                sb.Append(String.Format("OR id = '{0}'", currentIds[i].Item1));
            }

            NpgsqlCommand command = new NpgsqlCommand(sb.ToString(), conn);
            return NonQuery(command, "cardata");
        }



    }
}
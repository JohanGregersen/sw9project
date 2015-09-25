using System;
using System.Collections.Generic;
using Npgsql;
using System.Data;
using System.Text;
using Microsoft.Win32.SafeHandles;
using System.Configuration;

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
            string sql = "INSERT INTO cardata(id, entryid, carid, driverid, rdate, rtime, sat, hdop, maxspd, spd, strtcod, segmentkey, tripid, tripsegmentno, point, mpoint) VALUES (@id, @entryid, @carid, @driverid, @rdate, @rtime, @sat, @hdop, @maxspd, @spd, @strtcod, @segmentkey, @tripid, @tripsegmentno, ST_SetSRID(ST_MakePoint(:longitude, :latitude),4326), ST_SetSRID(ST_MakePoint(:mlongitude, :mlatitude),4326))";

            NpgsqlCommand command = new NpgsqlCommand(sql, conn);
            command.Parameters.AddWithValue("@id", entry.id);
            command.Parameters.AddWithValue("@entryid", entry.entryid);
            command.Parameters.AddWithValue("@carid", entry.carid);
            command.Parameters.AddWithValue("@driverid", entry.driverid);
            command.Parameters.AddWithValue("@rdate", entry.rdate);
            command.Parameters.AddWithValue("@rtime", entry.rtime);

            var longitude = command.CreateParameter();
            //longitude.Direction = ParameterDirection.Input;
            //longitude.DbType = DbType.Double;
            longitude.ParameterName = "longitude";
            longitude.Value = entry.point.Longitude;
            command.Parameters.Add(longitude);

            var latitude = command.CreateParameter();
            //latitude.Direction = ParameterDirection.Input;
            //latitude.DbType = DbType.Double;
            latitude.ParameterName = "latitude";
            latitude.Value = entry.point.Latitude;
            command.Parameters.Add(latitude);

            var mlongitude = command.CreateParameter();
            //longitude.Direction = ParameterDirection.Input;
            //longitude.DbType = DbType.Double;
            mlongitude.ParameterName = "mlongitude";
            mlongitude.Value = entry.mpoint.Longitude;
            command.Parameters.Add(mlongitude);

            var mlatitude = command.CreateParameter();
            //latitude.Direction = ParameterDirection.Input;
            //latitude.DbType = DbType.Double;
            mlatitude.ParameterName = "mlatitude";
            mlatitude.Value = entry.mpoint.Latitude;
            command.Parameters.Add(mlatitude);

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



        public List<Int64> GetAllDatesByCarId(Int64 carid) {
            string sql = String.Format("SELECT DISTINCT rdate FROM cardata WHERE carid = '{0}' ORDER BY rdate ASC", carid);
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
            string strDate = DateAndTimeConverter(rdate);
            string sql = String.Format("SELECT id, rtime FROM cardata WHERE rdate = '{0}' ORDER BY rtime ASC", rdate);
            DataRowCollection res = Query(sql);
            List<Tuple<Int64, DateTime>> timesByDate = new List<Tuple<Int64, DateTime>>();
            if (res.Count >= 1) {
                foreach (DataRow time in res) {
                    Int64 t = time.Field<Int64>("rtime");
                    String strTime = DateAndTimeConverter(t);

                    StringBuilder sb = new StringBuilder();
                    int year = Convert.ToInt32(sb.Append("20").Append(strDate[4]).Append(strDate[5]).ToString());
                    sb.Clear();
                    int month = Convert.ToInt32(sb.Append(strDate[2]).Append(strDate[3]).ToString());
                    sb.Clear();
                    int day = Convert.ToInt32(sb.Append(strDate[0]).Append(strDate[1]).ToString());
                    sb.Clear();
                    int hour = Convert.ToInt32(sb.Append(strTime[0]).Append(strTime[1]).ToString());
                    sb.Clear();
                    int minute = Convert.ToInt32(sb.Append(strTime[2]).Append(strTime[3]).ToString());
                    sb.Clear(); 
                    int second = Convert.ToInt32(sb.Append(strTime[4]).Append(strTime[5]).ToString());
                    
                    DateTime dt = new DateTime(year, month, day, hour, minute, second);


                    Int64 id = time.Field<Int64>("id");
                    timesByDate.Add(new Tuple<Int64, DateTime>(id, dt));
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


        public static string DateAndTimeConverter (Int64 dt) {
            string strDT = dt.ToString();
            
            if(strDT.Length == 5) {
                strDT = "0" + strDT;
            }
            return strDT;
        }
    }
}
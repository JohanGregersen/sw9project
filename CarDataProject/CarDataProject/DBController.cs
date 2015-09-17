using System;
using System.Collections.Generic;
using Npgsql;
using System.Data;
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
            string sql = String.Format("SELECT id, entryid, carid, driverid, rdate, rtime, sat, hdop, maxspd, spd, strtcod, segmentkey, tripid, tripsegmentno, ST_AsGeoJSON(point) AS point, ST_AsGeoJSON(mpoint) AS mpoint FROM cardata");
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
    }
}
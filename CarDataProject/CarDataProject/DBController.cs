using System;
using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Text;
using System.Device.Location;
using System.Linq;

namespace CarDataProject {
    public class DBController {
        NpgsqlConnection Connection;
        DataSet DataSet = new DataSet();
        DataTable DataTable = new DataTable();

        public DBController() {
            string connectionSettings = String.Format("Server={0};User Id={1};Password={2};Database={3};Pooling=false;Port={4};",
                Global.Database.Host, Global.Database.User, Global.Database.Password, Global.Database.Name, Global.Database.Port);

            Connection = new NpgsqlConnection(connectionSettings);

            try {
                Connection.Open();
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

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
        public int AddCarInformation(Int16 CarId) {
            string sql = @"INSERT INTO carinformation(carid) 
                           VALUES (@carid)";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            command.Parameters.AddWithValue("@carid", CarId);

            try {
                return NonQuery(command, "carinformation");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            return 0;
        }

        public Int64 AddTripInformation(int CarId) {
            string sql = @"INSERT INTO tripfact(carid)
                           VALUES (@carid)
                           RETURNING tripid";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            command.Parameters.AddWithValue("@carid", CarId);
            try {
                return NonQueryWithReturnValue(command, "tripfact");
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

            //lineString will be added this way
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

        public int AddDimTime(INFATITime dimTime) {
            string sql = @"INSERT INTO DimTime(timeid, hour, minute, second) VALUES (@timeid, @hour, @minute, @second)";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            command.Parameters.AddWithValue("@timeid", dimTime.TimeId);
            command.Parameters.AddWithValue("@hour", dimTime.Hour);
            command.Parameters.AddWithValue("@minute", dimTime.Minute);
            command.Parameters.AddWithValue("@second", dimTime.Second);

            try {
                return NonQuery(command, "DimTime");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            return 0;
        }

        //enum Quarter { january = 1, february = 1, march = 1, april = 2, may = 2, june = 2, july = 3, august = 3, september = 3, october = 4, november = 4, december = 4}
        //enum Season { march = 1, april = 1, may = 1, june = 2, july = 2, august = 2, september = 3, october = 3, november = 3, december = 4, january = 4, february = 4}
        enum Quarter { januar = 1, februar = 1, marts = 1, april = 2, maj = 2, juni = 2, juli = 3, august = 3, september = 3, oktober = 4, november = 4, december = 4 }
        enum Season { marts = 1, april = 1, maj = 1, juni = 2, juli = 2, august = 2, september = 3, oktober = 3, november = 3, december = 4, januar = 4, februar = 4 }

        public int AddDimDate(DateTime dimDate) {
            string sql = @"INSERT INTO DimDate(dateid, year, month, day, dayofweek, weekend, quarter, season) VALUES (@dateid, @year, @month, @day, @dayofweek, @weekend, @quarter, @season)";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            StringBuilder sb = new StringBuilder();
            sb.Append(dimDate.ToString("yyyy"));
            sb.Append(dimDate.Month.ToString("D2"));
            sb.Append(dimDate.Day.ToString("D2"));
            command.Parameters.AddWithValue("@dateid", Convert.ToInt32(sb.ToString()));

            command.Parameters.AddWithValue("@year", dimDate.Year);
            command.Parameters.AddWithValue("@month", dimDate.Month);
            command.Parameters.AddWithValue("@day", dimDate.Day);
            command.Parameters.AddWithValue("@dayofweek", (Int16)dimDate.DayOfWeek);

            if ((int)dimDate.DayOfWeek == 0 || (int)dimDate.DayOfWeek == 6) {
                command.Parameters.AddWithValue("@weekend", true);
            } else {
                command.Parameters.AddWithValue("@weekend", false);
            }

            //HOLIDAY

            command.Parameters.AddWithValue("@quarter", (Int16)(Quarter)Enum.Parse(typeof(Quarter), dimDate.ToString("MMMM")));
            command.Parameters.AddWithValue("@season", (Int16)(Season)Enum.Parse(typeof(Season), dimDate.ToString("MMMM")));

            try {
                return NonQuery(command, "DimDate");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            return 0;
        }

        public int AddINFATIEntry(INFATIEntry entry) {
            string sql = @"INSERT INTO gpsfact(carid, qualityid, segmentid, timeid, dateid, point, mpoint, speed, maxspeed) 
                        VALUES (@carid, @qualityid, @segmentid, @timeid, @dateid, 
                        ST_Transform(ST_SetSrid(ST_MakePoint(@UTMx, @UTMy), 23032), 4326), ST_Transform(ST_SetSrid(ST_MakePoint(@UTMmx, @UTMmy), 23032), 4326), @speed, @maxspeed)";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            command.Parameters.AddWithValue("@carid", entry.CarId);

            if (entry.SegmentId != 0) {
                command.Parameters.AddWithValue("@segmentid", entry.SegmentId);
            } else {
                command.Parameters.AddWithValue("@segmentid", DBNull.Value);
            }

            command.Parameters.AddWithValue("@qualityid", entry.QualityId);
            command.Parameters.AddWithValue("@timeid", Convert.ToInt32(entry.Timestamp.ToString("HHmmss")));
            command.Parameters.AddWithValue("@dateid", Convert.ToInt32(entry.Timestamp.ToString("yyyyMMdd")));
            command.Parameters.AddWithValue("@UTMx", entry.UTMx);
            command.Parameters.AddWithValue("@UTMy", entry.UTMy);
            command.Parameters.AddWithValue("@UTMmx", entry.UTMmx);
            command.Parameters.AddWithValue("@UTMmy", entry.UTMmy);
            command.Parameters.AddWithValue("@speed", entry.Speed);
            command.Parameters.AddWithValue("@maxspeed", entry.MaxSpeed);

            try {
                return NonQuery(command, "gpsfact");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            return 0;
        }

        public int AddFact(Fact fact) {
            string sql = @"INSERT INTO gpsfact(carid, tripid, localtripid, segmentid, qualityid, timeid, dateid, secondstolag, point, mpoint, distancetolag, 
                                                speed, maxspeed, acceleration, jerk, speeding, accelerating, braking, jerking) 
                                               VALUES (@carid, @tripid, @localtripid, @segmentid, @qualityid, @timeid, @dateid, @secondstolag,
                                                ST_SetSrid(ST_MakePoint(@pointlng, @pointlat), 4326), ST_SetSrid(ST_MakePoint(@mpointlng, @mpointlat), 4326), 
                                                @distancetolag, @speed, @maxspeed, @acceleration, @jerk, @speeding, @accelerating, @braking, @jerking)";

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);
            command.Parameters.AddWithValue("@carid", fact.CarId);
            command.Parameters.AddWithValue("@tripid", fact.TripId);
            command.Parameters.AddWithValue("@localtripid", fact.LocalTripId);

            //SegmentInformation
            if (fact.Segment != null) {
                command.Parameters.AddWithValue("@segmentid", fact.Segment.SegmentId);
            } else {
                command.Parameters.AddWithValue("@segmentid", DBNull.Value);
            }

            //QualityInformation
            if (fact.Quality != null) {
                command.Parameters.AddWithValue("@qualityid", fact.Quality.QualityId);
            } else {

                command.Parameters.AddWithValue("@qualityid", DBNull.Value);
            }

            //TemporalInformation

            //command.Parameters.AddWithValue("@timeid", Convert.ToInt32(fact.Temporal.Timestamp.ToString("HHmmss")));
            //command.Parameters.AddWithValue("@dateid", 20000101);

            command.Parameters.AddWithValue("@timeid", Convert.ToInt32(fact.Temporal.Timestamp.ToString("HHmmss")));
            command.Parameters.AddWithValue("@dateid", Convert.ToInt32(fact.Temporal.Timestamp.ToString("yyyyMMdd")));
            command.Parameters.AddWithValue("@secondstolag", fact.Temporal.SecondsToLag.TotalSeconds);

            //SpatialInformation
            //point
            //command.Parameters.AddWithValue("@pointlat", fact.Spatial.Point.Latitude);
            //command.Parameters.AddWithValue("@pointlng", fact.Spatial.Point.Longitude);
            command.Parameters.AddWithValue("@pointlat", 0);
            command.Parameters.AddWithValue("@pointlng", 0);

            //mpoint
            command.Parameters.AddWithValue("@mpointlat", fact.Spatial.MPoint.Latitude);
            command.Parameters.AddWithValue("@mpointlng", fact.Spatial.MPoint.Longitude);
            command.Parameters.AddWithValue("@distancetolag", fact.Spatial.DistanceToLag);

            //MeasureInformation
            command.Parameters.AddWithValue("@speed", fact.Measure.Speed);
            // ATTENTION
            //////// NO MAXSPEED ///////////
            // ATTENTION
            command.Parameters.AddWithValue("@maxspeed", 0);
            command.Parameters.AddWithValue("@acceleration", fact.Measure.Acceleration);
            command.Parameters.AddWithValue("@jerk", fact.Measure.Jerk);

            //FlagInformation
            command.Parameters.AddWithValue("@speeding", fact.Flag.Speeding);
            command.Parameters.AddWithValue("@accelerating", fact.Flag.Accelerating);
            command.Parameters.AddWithValue("@braking", fact.Flag.Braking);
            command.Parameters.AddWithValue("@jerking", fact.Flag.Jerking);

            try {
                return NonQuery(command, "gpsfact");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            return 0;
        }

        #endregion Creators

        #region Getters
        public Trip GetTripByTripId(Int64 tripId) {
            string sql = String.Format(@"SELECT *
                                        FROM tripfact
                                        WHERE tripid = '{0}'", tripId);
            DataRowCollection result = Query(sql);

            if (result.Count >= 1) {
                return new Trip(result[0]);
            }

            return null;
        }

        public Trip GetTripByCarIdAndTripId(Int16 carId, Int64 tripId) {
            string sql = String.Format(@"SELECT *
                                        FROM tripfact
                                        WHERE carid = {0} AND tripid = '{1}'", carId, tripId);
            DataRowCollection result = Query(sql);

            if (result.Count >= 1) {
                return new Trip(result[0]);
            }

            return null;
        }

        public DataRow GetTripViewByCarIdAndTripId(Int16 carId, Int64 tripId) {
            string sql = String.Format(@"SELECT *
                                        FROM tripfact
                                        WHERE carid = {0} AND tripid = '{1}'", carId, tripId);
            DataRowCollection result = Query(sql);

            if (result.Count >= 1) {
                return result[0];
            }

            return null;
        }

        public List<Trip> GetTripsByCarId(Int16 carId) {
            string sql = String.Format(@"SELECT *
                                        FROM tripfact
                                        WHERE carid = '{0}'", carId);
            DataRowCollection result = Query(sql);


            List<Trip> trips = new List<Trip>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    trips.Add(new Trip(row));
                }
            }

            return trips;
        }

        public List<Trip> GetAllTrips() {
            string sql = String.Format(@"SELECT *
                                        FROM tripfact
                                        ORDER BY startdateid ASC, starttimeid ASC");
            DataRowCollection result = Query(sql);


            List<Trip> trips = new List<Trip>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    trips.Add(new Trip(row));
                }
            }

            return trips;
        }

        public List<Trip> GetTripsForListByCarId(Int16 carId, int offset) {
            string sql = String.Format(@"SELECT tripid, carid, startdateid, enddateid, starttimeid, endtimeid, metersdriven, price, optimalscore, tripscore
                                        FROM tripfact
                                        WHERE carid = '{0}'
                                        ORDER BY tripid DESC, starttimeid DESC
                                        offset '{1}' ROWS FETCH NEXT 10 ROWS ONLY", carId, offset);
            DataRowCollection result = Query(sql);


            List<Trip> trips = new List<Trip>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    trips.Add(new Trip(row));
                }
            }

            return trips;
        }

        public List<Fact> GetFactsByTripId(Int64 tripId) {

            string sql = String.Format(@"SELECT *, ST_Y(mpoint) AS latitude, ST_X(mpoint) AS longitude
                                        FROM gpsfact
                                        INNER JOIN qualityinformation
                                        ON gpsfact.qualityid = qualityinformation.qualityid
                                        WHERE tripId = '{0}'
                                        ORDER BY gpsfact.dateid ASC, gpsfact.timeid ASC, gpsfact.entryid ASC", tripId);

            DataRowCollection result = Query(sql);

            List<Fact> facts = new List<Fact>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    facts.Add(new Fact(row));
                }
            }

            return facts;
        }

        public List<Fact> GetFactsByTripIdNoQuality(Int64 tripId) {

            string sql = String.Format(@"SELECT *, ST_Y(mpoint) AS latitude, ST_X(mpoint) AS longitude
                                        FROM gpsfact
                                        WHERE tripId = '{0}'
                                        ORDER BY gpsfact.dateid ASC, gpsfact.timeid ASC, gpsfact.entryid ASC", tripId);

            DataRowCollection result = Query(sql);

            List<Fact> facts = new List<Fact>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    facts.Add(new Fact(row));
                }
            }

            return facts;
        }

        public List<Fact> GetFactsByCarIdAndTripId(Int16 carId, Int64 tripId) {

            string sql = String.Format(@"SELECT *, ST_Y(mpoint) AS latitude, ST_X(mpoint) AS longitude
                                        FROM gpsfact
                                        INNER JOIN qualityinformation
                                        ON gpsfact.qualityid = qualityinformation.qualityid
                                        WHERE carId = {0} AND tripId = '{1}'
                                        ORDER BY gpsfact.dateid ASC, gpsfact.timeid ASC, gpsfact.entryid ASC", carId, tripId);

            DataRowCollection result = Query(sql);

            List<Fact> facts = new List<Fact>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    facts.Add(new Fact(row));
                }
            }

            return facts;
        }

        public List<Fact> GetFactsForMapByCarIdAndTripId(Int16 carId, Int64 tripId) {
            string sql = String.Format(@"SELECT entryid, ST_Y(mpoint) AS latitude, ST_X(mpoint) AS longitude, dateid, timeid
                                        FROM gpsfact
                                        where carid = '{0}' AND tripid = '{1}' 
                                        ORDER BY dateid ASC, timeid ASC, Entryid ASC", carId, tripId);

            DataRowCollection result = Query(sql);

            List<Fact> facts = new List<Fact>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    facts.Add(new Fact(row));
                }
            }

            return facts;
        }

        public List<Fact> GetFactsForMapMatching(Int16 carId, Int64 tripId) {
            string sql = String.Format(@"SELECT entryid, mpoint, dateid, timeid
                                        FROM gpsfact
                                        where carid = '{0}' AND tripid = '{1}' 
                                        ORDER BY dateid ASC, timeid ASC, Entryid ASC", carId, tripId);

            DataRowCollection result = Query(sql);

            List<Fact> facts = new List<Fact>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    facts.Add(new Fact(row));
                }
            }

            return facts;
        }

        public List<TemporalInformation> GetTimesByCarIdAndDate(Int16 carId, int date) {
            string sql = String.Format(@"SELECT entryid, timeid
                                         FROM gpsfact
                                         WHERE carid = '{0}' AND dateid = '{1}'
                                         ORDER BY timeid ASC", carId, date);
            DataRowCollection result = Query(sql);

            List<TemporalInformation> timesByDate = new List<TemporalInformation>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    Int64 entryId = row.Field<Int64>("entryid");
                    int time = row.Field<int>("timeid");
                    timesByDate.Add(new TemporalInformation(entryId, DateTimeHelper.ConvertToDateTime(date, time)));
                }
            }

            return timesByDate;
        }

        public List<QualityInformation> GetQualityByCarIdAndTripId(Int16 carId, Int64 tripId) {
            string sql = String.Format(@"SELECT entryid, satellites, hdop 
                                         FROM gpsfact LEFT JOIN qualityinformation
                                         ON(gpsfact.qualityid = qualityinformation.qualityid)
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
            string sql = String.Format(@"SELECT entryid, dateid, timeid, ST_Y(mpoint) AS latitude, ST_X(mpoint) AS longitude
                                         FROM gpsfact
                                         WHERE carid = '{0}' AND tripid = '{1}'", carId, tripId);
            DataRowCollection result = Query(sql);

            List<Fact> facts = new List<Fact>();
            List<SpatialInformation> mPoints = new List<SpatialInformation>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    Int64 entryId = row.Field<Int64>("entryid");
                    int date = row.Field<int>("dateid");
                    int time = row.Field<int>("timeid");
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
            string sql = String.Format(@"SELECT entryid, dateid, timeid
                                         FROM gpsfact
                                         WHERE carid = '{0}' AND tripid = '{1}'", carId, tripId);
            DataRowCollection result = Query(sql);

            List<TemporalInformation> timestamps = new List<TemporalInformation>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    Int64 entryId = row.Field<Int64>("entryid");
                    int date = row.Field<int>("dateid");
                    int time = row.Field<int>("timeid");

                    timestamps.Add(new TemporalInformation(entryId, DateTimeHelper.ConvertToDateTime(date, time)));
                }

                SortingHelper.TemporalInformationByDateTime(timestamps);
            }

            return timestamps;
        }

        public List<TemporalInformation> GetTimestampsByCarId(Int16 carId) {
            string sql = String.Format(@"SELECT entryid, dateid, timeid
                                         FROM gpsfact
                                         WHERE carid = '{0}'", carId);
            DataRowCollection result = Query(sql);

            List<TemporalInformation> timestamps = new List<TemporalInformation>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    Int64 entryId = row.Field<Int64>("entryid");
                    int date = row.Field<int>("dateid");
                    int time = row.Field<int>("timeid");

                    timestamps.Add(new TemporalInformation(entryId, DateTimeHelper.ConvertToDateTime(date, time)));
                }

                SortingHelper.TemporalInformationByDateTime(timestamps);
            }

            return timestamps;
        }

        ///<summary>
        ///<para>Gets list of roadtypes sorted by date and time</para>
        ///<para>Null entries will be assigned as "noinfo"</para>
        ///</summary>
        /// 

        public List<Global.Enums.RoadType> GetRoadTypesByTripId(Int64 tripId) {
            string sql = String.Format(@"select roadtype
                                         from 
                                             (select segmentid, dateid, timeid
                                             from gpsfact
                                             where tripid = '{0}' ) AS segments
                                         LEFT JOIN segmentinformation
                                         ON segments.segmentid = segmentinformation.segmentid
                                         ORDER BY segments.dateid ASC, segments.timeid ASC", tripId);
            DataRowCollection result = Query(sql);

            List<Global.Enums.RoadType> roadTypes = new List<Global.Enums.RoadType>();
            foreach (DataRow row in result) {
                if (DBNull.Value.Equals(row["roadtype"])) {
                    roadTypes.Add(Global.Enums.RoadType.noinfo);
                } else {
                    roadTypes.Add((Global.Enums.RoadType)(row.Field<Int16>("roadtype")));
                }
            }

            return roadTypes;
        }

        /*
        public List<Global.Enums.RoadType> GetRoadTypesByTripId(Int64 tripId) {
            string sql = String.Format(@"SELECT roadtype
                                         FROM gpsfact LEFT JOIN segmentinformation
                                         ON (gpsfact.segmentid = segmentinformation.segmentid)
                                         WHERE tripid = '{0}'
                                         ORDER BY dateid, timeid ASC", tripId);
            DataRowCollection result = Query(sql);

            List<Global.Enums.RoadType> roadTypes = new List<Global.Enums.RoadType>();
            foreach (DataRow row in result) {
                if (DBNull.Value.Equals(row["roadtype"])) {
                    roadTypes.Add(Global.Enums.RoadType.noinfo);
                } else {
                    roadTypes.Add((Global.Enums.RoadType)(row.Field<Int16>("roadtype")));
                }
            }

            return roadTypes;
        }
        */

        public List<Fact> GetSpatioTemporalByCarIdAndTripId(Int16 carId, Int64 tripId) {
            string sql = String.Format(@"SELECT entryid, dateid, timeid, ST_Y(mpoint) AS latitude, ST_X(mpoint) AS longitude, distancetolag
                                         FROM gpsfact
                                         WHERE carid = '{0}' AND tripid = '{1}'", carId, tripId);
            DataRowCollection result = Query(sql);

            List<Fact> facts = new List<Fact>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    Int64 entryId = row.Field<Int64>("entryid");
                    int date = row.Field<int>("dateid");
                    int time = row.Field<int>("timeid");
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
            string sql = String.Format(@"SELECT entryid, timeid, dateid, speed, distancetolag, secondstolag, acceleration, maxspeed
                                         FROM gpsfact LEFT JOIN segmentinformation
                                         ON(gpsfact.segmentid = segmentinformation.segmentid)
                                         WHERE carid = '{0}' AND tripid = '{1}'", carId, tripId);
            DataRowCollection res = Query(sql);

            List<Fact> speedInformation = new List<Fact>();
            if (res.Count > 0) {
                foreach (DataRow row in res) {
                    Int64 entryId = row.Field<Int64>("entryid");
                    int date = row.Field<int>("dateid");
                    int time = row.Field<int>("timeid");
                    double speed = row.Field<double>("speed");
                    Int16 maxSpeed = row.Field<Int16>("maxspeed");
                    double distanceToLag = row.Field<double>("distancetolag");
                    Int16 timeToLag = row.Field<Int16>("secondstolag");
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
                                         FROM tripfact 
                                         WHERE carid = '{0}'", carId);
            DataRowCollection result = Query(sql);

            return result[0].Field<Int64>("tripcount");
        }

        public Int64 GetTripCount() {
            string sql = String.Format(@"SELECT COUNT(tripid) AS tripcount
                                         FROM tripfact");
            DataRowCollection result = Query(sql);

            return result[0].Field<Int64>("tripcount");
        }

        public List<Int64> GetTripIdsByCarId(Int16 carId) {
            string sql = String.Format(@"SELECT tripid
                                         FROM tripfact
                                         WHERE carid = '{0}'
                                         ORDER BY tripid ASC", carId);
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

        public int UpdateGPSFactsWithMapMatching(Dictionary<int, List<SpatialInformation>> mapmatchedEntries) {
            string sql = String.Format(@"UPDATE gpsfact
                                            SET segmentid = @segmentid ,
                                            mpoint = ST_SetSrid(ST_MakePoint(@mpointlng, @mpointlat), 4326)
                                            WHERE entryid = @entryid");

            foreach (KeyValuePair<int, List<SpatialInformation>> segments in mapmatchedEntries) {
                foreach (SpatialInformation entry in segments.Value) {

                    NpgsqlCommand command = new NpgsqlCommand(sql, Connection);

                    command.Parameters.AddWithValue("@entryid", entry.EntryId);
                    command.Parameters.AddWithValue("@segmentid", segments.Key);
                    command.Parameters.AddWithValue("@mpointlat", entry.MPoint.Latitude);
                    command.Parameters.AddWithValue("@mpointlng", entry.MPoint.Longitude);

                    try {
                        NonQuery(command, "gpsfact");
                    } catch (Exception e) {
                        Console.WriteLine(e.ToString());
                    }
                }
            }

            return 0;
        }

        public int UpdateGPSFactWithMeasures(List<Fact> UpdatedFacts) {
            string sql = String.Format(@"UPDATE gpsfact
                                            SET pathline = ST_MakeLine(ST_SetSRID(ST_MakePoint(@prevMPointLng, @prevMpointLat), 4326), ST_SetSRID(ST_MakePoint(@MPointLng, @MpointLat),4326)),
                                            acceleration = @acceleration,
                                            jerk = @jerk,
                                            distancetolag = @distancetolag,
                                            secondstolag = @secondstolag,
                                            speeding = @speeding,
                                            accelerating = @accelerating,
                                            jerking = @jerking,
                                            braking = @braking,
                                            steadyspeed = @steadyspeed
                                            WHERE entryid = @entryid");

            for (int i = 1; i < UpdatedFacts.Count; i++) {
                NpgsqlCommand command = new NpgsqlCommand(sql, Connection);

                command.Parameters.AddWithValue("@entryid", UpdatedFacts[i].EntryId);
                command.Parameters.AddWithValue("@prevMPointLng", UpdatedFacts[i - 1].Spatial.MPoint.Longitude);
                command.Parameters.AddWithValue("@prevMpointLat", UpdatedFacts[i - 1].Spatial.MPoint.Latitude);
                command.Parameters.AddWithValue("@MPointLng", UpdatedFacts[i].Spatial.MPoint.Longitude);
                command.Parameters.AddWithValue("@MpointLat", UpdatedFacts[i].Spatial.MPoint.Latitude);
                command.Parameters.AddWithValue("@acceleration", UpdatedFacts[i].Measure.Acceleration);
                command.Parameters.AddWithValue("@jerk", UpdatedFacts[i].Measure.Jerk);
                command.Parameters.AddWithValue("@distancetolag", UpdatedFacts[i].Spatial.DistanceToLag);
                command.Parameters.AddWithValue("@secondstolag", UpdatedFacts[i].Temporal.SecondsToLag.Seconds);
                command.Parameters.AddWithValue("@speeding", UpdatedFacts[i].Flag.Speeding);
                command.Parameters.AddWithValue("@accelerating", UpdatedFacts[i].Flag.Accelerating);
                command.Parameters.AddWithValue("@jerking", UpdatedFacts[i].Flag.Jerking);
                command.Parameters.AddWithValue("@braking", UpdatedFacts[i].Flag.Braking);
                command.Parameters.AddWithValue("@steadyspeed", UpdatedFacts[i].Flag.SteadySpeed);

                try {
                    NonQuery(command, "gpsfact");
                } catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }

            return 0;
        }

        public int UpdateTripFactWithMeasures(Trip UpdatedTrip) {
            string sql = String.Format(@"UPDATE tripfact
                                         SET previoustripid = @previoustripid,
                                          startdateid = @startdateid,
                                          starttimeid = @starttimeid,
                                          enddateid = @enddateid,
                                          endtimeid = @endtimeid,
                                          secondsdriven = @secondsdriven,
                                          metersdriven = @metersdriven,
                                          tripscore = @tripscore,
                                          optimalscore = @optimalscore,
                                          jerkcount = @jerkcount,
                                          brakecount = @brakecount,
                                          accelerationcount = @accelerationcount,
                                          meterssped = @meterssped,
                                          timesped = @timesped,
                                          steadyspeeddistance = @steadyspeeddistance,
                                          steadyspeedtime = @steadyspeedtime,
                                          secondstolag = @secondstolag,
                                          roadtypesinterval = @roadtypesinterval,
                                          criticaltimeinterval =  @criticaltimeinterval,
                                          speedinterval = @speedinterval,
                                          accelerationinterval = @accelerationinterval,
                                          jerkinterval = @jerkinterval,
                                          brakinginterval = @brakinginterval,
                                          dataquality = @dataquality
                                         WHERE tripid = @tripid");

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);

            command.Parameters.AddWithValue("@tripid", UpdatedTrip.TripId);

            if (UpdatedTrip.PreviousTripId != 0) {
                command.Parameters.AddWithValue("@previoustripid", UpdatedTrip.PreviousTripId);
            } else {
                command.Parameters.AddWithValue("@previoustripid", DBNull.Value);
            }

            command.Parameters.AddWithValue("@startdateid", Convert.ToInt32(UpdatedTrip.StartTemporal.Timestamp.ToString("yyyyMMdd")));
            command.Parameters.AddWithValue("@starttimeid", Convert.ToInt32(UpdatedTrip.StartTemporal.Timestamp.ToString("HHmmss")));
            command.Parameters.AddWithValue("@enddateid", Convert.ToInt32(UpdatedTrip.EndTemporal.Timestamp.ToString("yyyyMMdd")));
            command.Parameters.AddWithValue("@endtimeid", Convert.ToInt32(UpdatedTrip.EndTemporal.Timestamp.ToString("HHmmss")));
            command.Parameters.AddWithValue("@secondsdriven", UpdatedTrip.SecondsDriven.TotalSeconds);
            command.Parameters.AddWithValue("@metersdriven", UpdatedTrip.MetersDriven);
            //price?
            command.Parameters.AddWithValue("@tripscore", UpdatedTrip.TripScore);
            command.Parameters.AddWithValue("@optimalscore", UpdatedTrip.OptimalScore);
            command.Parameters.AddWithValue("@jerkcount", UpdatedTrip.JerkCount);
            command.Parameters.AddWithValue("@brakecount", UpdatedTrip.BrakeCount);
            command.Parameters.AddWithValue("@accelerationcount", UpdatedTrip.AccelerationCount);
            command.Parameters.AddWithValue("@meterssped", UpdatedTrip.MetersSped);
            command.Parameters.AddWithValue("@timesped", UpdatedTrip.TimeSped.TotalSeconds);
            command.Parameters.AddWithValue("@steadyspeeddistance", UpdatedTrip.SteadySpeedDistance);
            command.Parameters.AddWithValue("@steadyspeedtime", UpdatedTrip.SteadySpeedTime.TotalSeconds);
            command.Parameters.AddWithValue("@secondstolag", UpdatedTrip.SecondsToLag.TotalSeconds);

            //Interval Information
            command.Parameters.AddWithValue("@roadtypesinterval", UpdatedTrip.IntervalInformation.RoadTypesInterval);
            command.Parameters.AddWithValue("@criticaltimeinterval", UpdatedTrip.IntervalInformation.CriticalTimeInterval);
            command.Parameters.AddWithValue("@speedinterval", UpdatedTrip.IntervalInformation.SpeedInterval);
            command.Parameters.AddWithValue("@accelerationinterval", UpdatedTrip.IntervalInformation.AccelerationInterval);
            command.Parameters.AddWithValue("@jerkinterval", UpdatedTrip.IntervalInformation.JerkInterval);
            command.Parameters.AddWithValue("@brakinginterval", UpdatedTrip.IntervalInformation.BrakingInterval);

            command.Parameters.AddWithValue("@dataquality", UpdatedTrip.DataQuality);

            try {
                NonQuery(command, "tripfact");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            return 0;
        }

        public int UpdateTripFactWithIntervals(Trip UpdatedTrip) {
            string sql = String.Format(@"UPDATE tripfact
                                         SET roadtypesinterval = @roadtypesinterval,
                                             criticaltimeinterval =  @criticaltimeinterval,
                                             speedinterval = @speedinterval,
                                             accelerationinterval = @accelerationinterval,
                                             jerkinterval = @jerkinterval,
                                             brakinginterval = @brakinginterval
                                         WHERE tripid = @tripid");

            NpgsqlCommand command = new NpgsqlCommand(sql, Connection);

            command.Parameters.AddWithValue("@tripid", UpdatedTrip.TripId);

            command.Parameters.AddWithValue("@roadtypesinterval", UpdatedTrip.IntervalInformation.RoadTypesInterval);
            command.Parameters.AddWithValue("@criticaltimeinterval", UpdatedTrip.IntervalInformation.CriticalTimeInterval);
            command.Parameters.AddWithValue("@speedinterval", UpdatedTrip.IntervalInformation.SpeedInterval);
            command.Parameters.AddWithValue("@accelerationinterval", UpdatedTrip.IntervalInformation.AccelerationInterval);
            command.Parameters.AddWithValue("@jerkinterval", UpdatedTrip.IntervalInformation.JerkInterval);
            command.Parameters.AddWithValue("@brakinginterval", UpdatedTrip.IntervalInformation.BrakingInterval);

            try {
                NonQuery(command, "tripfact");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            return 0;
        }

        public int InsertTripAndUpdateFactTable(INFATITrip trip) {
            Int64 tripId = AddTripInformation(trip.CarId);

            if (trip.Timestamps.Count > 2000) {
                List<INFATITrip> subTrips = new List<INFATITrip>();
                INFATITrip subTrip;
                int index = 0;

                while (true) {
                    if (index + 2000 > trip.Timestamps.Count) {
                        subTrip = new INFATITrip(trip.CarId);
                        subTrip.Timestamps = trip.Timestamps.GetRange(index, trip.Timestamps.Count - index);
                        subTrips.Add(subTrip);
                        break;
                    } else {
                        subTrip = new INFATITrip(trip.CarId);
                        subTrip.Timestamps = trip.Timestamps.GetRange(index, 2000);
                        subTrips.Add(subTrip);
                    }

                    index = index + 2000;
                }

                foreach (INFATITrip sub in subTrips) {
                    UpdateFactTable(tripId, sub);
                }
            } else {
                UpdateFactTable(tripId, trip);
            }

            return 0;
        }

        public int UpdateFactTable(Int64 tripId, INFATITrip trip) {
            string sql = String.Format("UPDATE gpsfact SET tripid = '{0}' WHERE entryid = '{1}'", tripId, trip.Timestamps[0].EntryId);
            StringBuilder sb = new StringBuilder(sql);

            for (int i = 1; i < trip.Timestamps.Count; i++) {
                sb.Append(String.Format(" OR entryid = '{0}'", trip.Timestamps[i].EntryId));
            }

            NpgsqlCommand command = new NpgsqlCommand(sb.ToString(), Connection);
            try {
                return NonQuery(command, "gpsfact");
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

        public void UpdateEntriesWithNoTrip(List<Int64> entries) {
            if (entries.Count() > 2000) {
                List<List<Int64>> subEntries = new List<List<Int64>>();
                List<Int64> subEntry;
                int index = 0;

                while (true) {
                    if (index + 2000 > entries.Count) {
                        subEntry = new List<Int64>();
                        subEntry = entries.GetRange(index, entries.Count - index);
                        subEntries.Add(subEntry);
                        break;
                    } else {
                        subEntry = new List<Int64>();
                        subEntry = entries.GetRange(index, 2000);
                        subEntries.Add(subEntry);
                    }

                    index = index + 2000;
                }

                foreach (List<Int64> subEnt in subEntries) {
                    UpdateFactNoTrip(subEnt);
                }
            } else {
                UpdateFactNoTrip(entries);
            }

        }

        public int UpdateFactNoTrip(List<Int64> entries) {
            string sql = String.Format("UPDATE gpsfact SET tripid = null WHERE entryid = {0}", entries[0]);
            StringBuilder sb = new StringBuilder(sql);

            for (int i = 1; i < entries.Count; i++) {
                sb.Append(String.Format(" OR entryid = {0}", entries[i]));
            }

            NpgsqlCommand command = new NpgsqlCommand(sb.ToString(), Connection);
            try {
                return NonQuery(command, "gpsfact");
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            return 0;
        }

        #endregion Updaters

        #region Cache
        public List<QualityInformation> GetQualityInformationTable() {
            string sql = String.Format(@"SELECT qualityid, satellites, hdop
                                        FROM qualityinformation");


            DataRowCollection result = Query(sql);

            List<QualityInformation> QIs = new List<QualityInformation>();
            if (result.Count >= 1) {
                foreach (DataRow row in result) {
                    QIs.Add(new QualityInformation(row.Field<int>("qualityid"), row.Field<Int16>("satellites"), (double)row.Field<Single>("hdop")));
                }
            }

            return QIs;
        }
        #endregion
    }
}

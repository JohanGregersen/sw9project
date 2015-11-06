using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using NpgsqlTypes;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    public static class INFATILoader {
        private enum RoadTypes { motorway = 1, trunk, primary, secondary, tertiary, motorway_link, primary_link, unclassified, road, residential,
                                    service, track, pedestrian, unpaved, living_street, trunk_link, secondary_link, ferry, tertiary_link, motorway_link_entry = 6, motorway_link_exit = 6};
        private enum Direction { forward = 0, backward = 0, FORWARD = 0, BACKWARD = 0, both = 1, BOTH = 1}
    
        public static void DBInitializer() {
            //Fill quality-information with data
            //QualityInformationInitializer(0, 30, 0, 0, 33);

            //Load Map.csv from INFATI-datacollection
            //LoadMap();


        }

        public static void DBLoader(List<INFATIEntry> entries) {
            DBController dbc = new DBController();

            //CarInformation

            int carId = dbc.AddCarInformation();





            foreach (INFATIEntry entry in entries) {
                entry.CarId = carId;
                //TemportalInformation


                //QualityInformation
                entry.QualityId = dbc.GetQualityInformationIdBySatHdop(entry.Sat, entry.Hdop);

                //SegmentInformation
                dbc.AddINFATIEntry(entry);
            }
            dbc.Close();
        }

        private static void QualityInformationInitializer(int minHdop, int maxHdop, int hdopDecimals, Int16 minSat, Int16 maxSat) {
            List<QualityInformation> SatHdopList = new List<QualityInformation>();


            for (Int16 x = minSat; x <= maxSat; x++) {
                for (int y = minHdop; y <= maxHdop; y++) {
                    SatHdopList.Add(new QualityInformation(x, y));
                }
            }





            /*
            DECIMALER TIL SAT/HDOP. DER ER PRECISION LOSS I DEN HER MÅDE AT GØRE DET PÅ
            FIND PÅ EN NY METODE HVIS DET ER VIGTIGT

            double decimalStart = 1;
            double tickDistance = 0;

            if (hdopDecimals > 0) {
                tickDistance = Math.Pow(0.1, hdopDecimals);
                decimalStart = decimalStart - tickDistance;

                for (Int16 x = minSat; x <= maxSat; x++) {
                    for (int y = minHdop; y <= maxHdop; y++) {
                        for (double z = decimalStart; z <= 0; z -= tickDistance) {
                            SatHdopList.Add(new QualityInformation(x, y + z));
                        }
                    }
                }

            } else {
                for (Int16 x = minSat; x <= maxSat; x++) {
                    for (int y = minHdop; y <= maxHdop; y++) {
                        SatHdopList.Add(new QualityInformation(x, y));
                    }
                }
            }
            */
            DBController dbc = new DBController();
            foreach(QualityInformation QI in SatHdopList) {
                dbc.AddQualityInformation(QI);
            }
            dbc.Close();
        }

        public static void LoadMap() {
            //Open file from path
            StreamReader file = new StreamReader(Global.Batch.INFATI.Path + "\\map.csv");

            //Discard data header
            file.ReadLine();

            //Read remaining file, split every row into its columns
            string entry;
            List<List<string>> rows = new List<List<string>>();

            while ((entry = file.ReadLine()) != null) {
                List<string> elements = entry.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                //Add to list of entries
                rows.Add(elements);
            }

            file.Close();

            //Assemble the entries into SegmentInformation
            List<Fact> facts = new List<Fact>();
            DBController dbc = new DBController();

            foreach (List<string> row in rows) {
                //(Int64 SegmentId, Int64 OSMId, string RoadName, Int16 RoadType, Int16 Oneway, Int16 Bridge, Int16 Tunnel, Int16 MaxSpeed, bool Direction, PostgisLineString RoadLine)
                //segmentkey0;segmentid1;name2;category3;startpoint4;endpoint5;direction6;speedlimit_forward7;speedlimit_backward8;meters9;geom10
                if (row.Count != 11) {
                    Int64 segmentIdC12 = Int64.Parse(row[0]);
                    Int64 osmIdC12 = Int64.Parse(row[1]);
                    string RoadNameC12 = row[2]+";"+row[3];

                    Int16 RoadTypeC12 = (Int16)(RoadTypes)Enum.Parse(typeof(RoadTypes), row[4]);
                    Int16 OneWayC12 = (Int16)(Direction)Enum.Parse(typeof(Direction), row[7]);


                    SegmentInformation segmentC12 = new SegmentInformation(segmentIdC12, osmIdC12, RoadNameC12, RoadTypeC12, OneWayC12, 0, 0, 0, false, null);

                    Int16 speedlimitForwardC12 = Int16.Parse(row[8]);
                    Int16 speedlimitBackwardC12 = Int16.Parse(row[9]);
                    string lineStringC12 = row[11];

                    dbc.AddSegment(segmentC12, speedlimitForwardC12, speedlimitBackwardC12, lineStringC12);
                    //REFACTOR POTENTIAL
                    continue;
                }

                Int64 segmentId = Int64.Parse(row[0]);
                Int64 osmId = Int64.Parse(row[1]);
                string RoadName = row[2];

                Int16 RoadType = (Int16)(RoadTypes)Enum.Parse(typeof(RoadTypes), row[3]);
                Int16 OneWay = (Int16)(Direction)Enum.Parse(typeof(Direction), row[6]);


                SegmentInformation segment = new SegmentInformation(segmentId, osmId, RoadName, RoadType, OneWay, 0, 0, 0, false, null);

                Int16 speedlimitForward = Int16.Parse(row[7]);
                Int16 speedlimitBackward = Int16.Parse(row[8]);
                string lineString = row[10];

                dbc.AddSegment(segment, speedlimitForward, speedlimitBackward, lineString);
            }
            dbc.Close();
        }


        /*
highway=motorway 1
highway=trunk 2
highway=primary 3 
highway=secondary 4 
highway=tertiary 5
highway=motorway link 6
highway=primary link 7
highway=unclassified 8
highway=road 9
highway=residential 10
highway=service 11
highway=track 12
highway=pedestrian 13
*/


        //Number of decimals
        //hdop 30
        //sat 33
        //https://en.wikipedia.org/wiki/List_of_GPS_satellites

    }
}

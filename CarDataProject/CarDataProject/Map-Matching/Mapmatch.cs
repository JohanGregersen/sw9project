using System;
using System.Collections.Generic;
using GeoCoordinatePortable;
using System.Text;
using System.Net;
using System.IO;

using Newtonsoft.Json.Linq;

namespace CarDataProject {
    public static class Mapmatch {
        private const string APPID = "496d0607";
        private const string APPKEY = "c8a39964a19641b526ff3eef7d39d272";

        public static void MatchTrip(Int16 carId, Int64 tripId) {
            DBController dbc = new DBController();
            List<Fact> facts = dbc.GetFactsForMapByCarIdAndTripId(carId, tripId);
            dbc.Close();
            string postData = "";
            
            //CSV FORMAT
            foreach (Fact f in facts) {
                postData += f.EntryId + "," + f.Spatial.MPoint.Longitude.ToString().Replace(",",".") + "," + f.Spatial.MPoint.Latitude.ToString().Replace(",", ".") + ",\"" + f.Temporal.Timestamp.ToString("yyyy-MM-dd") + "T" + f.Temporal.Timestamp.TimeOfDay + "\"" + System.Environment.NewLine;
            }

            //postData = ConvertToXML(facts);

            // Create a request using a URL that can receive a post. 
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://test.roadmatching.com/rest/mapmatch/?app_id=" + APPID + "&app_key=" + APPKEY + "&output.groupByWays=false&output.linkGeometries=false&output.osmProjection=false&output.linkMatchingError=false&output.waypoints=true&output.waypointsIds=true");
            
            // Set the Method property of the request to POST.
            request.Method = "POST";
            request.ContentType = "text/csv";
            //request.ContentType = "application/gpx+xml";

            request.Accept = "application/json";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine("Server response: " + responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            SaveMapMatchingToDB(responseFromServer);
        }

        private static void SaveMapMatchingToDB(string json) {
            dynamic response = JObject.Parse(json) as JObject;
            Dictionary<int, List<SpatialInformation>> linksWithWaypoints = new Dictionary<int, List<SpatialInformation>>();
            foreach (dynamic entries in response.diary.entries) {
                foreach (dynamic link in entries.route.links) {
                    List<SpatialInformation> waypoints = new List<SpatialInformation>();
                    if (link.wpts != null) {
                        int linkId = (int)link.id;

                        if (!linksWithWaypoints.ContainsKey(linkId)) {
                            linksWithWaypoints.Add(linkId, waypoints);
                        }

                        foreach (dynamic waypoint in link.wpts) {
                            int waypointId = (int)waypoint.id;
                            double x = (double)waypoint.x;
                            double y = (double)waypoint.y;
                            waypoints.Add(new SpatialInformation(waypointId, new GeoCoordinate(x, y)));
                        }
                    }
                }
            }

            DBController dbc = new DBController();
            dbc.UpdateGPSFactsWithMapMatching(linksWithWaypoints);
            dbc.Close();
        }
        private static string ConvertToXML(List<Fact> facts) {

            StringBuilder sb = new StringBuilder();

            sb.Append("<? xml version = \" 1.0 \" encoding = \"UTF-8\" ?>");
            sb.Append("< gpx xmlns = \"http://www.topografix.com/GPX/1/1 \" version = \"1.1\" creator = \"StudApp\" xmlns: xsi = \"http://www.w3.org/2001/XMLSchema-instance \" xsi: schemaLocation = \"http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd \" >");
            sb.Append("<trk>");
            sb.Append("<name>StudApp</name>");
            //< cmt >< ![CDATA[Warning: HDOP values aren't the HDOP as returned by the GPS device. They're approximated from the location accuracy in meters.]] ></ cmt >
            sb.Append("<trkseg>");
            foreach (Fact f in facts) {
                sb.Append("<trkpt lat=\"");
                sb.Append(f.Spatial.MPoint.Latitude.ToString().Replace(",", "."));
                sb.Append("\" lon=\"");
                sb.Append(f.Spatial.MPoint.Longitude.ToString().Replace(",", "."));
                sb.Append("\">");

                sb.Append("<time>");
                sb.Append(f.Temporal.Timestamp.ToString("yyyy-MM-dd") + "T" + f.Temporal.Timestamp.TimeOfDay + "Z");
                sb.Append("</time>");

                sb.Append("<name>");
                sb.Append(f.EntryId);
                sb.Append("</name>");
                sb.Append("</trkpt>");
            }

            sb.Append("</trkseg>");
            sb.Append("</trk>");
            sb.Append("</gpx>");

            return sb.ToString();
        }
    }
}

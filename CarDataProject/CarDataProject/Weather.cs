using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace CarDataProject
{
    class Weather {
        string locStations = "ghcnd-stations.txt";

        public string getsomething() {
            WebClient request = new WebClient();
            string url = "ftp://ftp.ncdc.noaa.gov/pub/data/ghcn/daily/" + locStations;
            request.Credentials = new NetworkCredential("anonymous", "anonymous@example.com");

            try {
                byte[] newFileData = request.DownloadData(url);
                string fileString = Encoding.UTF8.GetString(newFileData);
                return fileString;
            } catch (WebException e) {
                //TODO: Handle errors
            }
            return null;
        }
    }
}

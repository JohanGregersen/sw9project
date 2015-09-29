using System.Text;
using System.Net;

namespace CarDataProject {
    public static class Weather {
        //Credentials needed to download from NOAA
        private static string CredentialName = "anonymous";
        private static string CredentialMail = "anonymous@example.com";

        //Where to find files
        private static string Domain = "ftp://ftp.ncdc.noaa.gov/pub/data/ghcn/daily/";
        private static string Filetype = ".txt";

        //Receives a file from a requested location
        public static string getsomething() {
            string filename = "ghcnd-stations";
            WebClient request = new WebClient();
            request.Credentials = new NetworkCredential(CredentialName, CredentialMail);

            try {
                byte[] newFileData = request.DownloadData(Domain + filename + Filetype);
                return Encoding.UTF8.GetString(newFileData);
            } catch (WebException e) {
                //TODO: Handle errors
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    public static class INFATILoader {

        public static void DBInitializer() {
            //Fill quality-information with data
            List<QualityInformation> QILIst = QualityInformationInitializer(0, 30, 2, 0, 33);
        }

        public static void DBLoader(List<Fact> facts) {
            //CarInformation


        }


        private static List<QualityInformation> QualityInformationInitializer(int minHdop, int maxHdop, int hdopDecimals, Int16 minSat, Int16 maxSat) {
            List<QualityInformation> SatHdopList = new List<QualityInformation>();
            double decimalStart = 1;
            double tickDistance = 0;
            if (hdopDecimals > 0) {
                tickDistance = Math.Pow(0.1, hdopDecimals);
                decimalStart = decimalStart - tickDistance;

                for (Int16 x = minSat; x <= maxSat; x++) {
                    for (int y = minHdop; y <= maxHdop; y++) {
                        for (double z = decimalStart; z == 0; z -= tickDistance) {
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


            return SatHdopList;
        }


        //Number of decimals
        //hdop 30
        //sat 33
        //https://en.wikipedia.org/wiki/List_of_GPS_satellites

    }
}

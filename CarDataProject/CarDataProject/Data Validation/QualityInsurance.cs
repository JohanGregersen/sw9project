using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    public static class QualityInsurance {
        //Useful: dbc.GetEntriesByIds, fetches all information for a datarow by an id, universal for much of the quality-insurance

        
        ///<summary>
        ///<para>Removes invalid plots based on low number of satellites or high value of horizontal disposition</para>
        ///<para>Cuts the lowerBound of Sat and the upperbound of Hdop</para>
        ///</summary>
        public static List<int> RemoveHdopSatPoints(List<int> plots, double lowerBoundSatThreshold, double upperBoundHdopThreshold) {

            DBController dbc = new DBController();
            List<CarLogEntry> entries = dbc.GetEntriesByIds(plots);
            dbc.Close();

            List<CarLogEntry> cleanedEntries = new List<CarLogEntry>();

            foreach(CarLogEntry entry in entries) {
                if(entry.sat <= lowerBoundSatThreshold || entry.hdop >= upperBoundHdopThreshold) {
                    cleanedEntries.Add(entry);
                }
            }



            return new List<int>();
        }


    }
}

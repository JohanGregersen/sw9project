using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    class PerCarCalculator {

        public static Int64 GetTripsTaken(Int16 carid) {

            DBController dbc = new DBController();
            return dbc.GetAmountOfTrips(carid);

        }


    }
}

using System;
using System.Collections.Generic;

namespace CarDataProject {
    public class Policy {
        List<Global.Enums.RoadType> RoadTypes = new List<Global.Enums.RoadType>();

        public Policy(List<Global.Enums.RoadType> roadTypes) {
            RoadTypes = roadTypes;
        }
    }
}

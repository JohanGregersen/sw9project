using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class DefaultPolicy {
        public static List<Global.Enums.RoadType> roadTypes = new List<Global.Enums.RoadType> { Global.Enums.RoadType.motorway,
                                                                                      Global.Enums.RoadType.trunk,
                                                                                      Global.Enums.RoadType.primary,
                                                                                      Global.Enums.RoadType.secondary,
                                                                                      Global.Enums.RoadType.tertiary,
                                                                                      Global.Enums.RoadType.unclassified,
                                                                                      Global.Enums.RoadType.residential,
                                                                                      Global.Enums.RoadType.service };
    }
}

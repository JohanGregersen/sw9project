using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class SortingHelper {
        public static List<Fact> FactsByDateTime(List<Fact> facts) {
            facts.Sort((a, b) => a.Temporal.Timestamp.CompareTo(b.Temporal.Timestamp));
            return facts;
        }

        public static List<TemporalInformation> TemporalInformationByDateTime(List<TemporalInformation> temporals) {
            temporals.Sort((a, b) => a.Timestamp.CompareTo(b.Timestamp));
            return temporals;
        }
    }
}

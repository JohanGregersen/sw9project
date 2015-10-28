using System;
using System.Collections.Generic;

namespace CarDataProject {
    public static class SortingHelper {
        public static List<Fact> FactsByDateTime(List<Fact> facts) {
            facts.Sort((a, b) => a.Temporal.Timestamp.CompareTo(b.Temporal.Timestamp));
            return facts;
        }
    }
}

using System;
using System.Collections.Generic;

namespace CarDataProject {
    /*
    * Parses the database representation of intervals
    *
    * Interval format is: ixx1122334455667788
    * Where 11 - 88 are intervals representing 0 - 99%
    * and i is either 0 or 1, where 1 means the first interval with a non-zero value should be read as 100%
    * and xx is an insurance company policy ID
    */
    public static class IntervalHelper {
        public static Int64 Encode(List<double> intervals) {
            string encoding = "0000000000000000000";
            Int16 intervalTotal = 0;

            if (intervals.Count > 8) {
                throw new ArgumentException("Interval count should be 8 or less");
            } else {
                int encodingIndex = 3;

                foreach (double interval in intervals) {
                    Int16 value = (Int16) Math.Round(interval);
                    intervalTotal += value;
                    if (value > 101 || value < 0) {
                        throw new ArgumentOutOfRangeException("Intervals must be between 0% and 100%");
                    } else if (value >= 100) {
                        encoding = encoding.Remove(0, 1).Insert(0, "1");

                        //Insert 99 into encoded string since 100 can not be represented
                        encoding = encoding.Remove(encodingIndex, 2).Insert(encodingIndex, "99");

                        //With a value of 100% we are done - break the loop and return the result
                        break;
                    }

                    //Insert value into encoded string and ensure a leading zero if value is below 10
                    encoding = encoding.Remove(encodingIndex, 2).Insert(encodingIndex, value.ToString("D2"));
                    encodingIndex += 2;
                }
            }

            return Int64.Parse(encoding);
        }

        public static List<double> Decode(Int64 encodedIntervals) {
            string encoding = encodedIntervals.ToString("D19");
            Int16 indicator = Int16.Parse(encoding.Substring(0, 1));
            List<double> intervals = new List<double>();
            int encodingIndex = 3;

            //If indicator is 1, find the non-zero entry, and parse it as 100%. Parse all others as 0
            if (indicator == 1) {
                for (int i = 0; i < 8; i++) {
                    if (encoding.Substring(encodingIndex, 2).Equals("99")) {
                        intervals.Add(100);
                    } else if (encoding.Substring(encodingIndex, 2).Equals("00") || encoding.Substring(encodingIndex, 2).Equals("01")) {
                        intervals.Add(0);
                    } else {
                        throw new ArgumentException("Encoded intervals are not correctly formatted");
                    }

                    encodingIndex += 2;
                }

            //If indicator is neither 1 or 0, throw an error
            } else if (indicator != 0) {
                throw new ArgumentException("Indicator value must be 1 or 0");

            //Parse all entries normally otherwise
            } else {
                for (int i = 0; i < 8; i++) {
                    intervals.Add(Int16.Parse(encoding.Substring(encodingIndex, 2)));
                    encodingIndex += 2;
                }
            }

            return intervals;
        }
    }
}
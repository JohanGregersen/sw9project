using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;

namespace CarDataProject {
    static class INFATI {
        public static void AdjustAllLogs() {
            //Find all logs in the INFATI directory
            List<string> logs = new List<string>(Directory.GetFiles(Global.Batch.INFATI.Path));

            //Adjust all logs using the helper method
            foreach (string log in logs) {
                AdjustLogHelper(log);
            }
        }

        public static void AdjustLog(Int16 teamId, Int16 carId) {
            //Adjust log using the helper method
            AdjustLogHelper(Global.Batch.INFATI.Path + Global.Batch.INFATI.LogFile(teamId, carId));
        }

        public static List<INFATIEntry> ReadLog(Int16 teamId, Int16 carId) {
            //Open file from path
            StreamReader file = new StreamReader(Global.Batch.INFATI.Path + Global.Batch.INFATI.LogFile(teamId, carId));

            //Discard data header
            file.ReadLine();

            //Read remaining file, split every row into its columns
            string entry;
            List<List<string>> rows = new List<List<string>>();

            while ((entry = file.ReadLine()) != null) {
                List<string> elements = entry.Split(new char[] {'#'}, StringSplitOptions.RemoveEmptyEntries).ToList();

                //Add to list of entries
                rows.Add(elements);
            }

            file.Close();

            //Assemble the entries into facts
            List<INFATIEntry> entries = new List<INFATIEntry>();

            foreach (List<string> row in rows) {
                DateTime timestamp = DateTimeHelper.ConvertToDateTime(Int32.Parse(row[4]), Int32.Parse(row[5]));
                int UTMx = Int32.Parse(row[6]);
                int UTMy = Int32.Parse(row[7]);
                int UTMmx = Int32.Parse(row[8]);
                int UTMmy = Int32.Parse(row[9]);
                Int16 sat = Int16.Parse(row[10]);
                Int16 hdop = Int16.Parse(row[11]);
                Int16 maxspeed = Int16.Parse(row[12]);
                Int16 speed = Int16.Parse(row[13]);


                if (row.Count > 15) {
                    Int64 segmentId = Int64.Parse(row[15]);
                    entries.Add(new INFATIEntry(timestamp, UTMx, UTMy, UTMmx, UTMmy, sat, hdop, speed, maxspeed, segmentId));
                } else {
                    entries.Add(new INFATIEntry(timestamp, UTMx, UTMy, UTMmx, UTMmy, sat, hdop, speed, maxspeed));
                }

            }

            //Return all entries
            return entries;
        }

        private static void AdjustLogHelper(string logPath) {
            //Load all rows of file into memory
            List<string> rows = new List<string>(File.ReadAllLines(logPath));

            //Read the column names from the header
            List<string> columns = rows[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            //Save header entries in single string with seperators
            string header = columns[0];

            for (int i = 1; i < columns.Count; i++) {
                header += Global.Batch.INFATI.RowSeperator + columns[i];
            }

            List<string> adjustedRows = new List<string>();
            adjustedRows.Add(header);

            //For the remaining rows, save all entries with seperators, include null entries
            for (int i = 1; i < rows.Count; i++) {
                List<string> rowEntries = rows[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string row = rowEntries[0];

                for (int j = 1; j < rowEntries.Count; j++) {
                    row += Global.Batch.INFATI.RowSeperator + rowEntries[j];
                }

                for (int j = rowEntries.Count; j <= columns.Count; j++) {
                    row += Global.Batch.INFATI.RowSeperator;
                }

                adjustedRows.Add(row);
            }

            //Write all adjusted rows back to the file
            File.WriteAllLines(logPath, adjustedRows);
        }
    }
}

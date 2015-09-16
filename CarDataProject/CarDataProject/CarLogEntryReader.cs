using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    public static class CarLogEntryReader {

        public static List<CarLogEntry> ReadFile(string path) {
            List<CarLogEntry> entries = new List<CarLogEntry>();
            string line;

            //Open file from path
            System.IO.StreamReader file = new System.IO.StreamReader(path);

            //Discard first line because of header
            file.ReadLine();
            //Read one line at a time. For each line, find column values and create a CarLogEntry from the data
            while ((line = file.ReadLine()) != null) {
                List<string> lineColumnsRaw = line.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries).ToList();
                List<Int64> lineColumns = new List<Int64>();

                foreach (string column in lineColumnsRaw) {
                    lineColumns.Add(Convert.ToInt64(column));
                }
                //Add to list of entries
                entries.Add(new CarLogEntry(lineColumns));
            }

            file.Close();
            return entries;
        }
    }
}

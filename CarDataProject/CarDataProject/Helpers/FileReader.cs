using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarDataProject {
    class FileReader {

        public static List<CarLogEntry> CarLog(string filename) {
            string solutionPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string dataPath = @"\data\";
            string filetype = ".txt";

            List<CarLogEntry> entries = new List<CarLogEntry>();
            string entry;

            //Open file from path
            StreamReader file = new StreamReader(solutionPath + dataPath + filename + filetype);

            //Discard first line because of data header
            file.ReadLine();

            //Read one line at a time. For each line, find column values and create a CarLogEntry from the data
            while ((entry = file.ReadLine()) != null) {
                List<string> elements = entry.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                //Add to list of entries
                entries.Add(new CarLogEntry(elements));
            }

            file.Close();
            return entries;
        }

    }
}

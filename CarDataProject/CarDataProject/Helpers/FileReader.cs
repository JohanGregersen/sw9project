using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;

namespace CarDataProject {
    class FileReader {
        public static List<Fact> CarLog(string filename) {
            string solutionPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string dataPath = @"\data\";
            string filetype = ".txt";

            string entry;
            List<List<string>> rows = new List<List<string>>();

            //Open file from path
            StreamReader file = new StreamReader(solutionPath + dataPath + filename + filetype);

            //Discard first line because of data header
            file.ReadLine();

            //Read one line at a time. For each line, find column values and create a CarLogEntry from the data
            while ((entry = file.ReadLine()) != null) {
                List<string> elements = entry.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                //Add to list of entries
                rows.Add(elements);
            }

            file.Close();

            //Assemble the entries into facts
            List<Fact> facts = new List<Fact>();

            foreach(List<string> row in rows) {
                TimeInformation temporal = new TimeInformation(DateTimeHelper.ConvertToDateTime(Int32.Parse(row[4]), Int32.Parse(row[5])));
                PositionInformation spatial = new PositionInformation(new GeoCoordinate(Int32.Parse(row[6]), Int32.Parse(row[7])), new GeoCoordinate(Int32.Parse(row[6]), Int32.Parse(row[7])));
                QualityInformation quality = new QualityInformation(Int16.Parse(row[10]), Int16.Parse(row[11]));
                MeasureInformation measure = new MeasureInformation(double.Parse(row[13]));
                SegmentInformation segment = new SegmentInformation(Int64.Parse(row[15]));

                facts.Add(new Fact(quality, segment, temporal, spatial, measure));
            }

            return facts;
        }
    }
}

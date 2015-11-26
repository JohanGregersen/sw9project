using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CarDataProject {
    public class DimDate {
        public int DateId { get; set; }
        public Int16 Year { get; set; }
        public Int16 Month { get; set; }
        public Int16 Day { get; set; }
        public Int16 DayOfWeek { get; set; }
        public bool Weekend { get; set; }
        public bool Holiday { get; set; }
        public Int16 Quarter { get; set; }
        public Int16 Season { get; set; }

        public DimDate(int DateId, Int16 Year, Int16 Month, Int16 Day, Int16 DayOfWeek, bool Weekend, bool Holiday, Int16 Quarter, Int16 Season) {
            this.DateId = DateId;
            this.Year = Year;
            this.Month = Month;
            this.Day = Day;
            this.DayOfWeek = DayOfWeek;
            this.Weekend = Weekend;
            this.Holiday = Holiday;
            this.Quarter = Quarter;
            this.Season = Season;
        }

        public DimDate(DataRow row) {  
            this.DateId = row.Field<int>("dateid");
            this.Year = row.Field<Int16>("year");
            this.Month = row.Field<Int16>("month");
            this.Day = row.Field<Int16>("day");
            this.DayOfWeek = row.Field<Int16>("dayofweek");
            this.Weekend = row.Field<bool>("weekend");
            this.Holiday = row.Field<bool>("holiday");
            this.Quarter = row.Field<Int16>("quarter");
            this.Season = row.Field<Int16>("season");
        }
    }
}

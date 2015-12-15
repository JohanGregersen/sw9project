using System;
using System.Text;

namespace CarDataProject { 
    public class INFATITime {
        public int TimeId { get; set; }
        public Int16 Hour { get; set; }
        public Int16 Minute { get; set; }
        public Int16 Second { get; set; }

        public INFATITime(Int16 Hour, Int16 Minute) {
            this.Hour = Hour;
            this.Minute = Minute;
            StringBuilder sb = new StringBuilder();
            sb.Append(Hour.ToString("D2"));
            sb.Append(Minute.ToString("D2"));
            this.TimeId = Convert.ToInt32(sb.ToString());
        }

        public INFATITime(Int16 Hour, Int16 Minute, Int16 Second) {
            this.Hour = Hour;
            this.Minute = Minute;
            this.Second = Second;
            StringBuilder sb = new StringBuilder();
            sb.Append(Hour.ToString("D2"));
            sb.Append(Minute.ToString("D2"));
            sb.Append(Second.ToString("D2"));
            this.TimeId = Convert.ToInt32(sb.ToString());
            
        }

        public INFATITime(int TimeId, Int16 Hour, Int16 Minute, Int16 Second) {
            this.TimeId = TimeId;
            this.Hour = Hour;
            this.Minute = Minute;
            this.Second = Second;
        }
    }
}

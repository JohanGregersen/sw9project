using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CarDataProject {
    public class Worker {
        private Int16 teamId { get; set; }
        private Int16 carId { get; set; }

        public Worker(Int16 TeamId, Int16 CarId) {
            this.teamId = TeamId;
            this.carId = CarId;

            Console.WriteLine(this.teamId + ", " + this.carId);
        }

        public void Start() {

            Console.WriteLine("trying to work this shit out on thread " + this.carId);
            
            INFATILoader.LoadCarData(teamId, carId);

            GPSFactUpdater.Update(carId);

            TripFactUpdater.Update(carId);

            Console.WriteLine("worked out shit on " + this.carId + ". TERMINATED");

        }

        public override string ToString() {
            return "Team & CarId: " + this.teamId + " - " + this.carId;
        }
    }
}

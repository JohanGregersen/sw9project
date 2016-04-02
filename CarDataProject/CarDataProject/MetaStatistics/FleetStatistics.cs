using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    public class FleetStatistics {
        public List<Trip> AllTrips { get; set; }

        public FleetStatistics() {
            DBController dbc = new DBController();
            this.AllTrips = dbc.GetAllTrips();
            dbc.Close();

            int totalTrips = AllTrips.Count;
            RemoveBadTrips();
            Console.WriteLine("Number of bad trips: " + (totalTrips - AllTrips.Count));
            Console.WriteLine("Number of trips: " + AllTrips.Count);

            Console.WriteLine("Average Distance Driven: " + AverageDistanceDriven());
            Console.WriteLine("Average OptimalScore: " + AverageOptimalScore());
            Console.WriteLine("Average TripScore: " + AverageTripScore());
            Console.WriteLine("Average Percentage Above Optimal: " + (AveragePercentage() - 100));
            Trip worst = WorstPercentageTrip();
            Console.WriteLine("Worst Percentage: " + ((worst.TripScore / worst.OptimalScore * 100)-100) + " On Trip " + worst.TripId);

            
            
        }

        private void RemoveBadTrips() {
            //Kun trips over 50 meter.
            AllTrips = AllTrips.Where(trip => trip.MetersDriven >= 50).ToList();
            //Kun trips med en tripscore
            AllTrips = AllTrips.Where(trip => trip.TripScore > 0).ToList();
            //Kun trips med en optimalscore
            AllTrips = AllTrips.Where(trip => trip.OptimalScore > 0).ToList();

            AllTrips = AllTrips;

        }

        private double AverageDistanceDriven() {
            return AllTrips.Sum(x => x.MetersDriven) / AllTrips.Count;
        }

        private double AverageOptimalScore() {
            return AllTrips.Sum(x => x.OptimalScore) / AllTrips.Count;
        }

        private double AverageTripScore() {
            return AllTrips.Sum(x => x.TripScore) / AllTrips.Count;
        }
        
        private double AveragePercentage() {
            return AllTrips.Average(x => x.TripScore / x.OptimalScore * 100);
        }

        private Trip WorstPercentageTrip() {
            return AllTrips.OrderBy(x => x.TripScore / x.OptimalScore * 100).Last();
        }
    }
}

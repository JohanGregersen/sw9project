using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Runtime.Caching;
using System.IO;
using NpgsqlTypes;

namespace CarDataProject {
    class Program {
        static void Main(string[] args) {
            
            DBController dbc = new DBController();

            List<Trip> trips = dbc.GetAllTrips();
            
           for(int i = 0; i > trips.Count; i++) {
                GPSFactUpdater.UpdateRawGPS((Int16)trips[i].CarId, trips[i].TripId);
                List<Fact> facts = new List<Fact>();
                facts = dbc.GetFactsByTripId(trips[i].TripId);

                trips[i] = TripFactUpdater.UpdateTripWithCountsAndIntervals(trips[i], facts, dbc);
                dbc.UpdateTripFactWithCounts(trips[i]);
                dbc.UpdateTripFactWithIntervals(trips[i]);
            }

            /*
            List<Trip> trips = dbc.GetTripsByCarId(34);
            Console.WriteLine("AverageTripPercentage:");
            Console.WriteLine(UserProfile.AverageTripPercentage(trips).ToString());
            Console.WriteLine("AverageMetricPercentage:");
            UserProfile.print(UserProfile.AverageMetricPercentage(trips));
            Console.WriteLine("AverageMetricNormalized:");
            UserProfile.print(UserProfile.AverageMetricNormalized(trips));
            Console.WriteLine("AverageMetricDegree:");
            UserProfile.print(UserProfile.AverageMetricDegree(trips));
            
            Console.WriteLine("CorrelationMatrix:");
            Double[,] matrix = MetricCorrelation.getCorrelationMatrix(trips);
            MetricCorrelation.printMatrix(matrix);
            */
            Console.WriteLine("Aaaaand its done");
            Console.ReadLine();
        }

        /*
        * Reads all INFATI data and fills out all possible fields in the database
        */
        static void UpdateDatabaseThreaded() {
            List<Worker> workerPool = new List<Worker>();
            
            for (Int16 i = 1; i <= 1; i++) {
                workerPool.Add(new Worker(1, i));
            }
            
            /*
            for (Int16 i = 12; i <= 20; i++) {
                workerPool.Add(new Worker(2, i));
            }
            */
            List<Thread> threads = new List<Thread>();
            foreach (Worker worker in workerPool) {
                Thread thread = new Thread(new ThreadStart(worker.Start));
                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads) {
                thread.Join();
            }

            Console.WriteLine("All threads ended");
        }
    }
}

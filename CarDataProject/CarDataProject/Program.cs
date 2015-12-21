using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CarDataProject {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        static void UpdateIntervalsInDB() {
            List<Int64> tripIds;
            Trip trip;
            List<Fact> facts;

            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();
            for (int j = 0; j < carIds.Count; j++) {
                tripIds = dbc.GetTripIdsByCarId(carIds[j]);
                for(int i = 0; i < tripIds.Count; i++) {
                    Console.Clear();
                    Console.WriteLine("Encoding intervals on car {0}/{1}, trip {2}/{3}", j+1, carIds.Count+1, i+1, tripIds.Count+1);
                    trip = dbc.GetTripByCarIdAndTripId(carIds[j], tripIds[i]);
                    facts = dbc.GetFactsByCarIdAndTripId(carIds[j], tripIds[i]);

                    TripFactUpdater.UpdateTripWithIntervals(trip, facts);
                }
            }
        }

        static void LausThreadedDBStuff() {
            DBController dbc = new DBController();

            List<Worker> workerPool = new List<Worker>();
            
            for(Int16 i = 1; i <= 11; i++) {
                workerPool.Add(new Worker(1, i));
            }
            
            for (Int16 i = 12; i <= 20; i++) {
                workerPool.Add(new Worker(2, i));
            }

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

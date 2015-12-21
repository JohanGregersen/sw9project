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
            foreach (Int16 carId in carIds) {
                tripIds = dbc.GetTripIdsByCarId(carId);
                for(int i = 0; i < tripIds.Count; i++) {
                    Console.Clear();
                    Console.WriteLine("Encoding intervals on trip {0}/{1}", i, tripIds.Count);
                    trip = dbc.GetTripByCarIdAndTripId(carId, tripIds[i]);
                    facts = dbc.GetFactsByCarIdAndTripId(carId, tripIds[i]);

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

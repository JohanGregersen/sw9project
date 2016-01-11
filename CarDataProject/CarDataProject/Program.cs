using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace CarDataProject {
    class Program {
        static void Main(string[] args) {
        }

        /*
         * Prints all trip scores by all cars one by one
         */
        static void ReadAllTripScores() {
            DBController dbc = new DBController();
            List<Int16> carIds = dbc.GetCarIds();

            foreach (Int16 carId in carIds) {
                List<Int64> tripIds = dbc.GetTripIdsByCarId(carId);

                foreach (Int64 tripId in tripIds) {
                    FinalScore.CalculateReal(tripId);
                    Console.ReadLine();
                }
            }

            dbc.Close();
        }

        /*
        * Reads all INFATI data and fills out all possible fields in the database
        */
        static void UpdateDatabaseThreaded() {
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

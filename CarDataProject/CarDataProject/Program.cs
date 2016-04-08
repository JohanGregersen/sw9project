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

            //FleetStatistics FS = new FleetStatistics();

            //DBInitializer.DBInitialization();
            //UpdateDatabaseThreaded();

            //Mapmatch m = new Mapmatch();

            /*
            DBController dbc = new DBController();
            List<Fact> facts = dbc.GetFactsForMapByCarIdAndTripId(1, 22);
            dbc.Close();

            foreach (Fact f in facts) {
                Console.WriteLine();
                Console.WriteLine(f.EntryId + ", " + f.Spatial.MPoint.Latitude + ", " + f.Spatial.MPoint.Longitude + ",\"" + f.Temporal.Timestamp.ToString("yyyy-MM-dd") + "T" + f.Temporal.Timestamp.TimeOfDay + "\"");
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CarDataProject {
    class Program {
        static void Main(string[] args) {
            int breakpoint = 0;

            DBController dbc = new DBController();

            List<Worker> workerPool = new List<Worker>();


            /*
            for(Int16 i = 13; i <= 17; i++) {
                workerPool.Add(new Worker(2, i));
            }
            */

            for (Int16 i = 18; i <= 21; i++) {
                workerPool.Add(new Worker(2, i));
            }
            

            /*
            for (Int16 i = 8; i <= 8; i++) {
                workerPool.Add(new Worker(2, i));
            }
            for (Int16 i = 10; i <= 12; i++) {
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





            Console.WriteLine("done");
            Console.ReadLine();
        }


        /*static void InsertCarLogIntoDB() {
            List<Fact> facts = INFATI.ReadLog(1, 1);
            DBController dbc = new DBController();

            foreach (Fact fact in facts) {
                dbc.AddCarLogEntry(fact);
            }

            OldCar car = new OldCar(1);
            car.UpdateCarWithTripIdsOptimized(1);

            dbc.UpdateEntryWithPointAndMpoint(1);
            dbc.Close();
        }*/
    }
}

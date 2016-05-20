using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDataProject {
    class MetricCorrelation {
        public static Double Correlation(double[] FirstArray, double[] SecondArray) {
            
            Double sumX = 0;
            Double sumX2 = 0;
            Double sumY = 0;
            Double sumY2 = 0;
            Double sumXY = 0;

            int n = FirstArray.Length < SecondArray.Length ? FirstArray.Length : SecondArray.Length;

            for (int i = 0; i < n; ++i) {
                Double x = FirstArray[i];
                Double y = SecondArray[i];

                sumX += x;
                sumX2 += x * x;
                sumY += y;
                sumY2 += y * y;
                sumXY += x * y;
            }

            Double stdX = Math.Sqrt(sumX2 / n - sumX * sumX / n / n);
            Double stdY = Math.Sqrt(sumY2 / n - sumY * sumY / n / n);
            Double covariance = (sumXY / n - sumX * sumY / n / n);

            return covariance / stdX / stdY;
        }

        public static Double[,] getCorrelationMatrix(List<Trip> trips) {

            Double[,] CorrelationMatrix = new Double[6,6];
            
            List<Double[]> pearsonvalues = new List<double[]>();

            Double[] Roadtypes = new Double[trips.Count()];
            Double[] CriticalTime = new Double[trips.Count()];
            Double[] Speeding = new Double[trips.Count()];
            Double[] Accelerating = new Double[trips.Count()];
            Double[] Braking = new Double[trips.Count()];
            Double[] Jerking = new Double[trips.Count()];
           
            for(int i = 1; i < trips.Count; i++) {
                Roadtypes[i] = trips.ElementAt(i).RoadTypeScore;
                CriticalTime[i] = trips.ElementAt(i).CriticalTimeScore;
                Speeding[i] = trips.ElementAt(i).SpeedingScore;
                Accelerating[i] = trips.ElementAt(i).AccelerationScore;
                Braking[i] = trips.ElementAt(i).BrakeScore;
                Jerking[i] = trips.ElementAt(i).JerkScore;
            }

            pearsonvalues.Add(Roadtypes);
            pearsonvalues.Add(CriticalTime);
            pearsonvalues.Add(Speeding);
            pearsonvalues.Add(Accelerating);
            pearsonvalues.Add(Braking);
            pearsonvalues.Add(Jerking);

            for(int i = 0; i < pearsonvalues.Count; i++) {
                for(int j = 0; j < pearsonvalues.Count; j++) {
                    CorrelationMatrix[i,j] = Correlation(pearsonvalues[i], pearsonvalues[j]);
                }
            }
            
            return CorrelationMatrix;
        }

        public static void printMatrix(Double[,] matrix) {

            int rowLength = matrix.GetLength(0);
            int colLength = matrix.GetLength(1);

            for (int i = 0; i < rowLength; i++) {
                for (int j = 0; j < colLength; j++) {
                    Console.Write(string.Format("{0} ", matrix[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.ReadLine();
        }
    }
}

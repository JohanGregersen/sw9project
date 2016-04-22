using System;
using System.Data;

namespace CarDataProject {
    public class Car {
        public int CarId { get; }
        public string CarType { get; }
        public string Brand { get; }
        public string Model { get; }
        public double FuelConsumption { get; }
        public double EnergyConsumption { get; }
        public double Weight { get; }
        public Int16 Capacity { get; }
        public Int64 IMI { get; set; }
        public string Username { get; set; }

        public Car(int CarId, string CarType, string Brand, string Model, double FuelConsumption, double EnergyConsumption, double Weight, Int16 Capacity, Int64 IMI, string Username) {
            this.CarId = CarId;
            this.CarType = CarType;
            this.Brand = Brand;
            this.Model = Model;
            this.FuelConsumption = FuelConsumption;
            this.EnergyConsumption = EnergyConsumption;
            this.Weight = Weight;
            this.Capacity = Capacity;
            this.IMI = IMI;
            this.Username = Username;
        }

        public Car(DataRow row) {
            this.CarId = row.Field<int>("carid");
            this.CarType = row.Field<string>("cartype");
            this.Brand = row.Field<string>("brand");
            this.Model = row.Field<string>("model");
            this.FuelConsumption = row.Field<double>("fuelconsumption");
            this.EnergyConsumption = row.Field<double>("energyconsumption");
            this.Weight = row.Field<double>("weight");
            this.Capacity = row.Field<Int16>("capacity");
            this.IMI = row.Field<Int64>("imi");
            this.Username = row.Field<string>("username");
        }
    }
}

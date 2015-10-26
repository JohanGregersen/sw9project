using System;

namespace CarDataProject {
    class Car {
        public int CarId { get; }
        public string CarType { get; }
        public string Brand { get; }
        public string Model { get; }
        public double FuelConsumption { get; }
        public double EnergyConsumption { get; }
        public double Weight { get; }
        public Int16 Capacity { get; }

        public Car(int CarId, string CarType, string Brand, string Model, double FuelConsumption, double EnergyConsumption, double Weight, Int16 Capacity) {
            this.CarId = CarId;
            this.CarType = CarType;
            this.Brand = Brand;
            this.Model = Model;
            this.FuelConsumption = FuelConsumption;
            this.EnergyConsumption = EnergyConsumption;
            this.Weight = Weight;
            this.Capacity = Capacity;
        }
    }
}

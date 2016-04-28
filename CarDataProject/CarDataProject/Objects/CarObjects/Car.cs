using System;
using System.Data;
using System.Runtime.Serialization;

namespace CarDataProject {
    [DataContract]
    public class Car {
        [DataMember(Name = "carid")]
        public Int16 CarId { get; }
        public string CarType { get; }
        public string Brand { get; }
        public string Model { get; }
        public double FuelConsumption { get; }
        public double EnergyConsumption { get; }
        public double Weight { get; }
        public Int16 Capacity { get; }
        [DataMember(Name = "imei")]
        public Int64 IMEI { get; set; }
        [DataMember(Name = "username")]
        public string Username { get; set; }

        public Car(Int16 CarId, Int64 IMEI) {
            this.CarId = CarId;
            this.IMEI = IMEI;
        }

        public Car(Int16 CarId, string CarType, string Brand, string Model, double FuelConsumption, double EnergyConsumption, double Weight, Int16 Capacity, Int64 IMEI, string Username) {
            this.CarId = CarId;
            this.CarType = CarType;
            this.Brand = Brand;
            this.Model = Model;
            this.FuelConsumption = FuelConsumption;
            this.EnergyConsumption = EnergyConsumption;
            this.Weight = Weight;
            this.Capacity = Capacity;
            this.IMEI = IMEI;
            this.Username = Username;
        }

        public Car(DataRow row) {
            this.CarId = row.Field<Int16>("carid");
            if (row.Table.Columns.Contains("cartype")) {
                this.CarType = row.Field<string>("cartype");
                this.Brand = row.Field<string>("brand");
                this.Model = row.Field<string>("model");
                this.FuelConsumption = row.Field<double>("fuelconsumption");
                this.EnergyConsumption = row.Field<double>("energyconsumption");
                this.Weight = row.Field<double>("weight");
                this.Capacity = row.Field<Int16>("capacity");
            }
            row["imei"] = row["imei"] is DBNull ? 0 : row["imei"];
            this.IMEI = row.Field<Int64>("imei");
            row["username"] = row["username"] is DBNull ? "" : row["username"];
            this.Username = row.Field<string>("username");
        }
    }
}

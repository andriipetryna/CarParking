using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSA_Task
{
    public static class Settings
    {
        private const int DEFAULT_TIMEOUT_SECONDS = 3;
        private const int DEFAULT_PARKING_SPACE = 15;
        private const decimal DEFAULT_FINE = 1.5m;
        private const decimal DEFAULT_PRICE_TRUCK = 5;
        private const decimal DEFAULT_PRICE_PASSENGER = 3;
        private const decimal DEFAULT_PRICE_BUS = 2;
        private const decimal DEFAULT_PRICE_MOTORCYCLE = 1;
        private const string DEFAULT_FILE_NAME = "Transactions.log";

        public static int Timeout { get; }
        public static Dictionary<CarType, decimal> Prices { get; }
        public static int ParkingSpace { get; }
        public static decimal Fine { get; }
        public static string FileName { get; }

        static Settings()
        { 
            Timeout = DEFAULT_TIMEOUT_SECONDS;
            Prices = new Dictionary<CarType, decimal>()
            {
                { CarType.Truck, DEFAULT_PRICE_TRUCK },
                { CarType.Passenger, DEFAULT_PRICE_PASSENGER},
                { CarType.Bus, DEFAULT_PRICE_BUS },
                { CarType.Motorcyle, DEFAULT_PRICE_MOTORCYCLE }
            };
            ParkingSpace = DEFAULT_PARKING_SPACE;
            Fine = DEFAULT_FINE;
            FileName = DEFAULT_FILE_NAME;
        }

        public static decimal GetPrice(CarType type)
        {
            if (Prices.TryGetValue(type, out decimal price))
            {
                return price;
            }

            throw new ArgumentException($"Price for CarType.{type} is not setted");
        }
    }
}

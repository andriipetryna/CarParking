using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BSA_Task
{
    public enum CarType
    {
        Passenger = 1,
        Truck,
        Bus,
        Motorcyle
    }
    public class Car
    {
        private static int idCounter = 0;

        public int Id { get; private set; }
        public decimal Balance { get; private set; }
        public CarType Type { get; private set; }

        public Car(CarType type) : this(type, 0)
        {}
        public Car(CarType type, decimal balance)
        {
            Type = type;
            Balance = balance;
            Id = Interlocked.Increment(ref idCounter);
        }

        public void WithdrawBalance(decimal amount)
        {
            if(amount < 0)
            {
                throw new ArgumentException("Negative withdraw amount");
            }

            Balance -= amount;
        }
        public void RechargeBalance(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Negative recharge amount");
            }

            Balance += amount;
        }
        public override string ToString()
        {
            return $"ID: {Id}; Тип: {Type}; Баланс: {Balance};";
        }
    }
}

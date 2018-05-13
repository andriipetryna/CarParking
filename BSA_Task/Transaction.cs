using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSA_Task
{
    public class Transaction
    {
        public DateTime Time { get; }
        public int CarId { get; }
        public decimal Payment { get; }
        public Transaction(DateTime time, int carId, decimal payment)
        {
            Time = time;
            CarId = carId;
            Payment = payment; 
        }
        public override string ToString()
        {
            return $"Время: {Time}; ID автомобиля: {CarId}; Списанные средства: {Payment};";
        }
    }
}

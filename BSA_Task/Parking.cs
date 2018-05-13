using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BSA_Task
{
    public class Parking : IDisposable
    {
        private const double LOG_TIMER_TIMEOUT_SECONDS = 60;

        private IParkingLogger _logger;

        private static Lazy<Parking> Lazy { get; } = new Lazy<Parking>(() => new Parking());
        private Timer PaymentTimer { get; }
        private Timer LogTimer { get; }
        private List<Car> Cars { get; set; }
        private List<Transaction> Transactions { get; set; }

        public IParkingLogger Logger
        {
            private get { return _logger; }
            set { _logger = value ?? throw new NullReferenceException(); }
        }
        public static Parking Instance { get { return Lazy.Value; } }
        public decimal Balance { get; private set; }
        public int ParkingSpace { get; }
        public int UnavailableParkingSpace
        {
            get { return Cars.Count; }
        }
        public int AvailableParkingSpace
        {
            get { return ParkingSpace - UnavailableParkingSpace; }
        }
        
        private Parking()
        {
            Cars = new List<Car>();
            Transactions = new List<Transaction>();

            Logger = new ParkingLogger(Settings.FileName);
            TimeSpan loggerPeriod = TimeSpan.FromSeconds(LOG_TIMER_TIMEOUT_SECONDS);
            LogTimer = new Timer(Log, null, loggerPeriod, loggerPeriod);

            TimeSpan paymentPeriod = TimeSpan.FromSeconds(Settings.Timeout);
            PaymentTimer = new Timer(Pay, null, paymentPeriod, paymentPeriod);

            ParkingSpace = Settings.ParkingSpace;
        }
        public void AddCar(Car car)
        {
            if(IsFull())
            {
                throw new InvalidOperationException("Can't add car if parking is full");
            }

            Cars.Add(car ?? throw new NullReferenceException());
        }
        public bool RemoveCar(int carId)
        {
            Car car = GetCarById(carId) ?? throw new NullReferenceException();

            if (car.Balance >= 0)
            {
                Cars.Remove(car);
                return true;
            }

            return false;
        }
        public void RechargeCarBalance(int carId, decimal rechargeSum)
        {
            Car car = GetCarById(carId) ?? throw new NullReferenceException();
            
            if(car.Balance < 0)
            {
                // debit all fines from car balance to parking balance
                decimal finesSum = -car.Balance;
                Transactions.Add(new Transaction(DateTime.Now, carId, finesSum));
                Balance += finesSum;
            }

            car.RechargeBalance(rechargeSum);
        }
        public decimal GetLastMinuteEarnedMoney()
        {
            return Transactions.Sum(t => t.Payment);
        }
        public bool ContainsCar(int id)
        {
            return Cars.Where(c => c.Id == id).Count() != 0;
        }
        public decimal GetCarBalance(int carId)
        {
            Car car = GetCarById(carId) ?? throw new NullReferenceException();

            return car.Balance;
        }
        public decimal GetCarFineBalance(int carId)
        {
            decimal balance = GetCarBalance(carId);

            return (balance < 0 ? -balance : 0);
        }
        public bool IsFull()
        {
            return AvailableParkingSpace == 0;
        }
        private void Log(object obj)
        {
            Logger.LogTransactionsSumToFile(GetLastMinuteEarnedMoney());
            Transactions.Clear();
        }
        private void Pay(object obj)
        {
            foreach (Car car in Cars)
            {
                decimal price = Settings.GetPrice(car.Type);
                
                if (price <= car.Balance)
                {
                    car.WithdrawBalance(price);
                    Balance += price;

                    Transactions.Add(new Transaction(DateTime.Now, car.Id, price));
                }
                else
                {
                    car.WithdrawBalance(price * Settings.Fine);
                }
            }
        }
        public IEnumerable<Transaction> GetLastMinuteTransactions()
        {
            return Transactions;
        }
        public IEnumerable<string> GetTransactionsLog()
        {
            return Logger.ReadLogFromFile().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }
        private Car GetCarById(int carId)
        {
            return Cars.Where(c => c.Id == carId).FirstOrDefault();
        }
        public void Dispose()
        {
            LogTimer.Dispose();
            PaymentTimer.Dispose();
        }
    }
}

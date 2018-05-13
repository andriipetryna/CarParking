using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSA_Task
{
    public class Menu
    {
        private Parking CarParking { get; }
        public Menu(Parking parking)
        {
            CarParking = parking;
        }
        public bool ShowMenu()
        {
            PrintMainMenu();
            bool isMenuActive = MenuAction();

            return isMenuActive;
        }
        private void PrintMainMenu()
        {
            SetGrayLineColor();

            PrintFullLine("[1] - Поставить машину на парковку"); // +
            PrintFullLine("[2] - Забрать машину с парковки"); // +
            PrintFullLine("[3] - Пополнить баланс машины"); // +
            PrintFullLine("[4] - Вывести количество свободных мест на парковке"); // +
            PrintFullLine("[5] - Вывести количество занятых мест на парковке"); // +
            PrintFullLine("[6] - Вывести общий доход парковки"); // +
            PrintFullLine("[7] - Вывести истории транзакций за последнюю минуту"); // +
            PrintFullLine("[8] - Вывести Transactions.log"); // +
            PrintFullLine("[С] - Очистить консоль"); // +
            PrintFullLine("[X] - Выход"); // +

            Console.ResetColor();
        }
        private bool MenuAction()
        {
            Console.Write("Выбран пункт меню: ");
            var pressedKey = Console.ReadKey();
            Console.WriteLine();

            bool isMenuActive = true;

            switch (pressedKey.Key)
            {
                case ConsoleKey.X:
                    isMenuActive = false;
                    break;
                case ConsoleKey.C:
                    Console.Clear();
                    break;
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    AddCarToParking();
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    RemoveCarFromParking();
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    RechargeCarBalance();
                    break;
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    PrintAvailableParkingSpace();
                    break;
                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    PrintUnavailableParkingSpace();
                    break;
                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                    PrintParkingBalance();
                    break;
                case ConsoleKey.D7:
                case ConsoleKey.NumPad7:
                    PrintTransactinsHistory();
                    break;
                case ConsoleKey.D8:
                case ConsoleKey.NumPad8:
                    PrintTransactionsLog();
                    break;
                default:
                    PrintRedLine("Вы выбрали недействительный пункт меню!");
                    break;
            }

            return isMenuActive;
        }
        private void AddCarToParking()
        {
            if (CarParking.IsFull())
            {
                PrintRedLine($"К сожалению все парковочные места уже заняты! Приезжайте попозже");
                return;
            }

            PrintAllCarTypes();
            Console.Write("Введите код типа транспортного средства и нажмите [ENTER]: ");

            bool isCarTypeNumberValid = Int32.TryParse(Console.ReadLine(), out int carTypeNumber);
            bool isCarTypeNumberExists = IsCarTypeNumberExists(carTypeNumber);

            if (isCarTypeNumberValid && isCarTypeNumberExists)
            {
                Console.Write("Введите сумму пополнения и нажмите [ENTER]: ");
                bool isRechargeSumValid = Int32.TryParse(Console.ReadLine(), out int rechargeSum);

                if (isRechargeSumValid && rechargeSum > 0)
                {
                    Car car = new Car((CarType)carTypeNumber, rechargeSum);
                    CarParking.AddCar(car);

                    PrintGreenLine($"Ваш автомобиль: {car} успешно поставлен на парковку");
                }
                else
                {
                    PrintRedLine($"Введена недействительная сумма пополнения!");
                }
            }
            else
            {
                PrintRedLine($"Введен недействительный тип транспортого средства!");
            }
        }
        private void PrintAllCarTypes()
        {
            foreach (var type in Enum.GetValues(typeof(CarType)).Cast<CarType>().OrderBy(t => t))
            {
                Console.WriteLine($"{(int)type} - {type}");
            }
        }
        private bool IsCarTypeNumberExists(int carTypeNumber)
        {
            return Enum.GetValues(typeof(CarType)).Cast<int>().Contains(carTypeNumber);
        }
        private void RemoveCarFromParking()
        {
            Console.Write("Введите ID транспортного средства и нажмите [ENTER]: ");
            bool isCarIdValid = Int32.TryParse(Console.ReadLine(), out int carId);

            if (isCarIdValid && CarParking.ContainsCar(carId))
            {
                if (CarParking.RemoveCar(carId))
                {
                    PrintGreenLine("Машина успешно забрана с парковки");
                }
                else
                {
                    PrintRedLine("Невозможно забрать машину с парковки: сначала нужно оплатить штрафы!");
                    PrintRedLine($"Сумма штрафов: {CarParking.GetCarFineBalance(carId)}");
                }
            }
            else
            {
                PrintRedLine("Транспортного средства с таким идентификатором нет на парковке!");
            }
        }
        private void RechargeCarBalance()
        {
            Console.Write("Введите ID транспортного средства и нажмите [ENTER]: ");

            bool isCarIdValid = Int32.TryParse(Console.ReadLine(), out int carId);

            if (isCarIdValid && CarParking.ContainsCar(carId))
            {
                Console.Write("Введите сумму пополнения и нажмите [ENTER]: ");
                bool isRechargeSumValid = Int32.TryParse(Console.ReadLine(), out int rechargeSum);

                if (isRechargeSumValid && rechargeSum > 0)
                {
                    CarParking.RechargeCarBalance(carId, rechargeSum);
                    PrintGreenLine($"Счет автомобиля ID {carId} пополнен на {rechargeSum}, на счету {CarParking.GetCarBalance(carId)}");
                }
                else
                {
                    PrintRedLine("Введена недействительная сумма пополнения!");
                }
            }
            else
            {
                PrintRedLine("Транспортного средства с таким идентификатором нет на парковке!");
            }
        }
        private void PrintAvailableParkingSpace()
        {
            PrintGreenLine($"Количество свободных мест на парковке: {CarParking.AvailableParkingSpace} из {CarParking.ParkingSpace}");
        }
        private void PrintUnavailableParkingSpace()
        {
            PrintGreenLine($"Количество занятых мест на парковке: {CarParking.UnavailableParkingSpace} из {CarParking.ParkingSpace}");
        }
        private void PrintParkingBalance()
        {
            PrintGreenLine($"Общий доход парковки: {CarParking.Balance}");
        }
        private void PrintTransactinsHistory()
        {
            var transactions = CarParking.GetLastMinuteTransactions();

            if (transactions.Count() != 0)
            {
                PrintGreenLine("Список транзакций за последнюю минуту:");
                Console.WriteLine(String.Join(Environment.NewLine, transactions));
            }
            else
            {
                PrintGreenLine("Транзакции за последнюю минуту отсутствуют!");
            }
        }
        private void PrintTransactionsLog()
        {
            var log = CarParking.GetTransactionsLog();

            if (log.Count() != 0)
            {
                PrintGreenLine("Transactions.log");
                Console.WriteLine("Дата/время \t\t Сумма транзакций");
                Console.WriteLine(String.Join(Environment.NewLine, log));
            }
            else
            {
                PrintGreenLine("Файл Transactions.log пуст!");
            }
        }
        private void PrintFullLine(string str)
        {
            Console.WriteLine(str.PadRight(Console.WindowWidth - 1));
        }
        private void SetGrayLineColor()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Gray;
        }

        private void SetRedLineColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Red;
        }

        private void SetGreenLineColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGreen;
        }

        private void PrintGrayLine(string str)
        {
            SetGrayLineColor();
            PrintFullLine(str);
            Console.ResetColor();
        }
        private void PrintGreenLine(string str)
        {
            SetGreenLineColor();
            PrintFullLine(str);
            Console.ResetColor();
        }
        private void PrintRedLine(string str)
        {
            SetRedLineColor();
            PrintFullLine(str);
            Console.ResetColor();
        }
    }
}

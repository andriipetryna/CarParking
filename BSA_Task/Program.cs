using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace BSA_Task
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Car Parking";

            Menu menu = new Menu(Parking.Instance);

            bool isMenuActive = true;
            do
            {
                isMenuActive = menu.ShowMenu();
            } while (isMenuActive);

            Parking.Instance.Dispose();
        }
    }
}

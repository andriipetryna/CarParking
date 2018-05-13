using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSA_Task
{
    public interface IParkingLogger
    {
        void LogTransactionsSumToFile(decimal sum);
        string ReadLogFromFile();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace BSA_Task
{
    public class ParkingLogger : IParkingLogger
    {
        private const string DEFAULT_FILE_NAME = "Logger.log";
        public string FileName { get; set; }

        public ParkingLogger() : this(DEFAULT_FILE_NAME)
        { }
        public ParkingLogger(string fileName)
        {
            FileName = fileName;
        }
        public void LogTransactionsSumToFile(decimal sum)
        {
            try
            {
                using (StreamWriter logFile = new StreamWriter(FileName, true))
                {
                    logFile.WriteLine($"{DateTime.Now} \t {sum}");
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            catch (FileLoadException ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
        public string ReadLogFromFile()
        {
            try
            {
                using (StreamReader logFile = new StreamReader(FileName, true))
                {
                    return logFile.ReadToEnd();
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            catch (FileLoadException ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            catch(UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}

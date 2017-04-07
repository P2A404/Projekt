using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LoadDataTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string[,,] Data;
            LoadingData Load = new LoadingData();
            int NumberOfFiles = Load.GetFileNames().Length;
            Data = Load.LoadCsv(@"C:\Users\Rasmus\Desktop\Python\Backup\Data\CSV Files Champion Data\EU 2016 Spring Champion Statistics.csv");
            Load.PrintArray(Data);
            //Load.GetFileNames();
            Console.ReadKey();
        }
    }
}
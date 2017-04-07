using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LoadDataTest
{
    public class LoadData
    {
        public void LoadFromPath()
        {
            int NumberOfRows = 0;
            int NumberOfCollums = 0;
            string line;
            string path = @"C:\Users\Rasmus\Desktop\Python\Backup\Data\CSV Files Champion Data\EU 2016 Spring Champion Statistics.csv";

            // Counting the number of collums and rows
            using (StreamReader Reader = new StreamReader(path))
            {
                while ((line = Reader.ReadLine()) != null)
                {
                    NumberOfRows++;
                    NumberOfCollums = line.Length - line.Replace(";", "").Length + 1;
                }               
            }
            string[,] DataArray = new string[NumberOfCollums, NumberOfRows];
        }
    }
}

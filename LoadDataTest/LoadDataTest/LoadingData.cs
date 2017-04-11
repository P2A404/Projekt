using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LoadDataTest
{
    class LoadingData
    {
        public string [][,] LoadPath(string path)
        {
            int NOFilesInPath = GetFileNames(path).Length;
            string[][,] FullDataArray = new string[NOFilesInPath][,];
            string[,] Data;

            // Loading in all files in  given path
            for (int i = 0; i < NOFilesInPath; i++)
            {
                // Loading one file
                Data = LoadCsv(path + @"\" + GetFileNames(path)[i]);

                // Calculating the number of rows and collums
                int NumberOfRows = Data.GetLength(0);
                int NumberOfCollums = Data.GetLength(1);

                // New instance of 2d array
                FullDataArray[i] = new string[NumberOfRows, NumberOfCollums];

                //Loading data into Full Data Array
                for (int x = 0; x < NumberOfRows; x++)
                {
                    for (int y = 0; y < NumberOfCollums; y++)
                    {
                        FullDataArray[i][x, y] = Data[x, y];
                    }
                }
            }
            return FullDataArray;

        }

        public string[] GetFileNames(string path)
        {
            string[] files = Directory.GetFiles(path, "*.csv");

            for (int index = 0; index < files.Length; index++)
            {
                files[index] = Path.GetFileName(files[index]);
            }

            return files;
        }

        public string[,] LoadCsv(string path)
        {

            // Get the file's text.
            string CurrentFile = File.ReadAllText(path);

            // Split into lines.
            CurrentFile = CurrentFile.Replace('\n', '\r');
            string[] lines = CurrentFile.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            // See how many rows and columns there are.
            int num_rows = lines.Length;
            int num_cols = lines[0].Split(';').Length;


            // Allocate the data array.
            string[,] values = new string[num_rows, num_cols];

            // Load the array.
            for (int r = 0; r < num_rows; r++)
            {
                string[] line_r = lines[r].Split(';');
                for (int c = 0; c < num_cols; c++)
                {
                    values[r, c] = line_r[c];
                }
            }

            // Return the values.
            return values;
        }

        public void PrintArray(string[,] values)
        {
            foreach (var item in values)
            {
                Console.WriteLine(item);
            }
        }     
    }
}

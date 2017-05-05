using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Reflection;

namespace LoadDataTest
{
    class LoadingData
    {
        public string[][,] LoadPath(string path)
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

        public string[,] LoadCsvFromResources(string file)
        {

            // Get the file's text.
            string CurrentFile = file;

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

        // Still not done
        public void ReadJson(string path)
        {
            

            
        }

        public string GetLocalDirectory()
        {
            string LocalDirectory = Assembly.GetExecutingAssembly().Location;
            LocalDirectory = LocalDirectory.Remove(Regex.Match(LocalDirectory, "LoadDataTest").Index);
            return LocalDirectory;
        }

        public double[][,] ConvertToIntArray(string[][,] array)
        {
            int NumberOfFiles = array.GetLength(0);
            double[][,] ConvertedArray = new double[NumberOfFiles][,];

            for (int CurrentFile = 0; CurrentFile < NumberOfFiles; CurrentFile++)
            {
                int NumberOfRows = array[CurrentFile].GetLength(0);
                int NumberOfCollums = array[CurrentFile].GetLength(1);
                ConvertedArray[CurrentFile] = new double[NumberOfRows, NumberOfCollums];

                for (int First2D = 1; First2D < NumberOfRows; First2D++)
                {
                    for (int Second2D = 1; Second2D < NumberOfCollums; Second2D++)
                    {

                        double TempVariable;
                        var currentCulture = System.Globalization.CultureInfo.InstalledUICulture;
                        var numberFormat = (System.Globalization.NumberFormatInfo)currentCulture.NumberFormat.Clone();
                        numberFormat.NumberDecimalSeparator = ",";

                        bool isNumeric = double.TryParse(array[CurrentFile][First2D, Second2D], System.Globalization.NumberStyles.Any, numberFormat, out TempVariable);
                        bool Contains2Dots = array[CurrentFile][First2D, Second2D].IndexOf('.') != array[CurrentFile][First2D, Second2D].LastIndexOf('.');

                        if (isNumeric)
                        {
                            ConvertedArray[CurrentFile][First2D, Second2D] = TempVariable;
                        }
                        else if (Contains2Dots)
                        {
                            string tempString = array[CurrentFile][First2D, Second2D].Remove(array[CurrentFile][First2D, Second2D].Length - 3);
                            array[CurrentFile][First2D, Second2D] = tempString;

                            double.TryParse(tempString, out TempVariable);
                            ConvertedArray[CurrentFile][First2D, Second2D] = TempVariable;
                        }
                        else
                        {
                            double ConvertedNumber;
                            array[CurrentFile][First2D, Second2D] = array[CurrentFile][First2D, Second2D].Trim('k', '%');
                            isNumeric = double.TryParse(array[CurrentFile][First2D, Second2D], out ConvertedNumber);
                            if (isNumeric)
                            {
                                ConvertedArray[CurrentFile][First2D, Second2D] = ConvertedNumber;
                            }
                        }
                    }
                }
            }
            return ConvertedArray;
        }
    }
}

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

        public string[] GetFileNames()
        {            
            string[] files = Directory.GetFiles(@"C:\Users\Rasmus\Desktop\Python\Backup\Data\CSV Files Champion Data\", "*.csv");

            for (int index = 0; index < files.Length; index++)
            {
                files[index] = Path.GetFileName(files[index]);
            }
            
            foreach (var item in files)
            {
                Console.WriteLine(item);
            }
            
            return files;    
        }

        

        public string[,,] LoadCsv(string path)
        {

            // Get the file's text.
            string CurrentFile = File.ReadAllText(path);

            // Split into lines.
            CurrentFile = CurrentFile.Replace('\n', '\r');
            string[] lines = CurrentFile.Split(new char[] { '\r' },
                StringSplitOptions.RemoveEmptyEntries);

            // See how many rows and columns there are.
            int num_rows = lines.Length;
            int num_cols = lines[0].Split(',').Length;
            int num_files = GetFileNames().Length;

            // Allocate the data array.
            string[,,] values = new string[num_rows, num_cols, num_files];

            // Load the array.
            for (int r = 0; r < num_rows; r++)
            {
                string[] line_r = lines[r].Split(',');
                for (int c = 0; c < num_cols; c++)
                {
                    values[r, c, 0] = line_r[c];
                }
            }

            // Return the values.
            return values;
        }

        public void PrintArray(string[,,] values)
        {
            foreach (var item in values)
            {
                Console.WriteLine(item);
            }
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JsonReader
{
    class Program
    {
        static public string GetLocalDirectory()
        {
            string LocalDirectory = Assembly.GetExecutingAssembly().Location;
            LocalDirectory = LocalDirectory.Remove(Regex.Match(LocalDirectory, "JsonReader").Index);
            return LocalDirectory;
        }

        static void Main(string[] args)
        {
            NNInputFormatter formatter = new NNInputFormatter();

            string LocalPath = GetLocalDirectory() + @"Data\Matches\";
            for (int index = 1; index < 2827; index++)
            {
                using (StreamReader Reader = new StreamReader(LocalPath + $@"{index}.json"))
                {
                    // Current file read to the end
                    string json = Reader.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<GameInfo.Match>(json);
                    // Adding to final list of matches
                    formatter.matches.Add(result);
                    Console.WriteLine(index + " done");
                }
            }
            formatter.ConvertGames();
            Console.Read();
        }
    }
}
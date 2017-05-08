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
            GameInfo GameReader = new GameInfo();
            List<JObject> matches = new List<JObject>();

            string LocalPath = GetLocalDirectory() + @"Data\Matches\";

            for (int index = 1; index < 2827; index++)
            {
                using (StreamReader r = new StreamReader(LocalPath + $@"{index}.json"))
                {
                    string json = r.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    matches.Add(array);
                    Console.WriteLine(index + " done");
                }
            }
            Console.Read();
        }
    }
}
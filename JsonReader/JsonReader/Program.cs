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
            List<JObject> Matches = new List<JObject>();

            string LocalPath = GetLocalDirectory() + @"Data\Matches\";

            for (int index = 1; index < 2827; index++)
            {
                using (StreamReader Reader = new StreamReader(LocalPath + $@"{index}.json"))
                {
                    // Current file read to the end
                    string json = Reader.ReadToEnd();

                    //Dynamic = Json Object and loading things into object
                    dynamic array = JsonConvert.DeserializeObject(json);
                    var result = JsonConvert.DeserializeObject<GameInfo>(json);
                    Console.WriteLine(result);

                    // Adding to final list of matches
                    Matches.Add(array);
                    Console.WriteLine(index + " done");
                }
            }
            JObject test = Matches[5];
            
            Console.Read();
        }
    }
}
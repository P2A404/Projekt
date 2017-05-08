using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JsonReader
{
    class Program
    {
        static void Main(string[] args)
        {
            GameInfo GameReader = new GameInfo();
            List<JObject> matches = new List<JObject>();

            for (int i = 0; i < 2827; i++)
            {
                using (StreamReader r = new StreamReader($@"C:\Users\Rasmus\Desktop\{i}.json"))
                {
                    string json = r.ReadToEnd();
                    dynamic array = JsonConvert.DeserializeObject(json);
                    matches.Add(array);
                    Console.WriteLine("{0} {1} {2}", array[0].gameId, array.mapId, array.GetType());

                    Console.Read();
                }
            }

        }




    }
}
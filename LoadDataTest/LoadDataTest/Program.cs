using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace LoadDataTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Time taking for the loading process
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            // New instance of Loading data
            LoadingData Load = new LoadingData();

            string LocalPath = Load.GetLocalDirectory();

            // Data Paths
            string ChampionDataPath = LocalPath + @"Data\CSV Files Champion Data";
            string EsportsDataPath = LocalPath + @"Data\CSV Files Lol Esport Data";
            string MatchupsDataPath = LocalPath + @"\Data\CSV Files Matchups";

            // Number of files in datapaths
            int NOFilesInChampion = Load.GetFileNames(ChampionDataPath).Length;
            int NOFilesInEsports = Load.GetFileNames(EsportsDataPath).Length;
            int NOFilesInMatchups = Load.GetFileNames(MatchupsDataPath).Length;
            int AllMatches = NOFilesInChampion + NOFilesInEsports + NOFilesInMatchups;

            // New arrays with 2d array where the data should be stored
            string[][,] ChampionData = new string[NOFilesInChampion][,];
            string[][,] EsportsData = new string[NOFilesInEsports][,];
            string[][,] MatchupsData = new string[NOFilesInMatchups][,];

            // Loading data into the arrays
            ChampionData = Load.LoadPath(ChampionDataPath);
            EsportsData = Load.LoadPath(EsportsDataPath);
            MatchupsData = Load.LoadPath(MatchupsDataPath);

            double[][,] DoubleChampionData = new double[NOFilesInChampion][,];
            double[][,] DoubleEsportsData = new double[NOFilesInEsports][,];
            double[][,] DoubleMatchupsData = new double[NOFilesInMatchups][,];


            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            //Console.WriteLine("Number of files loaded: " + AllMatches);
            //Console.WriteLine("done it took " + elapsedMs + " milliseconds to load the data");

            //Load.ReadJson(@"C: \Users\Rasmus\Desktop\Uni\P2\Git\Projekt\Data\Player Pool Data\out.json");

            
            

            DoubleChampionData = Load.ConvertToIntArray(ChampionData);

            //Console.WriteLine(ChampionData[0][6,11]);
            
               









            Console.ReadKey();

        }
    }
}
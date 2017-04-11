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
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here

            // New instance of Loading data
            LoadingData Load = new LoadingData();

            // Data Paths
            string ChampionDataPath = @"C: \Users\Rasmus\Desktop\Uni\P2\Git\Projekt\Data\CSV Files Champion Data";
            string EsportsDataPath = @"C:\Users\Rasmus\Desktop\Uni\P2\Git\Projekt\Data\CSV Files Lol Esport Data";
            string MatchupsDataPath = @"C:\Users\Rasmus\Desktop\Uni\P2\Git\Projekt\Data\CSV Files Matchups";

            // Number of files in datapaths
            int NOFilesInChampion = Load.GetFileNames(ChampionDataPath).Length;
            int NOFilesInEsports = Load.GetFileNames(EsportsDataPath).Length;
            int NOFilesInMatchups = Load.GetFileNames(MatchupsDataPath).Length;

            // New arrays with 2d array where the data should be stored
            string[][,] ChampionData = new string[NOFilesInChampion][,];
            string[][,] EsportsData = new string[NOFilesInEsports][,];
            string[][,] MatchupsData = new string[NOFilesInMatchups][,];

            // Loading data into the arrays
            ChampionData = Load.LoadPath(ChampionDataPath);
            EsportsData = Load.LoadPath(EsportsDataPath);
            MatchupsData = Load.LoadPath(MatchupsDataPath);


            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine("done it took " + elapsedMs + " milliseconds to load the data");           
            Console.ReadKey();

        }
    }
}
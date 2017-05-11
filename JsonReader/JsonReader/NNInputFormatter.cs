using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JsonReader
{
    class NNInputFormatter
    {
        public NNInputFormatter()
        {
            JSONLoad();
            ConvertGames();
            LoadChampionIdDictionary();

        }

        public List<SaveGameInfo.Game> games = new List<SaveGameInfo.Game>();
        public List<GameInfo.Match> matches = new List<GameInfo.Match>();
        public Dictionary<int, int[]> championIds = new Dictionary<int, int[]>();
        public Dictionary<string, int[]> teamNames = new Dictionary<string, int[]>();

        private string GetLocalDirectory()
        {
            string LocalDirectory = Assembly.GetExecutingAssembly().Location;
            LocalDirectory = LocalDirectory.Remove(Regex.Match(LocalDirectory, "JsonReader").Index);
            return LocalDirectory;
        }

        public void JSONLoad()
        {
            string LocalPath = GetLocalDirectory() + @"Data\Matches\";
            for (int index = 1; index < 2827; index++)
            {
                using (StreamReader Reader = new StreamReader(LocalPath + $@"{index}.json"))
                {
                    // Current file read to the end
                    string json = Reader.ReadToEnd();
                    var result = JsonConvert.DeserializeObject<GameInfo.Match>(json);
                    // Adding to final list of matches
                    matches.Add(result);
                    Console.WriteLine(index + " done");
                }
            }
        }

        public double Normalization(double currentValue, double minValue, double maxValue)
        {
            return (2 * ((currentValue - minValue) / (maxValue - minValue)) - 1);
        }

        public void ConvertGames()
        {
            foreach (GameInfo.Match match in matches)
            {
                games.Add(GameToSaveGame(match));
            }
            matches = null;
        }
        public void LoadTeamNamesDictionary()
        {
            List<string> uniqueTeamNames = new List<string>();

            foreach (SaveGameInfo.Game game in games)
            {
                foreach (SaveGameInfo.Team team in game.teams)
                {
                    string teamName = team.players[0].summonerName.Substring(0, team.players[0].summonerName.IndexOf(' '));
                    if (!uniqueTeamNames.Contains(teamName))
                    {
                        uniqueTeamNames.Add(teamName);
                    }
                }
            }
            int num = uniqueTeamNames.Count();
            for (int i = 0; i < num; i++)
            {
                int[] uniqueTeamNameArray = new int[uniqueTeamNames.Count];
                Array.Clear(uniqueTeamNameArray, 0, uniqueTeamNames.Count);
                uniqueTeamNameArray[i] = 1;
                teamNames.Add(uniqueTeamNames[i], uniqueTeamNameArray);
            }

        }

        public void LoadChampionIdDictionary()
        {
            List<int> uniqueChampionId = new List<int>();

            foreach (SaveGameInfo.Game game in games)
            {
                foreach (SaveGameInfo.Team team in game.teams)
                {
                    foreach (SaveGameInfo.Player player in team.players)
                    {
                        if (!uniqueChampionId.Contains(player.championId))
                        {
                            uniqueChampionId.Add(player.championId);
                        }
                    }
                    foreach (int id in team.bannedChampionId)
                    {
                        if (!uniqueChampionId.Contains(id))
                        {
                            uniqueChampionId.Add(id);
                        }
                    }
                }
            }
            int num = uniqueChampionId.Count();
            for (int i = 0; i < num; i++)
            {
                int[] uniqueChampionIdArray = new int[uniqueChampionId.Count];
                Array.Clear(uniqueChampionIdArray, 0, uniqueChampionId.Count);
                uniqueChampionIdArray[i] = 1;
                championIds.Add(uniqueChampionId[i], uniqueChampionIdArray);
            }
        }

        double[] SaveGameToInputNeurons(SaveGameInfo.Game game)
        {

            return null;
        }

        double[] SaveGameToPlayerInputNeurons(SaveGameInfo.Game game)
        {

            return null;
        }

        double[] CombineArrays(double[][] jaggedArray)
        {
            int totalArrayLength = 0;
            for (int i = 0; i < jaggedArray.GetLength(0); i++)
            {
                totalArrayLength += jaggedArray[i].Length;
            }
            double[] returnArray = new double[totalArrayLength];
            for (int i = 0; i < jaggedArray.GetLength(0); i++)
            {
                for (int j = 0; j < jaggedArray[i].GetLength(1); j++)
                {

                }
            }
            return null;
        }

        SaveGameInfo.Game GameToSaveGame(GameInfo.Match game)
        {
            SaveGameInfo.Game returnGame = new SaveGameInfo.Game();
            returnGame.gameCreation = game.gameCreation;
            returnGame.teams = new SaveGameInfo.Team[2];
            for (int i = 0; i < returnGame.teams.Length; i++)
            {
                returnGame.teams[i] = new SaveGameInfo.Team();
                returnGame.teams[i].players = new SaveGameInfo.Player[5];
                for (int j = 0; j < returnGame.teams[i].players.Length; j++)
                {
                    returnGame.teams[i].players[j] = new SaveGameInfo.Player();
                    returnGame.teams[i].players[j].championId = game.participants[(i * 5) + j].championId;
                }
            }
            return null;
        }
    }
}

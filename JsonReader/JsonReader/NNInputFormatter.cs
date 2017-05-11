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
            LoadTeamsDictionary();
        }

        public List<SaveGameInfo.Game> games = new List<SaveGameInfo.Game>();
        public List<GameInfo.Match> matches = new List<GameInfo.Match>();
        public Dictionary<int, double[]> championIds = new Dictionary<int, double[]>();
        public Dictionary<string, Team> teams = new Dictionary<string, Team>();
        public Dictionary<int, int[]> championIds = new Dictionary<int, int[]>();
        public Dictionary<string, int[]> teamNames = new Dictionary<string, int[]>();
        public Dictionary<string, int[]> playerNames = new Dictionary<string, int[]>();

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

        public void LoadPlayerNamesDictionary()
        {
            List<string> uniquePlayerNames = new List<string>();

            foreach (SaveGameInfo.Game game in games)
            {
                foreach (SaveGameInfo.Team team in game.teams)
                {
                    foreach (SaveGameInfo.Player player in team.players)
                    {
                        if (!uniquePlayerNames.Contains(player.summonerName))
                        {
                            uniquePlayerNames.Add(player.summonerName);
                        }
                    }
                }
            }
            int num = uniquePlayerNames.Count;
            for (int i = 0; i < num; i++)
            {
                int[] uniquePlayerNamesArray = new int[uniquePlayerNames.Count];
                Array.Clear(uniquePlayerNamesArray, 0, uniquePlayerNames.Count);
                uniquePlayerNamesArray[i] = 1;
                playerNames.Add(uniquePlayerNames[i], uniquePlayerNamesArray);
            }
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
                double[] uniqueTeamNameArray = new double[uniqueTeamNames.Count];
                Array.Clear(uniqueTeamNameArray, 0, uniqueTeamNames.Count);
                uniqueTeamNameArray[i] = 1;
                teams.Add(uniqueTeamNames[i], new Team(uniqueTeamNames[i], uniqueTeamNameArray));
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
                double[] uniqueChampionIdArray = new double[uniqueChampionId.Count];
                Array.Clear(uniqueChampionIdArray, 0, uniqueChampionId.Count);
                uniqueChampionIdArray[i] = 1;
                championIds.Add(uniqueChampionId[i], uniqueChampionIdArray);
            }
        }

        public string GetTeamName(SaveGameInfo.Game game, bool blueTeam)
        {
            if (blueTeam)
            {
                return game.teams[0].players[0].summonerName.Substring(0, game.teams[0].players[0].summonerName.IndexOf(' '));
            }
            else
            {
                return game.teams[1].players[0].summonerName.Substring(0, game.teams[1].players[0].summonerName.IndexOf(' '));
            }
        }

        double[] SaveGameToInputNeurons(SaveGameInfo.Game game)
        {
            double[][] inputNeuronArray = new double[12][];
            for (int i = 0; i < 5; i++)
            {
                inputNeuronArray[i] = SaveGameToPlayerInputNeurons(game.teams[0].players[i]);
            }
            for (int i = 0; i < 5; i++)
            {
                inputNeuronArray[i + 5] = SaveGameToPlayerInputNeurons(game.teams[1].players[i]);
            }
            inputNeuronArray[10] = teams[GetTeamName(game, true)].TeamNeuronInput;
            inputNeuronArray[11] = teams[GetTeamName(game, false)].TeamNeuronInput;
            return CombineArrays(inputNeuronArray);
        }

        double[] SaveGameToPlayerInputNeurons(SaveGameInfo.Player player)
        {
            return championIds[player.championId];
        }

        double[] CombineArrays(double[][] jaggedArray)
        {
            int totalArrayLength = 0;
            for (int i = 0; i < jaggedArray.GetLength(0); i++)
            {
                totalArrayLength += jaggedArray[i].Length;
            }
            double[] returnArray = new double[totalArrayLength];
            int currentId = 0;
            for (int i = 0; i < jaggedArray.GetLength(0); i++, currentId++)
            {
                for (int j = 0; j < jaggedArray[i].Length; j++, currentId++)
                {
                    returnArray[currentId] = jaggedArray[i][j];
                }
            }
            return returnArray;
        }

        SaveGameInfo.Game GameToSaveGame(GameInfo.Match game)
        {
            //still missing some stuff
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

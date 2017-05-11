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
            LoadPlayerNamesDictionary();
        }

        public List<SaveGameInfo.Game> games = new List<SaveGameInfo.Game>();
        public List<GameInfo.Match> matches = new List<GameInfo.Match>();
        public Dictionary<int, double[]> championIds = new Dictionary<int, double[]>();
        public Dictionary<string, Team> teams = new Dictionary<string, Team>();
        public Dictionary<string, double[]> playerNames = new Dictionary<string, double[]>();

        public double[] loadSaveInfo(SaveGameInfo.Game game)
        {
            List<double> gameData = new List<double>();
            double[] input;

            foreach (SaveGameInfo.Team team in game.teams)
            {

                gameData.Add(team.towerKills);
                gameData.Add(team.inhibitorKills);
                gameData.Add(team.baronKills);
                gameData.Add(team.dragonKills);
                gameData.Add(team.riftHeraldKills);

                if (team.firstBlood)
                { gameData.Add(1); }
                else { gameData.Add(0); }

                if (team.firstTower)
                { gameData.Add(1); }
                else { gameData.Add(0); }

                if (team.firstInhibitor)
                { gameData.Add(1); }
                else { gameData.Add(0); }

                if (team.firstBaron)
                { gameData.Add(1); }
                else { gameData.Add(0); }

                if (team.firstDragon)
                { gameData.Add(1); }
                else { gameData.Add(0); }

                if(team.firstRiftHerald)
                { gameData.Add(1); }
                else { gameData.Add(0); }

                foreach (SaveGameInfo.Player player in team.players)
                {
                    gameData.Add(player.stats.kills);
                    gameData.Add(player.stats.deaths);
                    gameData.Add(player.stats.assists);
                    gameData.Add(player.stats.largestKillingSpree);
                    gameData.Add(player.stats.largestMultiKill);
                    gameData.Add(player.stats.killingSprees);
                    gameData.Add(player.stats.longestTimeSpentLiving);
                    gameData.Add(player.stats.doubleKills);
                    gameData.Add(player.stats.tripleKills);
                    gameData.Add(player.stats.quadraKills);
                    gameData.Add(player.stats.pentaKills);
                    gameData.Add(player.stats.unrealKills);
                    gameData.Add(player.stats.totalDamageDealt);
                    gameData.Add(player.stats.magicDamageDealt);
                    gameData.Add(player.stats.physicalDamageDealt);
                    gameData.Add(player.stats.trueDamageDealt);
                    gameData.Add(player.stats.largestCriticalStrike);
                    gameData.Add(player.stats.totalDamageDealtToChampions);
                    gameData.Add(player.stats.magicDamageDealtToChampions);
                    gameData.Add(player.stats.physicalDamageDealtToChampions);
                    gameData.Add(player.stats.trueDamageDealtToChampions);
                    gameData.Add(player.stats.totalHeal);
                    gameData.Add(player.stats.totalUnitsHealed);
                    gameData.Add(player.stats.damageSelfMitigated);
                    gameData.Add(player.stats.damageDealtToObjectives);
                    gameData.Add(player.stats.damageDealtToTurrets);
                    gameData.Add(player.stats.timeCCingOthers);
                    gameData.Add(player.stats.totalDamageTaken);
                    gameData.Add(player.stats.magicalDamageTaken);
                    gameData.Add(player.stats.physicalDamageTaken);
                    gameData.Add(player.stats.trueDamageTaken);
                    gameData.Add(player.stats.goldEarned);
                    gameData.Add(player.stats.goldSpent);
                    gameData.Add(player.stats.turretKills);
                    gameData.Add(player.stats.inhibitorKills);
                    gameData.Add(player.stats.totalMinionsKilled);
                    gameData.Add(player.stats.neutralMinionsKilled);
                    gameData.Add(player.stats.neutralMinionsKilledTeamJungle);
                    gameData.Add(player.stats.neutralMinionsKilledEnemyJungle);
                    gameData.Add(player.stats.totalTimeCrowdControlDealt);
                    gameData.Add(player.stats.champLevel);
                    gameData.Add(player.stats.visionWardsBoughtInGame);
                    gameData.Add(player.stats.wardsPlaced);
                    gameData.Add(player.stats.wardsKilled);

                    if (player.stats.firstBloodKill)
                    { gameData.Add(1); }
                    else { gameData.Add(0); }

                    if (player.stats.firstBloodAssist)
                    { gameData.Add(1); }
                    else { gameData.Add(0); }

                    if (player.stats.firstTowerKill)
                    { gameData.Add(1); }
                    else { gameData.Add(0); }

                    if (player.stats.firstTowerAssist)
                    { gameData.Add(1); }
                    else { gameData.Add(0); };

                    if (player.stats.firstInhibitorKill)
                    { gameData.Add(1); }
                    else { gameData.Add(0); }

                    if (player.stats.firstInhibitorAssist)
                    { gameData.Add(1); }
                    else { gameData.Add(0); }

                    gameData.Add(player.timeline.creepsPerMinDeltas._010);
                    gameData.Add(player.timeline.creepsPerMinDeltas._1020);
                    gameData.Add(player.timeline.creepsPerMinDeltas._2030);
                    gameData.Add(player.timeline.creepsPerMinDeltas._3040);
                    gameData.Add(player.timeline.creepsPerMinDeltas._40end);

                    gameData.Add(player.timeline.xpPerMinDeltas._010);
                    gameData.Add(player.timeline.xpPerMinDeltas._1020);
                    gameData.Add(player.timeline.xpPerMinDeltas._3040);
                    gameData.Add(player.timeline.xpPerMinDeltas._2030);
                    gameData.Add(player.timeline.xpPerMinDeltas._40end);

                    gameData.Add(player.timeline.goldPerMinDeltas._010);
                    gameData.Add(player.timeline.goldPerMinDeltas._1020);
                    gameData.Add(player.timeline.goldPerMinDeltas._2030);
                    gameData.Add(player.timeline.goldPerMinDeltas._3040);
                    gameData.Add(player.timeline.goldPerMinDeltas._40end);

                    gameData.Add(player.timeline.csDiffPerMinDeltas._010);
                    gameData.Add(player.timeline.csDiffPerMinDeltas._1020);
                    gameData.Add(player.timeline.csDiffPerMinDeltas._2030);
                    gameData.Add(player.timeline.csDiffPerMinDeltas._3040);
                    gameData.Add(player.timeline.csDiffPerMinDeltas._40end);

                    gameData.Add(player.timeline.xpDiffPerMinDeltas._010);
                    gameData.Add(player.timeline.xpDiffPerMinDeltas._1020);
                    gameData.Add(player.timeline.xpDiffPerMinDeltas._2030);
                    gameData.Add(player.timeline.xpDiffPerMinDeltas._3040);
                    gameData.Add(player.timeline.xpDiffPerMinDeltas._40end);

                    gameData.Add(player.timeline.damageTakenPerMinDeltas._010);
                    gameData.Add(player.timeline.damageTakenPerMinDeltas._1020);
                    gameData.Add(player.timeline.damageTakenPerMinDeltas._2030);
                    gameData.Add(player.timeline.damageTakenPerMinDeltas._3040);
                    gameData.Add(player.timeline.damageTakenPerMinDeltas._40end);

                    gameData.Add(player.timeline.damageTakenDiffPerMinDeltas._010);
                    gameData.Add(player.timeline.damageTakenDiffPerMinDeltas._1020);
                    gameData.Add(player.timeline.damageTakenDiffPerMinDeltas._2030);
                    gameData.Add(player.timeline.damageTakenDiffPerMinDeltas._3040);
                    gameData.Add(player.timeline.damageTakenDiffPerMinDeltas._40end);
                }
            }

            input = gameData.ToArray();
            return input;

        }

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
                double[] uniquePlayerNamesArray = new double[uniquePlayerNames.Count];
                Array.Clear(uniquePlayerNamesArray, 0, uniquePlayerNames.Count);
                uniquePlayerNamesArray[i] = 1;
                playerNames.Add(uniquePlayerNames[i], uniquePlayerNamesArray);
            }
        }

        public void LoadTeamsDictionary()
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

        public double[] GetAllPlayersInTeam(SaveGameInfo.Team team)
        {
            double[][] players = new double[5][];
            double[] allTeamPlayers;
            for (int i = 0; i < players.GetLength(0); i++)
            {
                players[i] = playerNames[team.players[i].summonerName];
            }
            allTeamPlayers = players[0];
            for (int i = 1; i < players.GetLength(0); i++)
            {
                for (int j = 0; j < players[i].Length; j++)
                {
                    if (players[i][j] == 1)
                    {
                        allTeamPlayers[j] = 1;
                    }
                }
            }
            return allTeamPlayers;
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
            returnGame.gameDuration = game.gameDuration;
            returnGame.teams = new SaveGameInfo.Team[2];
            for (int i = 0; i < returnGame.teams.Length; i++)
            {
                returnGame.teams[i] = new SaveGameInfo.Team();
                returnGame.teams[i].players = new SaveGameInfo.Player[5];
                for (int j = 0; j < returnGame.teams[i].players.Length; j++)
                {
                    returnGame.teams[i].players[j] = new SaveGameInfo.Player();
                    returnGame.teams[i].players[j].championId = game.participants[(i * 5) + j].championId;
                    returnGame.teams[i].players[j].playerId = game.participants[(i * 5) + j].championId;
                    returnGame.teams[i].players[j].summonerName = game.participantIdentities[(i * 5 + j)].player.summonerName;

                }
            }
            return null;
        }
    }
}

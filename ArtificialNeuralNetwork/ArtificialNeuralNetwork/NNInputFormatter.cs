using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ArtificialNeuralNetwork
{
    class NNInputFormatter
    {
        public NNInputFormatter()
        {
            JSONLoad();
            ConvertGames();
            games = games.OrderByDescending(x => x.gameDuration).ToList();
            LoadChampionIdDictionary();
            LoadTeamsDictionary();
            LoadPlayerNamesDictionary();
            bufferGames = FindBufferGames();
            MakeTestCases();
        }

        public List<NNTestCase> testCases = new List<NNTestCase>();
        public List<SaveGameInfo.Game> games = new List<SaveGameInfo.Game>();
        public List<SaveGameInfo.Game> bufferGames = new List<SaveGameInfo.Game>();
        public List<GameInfo.Match> matches = new List<GameInfo.Match>();
        public Dictionary<int, double[]> championIds = new Dictionary<int, double[]>();
        public Dictionary<string, Team> teams = new Dictionary<string, Team>();
        public Dictionary<string, double[]> playerNames = new Dictionary<string, double[]>();

        public List<SaveGameInfo.Game> FindBufferGames()
        {
            List<SaveGameInfo.Game> saveGames = new List<SaveGameInfo.Game>();
            int[] gamesSavedByTeam = new int[teams.Count];
            for (int i = 0; i < gamesSavedByTeam.Length; i++)
            {
                for (int j = 0; gamesSavedByTeam[i] < 3; j++)
                {
                    if (games[j].teams[0].teamName == teams.ElementAt(i).Key || games[j].teams[1].teamName == teams.ElementAt(i).Key)
                    {
                        saveGames.Add(games[j]);
                        gamesSavedByTeam[i]++;
                    }
                }
            }
            Console.WriteLine($"Done finding {saveGames.Count} Buffer Games.");
            return saveGames;
        }

        public void MakeTestCases()
        {
            //foreach non-buffer game make a testcase with previous 3 games from same team
            for (int i = 0; i < games.Count; i++)
            {
                //check if the current game is a buffer game
                bool isBuffer = false;
                for (int j = 0; j < bufferGames.Count; j++)
                {
                    if (games[i].Equals(bufferGames[j]))
                    {
                        isBuffer = true;
                    }
                }
                if (!isBuffer)
                {
                    string blueTeamName = games[i].teams[0].teamName;
                    string redTeamName = games[i].teams[1].teamName;
                    List<SaveGameInfo.Team> recentBlueGames = new List<SaveGameInfo.Team>();
                    List<SaveGameInfo.Team> recentRedGames = new List<SaveGameInfo.Team>();
                    //Find previous 3 games for each team
                    for (int j = i-1; recentBlueGames.Count < 3; j--)
                    {
                        if (games[j].teams[0].teamName == blueTeamName)
                        { recentBlueGames.Add(games[j].teams[0]); }
                        if (games[j].teams[1].teamName == blueTeamName)
                        { recentBlueGames.Add(games[j].teams[1]); }
                    }
                    for (int j = i - 1; recentRedGames.Count < 3; j--)
                    {
                        if (games[j].teams[0].teamName == redTeamName)
                        { recentRedGames.Add(games[j].teams[0]); }
                        if (games[j].teams[1].teamName == redTeamName)
                        { recentRedGames.Add(games[j].teams[1]); }
                    }
                    //add the new testCase to the list of testcases
                    testCases.Add(new NNTestCase(recentBlueGames.ToArray(), recentRedGames.ToArray(), games[i]));
                }
            }
            //make recent games for each test case into input neurons
            CalculateTestCasesInputNeurons();
            Console.WriteLine($"Done making {testCases.Count} Test Cases.");
        }

        private void CalculateTestCasesInputNeurons()
        {
            for(int i = 0; i < testCases.Count; i++)
            {
                double[][] recentGamesNeurons = new double[6][];
                for (int j = 0; j < 3; j++)
                {
                    recentGamesNeurons[j] = SaveTeamToTeamNeurons(testCases[i].blueTeamLatestGames[j]);
                }
                for (int j = 0; j < 3; j++)
                {
                    recentGamesNeurons[j+3] = SaveTeamToTeamNeurons(testCases[i].redTeamLatestGames[j]);
                }
                testCases[i].inputNeurons = CombineArrays(recentGamesNeurons);
            }
        }

        public void PrintDoubleArrayIgnoreZero(double[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != 0)
                {
                    Console.WriteLine(array[i].ToString());
                }
            }
        }

        private string GetLocalDirectory()
        {
            string LocalDirectory = Assembly.GetExecutingAssembly().Location;
            LocalDirectory = LocalDirectory.Remove(Regex.Match(LocalDirectory, "ArtificialNeuralNetwork").Index);
            return LocalDirectory;
        }

        private void JSONLoad()
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
                }
            }
            Console.WriteLine("Done loading JSON Files into JSON Class");
        }

        public double Normalization(double currentValue, double minValue, double maxValue)
        {
            return (2 * ((currentValue - minValue) / (maxValue - minValue)) - 1);
        }

        private void ConvertGames()
        {
            for (int i = 0; i < matches.Count; i++)
            {
                games.Add(MatchToSaveGame(matches[i]));
            }
            matches = null;
            games = games.OrderByDescending(x => x.gameDuration).ToList();
            Console.WriteLine($"Done converting from JSON Class to Game Class");
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

        private string GetTeamName(SaveGameInfo.Game game, bool blueTeam)
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

        public double[] SaveGameToNeurons(SaveGameInfo.Game game)
        {
            return CombineArrays(new double[][] { SaveGameToTeamNeurons(game), SaveGameToMiscNeurons(game) });
        }

        /*public NNTestCase SaveGameToTestCase(SaveGameInfo.Game game)
        {
            NNTestCase testCase = new NNTestCase();
            if(game.teams[0].win == true) { testCase.winningTeam = 0; }
            else { testCase.winningTeam = 1; }
            testCase.inputNeurons = SaveGameToTeamNeurons(game);
            return testCase;
        }*/

        private double[] SaveTeamToTeamNeurons(SaveGameInfo.Team team)
        {
            double[][] inputNeuronArray = new double[6][];
            inputNeuronArray[0] = teams[team.teamName].TeamNeuronInput;
            for (int i = 0; i < 5; i++)
            {
                inputNeuronArray[i+1] = SaveGameToPlayerNeurons(team.players[i]);
            }
            return CombineArrays(inputNeuronArray);
        }

        private double[] SavePlayerToPlayerNeurons(SaveGameInfo.Player player)
        {
            return championIds[player.championId];
        }
        
        private double[] SaveGameToTeamNeurons(SaveGameInfo.Game game)
        {
            double[][] inputNeuronArray = new double[12][];
            for (int i = 0; i < 5; i++)
            {
                inputNeuronArray[i] = SaveGameToPlayerNeurons(game.teams[0].players[i]);
            }
            for (int i = 0; i < 5; i++)
            {
                inputNeuronArray[i + 5] = SaveGameToPlayerNeurons(game.teams[1].players[i]);
            }
            inputNeuronArray[10] = teams[GetTeamName(game, true)].TeamNeuronInput;
            inputNeuronArray[11] = teams[GetTeamName(game, false)].TeamNeuronInput;
            return CombineArrays(inputNeuronArray);
        }

        private double[] SaveGameToPlayerNeurons(SaveGameInfo.Player player)
        {
            return championIds[player.championId];
        }

        private double[] SaveGameToMiscNeurons(SaveGameInfo.Game game)
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

                if (team.firstRiftHerald)
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

        double[] CombineArrays(double[][] jaggedArray)
        {
            //goes out of range
            int totalArrayLength = 0;
            for (int i = 0; i < jaggedArray.GetLength(0); i++)
            {
                totalArrayLength += jaggedArray[i].Length;
            }
            double[] returnArray = new double[totalArrayLength];
            int currentId = 0;
            for (int i = 0; i < jaggedArray.GetLength(0); i++)
            {
                for (int j = 0; j < jaggedArray[i].Length; j++, currentId++)
                {
                    returnArray[currentId] = jaggedArray[i][j];
                }
            }
            return returnArray;
        }

        SaveGameInfo.Game MatchToSaveGame(GameInfo.Match match)
        {
            //still missing some stuff
            SaveGameInfo.Game returnGame = new SaveGameInfo.Game();
            returnGame.gameCreation = match.gameCreation;
            returnGame.gameDuration = match.gameDuration;
            returnGame.gameVersion = match.gameVersion;
            returnGame.teams = new SaveGameInfo.Team[2];
            for (int i = 0; i < returnGame.teams.Length; i++)
            {
                returnGame.teams[i] = new SaveGameInfo.Team();
                returnGame.teams[i].teamId = match.teams[i].teamId;
                returnGame.teams[i].players = new SaveGameInfo.Player[5];
                returnGame.teams[i].teamName = match.participantIdentities[i * 5].player.summonerName.Substring(0, match.participantIdentities[i * 5].player.summonerName.IndexOf(' '));
                returnGame.teams[i].win = match.participants[i * 5].stats.win;
                returnGame.teams[i].firstBlood = match.teams[i].firstBlood;
                returnGame.teams[i].firstTower = match.teams[i].firstTower;
                returnGame.teams[i].firstInhibitor = match.teams[i].firstInhibitor;
                returnGame.teams[i].firstBaron = match.teams[i].firstBaron;
                returnGame.teams[i].bannedChampionId = new int[3];
                for (int j = 0; j < 3; j++)
                {
                    if (j < match.teams[i].bans.Length)
                    { returnGame.teams[i].bannedChampionId[j] = match.teams[i].bans[j].championId; }
                    else
                    { returnGame.teams[i].bannedChampionId[j] = 0; }
                }
                for (int j = 0; j < returnGame.teams[i].players.Length; j++)
                {
                    returnGame.teams[i].players[j] = new SaveGameInfo.Player();
                    returnGame.teams[i].players[j].championId = match.participants[(i * 5) + j].championId;
                    returnGame.teams[i].players[j].playerId = match.participants[(i * 5) + j].championId;
                    returnGame.teams[i].players[j].summonerName = match.participantIdentities[(i * 5 + j)].player.summonerName;
                    returnGame.teams[i].players[j].stats = StatsToSaveStats(match.participants[(i * 5) + j].stats);
                    returnGame.teams[i].players[j].playerId = match.participantIdentities[j].participantId;
                    //spells here if needed
                    //timeline here if needed
                }
            }
            return returnGame;
        }

        SaveGameInfo.Stats StatsToSaveStats(GameInfo.Stats stats)
        {
            SaveGameInfo.Stats returnStats = new SaveGameInfo.Stats();
            returnStats.participantId = stats.participantId;
            returnStats.win = stats.win;
            returnStats.kills = stats.kills;
            returnStats.deaths = stats.deaths;
            returnStats.assists = stats.assists;
            returnStats.largestKillingSpree = stats.largestKillingSpree;
            returnStats.largestMultiKill = stats.largestMultiKill;
            returnStats.killingSprees = stats.killingSprees;
            returnStats.longestTimeSpentLiving = stats.longestTimeSpentLiving;
            returnStats.doubleKills = stats.doubleKills;
            returnStats.tripleKills = stats.tripleKills;
            returnStats.quadraKills = stats.quadraKills;
            returnStats.pentaKills = stats.pentaKills;
            returnStats.unrealKills = stats.unrealKills;
            returnStats.totalDamageDealt = stats.totalDamageDealt;
            returnStats.magicDamageDealt = stats.magicDamageDealt;
            returnStats.physicalDamageDealt = stats.physicalDamageDealt;
            returnStats.trueDamageDealt = stats.physicalDamageDealt;
            returnStats.largestCriticalStrike = stats.largestCriticalStrike;
            returnStats.totalDamageDealtToChampions = stats.totalDamageDealtToChampions;
            returnStats.magicDamageDealtToChampions = stats.magicDamageDealtToChampions;
            returnStats.physicalDamageDealtToChampions = stats.physicalDamageDealtToChampions;
            returnStats.trueDamageDealtToChampions = stats.trueDamageDealtToChampions;
            returnStats.totalHeal = stats.totalHeal;
            returnStats.totalUnitsHealed = stats.totalUnitsHealed;
            returnStats.damageSelfMitigated = stats.damageSelfMitigated;
            returnStats.damageDealtToObjectives = stats.damageDealtToObjectives;
            returnStats.damageDealtToTurrets = stats.damageDealtToTurrets;
            returnStats.timeCCingOthers = stats.timeCCingOthers;
            returnStats.totalUnitsHealed = stats.totalDamageTaken = stats.totalDamageTaken;
            returnStats.magicalDamageTaken = stats.magicalDamageTaken;
            returnStats.physicalDamageTaken = stats.physicalDamageTaken;
            returnStats.trueDamageTaken = stats.trueDamageTaken;
            returnStats.goldEarned = stats.goldEarned;
            returnStats.goldSpent = stats.goldSpent;
            returnStats.turretKills = stats.turretKills;
            returnStats.inhibitorKills = stats.inhibitorKills;
            returnStats.totalMinionsKilled = stats.totalMinionsKilled;
            returnStats.neutralMinionsKilled = stats.neutralMinionsKilled;
            returnStats.neutralMinionsKilledTeamJungle = stats.neutralMinionsKilledTeamJungle;
            returnStats.neutralMinionsKilledEnemyJungle = stats.neutralMinionsKilledEnemyJungle;
            returnStats.totalTimeCrowdControlDealt = stats.totalTimeCrowdControlDealt;
            returnStats.champLevel = stats.champLevel;
            returnStats.visionWardsBoughtInGame = stats.visionWardsBoughtInGame;
            returnStats.firstBloodKill = stats.firstBloodKill;
            returnStats.firstBloodAssist = stats.firstBloodAssist;
            returnStats.firstTowerKill = stats.firstTowerKill;
            returnStats.firstTowerAssist = stats.firstTowerAssist;
            returnStats.firstInhibitorKill = stats.firstInhibitorKill;
            returnStats.firstInhibitorAssist = stats.firstInhibitorAssist;
            return returnStats;
        }
    }
}

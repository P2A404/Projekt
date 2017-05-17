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
        public NNInputFormatter(int TrainingPoolSize)
        {
            TrainingTestCases = new TestCase[TrainingPoolSize];
            JSONLoad();
            ConvertGames();
            games = games.OrderByDescending(x => x.gameDuration).ToList();
            LoadChampionIdDictionary();
            LoadTeamsDictionary();
            LoadPlayerNamesDictionary();
            bufferGames = FindBufferGames();
            //minMaxList is not 100% correct
            List<SaveGameInfo.Game> minMaxList = games.GetRange(0, TrainingPoolSize);
            minMaxList.AddRange(bufferGames);
            MinMax(minMaxList);
            MakeTestCases();
            InputNeuronSize = testCases[0].inputNeurons.Length;
            TrainingTestCases = testCases.Take(TrainingPoolSize).ToArray();
            TestingTestCases = testCases.GetRange(TrainingPoolSize, (testCases.Count - TrainingPoolSize)).ToArray();
            PrintDoubleArrayIgnoreZero(TrainingTestCases[0].inputNeurons);
        }

        public TestCase[] TrainingTestCases;
        public TestCase[] TestingTestCases;
        public List<TestCase> testCases = new List<TestCase>();
        public List<SaveGameInfo.Game> games = new List<SaveGameInfo.Game>();
        public List<SaveGameInfo.Game> bufferGames = new List<SaveGameInfo.Game>();
        public List<GameInfo.Match> matches = new List<GameInfo.Match>();
        public Dictionary<int, double[]> championIds = new Dictionary<int, double[]>();
        public Dictionary<string, double[]> teams = new Dictionary<string, double[]>();
        public Dictionary<string, double[]> playerNames = new Dictionary<string, double[]>();
        public int InputNeuronSize;
        private SaveGameInfo.Team minimumTeam;
        private SaveGameInfo.Team maximumTeam;

        void MinMax(List<SaveGameInfo.Game> GameList)
        {
            #region MinMaxInitilization
            minimumTeam = new SaveGameInfo.Team();
            minimumTeam.baronKills = 1000000;
            minimumTeam.dragonKills = 1000000;
            minimumTeam.riftHeraldKills = 1000000;
            minimumTeam.towerKills = 1000000;
            minimumTeam.inhibitorKills = 1000000;
            minimumTeam.baronKills = 1000000;
            minimumTeam.players = new SaveGameInfo.Player[1];
            minimumTeam.players[0] = new SaveGameInfo.Player();
            minimumTeam.players[0].stats = new SaveGameInfo.Stats();
            minimumTeam.players[0].stats.kills = 1000000;
            minimumTeam.players[0].stats.deaths = 1000000;
            minimumTeam.players[0].stats.assists = 1000000;
            minimumTeam.players[0].stats.largestKillingSpree = 1000000;
            minimumTeam.players[0].stats.largestMultiKill = 1000000;
            minimumTeam.players[0].stats.killingSprees = 1000000;
            minimumTeam.players[0].stats.longestTimeSpentLiving = 1000000;
            minimumTeam.players[0].stats.doubleKills = 1000000;
            minimumTeam.players[0].stats.tripleKills = 1000000;
            minimumTeam.players[0].stats.quadraKills = 1000000;
            minimumTeam.players[0].stats.pentaKills = 1000000;
            minimumTeam.players[0].stats.unrealKills = 1000000;
            minimumTeam.players[0].stats.totalDamageDealt = 1000000;
            minimumTeam.players[0].stats.magicDamageDealt = 1000000;
            minimumTeam.players[0].stats.physicalDamageDealt = 1000000;
            minimumTeam.players[0].stats.trueDamageDealt = 1000000;
            minimumTeam.players[0].stats.largestCriticalStrike = 1000000;
            minimumTeam.players[0].stats.totalDamageDealtToChampions = 1000000;
            minimumTeam.players[0].stats.magicDamageDealtToChampions = 1000000;
            minimumTeam.players[0].stats.physicalDamageDealtToChampions = 1000000;
            minimumTeam.players[0].stats.trueDamageDealtToChampions = 1000000;
            minimumTeam.players[0].stats.totalHeal = 1000000;
            minimumTeam.players[0].stats.totalUnitsHealed = 1000000;
            minimumTeam.players[0].stats.damageSelfMitigated = 1000000;
            minimumTeam.players[0].stats.damageDealtToObjectives = 1000000;
            minimumTeam.players[0].stats.damageDealtToTurrets = 1000000;
            minimumTeam.players[0].stats.timeCCingOthers = 1000000;
            minimumTeam.players[0].stats.totalDamageTaken = 1000000;
            minimumTeam.players[0].stats.magicalDamageTaken = 1000000;
            minimumTeam.players[0].stats.physicalDamageTaken = 1000000;
            minimumTeam.players[0].stats.trueDamageTaken = 1000000;
            minimumTeam.players[0].stats.goldEarned = 1000000;
            minimumTeam.players[0].stats.goldSpent = 1000000;
            minimumTeam.players[0].stats.turretKills = 1000000;
            minimumTeam.players[0].stats.inhibitorKills = 1000000;
            minimumTeam.players[0].stats.totalMinionsKilled = 1000000;
            minimumTeam.players[0].stats.neutralMinionsKilled = 1000000;
            minimumTeam.players[0].stats.neutralMinionsKilledTeamJungle = 1000000;
            minimumTeam.players[0].stats.neutralMinionsKilledEnemyJungle = 1000000;
            minimumTeam.players[0].stats.totalTimeCrowdControlDealt = 1000000;
            minimumTeam.players[0].stats.champLevel = 1000000;
            minimumTeam.players[0].stats.visionWardsBoughtInGame = 1000000;
            minimumTeam.players[0].stats.wardsPlaced = 1000000;
            minimumTeam.players[0].stats.wardsKilled = 1000000;

            maximumTeam = new SaveGameInfo.Team();
            maximumTeam.baronKills = 0;
            maximumTeam.dragonKills = 0;
            maximumTeam.riftHeraldKills = 0;
            maximumTeam.towerKills = 0;
            maximumTeam.inhibitorKills = 0;
            maximumTeam.baronKills = 0;
            maximumTeam.players = new SaveGameInfo.Player[1];
            maximumTeam.players[0] = new SaveGameInfo.Player();
            maximumTeam.players[0].stats = new SaveGameInfo.Stats();
            maximumTeam.players[0].stats.kills = 0;
            maximumTeam.players[0].stats.deaths = 0;
            maximumTeam.players[0].stats.assists = 0;
            maximumTeam.players[0].stats.largestKillingSpree = 0;
            maximumTeam.players[0].stats.largestMultiKill = 0;
            maximumTeam.players[0].stats.killingSprees = 0;
            maximumTeam.players[0].stats.longestTimeSpentLiving = 0;
            maximumTeam.players[0].stats.doubleKills = 0;
            maximumTeam.players[0].stats.tripleKills = 0;
            maximumTeam.players[0].stats.quadraKills = 0;
            maximumTeam.players[0].stats.pentaKills = 0;
            maximumTeam.players[0].stats.unrealKills = 0;
            maximumTeam.players[0].stats.totalDamageDealt = 0;
            maximumTeam.players[0].stats.magicDamageDealt = 0;
            maximumTeam.players[0].stats.physicalDamageDealt = 0;
            maximumTeam.players[0].stats.trueDamageDealt = 0;
            maximumTeam.players[0].stats.largestCriticalStrike = 0;
            maximumTeam.players[0].stats.totalDamageDealtToChampions = 0;
            maximumTeam.players[0].stats.magicDamageDealtToChampions = 0;
            maximumTeam.players[0].stats.physicalDamageDealtToChampions = 0;
            maximumTeam.players[0].stats.trueDamageDealtToChampions = 0;
            maximumTeam.players[0].stats.totalHeal = 0;
            maximumTeam.players[0].stats.totalUnitsHealed = 0;
            maximumTeam.players[0].stats.damageSelfMitigated = 0;
            maximumTeam.players[0].stats.damageDealtToObjectives = 0;
            maximumTeam.players[0].stats.damageDealtToTurrets = 0;
            maximumTeam.players[0].stats.timeCCingOthers = 0;
            maximumTeam.players[0].stats.totalDamageTaken = 0;
            maximumTeam.players[0].stats.magicalDamageTaken = 0;
            maximumTeam.players[0].stats.physicalDamageTaken = 0;
            maximumTeam.players[0].stats.trueDamageTaken = 0;
            maximumTeam.players[0].stats.goldEarned = 0;
            maximumTeam.players[0].stats.goldSpent = 0;
            maximumTeam.players[0].stats.turretKills = 0;
            maximumTeam.players[0].stats.inhibitorKills = 0;
            maximumTeam.players[0].stats.totalMinionsKilled = 0;
            maximumTeam.players[0].stats.neutralMinionsKilled = 0;
            maximumTeam.players[0].stats.neutralMinionsKilledTeamJungle = 0;
            maximumTeam.players[0].stats.neutralMinionsKilledEnemyJungle = 0;
            maximumTeam.players[0].stats.totalTimeCrowdControlDealt = 0;
            maximumTeam.players[0].stats.champLevel = 0;
            maximumTeam.players[0].stats.visionWardsBoughtInGame = 0;
            maximumTeam.players[0].stats.wardsPlaced = 0;
            maximumTeam.players[0].stats.wardsKilled = 0;
            #endregion

            foreach (SaveGameInfo.Game g in GameList)
            {
                foreach(SaveGameInfo.Team t in g.teams)
                {
                    foreach (SaveGameInfo.Player p in t.players)
                    {
                        MinMaxPlayer(p);
                    }
                    maximumTeam.baronKills = Math.Max(maximumTeam.baronKills, t.baronKills);
                    maximumTeam.dragonKills = Math.Max(maximumTeam.dragonKills, t.dragonKills);
                    maximumTeam.riftHeraldKills = Math.Max(maximumTeam.riftHeraldKills, t.riftHeraldKills);
                    maximumTeam.towerKills = Math.Max(maximumTeam.towerKills, t.towerKills);
                    maximumTeam.inhibitorKills = Math.Max(maximumTeam.inhibitorKills, t.inhibitorKills);

                    minimumTeam.baronKills = Math.Min(minimumTeam.baronKills, t.baronKills);
                    minimumTeam.dragonKills = Math.Min(minimumTeam.dragonKills, t.dragonKills);
                    minimumTeam.riftHeraldKills = Math.Min(minimumTeam.riftHeraldKills, t.riftHeraldKills);
                    minimumTeam.towerKills = Math.Min(minimumTeam.towerKills, t.towerKills);
                    minimumTeam.inhibitorKills = Math.Min(minimumTeam.inhibitorKills, t.inhibitorKills);
                }
            }
        }

        void MinMaxPlayer(SaveGameInfo.Player p)
        {
            SaveGameInfo.Stats PlayerStats = p.stats;

            maximumTeam.players[0].stats.assists = Math.Max(maximumTeam.players[0].stats.assists, PlayerStats.assists);
            maximumTeam.players[0].stats.champLevel = Math.Max(maximumTeam.players[0].stats.champLevel, PlayerStats.champLevel);
            maximumTeam.players[0].stats.damageDealtToObjectives = Math.Max(maximumTeam.players[0].stats.damageDealtToObjectives, PlayerStats.damageDealtToObjectives);
            maximumTeam.players[0].stats.damageDealtToTurrets = Math.Max(maximumTeam.players[0].stats.damageDealtToTurrets, PlayerStats.damageDealtToTurrets);
            maximumTeam.players[0].stats.damageSelfMitigated = Math.Max(maximumTeam.players[0].stats.damageSelfMitigated, PlayerStats.damageSelfMitigated);
            maximumTeam.players[0].stats.deaths = Math.Max(maximumTeam.players[0].stats.deaths, PlayerStats.deaths);
            maximumTeam.players[0].stats.doubleKills = Math.Max(maximumTeam.players[0].stats.doubleKills, PlayerStats.doubleKills);
            maximumTeam.players[0].stats.goldEarned = Math.Max(maximumTeam.players[0].stats.goldEarned, PlayerStats.goldEarned);
            maximumTeam.players[0].stats.goldSpent = Math.Max(maximumTeam.players[0].stats.goldSpent, PlayerStats.goldSpent);
            maximumTeam.players[0].stats.inhibitorKills = Math.Max(maximumTeam.players[0].stats.inhibitorKills, PlayerStats.inhibitorKills);
            maximumTeam.players[0].stats.killingSprees = Math.Max(maximumTeam.players[0].stats.killingSprees, PlayerStats.killingSprees);
            maximumTeam.players[0].stats.kills = Math.Max(maximumTeam.players[0].stats.kills, PlayerStats.kills);
            maximumTeam.players[0].stats.largestCriticalStrike = Math.Max(maximumTeam.players[0].stats.largestCriticalStrike, PlayerStats.largestCriticalStrike);
            maximumTeam.players[0].stats.largestKillingSpree = Math.Max(maximumTeam.players[0].stats.largestKillingSpree, PlayerStats.largestKillingSpree);
            maximumTeam.players[0].stats.largestMultiKill = Math.Max(maximumTeam.players[0].stats.largestMultiKill, PlayerStats.largestMultiKill);
            maximumTeam.players[0].stats.longestTimeSpentLiving = Math.Max(maximumTeam.players[0].stats.longestTimeSpentLiving, PlayerStats.longestTimeSpentLiving);
            maximumTeam.players[0].stats.magicalDamageTaken = Math.Max(maximumTeam.players[0].stats.magicalDamageTaken, PlayerStats.magicalDamageTaken);
            maximumTeam.players[0].stats.magicDamageDealt = Math.Max(maximumTeam.players[0].stats.magicDamageDealt, PlayerStats.magicDamageDealt);
            maximumTeam.players[0].stats.magicDamageDealtToChampions = Math.Max(maximumTeam.players[0].stats.magicDamageDealtToChampions, PlayerStats.magicDamageDealtToChampions);
            maximumTeam.players[0].stats.neutralMinionsKilled = Math.Max(maximumTeam.players[0].stats.neutralMinionsKilled, PlayerStats.neutralMinionsKilled);
            maximumTeam.players[0].stats.neutralMinionsKilledEnemyJungle = Math.Max(maximumTeam.players[0].stats.neutralMinionsKilledEnemyJungle, PlayerStats.neutralMinionsKilledEnemyJungle);
            maximumTeam.players[0].stats.neutralMinionsKilledTeamJungle = Math.Max(maximumTeam.players[0].stats.neutralMinionsKilledTeamJungle, PlayerStats.neutralMinionsKilledTeamJungle);
            maximumTeam.players[0].stats.pentaKills = Math.Max(maximumTeam.players[0].stats.pentaKills, PlayerStats.pentaKills);
            maximumTeam.players[0].stats.physicalDamageDealt = Math.Max(maximumTeam.players[0].stats.physicalDamageDealt, PlayerStats.physicalDamageDealt);
            maximumTeam.players[0].stats.physicalDamageDealtToChampions = Math.Max(maximumTeam.players[0].stats.physicalDamageDealtToChampions, PlayerStats.physicalDamageDealtToChampions);
            maximumTeam.players[0].stats.physicalDamageTaken = Math.Max(maximumTeam.players[0].stats.physicalDamageTaken, PlayerStats.physicalDamageTaken);
            maximumTeam.players[0].stats.quadraKills = Math.Max(maximumTeam.players[0].stats.quadraKills, PlayerStats.quadraKills);
            maximumTeam.players[0].stats.timeCCingOthers = Math.Max(maximumTeam.players[0].stats.timeCCingOthers, PlayerStats.timeCCingOthers);
            maximumTeam.players[0].stats.totalDamageDealt = Math.Max(maximumTeam.players[0].stats.totalDamageDealt, PlayerStats.totalDamageDealt);
            maximumTeam.players[0].stats.totalDamageDealtToChampions = Math.Max(maximumTeam.players[0].stats.totalDamageDealtToChampions, PlayerStats.totalDamageDealtToChampions);
            maximumTeam.players[0].stats.totalDamageTaken = Math.Max(maximumTeam.players[0].stats.totalDamageTaken, PlayerStats.totalDamageTaken);
            maximumTeam.players[0].stats.totalHeal = Math.Max(maximumTeam.players[0].stats.totalHeal, PlayerStats.totalHeal);
            maximumTeam.players[0].stats.totalMinionsKilled = Math.Max(maximumTeam.players[0].stats.totalMinionsKilled, PlayerStats.totalMinionsKilled);
            maximumTeam.players[0].stats.totalTimeCrowdControlDealt = Math.Max(maximumTeam.players[0].stats.totalTimeCrowdControlDealt, PlayerStats.totalTimeCrowdControlDealt);
            maximumTeam.players[0].stats.totalUnitsHealed = Math.Max(maximumTeam.players[0].stats.totalUnitsHealed, PlayerStats.totalUnitsHealed);
            maximumTeam.players[0].stats.tripleKills = Math.Max(maximumTeam.players[0].stats.tripleKills, PlayerStats.tripleKills);
            maximumTeam.players[0].stats.trueDamageDealt = Math.Max(maximumTeam.players[0].stats.trueDamageDealt, PlayerStats.trueDamageDealt);
            maximumTeam.players[0].stats.trueDamageDealtToChampions = Math.Max(maximumTeam.players[0].stats.trueDamageDealtToChampions, PlayerStats.trueDamageDealtToChampions);
            maximumTeam.players[0].stats.trueDamageTaken = Math.Max(maximumTeam.players[0].stats.trueDamageTaken, PlayerStats.trueDamageTaken);
            maximumTeam.players[0].stats.turretKills = Math.Max(maximumTeam.players[0].stats.turretKills, PlayerStats.turretKills);
            maximumTeam.players[0].stats.unrealKills = Math.Max(maximumTeam.players[0].stats.unrealKills, PlayerStats.unrealKills);
            maximumTeam.players[0].stats.visionWardsBoughtInGame = Math.Max(maximumTeam.players[0].stats.visionWardsBoughtInGame, PlayerStats.visionWardsBoughtInGame);
            maximumTeam.players[0].stats.wardsKilled = Math.Max(maximumTeam.players[0].stats.wardsKilled, PlayerStats.wardsKilled);
            maximumTeam.players[0].stats.wardsPlaced = Math.Max(maximumTeam.players[0].stats.wardsPlaced, PlayerStats.wardsPlaced);


            minimumTeam.players[0].stats.assists = Math.Min(minimumTeam.players[0].stats.assists, PlayerStats.assists);
            minimumTeam.players[0].stats.champLevel = Math.Min(minimumTeam.players[0].stats.champLevel, PlayerStats.champLevel);
            minimumTeam.players[0].stats.damageDealtToObjectives = Math.Min(minimumTeam.players[0].stats.damageDealtToObjectives, PlayerStats.damageDealtToObjectives);
            minimumTeam.players[0].stats.damageDealtToTurrets = Math.Min(minimumTeam.players[0].stats.damageDealtToTurrets, PlayerStats.damageDealtToTurrets);
            minimumTeam.players[0].stats.damageSelfMitigated = Math.Min(minimumTeam.players[0].stats.damageSelfMitigated, PlayerStats.damageSelfMitigated);
            minimumTeam.players[0].stats.deaths = Math.Min(minimumTeam.players[0].stats.deaths, PlayerStats.deaths);
            minimumTeam.players[0].stats.doubleKills = Math.Min(minimumTeam.players[0].stats.doubleKills, PlayerStats.doubleKills);
            minimumTeam.players[0].stats.goldEarned = Math.Min(minimumTeam.players[0].stats.goldEarned, PlayerStats.goldEarned);
            minimumTeam.players[0].stats.goldSpent = Math.Min(minimumTeam.players[0].stats.goldSpent, PlayerStats.goldSpent);
            minimumTeam.players[0].stats.inhibitorKills = Math.Min(minimumTeam.players[0].stats.inhibitorKills, PlayerStats.inhibitorKills);
            minimumTeam.players[0].stats.killingSprees = Math.Min(minimumTeam.players[0].stats.killingSprees, PlayerStats.killingSprees);
            minimumTeam.players[0].stats.kills = Math.Min(minimumTeam.players[0].stats.kills, PlayerStats.kills);
            minimumTeam.players[0].stats.largestCriticalStrike = Math.Min(minimumTeam.players[0].stats.largestCriticalStrike, PlayerStats.largestCriticalStrike);
            minimumTeam.players[0].stats.largestKillingSpree = Math.Min(minimumTeam.players[0].stats.largestKillingSpree, PlayerStats.largestKillingSpree);
            minimumTeam.players[0].stats.largestMultiKill = Math.Min(minimumTeam.players[0].stats.largestMultiKill, PlayerStats.largestMultiKill);
            minimumTeam.players[0].stats.longestTimeSpentLiving = Math.Min(minimumTeam.players[0].stats.longestTimeSpentLiving, PlayerStats.longestTimeSpentLiving);
            minimumTeam.players[0].stats.magicalDamageTaken = Math.Min(minimumTeam.players[0].stats.magicalDamageTaken, PlayerStats.magicalDamageTaken);
            minimumTeam.players[0].stats.magicDamageDealt = Math.Min(minimumTeam.players[0].stats.magicDamageDealt, PlayerStats.magicDamageDealt);
            minimumTeam.players[0].stats.magicDamageDealtToChampions = Math.Min(minimumTeam.players[0].stats.magicDamageDealtToChampions, PlayerStats.magicDamageDealtToChampions);
            minimumTeam.players[0].stats.neutralMinionsKilled = Math.Min(minimumTeam.players[0].stats.neutralMinionsKilled, PlayerStats.neutralMinionsKilled);
            minimumTeam.players[0].stats.neutralMinionsKilledEnemyJungle = Math.Min(minimumTeam.players[0].stats.neutralMinionsKilledEnemyJungle, PlayerStats.neutralMinionsKilledEnemyJungle);
            minimumTeam.players[0].stats.neutralMinionsKilledTeamJungle = Math.Min(minimumTeam.players[0].stats.neutralMinionsKilledTeamJungle, PlayerStats.neutralMinionsKilledTeamJungle);
            minimumTeam.players[0].stats.pentaKills = Math.Min(minimumTeam.players[0].stats.pentaKills, PlayerStats.pentaKills);
            minimumTeam.players[0].stats.physicalDamageDealt = Math.Min(minimumTeam.players[0].stats.physicalDamageDealt, PlayerStats.physicalDamageDealt);
            minimumTeam.players[0].stats.physicalDamageDealtToChampions = Math.Min(minimumTeam.players[0].stats.physicalDamageDealtToChampions, PlayerStats.physicalDamageDealtToChampions);
            minimumTeam.players[0].stats.physicalDamageTaken = Math.Min(minimumTeam.players[0].stats.physicalDamageTaken, PlayerStats.physicalDamageTaken);
            minimumTeam.players[0].stats.quadraKills = Math.Min(minimumTeam.players[0].stats.quadraKills, PlayerStats.quadraKills);
            minimumTeam.players[0].stats.timeCCingOthers = Math.Min(minimumTeam.players[0].stats.timeCCingOthers, PlayerStats.timeCCingOthers);
            minimumTeam.players[0].stats.totalDamageDealt = Math.Min(minimumTeam.players[0].stats.totalDamageDealt, PlayerStats.totalDamageDealt);
            minimumTeam.players[0].stats.totalDamageDealtToChampions = Math.Min(minimumTeam.players[0].stats.totalDamageDealtToChampions, PlayerStats.totalDamageDealtToChampions);
            minimumTeam.players[0].stats.totalDamageTaken = Math.Min(minimumTeam.players[0].stats.totalDamageTaken, PlayerStats.totalDamageTaken);
            minimumTeam.players[0].stats.totalHeal = Math.Min(minimumTeam.players[0].stats.totalHeal, PlayerStats.totalHeal);
            minimumTeam.players[0].stats.totalMinionsKilled = Math.Min(minimumTeam.players[0].stats.totalMinionsKilled, PlayerStats.totalMinionsKilled);
            minimumTeam.players[0].stats.totalTimeCrowdControlDealt = Math.Min(minimumTeam.players[0].stats.totalTimeCrowdControlDealt, PlayerStats.totalTimeCrowdControlDealt);
            minimumTeam.players[0].stats.totalUnitsHealed = Math.Min(minimumTeam.players[0].stats.totalUnitsHealed, PlayerStats.totalUnitsHealed);
            minimumTeam.players[0].stats.tripleKills = Math.Min(minimumTeam.players[0].stats.tripleKills, PlayerStats.tripleKills);
            minimumTeam.players[0].stats.trueDamageDealt = Math.Min(minimumTeam.players[0].stats.trueDamageDealt, PlayerStats.trueDamageDealt);
            minimumTeam.players[0].stats.trueDamageDealtToChampions = Math.Min(minimumTeam.players[0].stats.trueDamageDealtToChampions, PlayerStats.trueDamageDealtToChampions);
            minimumTeam.players[0].stats.trueDamageTaken = Math.Min(minimumTeam.players[0].stats.trueDamageTaken, PlayerStats.trueDamageTaken);
            minimumTeam.players[0].stats.turretKills = Math.Min(minimumTeam.players[0].stats.turretKills, PlayerStats.turretKills);
            minimumTeam.players[0].stats.unrealKills = Math.Min(minimumTeam.players[0].stats.unrealKills, PlayerStats.unrealKills);
            minimumTeam.players[0].stats.visionWardsBoughtInGame = Math.Min(minimumTeam.players[0].stats.visionWardsBoughtInGame, PlayerStats.visionWardsBoughtInGame);
            minimumTeam.players[0].stats.wardsKilled = Math.Min(minimumTeam.players[0].stats.wardsKilled, PlayerStats.wardsKilled);
            minimumTeam.players[0].stats.wardsPlaced = Math.Min(minimumTeam.players[0].stats.wardsPlaced, PlayerStats.wardsPlaced);

        }

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
                    for (int j = i - 1; recentBlueGames.Count < 3; j--)
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
                    testCases.Add(new TestCase(recentBlueGames.ToArray(), recentRedGames.ToArray(), games[i]));
                }
            }
            //make recent games for each test case into input neurons
            CalculateTestCasesInputNeurons();
            Console.WriteLine($"Done making {testCases.Count} Test Cases.");
        }

        private void CalculateTestCasesInputNeurons()
        {
            for (int i = 0; i < testCases.Count; i++)
            {
                double[][] recentGamesNeurons = new double[6][];
                for (int j = 0; j < 3; j++)
                {
                    recentGamesNeurons[j] = SaveTeamToTeamNeurons(testCases[i].blueTeamLatestGames[j]);
                }
                for (int j = 0; j < 3; j++)
                {
                    recentGamesNeurons[j + 3] = SaveTeamToTeamNeurons(testCases[i].redTeamLatestGames[j]);
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
            if (maxValue-minValue == 0)
            { return 0; }
            return (2 * ((currentValue - minValue) / (maxValue - minValue)) - 1);
        }

        private void ConvertGames()
        {
            for (int i = 0; i < matches.Count; i++)
            {
                games.Add(GameInfoMatchToSaveGameInfoGame(matches[i]));
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
                teams.Add(uniqueTeamNames[i], uniqueTeamNameArray);
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

        private double[] SaveTeamToTeamNeurons(SaveGameInfo.Team team)
        {
            double[][] inputNeuronArray = new double[7][];
            inputNeuronArray[0] = teams[team.teamName];
            inputNeuronArray[1] = SaveTeamToMiscNeurons(team);
            for (int i = 0; i < 5; i++)
            {
                inputNeuronArray[i + 2] = SavePlayerToPlayerNeurons(team.players[i]);
            }
            return CombineArrays(inputNeuronArray);
        }

        private double[] SavePlayerToPlayerNeurons(SaveGameInfo.Player player)
        {
            return championIds[player.championId];
        }

        /*public NNTestCase SaveGameToTestCase(SaveGameInfo.Game game)
        {
            NNTestCase testCase = new NNTestCase();
            if(game.teams[0].win == true) { testCase.winningTeam = 0; }
            else { testCase.winningTeam = 1; }
            testCase.inputNeurons = SaveGameToTeamNeurons(game);
            return testCase;
        }*/

        /*public double[] SaveGameToNeurons(SaveGameInfo.Game game)
        {
            return CombineArrays(new double[][] { SaveGameToTeamNeurons(game), SaveGameToMiscNeurons(game) });
        }*/

        /*private double[] SaveGameToTeamNeurons(SaveGameInfo.Game game)
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
        }*/

        private double[] SaveTeamToMiscNeurons(SaveGameInfo.Team team)
        {
            List<double> gameData = new List<double>();
            double[] input;

            gameData.Add(Normalization(team.towerKills, minimumTeam.towerKills, maximumTeam.towerKills));
            gameData.Add(Normalization(team.inhibitorKills, minimumTeam.inhibitorKills, maximumTeam.inhibitorKills));
            gameData.Add(Normalization(team.baronKills, minimumTeam.baronKills, maximumTeam.baronKills));
            gameData.Add(Normalization(team.dragonKills, minimumTeam.dragonKills, maximumTeam.dragonKills));
            gameData.Add(Normalization(team.riftHeraldKills, minimumTeam.riftHeraldKills, maximumTeam.riftHeraldKills));

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
                gameData.Add(Normalization(player.stats.kills, minimumTeam.players[0].stats.kills, maximumTeam.players[0].stats.kills));
                gameData.Add(Normalization(player.stats.deaths, minimumTeam.players[0].stats.deaths, maximumTeam.players[0].stats.deaths));
                gameData.Add(Normalization(player.stats.assists, minimumTeam.players[0].stats.assists, maximumTeam.players[0].stats.assists));
                gameData.Add(Normalization(player.stats.largestKillingSpree, minimumTeam.players[0].stats.largestKillingSpree, maximumTeam.players[0].stats.largestKillingSpree));
                gameData.Add(Normalization(player.stats.largestMultiKill, minimumTeam.players[0].stats.largestMultiKill, maximumTeam.players[0].stats.largestMultiKill));
                gameData.Add(Normalization(player.stats.killingSprees, minimumTeam.players[0].stats.killingSprees, maximumTeam.players[0].stats.killingSprees));
                gameData.Add(Normalization(player.stats.longestTimeSpentLiving, minimumTeam.players[0].stats.longestTimeSpentLiving, maximumTeam.players[0].stats.longestTimeSpentLiving));
                gameData.Add(Normalization(player.stats.doubleKills, minimumTeam.players[0].stats.doubleKills, maximumTeam.players[0].stats.doubleKills));
                gameData.Add(Normalization(player.stats.tripleKills, minimumTeam.players[0].stats.tripleKills, maximumTeam.players[0].stats.tripleKills));
                gameData.Add(Normalization(player.stats.quadraKills, minimumTeam.players[0].stats.quadraKills, maximumTeam.players[0].stats.quadraKills));
                gameData.Add(Normalization(player.stats.pentaKills, minimumTeam.players[0].stats.pentaKills, maximumTeam.players[0].stats.pentaKills));
                gameData.Add(Normalization(player.stats.unrealKills, minimumTeam.players[0].stats.unrealKills, maximumTeam.players[0].stats.unrealKills));
                gameData.Add(Normalization(player.stats.totalDamageDealt, minimumTeam.players[0].stats.totalDamageDealt, maximumTeam.players[0].stats.totalDamageDealt));
                gameData.Add(Normalization(player.stats.magicDamageDealt, minimumTeam.players[0].stats.magicDamageDealt, maximumTeam.players[0].stats.magicDamageDealt));
                gameData.Add(Normalization(player.stats.physicalDamageDealt, minimumTeam.players[0].stats.physicalDamageDealt, maximumTeam.players[0].stats.physicalDamageDealt));
                gameData.Add(Normalization(player.stats.trueDamageDealt, minimumTeam.players[0].stats.trueDamageDealt, maximumTeam.players[0].stats.trueDamageDealt));
                gameData.Add(Normalization(player.stats.largestCriticalStrike, minimumTeam.players[0].stats.largestCriticalStrike, maximumTeam.players[0].stats.largestCriticalStrike));
                gameData.Add(Normalization(player.stats.totalDamageDealtToChampions, minimumTeam.players[0].stats.totalDamageDealtToChampions, maximumTeam.players[0].stats.totalDamageDealtToChampions));
                gameData.Add(Normalization(player.stats.magicDamageDealtToChampions, minimumTeam.players[0].stats.magicDamageDealtToChampions, maximumTeam.players[0].stats.magicDamageDealtToChampions));
                gameData.Add(Normalization(player.stats.physicalDamageDealtToChampions, minimumTeam.players[0].stats.physicalDamageDealtToChampions, maximumTeam.players[0].stats.physicalDamageDealtToChampions));
                gameData.Add(Normalization(player.stats.trueDamageDealtToChampions, minimumTeam.players[0].stats.trueDamageDealtToChampions, maximumTeam.players[0].stats.trueDamageDealtToChampions));
                gameData.Add(Normalization(player.stats.totalHeal, minimumTeam.players[0].stats.totalHeal, maximumTeam.players[0].stats.totalHeal));
                gameData.Add(Normalization(player.stats.totalUnitsHealed, minimumTeam.players[0].stats.totalUnitsHealed, maximumTeam.players[0].stats.totalUnitsHealed));
                gameData.Add(Normalization(player.stats.damageSelfMitigated, minimumTeam.players[0].stats.damageSelfMitigated, maximumTeam.players[0].stats.damageSelfMitigated));
                gameData.Add(Normalization(player.stats.damageDealtToObjectives, minimumTeam.players[0].stats.damageDealtToObjectives, maximumTeam.players[0].stats.damageDealtToObjectives));
                gameData.Add(Normalization(player.stats.damageDealtToTurrets, minimumTeam.players[0].stats.damageDealtToTurrets, maximumTeam.players[0].stats.damageDealtToTurrets));
                gameData.Add(Normalization(player.stats.timeCCingOthers, minimumTeam.players[0].stats.timeCCingOthers, maximumTeam.players[0].stats.timeCCingOthers));
                gameData.Add(Normalization(player.stats.totalDamageTaken, minimumTeam.players[0].stats.totalDamageTaken, maximumTeam.players[0].stats.totalDamageTaken));
                gameData.Add(Normalization(player.stats.magicalDamageTaken, minimumTeam.players[0].stats.magicalDamageTaken, maximumTeam.players[0].stats.magicalDamageTaken));
                gameData.Add(Normalization(player.stats.physicalDamageTaken, minimumTeam.players[0].stats.physicalDamageTaken, maximumTeam.players[0].stats.physicalDamageTaken));
                gameData.Add(Normalization(player.stats.trueDamageTaken, minimumTeam.players[0].stats.trueDamageTaken, maximumTeam.players[0].stats.trueDamageTaken));
                gameData.Add(Normalization(player.stats.goldEarned, minimumTeam.players[0].stats.goldEarned, maximumTeam.players[0].stats.goldEarned));
                gameData.Add(Normalization(player.stats.goldSpent, minimumTeam.players[0].stats.goldSpent, maximumTeam.players[0].stats.goldSpent));
                gameData.Add(Normalization(player.stats.turretKills, minimumTeam.players[0].stats.turretKills, maximumTeam.players[0].stats.turretKills));
                gameData.Add(Normalization(player.stats.inhibitorKills, minimumTeam.players[0].stats.inhibitorKills, maximumTeam.players[0].stats.inhibitorKills));
                gameData.Add(Normalization(player.stats.totalMinionsKilled, minimumTeam.players[0].stats.totalMinionsKilled, maximumTeam.players[0].stats.totalMinionsKilled));
                gameData.Add(Normalization(player.stats.neutralMinionsKilled, minimumTeam.players[0].stats.neutralMinionsKilled, maximumTeam.players[0].stats.neutralMinionsKilled));
                gameData.Add(Normalization(player.stats.neutralMinionsKilledTeamJungle, minimumTeam.players[0].stats.neutralMinionsKilledTeamJungle, maximumTeam.players[0].stats.neutralMinionsKilledTeamJungle));
                gameData.Add(Normalization(player.stats.neutralMinionsKilledEnemyJungle, minimumTeam.players[0].stats.neutralMinionsKilledEnemyJungle, maximumTeam.players[0].stats.neutralMinionsKilledEnemyJungle));
                gameData.Add(Normalization(player.stats.totalTimeCrowdControlDealt, minimumTeam.players[0].stats.totalTimeCrowdControlDealt, maximumTeam.players[0].stats.totalTimeCrowdControlDealt));
                gameData.Add(Normalization(player.stats.champLevel, minimumTeam.players[0].stats.champLevel, maximumTeam.players[0].stats.champLevel));
                gameData.Add(Normalization(player.stats.visionWardsBoughtInGame, minimumTeam.players[0].stats.visionWardsBoughtInGame, maximumTeam.players[0].stats.visionWardsBoughtInGame));
                gameData.Add(Normalization(player.stats.wardsPlaced, minimumTeam.players[0].stats.wardsPlaced, maximumTeam.players[0].stats.wardsPlaced));
                gameData.Add(Normalization(player.stats.wardsKilled, minimumTeam.players[0].stats.wardsKilled, maximumTeam.players[0].stats.wardsKilled));

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

                /*gameData.Add(player.timeline.creepsPerMinDeltas._010);
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
                gameData.Add(player.timeline.damageTakenDiffPerMinDeltas._40end);*/
            }


            input = gameData.ToArray();
            return input;

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
            for (int i = 0; i < jaggedArray.GetLength(0); i++)
            {
                for (int j = 0; j < jaggedArray[i].Length; j++, currentId++)
                {
                    returnArray[currentId] = jaggedArray[i][j];
                }
            }
            return returnArray;
        }

        SaveGameInfo.Game GameInfoMatchToSaveGameInfoGame(GameInfo.Match match)
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
                returnGame.teams[i].gameDuration = match.gameDuration;
                returnGame.teams[i].teamId = match.teams[i].teamId;
                returnGame.teams[i].players = new SaveGameInfo.Player[5];
                returnGame.teams[i].teamName = match.participantIdentities[i * 5].player.summonerName.Substring(0, match.participantIdentities[i * 5].player.summonerName.IndexOf(' '));
                returnGame.teams[i].win = match.participants[i * 5].stats.win;
                returnGame.teams[i].firstBlood = match.teams[i].firstBlood;
                returnGame.teams[i].firstTower = match.teams[i].firstTower;
                returnGame.teams[i].firstInhibitor = match.teams[i].firstInhibitor;
                returnGame.teams[i].firstBaron = match.teams[i].firstBaron;
                returnGame.teams[i].firstDragon = match.teams[i].firstDragon;
                returnGame.teams[i].firstRiftHerald = match.teams[i].firstRiftHerald;
                returnGame.teams[i].towerKills = match.teams[i].towerKills;
                returnGame.teams[i].inhibitorKills = match.teams[i].inhibitorKills;
                returnGame.teams[i].baronKills = match.teams[i].baronKills;
                returnGame.teams[i].dragonKills = match.teams[i].dragonKills;
                returnGame.teams[i].riftHeraldKills = match.teams[i].riftHeraldKills;
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
                    returnGame.teams[i].players[j].stats = GameInfoStatsToSaveGameInfoStats(match.participants[(i * 5) + j].stats);
                    returnGame.teams[i].players[j].playerId = match.participantIdentities[j].participantId;
                    returnGame.teams[i].players[j].spell1Id = match.participants[j].spell1Id;
                    returnGame.teams[i].players[j].spell2Id = match.participants[j].spell2Id;
                    //timeline here if needed
                }
            }
            return returnGame;
        }

        SaveGameInfo.Stats GameInfoStatsToSaveGameInfoStats(GameInfo.Stats stats)
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

        private double[] TeamToDoubleArray(SaveGameInfo.Team team)
        {
            
            double[][] NameOfPlayers = new double[5][];
            int[] IndexesOfPlayers = new int[5];
            double[] ReturnArray = new double[NameOfPlayers[0].Length];

            // Finding the arrays with players in the dictionary
            for (int index = 0; index < team.players.Length; index++)
            {
                NameOfPlayers[index] = playerNames[team.players[index].summonerName];
            }

            // Finding the index of each player in the current player array
            for (int currPlayer = 0; currPlayer < NameOfPlayers.GetLength(0); currPlayer++)
            {
                for (int index = 0; index < NameOfPlayers.GetLength(1); index++)
                {
                    if (NameOfPlayers[currPlayer][index] != 0)
                    {
                        IndexesOfPlayers[currPlayer] = index;
                    }
                }
            }

            // Setting the indexes for all players into the return array
            for (int i = 0; i < IndexesOfPlayers.Length; i++)
            {
                ReturnArray[IndexesOfPlayers[i]] = 1; 
            }

            return ReturnArray;
        }
    }
}

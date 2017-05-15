using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonReader
{
    public class InputNeuronTestLayer
    {
        public SaveGameInfo.Game game { set; get; }
        public int win { set; get; }
    }

    public class SaveGameInfo
    {
        public class Game
        {
            public long gameCreation { get; set; }
            public int gameDuration { get; set; }
            public string gameVersion { get; set; }
            public Team[] teams { get; set; }
        }

        public class Team
        {
            public int teamId { get; set; }
            public string teamName { get; set; }
            public bool win { get; set; }
            public bool firstBlood { get; set; }
            public bool firstTower { get; set; }
            public bool firstInhibitor { get; set; }
            public bool firstBaron { get; set; }
            public bool firstDragon { get; set; }
            public bool firstRiftHerald { get; set; }
            public int towerKills { get; set; }
            public int inhibitorKills { get; set; }
            public int baronKills { get; set; }
            public int dragonKills { get; set; }
            public int riftHeraldKills { get; set; }
            public int[] bannedChampionId { get; set; }
            public Player[] players { get; set; }
        }

        public class Player
        {
            public string summonerName { get; set; }
            public int playerId { get; set; }
            public int teamId { get; set; }
            public int championId { get; set; }
            public int spell1Id { get; set; }
            public int spell2Id { get; set; }
            public Stats stats { get; set; }
            public Timeline timeline { get; set; }
        }

        public class Stats
        {
            public int participantId { get; set; }
            public bool win { get; set; }
            public int kills { get; set; }
            public int deaths { get; set; }
            public int assists { get; set; }
            public int largestKillingSpree { get; set; }
            public int largestMultiKill { get; set; }
            public int killingSprees { get; set; }
            public int longestTimeSpentLiving { get; set; }
            public int doubleKills { get; set; }
            public int tripleKills { get; set; }
            public int quadraKills { get; set; }
            public int pentaKills { get; set; }
            public int unrealKills { get; set; }
            public int totalDamageDealt { get; set; }
            public int magicDamageDealt { get; set; }
            public int physicalDamageDealt { get; set; }
            public int trueDamageDealt { get; set; }
            public int largestCriticalStrike { get; set; }
            public int totalDamageDealtToChampions { get; set; }
            public int magicDamageDealtToChampions { get; set; }
            public int physicalDamageDealtToChampions { get; set; }
            public int trueDamageDealtToChampions { get; set; }
            public int totalHeal { get; set; }
            public int totalUnitsHealed { get; set; }
            public int damageSelfMitigated { get; set; }
            public int damageDealtToObjectives { get; set; }
            public int damageDealtToTurrets { get; set; }
            public int timeCCingOthers { get; set; }
            public int totalDamageTaken { get; set; }
            public int magicalDamageTaken { get; set; }
            public int physicalDamageTaken { get; set; }
            public int trueDamageTaken { get; set; }
            public int goldEarned { get; set; }
            public int goldSpent { get; set; }
            public int turretKills { get; set; }
            public int inhibitorKills { get; set; }
            public int totalMinionsKilled { get; set; }
            public int neutralMinionsKilled { get; set; }
            public int neutralMinionsKilledTeamJungle { get; set; }
            public int neutralMinionsKilledEnemyJungle { get; set; }
            public int totalTimeCrowdControlDealt { get; set; }
            public int champLevel { get; set; }
            public int visionWardsBoughtInGame { get; set; }
            public int wardsPlaced { get; set; }
            public int wardsKilled { get; set; }
            public bool firstBloodKill { get; set; }
            public bool firstBloodAssist { get; set; }
            public bool firstTowerKill { get; set; }
            public bool firstTowerAssist { get; set; }
            public bool firstInhibitorKill { get; set; }
            public bool firstInhibitorAssist { get; set; }
        }

        public class Timeline
        {
            public int participantId { get; set; } //1 = top, 2 = jungle, 3 = mid, 4 = adc, 5 = support
            public Differencial creepsPerMinDeltas { get; set; }
            public Differencial xpPerMinDeltas { get; set; }
            public Differencial goldPerMinDeltas { get; set; }
            public Differencial csDiffPerMinDeltas { get; set; }
            public Differencial xpDiffPerMinDeltas { get; set; }
            public Differencial damageTakenPerMinDeltas { get; set; }
            public Differencial damageTakenDiffPerMinDeltas { get; set; }
            
            public string role { get; set; } //figure out what to do with this
            public string lane { get; set; } //this too.
        }

        public class Differencial
        {
            //has diffrent amount of variables based on time.
            public float _40end { get; set; }
            public float _3040 { get; set; }
            public float _1020 { get; set; }
            public float _2030 { get; set; }
            public float _010 { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNeuralNetwork
{
    public class SaveGameInfo
    {
        public class Game : IComparable<Game>
        {
            public long gameCreation { get; set; }
            public int gameDuration { get; set; }
            public string gameVersion { get; set; }
            public Team[] teams { get; set; }
            
            public int CompareTo(Game other)
            {
                if (other == null) { return 1; }
                return this.gameCreation.CompareTo(other.gameCreation);
            }

            public bool Equals(Game other)
            {
                return this.gameCreation == other.gameCreation;
            }
        }

        public class Team
        {
            public double gameDuration { get; set; }
            public int teamId { get; set; }
            public string teamName { get; set; }
            public bool win { get; set; }
            public bool firstBlood { get; set; }
            public bool firstTower { get; set; }
            public bool firstInhibitor { get; set; }
            public bool firstBaron { get; set; }
            public bool firstDragon { get; set; }
            public bool firstRiftHerald { get; set; }
            public double towerKills { get; set; }
            public double inhibitorKills { get; set; }
            public double baronKills { get; set; }
            public double dragonKills { get; set; }
            public double riftHeraldKills { get; set; }
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
            public double kills { get; set; }
            public double deaths { get; set; }
            public double assists { get; set; }
            public double largestKillingSpree { get; set; }
            public double largestMultiKill { get; set; }
            public double killingSprees { get; set; }
            public double longestTimeSpentLiving { get; set; }
            public double doubleKills { get; set; }
            public double tripleKills { get; set; }
            public double quadraKills { get; set; }
            public double pentaKills { get; set; }
            public double unrealKills { get; set; }
            public double totalDamageDealt { get; set; }
            public double magicDamageDealt { get; set; }
            public double physicalDamageDealt { get; set; }
            public double trueDamageDealt { get; set; }
            public double largestCriticalStrike { get; set; }
            public double totalDamageDealtToChampions { get; set; }
            public double magicDamageDealtToChampions { get; set; }
            public double physicalDamageDealtToChampions { get; set; }
            public double trueDamageDealtToChampions { get; set; }
            public double totalHeal { get; set; }
            public double totalUnitsHealed { get; set; }
           // public double damageSelfMitigated { get; set; }
            //public double damageDealtToObjectives { get; set; }
            //public double damageDealtToTurrets { get; set; }
            //public double timeCCingOthers { get; set; }
            public double totalDamageTaken { get; set; }
            public double magicalDamageTaken { get; set; }
            public double physicalDamageTaken { get; set; }
            public double trueDamageTaken { get; set; }
            public double goldEarned { get; set; }
            public double goldSpent { get; set; }
            public double turretKills { get; set; }
            public double inhibitorKills { get; set; }
            public double totalMinionsKilled { get; set; }
            public double neutralMinionsKilled { get; set; }
            public double neutralMinionsKilledTeamJungle { get; set; }
            public double neutralMinionsKilledEnemyJungle { get; set; }
            public double totalTimeCrowdControlDealt { get; set; }
            public double champLevel { get; set; }
            public double visionWardsBoughtInGame { get; set; }
            public double wardsPlaced { get; set; }
            public double wardsKilled { get; set; }
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
            public float _50end { get; set; }
            public float _4050 { get; set; }
            public float _3040 { get; set; }
            public float _1020 { get; set; }
            public float _2030 { get; set; }
            public float _010 { get; set; }
        }
    }
}

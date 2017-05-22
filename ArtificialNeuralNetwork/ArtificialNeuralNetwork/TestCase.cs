using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNeuralNetwork
{
    public class TestCase
    {
        public TestCase(SaveGameInfo.Team[] blueRecent, SaveGameInfo.Team[] redRecent, SaveGameInfo.Game current)
        {
            actualGame = current;
            if (current.teams[0].win)
            { winningTeam = 1; }
            else
            { winningTeam = 0; }
            blueTeamLatestGames = blueRecent;
            redTeamLatestGames = redRecent;
        }

        public TestCase(SaveGameInfo.Team[] blueRecent, SaveGameInfo.Team[] redRecent)
        {
            blueTeamLatestGames = blueRecent;
            redTeamLatestGames = redRecent;
        }

        public SaveGameInfo.Game actualGame;
        public SaveGameInfo.Team[] blueTeamLatestGames;
        public SaveGameInfo.Team[] redTeamLatestGames;
        public double[] inputNeurons { set; get; }
        public int winningTeam { set; get; }
    }
}

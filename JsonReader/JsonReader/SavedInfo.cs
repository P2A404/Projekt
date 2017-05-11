using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonReader
{

    class Team
    {
        public Team (string teamName, int[] teamNeuronInput)
        {
            TeamName = teamName;
            TeamNeuronInput = teamNeuronInput;
        }
        public string TeamName { get; set; }
        public int[] TeamNeuronInput { get; set; }
        SaveGameInfo.Game[] lastest3Games = new SaveGameInfo.Game[5];
        SaveGameInfo.Game averageGame = new SaveGameInfo.Game();
        //Dictionary<string, SaveGameInfo> vsGames = new Dictionary<string, SaveGameInfo>();
    }
}

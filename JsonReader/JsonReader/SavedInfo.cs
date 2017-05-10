using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonReader
{
    //might not be the best
    enum teamName
    { }

    class Team
    {
        string TeamName { get; set; }
        SaveGameInfo[] lastestGames;
        Dictionary<string, SaveGameInfo> vsGames = new Dictionary<string, SaveGameInfo>();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonReader
{
    class NNInputFormatter
    {
        public List<SaveGameInfo.Game> games = new List<SaveGameInfo.Game>();
        public List<GameInfo.Match> matches = new List<GameInfo.Match>();
        public Dictionary<int, int[]> championIds = new Dictionary<int, int[]>();

        public double Normalization (double currentValue, double minValue, double maxValue)
        {
            return (2*((currentValue - minValue) / (maxValue - minValue))-1);
        }

        public void ConvertGames ()
        {
            foreach(GameInfo.Match match in matches)
            {
                games.Add(GameToSaveGame(match));
            }
            matches = null;
        }

        public void LoadChampionIdDictionary ()
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
                int[] WilhelmArray = new int[uniqueChampionId.Count];
                Array.Clear(WilhelmArray, 0, uniqueChampionId.Count);
                WilhelmArray[i] = 1;
                championIds.Add(uniqueChampionId[i], WilhelmArray);
            }
        }

        double[] SaveGameToInputNeurons(SaveGameInfo.Game game)
        {
            
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
                for (int j = 0; i < returnGame.teams[i].players.Length; i++)
                {
                    returnGame.teams[i].players[j] = new SaveGameInfo.Player();
                    returnGame.teams[i].players[j].championId = game.participants[(i * 5) + j].championId;
                }
            }

            return null;
        }
    }
}

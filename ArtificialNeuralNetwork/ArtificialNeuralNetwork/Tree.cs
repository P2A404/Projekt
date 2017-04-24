using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNeuralNetwork
{
    class Tree
    {
        double[] GetChances(int BestOf, int HandicapHome, int HandicapOut, double ChanceHome, double ChanceOut)
        {
            double homeChance = 0, outChance = 0, drawChance = 0;
            int TempBestOf = BestOf + HandicapHome + HandicapOut;
            int PseudoRound = HandicapOut + HandicapHome;
            homeChance = GetSingleTeamChance(HandicapHome, HandicapOut, TempBestOf, ChanceHome, ChanceOut, PseudoRound);
            outChance = GetSingleTeamChance(HandicapOut, HandicapHome, TempBestOf, ChanceOut, ChanceHome, PseudoRound);
            drawChance = 1 - homeChance - outChance;
            double[] Chances = new double[3] { homeChance, outChance, drawChance };
            return Chances;
        }

        double GetSingleTeamChance(int HomeWon, int OutWon, int BestOf, double ChanceHome, double ChanceOut, int Round)
        {
            if (HomeWon == (BestOf/2) + 1)
            { return 1; }
            else if (OutWon == (BestOf / 2) + 1 || Round == BestOf)
            { return 0; }
            else
            {
                double ReturnValue = ChanceHome * GetSingleTeamChance(HomeWon + 1 , OutWon, BestOf, ChanceHome, ChanceOut, Round)
                    + ChanceOut * GetSingleTeamChance(HomeWon, OutWon + 1, BestOf, ChanceHome, ChanceOut, Round);
            }
        }
    }
}

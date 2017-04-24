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
            double HomeChance = 0;
            double OutChance = 0;
            double DrawChance = 0;
            int TempBestOf = BestOf + HandicapHome + HandicapOut;
            int PseudoRound = HandicapOut + HandicapHome;
            HomeChance = GetSingleTeamChance(HandicapHome, HandicapOut, TempBestOf, ChanceHome, ChanceOut, PseudoRound);

            OutChance = GetSingleTeamChance(HandicapOut, HandicapHome, TempBestOf, ChanceOut, ChanceHome, PseudoRound);

            DrawChance = 1 - HomeChance - OutChance;

            double[] Chances = new double[3];
            Chances[0] = HomeChance;
            Chances[1] = OutChance;
            Chances[2] = DrawChance;

            return Chances;
        }

        double GetSingleTeamChance(int HomeWon, int OutWon, int BestOf, double ChanceHome, double ChanceOut, int Round)
        {
            if (HomeWon == (BestOf/2) + 1) { return 1; }
            else if (OutWon == (BestOf / 2) + 1 || Round == BestOf) { return 0; }
            else
            {
                double ReturnValue = ChanceHome * GetSingleTeamChance(HomeWon + 1 , OutWon, BestOf, ChanceHome, ChanceOut, Round)
                    + ChanceOut * GetSingleTeamChance(HomeWon + 1, OutWon, BestOf, ChanceHome, ChanceOut, Round);
                return ReturnValue;
            }
        }
    }
}

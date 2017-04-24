using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNeuralNetwork
{

    class Team
    {
        public Team (string teamName)
        {
            _teamName = teamName;
        }
        string _teamName;
    }

    class Match
    {
        public Match(Team firstTeam, Team secondTeam, Team winnerTeam)
        {
            _firstTeam = firstTeam;
            _secondTeam = secondTeam;
            _winnerTeam = winnerTeam;
        }
        Team _firstTeam;
        Team _secondTeam;
        Team _winnerTeam;
    }

    class Program
    {
        static void Main(string[] args)
        {
            #region start
            Console.Clear();
            Console.WriteLine("Hello World!");
            #endregion
            List<Team> teams = new List<Team>();
            teams.Add(new Team("ELE"));
            teams.Add(new Team("FNC"));
            teams.Add(new Team("G2"));
            teams.Add(new Team("GIA"));
            teams.Add(new Team("H2K"));
            teams.Add(new Team("OG"));
            teams.Add(new Team("ROC"));
            teams.Add(new Team("S04"));
            teams.Add(new Team("SPY"));
            teams.Add(new Team("UOL"));
            teams.Add(new Team("VIT"));
            TransferFunctions tf = new TransferFunctions();
            NeuralNetwork NN1 = new NeuralNetwork(new int[] { 2, 3, 3, 2 }, tf.Logistic, tf.SoftMax, tf.LogistikDerivative, tf.LogistikDerivative);
            NN1.PrintArray("Cycle:", NN1.Cycle(new double[] { 33.99521, 47.5 }));
            #region end
            Console.WriteLine("Goodbye Cruel World.");
            Console.ReadLine();
            #endregion
        }
    }
}

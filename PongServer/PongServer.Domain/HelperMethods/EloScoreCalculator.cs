using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PongServer.Domain.Enums;

namespace PongServer.Domain.HelperMethods
{
    public static class EloScoreCalculator
    {
        public static int CalculateScore(GameResult gameResult, int playerScore, int opponentScore)
        {
            var k = 32;
            if (playerScore > 2400)
            {
                k = 16;
            }

            return (int)Math.Round(playerScore + k * ((int)gameResult - CalculateWinChance(playerScore, opponentScore)), 0);
        }

        public static double CalculateWinChance(int playerScore, int opponentScore)
        {
            return 1.0 / (1.0 + Math.Pow(10, (playerScore - opponentScore) / 400.0));
        }
    }
}

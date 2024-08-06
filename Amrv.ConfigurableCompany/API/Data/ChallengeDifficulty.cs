using System;
using System.Collections.Generic;
using System.Text;

namespace Amrv.ConfigurableCompany.API.Data
{
    public enum ChallengeDifficulty
    {
        EASY = 0,
        MEDIUM = 1,
        HARD = 2,
        EXPERT = 3,
    }

    public static class ChallengeDifficultyExtensions
    {
        public static ChallengeDifficulty FromNumber(int number)
        {
            return (ChallengeDifficulty)(Math.Abs(number) % 4);
        }
    }
}

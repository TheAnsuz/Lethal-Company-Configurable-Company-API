using System;
using System.Globalization;

namespace Amrv.ConfigurableCompany.API.Data
{
    public readonly struct InfoChallenge
    {
        public static readonly InfoChallenge Default = new(new DateTime(ticks: 1234567890, DateTimeKind.Utc));

        public readonly int Week;
        public readonly int Year;
        public readonly int Month;
        public readonly bool LeapYear;
        public readonly ChallengeDifficulty Difficulty;
        public readonly float YearProgress;
        public readonly RNGProvider ExtraRandom;

        public InfoChallenge(DateTime date)
        {
            Year = date.Year;
            Month = date.Month;
            Week = ISOWeek.GetWeekOfYear(date);
            LeapYear = DateTime.IsLeapYear(Year);
            YearProgress = Week / ISOWeek.GetWeeksInYear(Year);
            Difficulty = ChallengeDifficultyExtensions.FromNumber(Week);
            ExtraRandom = new RNGProvider(Week * Year);
        }
    }
}
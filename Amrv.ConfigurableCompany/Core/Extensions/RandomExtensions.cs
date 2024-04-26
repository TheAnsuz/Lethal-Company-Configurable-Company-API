using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Amrv.ConfigurableCompany.Core.Extensions
{
    public static class RandomExtensions
    {
        //returns a uniformly random ulong between ulong.Min inclusive and ulong.Max inclusive
        public static ulong NextULong(this Random rng)
        {
            byte[] buf = new byte[8];
            rng.NextBytes(buf);
            return BitConverter.ToUInt64(buf, 0);
        }

        //returns a uniformly random ulong between ulong.Min and Max without modulo bias
        public static ulong NextULong(this Random rng, ulong max, bool inclusiveUpperBound = false)
        {
            return rng.NextULong(ulong.MinValue, max, inclusiveUpperBound);
        }

        //returns a uniformly random ulong between Min and Max without modulo bias
        public static ulong NextULong(this Random rng, ulong min, ulong max, bool inclusiveUpperBound = false)
        {
            ulong range = max - min;

            if (inclusiveUpperBound)
            {
                if (range == ulong.MaxValue)
                {
                    return rng.NextULong();
                }

                range++;
            }

            if (range <= 0)
            {
                throw new ArgumentOutOfRangeException("Max must be greater than min when inclusiveUpperBound is false, and greater than or equal to when true", "max");
            }

            ulong limit = ulong.MaxValue - ulong.MaxValue % range;
            ulong r;
            do
            {
                r = rng.NextULong();
            } while (r > limit);

            return r % range + min;
        }

        //returns a uniformly random long between long.Min inclusive and long.Max inclusive
        public static long NextLong(this Random rng)
        {
            byte[] buf = new byte[8];
            rng.NextBytes(buf);
            return BitConverter.ToInt64(buf, 0);
        }

        //returns a uniformly random long between long.Min and Max without modulo bias
        public static long NextLong(this Random rng, long max, bool inclusiveUpperBound = false)
        {
            return rng.NextLong(long.MinValue, max, inclusiveUpperBound);
        }

        //returns a uniformly random long between Min and Max without modulo bias
        public static long NextLong(this Random rng, long min, long max, bool inclusiveUpperBound = false)
        {
            ulong range = (ulong)(max - min);

            if (inclusiveUpperBound)
            {
                if (range == ulong.MaxValue)
                {
                    return rng.NextLong();
                }

                range++;
            }

            if (range <= 0)
            {
                throw new ArgumentOutOfRangeException("Max must be greater than min when inclusiveUpperBound is false, and greater than or equal to when true", "max");
            }

            ulong limit = ulong.MaxValue - ulong.MaxValue % range;
            ulong r;
            do
            {
                r = rng.NextULong();
            } while (r > limit);
            return (long)(r % range + (ulong)min);
        }

        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static bool NextBool(this Random random)
        {
            return random.Next() % 2 == 0;
        }

        public static long Gaussian(this Random random, long expected, double deviation)
        {
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();
            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return (long)(y1 * deviation + expected);
        }

        public static double Gaussian(this Random random, double expected, double deviation)
        {
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();
            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return y1 * deviation + expected;
        }
    }
}

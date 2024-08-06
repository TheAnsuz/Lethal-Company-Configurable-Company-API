using System;
using System.Collections.Generic;
using System.Linq;

namespace Amrv.ConfigurableCompany.API
{
    public sealed class RNGProvider
    {
        public static readonly RNGProvider Static = new();

        private bool _useAlpha = false;
        public readonly int SeedAlpha;
        public readonly int SeedBeta;
        public readonly Random RandomAlpha;
        public readonly Random RandomBeta;

        private Random Random
        {
            get
            {
                _useAlpha = !_useAlpha;
                return _useAlpha ? RandomBeta : RandomAlpha;
            }
        }

        public RNGProvider() : this(new Random()) { }

        public RNGProvider(Random random)
        {
            SeedAlpha = random.Next();
            SeedBeta = random.Next();
            RandomAlpha = new(SeedAlpha);
            RandomBeta = new(SeedBeta);
            _useAlpha = SeedAlpha > SeedBeta;
        }

        public RNGProvider(int alpha, int beta)
        {
            SeedAlpha = alpha;
            SeedBeta = beta;
            RandomAlpha = new(SeedAlpha);
            RandomBeta = new(SeedBeta);
            _useAlpha = SeedAlpha > SeedBeta;
        }

        public RNGProvider(int seed) : this(seed, ~seed) { }

        public RNGProvider(long seed) : this((int)(seed & 0x00000000FFFFFFFF), (int)(seed >> 32)) { }

        public string String() => String(Random.Next());
        public string String(IEnumerable<char> posibleChars) => String(posibleChars.ToArray(), Random.Next());
        public string String(IEnumerable<char> posibleChars, int length) => String(posibleChars.ToArray(), length);
        public string String(char[] posibleChars) => String(posibleChars, Random.Next());
        public string String(char[] posibleChars, int length)
        {
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
                chars[i] = posibleChars[Random.Next(posibleChars.Length)];
            return new string(chars);
        }
        public string String(int length)
        {
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
                chars[i] = (char)Random.Next(char.MinValue, char.MaxValue);
            return new string(chars);
        }

        public bool Bool() => Random.Next() % 2 == 0;
        public bool Bool(int trueWeight, int falseWeight) => Random.Next(trueWeight + falseWeight) < trueWeight;

        public byte Byte() => Byte(1)[0];
        public byte[] Byte(int amount)
        {
            byte[] buffer = new byte[amount];
            Random.NextBytes(buffer);
            return buffer;
        }

        public T Value<T>(params T[] items) => items[Random.Next(items.Length)];

        public int IndexProbability(params float[] probabilities)
        {
            double roll = Random.NextDouble() * probabilities.Sum();

            double accumulated = 0;
            for (int i = 0; i < probabilities.Length; i++)
            {
                accumulated += probabilities[i];
                if (roll < accumulated)
                    return i;
            }

            return 0;
        }

        public int IndexWeight(params int[] weights)
        {
            int value = Random.Next(weights.Sum());

            for (int i = 0; i < weights.Length; i++)
            {
                value -= weights[i];
                if (value < 0)
                    return i;
            }
            return 0;
        }

        public int Int() => Random.Next();
        public int Int(int max) => Random.Next(max);
        public int Int(int min, int max) => Random.Next(min, max);

        public ulong ULong() => BitConverter.ToUInt64(Byte(8), 0);
        public ulong ULong(ulong max) => ULong(0, max);
        public ulong ULong(ulong min, ulong max)
        {
            if (min > max)
                (max, min) = (min, max);

            ulong range = max - min + 1;

            if (range == 0)
                return min;

            return ULong() % range + min;
        }

        public long Long() => BitConverter.ToInt64(Byte(8), 0);
        public long Long(long max) => Long(0, max);
        public long Long(long min, long max)
        {
            ulong rmin = (ulong)min;
            ulong rmax = (ulong)max;

            if (min > max)
                (max, min) = (min, max);
            ulong range = (ulong)(max - min) + 1;

            if (range == 0)
                return min;

            return (long)((ULong() % range) + (ulong)min);
        }

        public float Float() => (float)(Random.NextDouble() * float.MaxValue);
        public float FloatUnit() => (float)Random.NextDouble();
        public float Float(float max) => (float)(Random.NextDouble() * max);
        public float Float(float min, float max) => (float)(Random.NextDouble() * (max - min) + min);

        public double Double() => Random.NextDouble() * double.MaxValue;
        public double DoubleUnit() => Random.NextDouble();
        public double Double(double max) => Random.NextDouble() * max;
        public double Double(double min, double max) => Random.NextDouble() * (max - min) + min;

        public decimal Decimal() => (decimal)Random.NextDouble() * decimal.MaxValue;
        public decimal DecimalUnit() => (decimal)Random.NextDouble();
        public decimal Decimal(decimal max) => (decimal)Random.NextDouble() * max;
        public decimal Decimal(decimal min, decimal max) => (decimal)Random.NextDouble() * (max - min) + min;

        public double DistributionNormalUnit() => Math.Sqrt(-2.0 * Math.Log(1 - Random.NextDouble())) * Math.Cos(2.0 * Math.PI * Random.NextDouble());
        public double DistributionNormal(double mean, double deviation) => DistributionNormalUnit() * deviation + mean;

        public double DistributionBeta(double alpha, double beta)
        {
            double rnd = Random.NextDouble();
            double x = Math.Pow(rnd, 1 / Math.Abs(alpha));
            double y = Math.Pow(1 - rnd, 1 / Math.Abs(beta));
            return x / (x + y);
        }

        public double DistributionBetaNoncentral(double alpha, double beta, double lambda1, double lambda2)
        {
            double rnd = Random.NextDouble();
            double x = Math.Pow(rnd, 1 / Math.Abs(alpha)) * Math.Exp(lambda1);
            double y = Math.Pow(1 - rnd, 1 / Math.Abs(beta)) * Math.Exp(lambda2);
            return x / (x + y);
        }

        public double DistributionBiased(double min, double max, double normal, double variation) => DistributionBiased(min, max, normal, variation, variation);
        public double DistributionBiased(double min, double max, double normal, double alpha, double beta)
        {
            double scaled = (normal - min) / (max - min);
            Console.WriteLine($"min: {min}");
            Console.WriteLine($"max: {max}");
            Console.WriteLine($"normal: {normal}");
            Console.WriteLine($"Status: min < max {min < max} | normal > min {normal > min} | normal < max {normal < max} | scale: {normal - Math.Truncate(normal)}");
            double unit = DistributionBetaNoncentral(alpha, beta, scaled, 1 - scaled);
            return unit * (max - min) + min;
        }

        public double DistributionSkewedNormal(double mean, double deviation, double skewness)
        {
            double u1 = Random.NextDouble();
            double u2 = Random.NextDouble();
            double z0 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);

            if (skewness != 0)
                z0 += skewness * (Math.Abs(z0) * z0);

            return mean + deviation * z0;
        }

        public static implicit operator RNGProvider(Random random)
        {
            return new RNGProvider(random);
        }
    }
}

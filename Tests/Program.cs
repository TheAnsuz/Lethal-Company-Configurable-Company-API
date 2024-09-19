using Amrv.ConfigurableCompany.API;

namespace Tests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            int a = 0b1111_1100_1010;
            int b = 0b0000_0011_0101;
            RNGProvider provider = new RNGProvider(a, b);
            Console.WriteLine(provider.SeedLong);
        }

        private static long join(int a, int b)
        {
            return ((long)a << 32) | b;
        }
    }
}

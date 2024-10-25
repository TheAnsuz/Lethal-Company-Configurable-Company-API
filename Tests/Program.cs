using Amrv.ConfigurableCompany.API;

namespace Tests
{
    internal class Program
    {
        private static void Main()
        {
            int a = 0b1111_1100_1010;
            int b = 0b0000_0011_0101;
            RNGProvider provider = new(a, b);
            Console.WriteLine(provider.SeedLong);
            Console.WriteLine(Join(a, b));
        }

        private static long Join(int a, int b)
        {
            return ((long)a << 32) | (long)b;
        }
    }
}

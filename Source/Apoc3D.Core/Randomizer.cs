using System;
using System.Collections.Generic;
using System.Text;

namespace Apoc3D
{
    public static class Randomizer
    {
        static Random random = new Random();

        public static bool GetRandomBool()
        {
            return GetRandomInt(int.MaxValue) % 2 == 0;
        }
        public static int GetRandomInt(int max)
        {
            return random.Next(max);
        }

        public static float GetRandomSingle()
        {
            return (float)random.NextDouble();
        }
    }
}

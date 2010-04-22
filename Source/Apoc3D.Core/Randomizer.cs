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

        public unsafe static int Random(float* p, int count)
        {
            float total = 0;
            for (int i = 0; i < count; i++)
            {
                total += p[i];
            }

            float rnd = GetRandomSingle() * total;

            float cmp = 0;
            for (int i = 0; i < count; i++)
            {
                cmp += p[i];
                if (rnd < cmp)
                {
                    return i;
                }
            }
            return 0;
        }

        public static int Random(float[] p)
        {
            float total = 0;
            for (int i = 0; i < p.Length; i++)
            {
                total += p[i];
            }

            float rnd = GetRandomSingle() * total;

            float cmp = 0;
            for (int i = 0; i < p.Length; i++)
            {
                cmp += p[i];
                if (rnd < cmp)
                {
                    return i;
                }                
            }
            return 0;
        }
    }
}

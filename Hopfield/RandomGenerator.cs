using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Hopfield
{
    // Генератор случайного числа от даты и времени для зашумления
    public static class RandomGenerator
    {
        private static Random rand = new Random(DateTime.Now.Millisecond);

        public static bool Match(int probability)
        {
            return rand.Next(100) < probability;
        }

        public static int Next(int limit)
        {
            return rand.Next(limit);
        }

        public static Point NextPoint(int limit)
        {
            return new Point { X = Next(limit), Y = Next(limit) };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSP.Util
{
    public static class ArrayExtensions
    {
        public static T[] Shuffle<T>(this T[] array)
        {
            Random random = new Random();
            return array.OrderBy(e => random.Next()).ToArray();
        }

        public static void Fill<T>(ref T[] array, T value)
        {
            for(int i = 0; i<array.Length;i++)
            {
                array[i] = value;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace HackerRank
{
    public static  class DoubleonMatch
    {
        public static void Run()
        {
            Console.WriteLine("Enter the size of the array");
            int size = Convert.ToInt32(Console.ReadLine());
            List<long> arr = new List<long>();            
            for (int i = 0; i < size; i++)
            {
                arr.Add(Convert.ToInt32(Console.ReadLine()));
            }
            Console.WriteLine("Enter the number");
            long num = Convert.ToInt32(Console.ReadLine());

            var res = doubleSize(arr, num);
            Console.WriteLine("Result: " + res);
        }

        public static long doubleSize(List<long> arr, long b)
        {
            if (b == 0)
                return 0;
           arr.Sort();
            for (int i = 0; i < arr.Count; i++)
                if (arr[i] == b)
                    b *= 2;
           return b;
        }
    }
}

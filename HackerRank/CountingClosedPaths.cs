using System;
using System.Collections.Generic;
using System.Text;

namespace HackerRank
{
    public static class CountingClosedPaths
    {
        public static void Run()
        {
            Console.WriteLine("Enter the number:");
            int num = Convert.ToInt32(Console.ReadLine());
            int res = closedPaths(num);
            Console.WriteLine("Result: " + res);

        }

        public static int closedPaths(int number)
        {
            int result = 0;
            if (number < 1 || number > Math.Pow(10, 9))
                return result;


            List<int> singleClosedPathNumbers = new List<int>() { 0, 4, 6, 9 };

            while (number > 0)
            {
                var rem = number % 10;
                if (singleClosedPathNumbers.Contains(rem))
                    result += 1;
                else if (rem == 8)
                    result += 2;
                number = number / 10;
            }

            return result;
        }
    }
}

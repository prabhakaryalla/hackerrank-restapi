using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;

namespace HackerRank
{
    public static class CountingValleys
    {

        public static void Run()
        {
            Console.WriteLine("Enter the steps");
            int steps = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the path");
            string path = Console.ReadLine();
            var res = countingValleys(steps, path);
            Console.WriteLine("Result: " + res);
        }

        /*
     * Complete the 'countingValleys' function below.
     *
     * The function is expected to return an INTEGER.
     * The function accepts following parameters:
     *  1. INTEGER steps
     *  2. STRING path
     */

        public static int countingValleys(int steps, string path)
        {
            int countingValleys = 0;
            int mountainCount = 0;
            int valleyCount = 0;
            char lastStep = 'E';

            foreach (char c in path)
            {
                if(lastStep == 'E')
                {
                    if (c == 'U')
                        mountainCount += 1;
                    else
                        valleyCount += 1;
                }
                else if(lastStep == 'U')
                {
                    if (c == 'U')
                        mountainCount += 1;
                    else
                        mountainCount -= 1;
                }
                else if (lastStep == 'D')
                {
                    if (c == 'D')
                        valleyCount += 1;
                    else
                        valleyCount -= 1;

                    if (valleyCount == 0)
                    {
                        countingValleys += 1;
                    }
                }

                if (lastStep == 'E')
                {
                    lastStep = c;
                }
                else if ((lastStep == 'U' && mountainCount == 0 && valleyCount == 0) || (lastStep == 'D' && valleyCount == 0 && mountainCount == 0))
                {
                    lastStep = 'E';
                }
            }

            return countingValleys;
        }
    }
}

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
    public static class GreedyAlgorithmnMaxMin
    {
        /*
     * Complete the 'maxMin' function below.
     *
     * The function is expected to return an INTEGER.
     * The function accepts following parameters:
     *  1. INTEGER k
     *  2. INTEGER_ARRAY arr
     */

        /// WRONG ANSWER
        /// expected Answer : 1345 somthing but not 2000
        public static int MaxMin(int k, List<int> arr)
        {
            int res = 0;
            arr.Sort();
            List<int> possibleunfairness = arr.Take(k).ToList();
            res = possibleunfairness[k-1] - possibleunfairness[0];
            return res;

        }
    }
}

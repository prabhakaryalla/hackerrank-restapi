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
    public static class JumpingOnClouds
    {
        /*
     * Complete the 'jumpingOnClouds' function below.
     *
     * The function is expected to return an INTEGER.
     * The function accepts INTEGER_ARRAY c as parameter.
     */

        public static int jumpingOnClouds(List<int> c)
        {
            int res = 0;

            for (int i=0; i< c.Count; i++)
            {
                if(i+1 <= c.Count -1 && i + 2 <= c.Count -1 && c[i+1] == 0  && c[i+2] == 0)
                {
                    i = i + 1;
                }
                else if(i + 1 <= c.Count && c[i+1] == 1)
                {
                    i = i + 1;
                }
                res += 1;
                if (i + 1 == c.Count - 1)
                    break;
                
            }
            return res;
        }
    }
}

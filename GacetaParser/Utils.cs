using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GacetaParser
{
    public class Utils
    {
        public static List<string> LookValueGroup(string lookIn, Regex reg, int[] pos)
        {
            MatchCollection mtc = reg.Matches(lookIn);

            if (mtc == null || mtc.Count == 0)
                return null;

            List<string> vals = new List<string>();

            foreach (Match m in mtc)
            {
                for (int i = 0; i < pos.Length; i++)
                {
                    if (m.Groups[pos[i]] != null)
                    {
                        vals.Add(m.Groups[pos[i]].Value);
                    }
                }                
            }

            return vals;
        }

        public static List<string> LookValue(string lookIn, Regex reg, int[] pos)
        {
            MatchCollection mtc = reg.Matches(lookIn);

            if (mtc == null || mtc.Count == 0)
                return null;

            List<string> vals = new List<string>();

            foreach (Match m in mtc)
            {
                string innerVal = "";

                for (int i = 0; i < pos.Length; i++)
                {
                    if (m.Groups[pos[i]] != null)
                    {
                        innerVal += m.Groups[pos[i]].Value + "|";
                    }
                }

                vals.Add(innerVal);
            }

            return vals;
        }


        public static string LookValue(string lookIn, Regex reg, int pos)
        {
            MatchCollection mtc = reg.Matches(lookIn);

            if (mtc == null || mtc.Count == 0)
                return null;

            foreach (Match m in mtc)
            {

                if (m.Groups[pos] != null)
                {
                    return m.Groups[pos].Value;
                }
            }

            return "";
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MCForge
{
    static class ProfanityFilter
    {
        private static Dictionary<string, string> RegexReduce;
        private static List<string> BadWords;
        public static void Init()
        {
            // Initializes the reduction dictionary and word list
            RegexReduce = new Dictionary<string, string>();
            RegexReduce.Add("a", "[@]");
            RegexReduce.Add("b", "i3|l3");
            RegexReduce.Add("c", "[(]");
            RegexReduce.Add("e", "[3]");
            RegexReduce.Add("f", "ph");
            RegexReduce.Add("g", "[6]");
            // Because Is and Ls are similar, the swear list will contain a lowercase I instead of Ls.
            RegexReduce.Add("i", "[l!1]");
            RegexReduce.Add("o", "[0]");
            RegexReduce.Add("q", "[9]");
            RegexReduce.Add("s", "[$5]");
            RegexReduce.Add("w", "vv");
            RegexReduce.Add("z", "[2]");

            // TODO: Load/create the badwords.txt file and import them into the BadWords list
        }

        public static string Parse(string text)
        {
            var result = new List<string>();
            var originalWords = text.Split(' ');
            var reducedWords = Reduce(text).Split(' ');
            for(var i=0; i < originalWords.Length; i++)
            {
                if (BadWords.Contains(originalWords[i]))
                {
                    // A reduced word matched a bad word from our file!
                    result.Add(new String('*', originalWords[i].Length));
                }
                else
                {
                    result.Add(originalWords[i]);
                }
            }

            return String.Join(" ", result.ToArray());
        }


        private static string Reduce(string text)
        {
            text = text.ToLower();
            foreach (var pattern in RegexReduce)
            {
                text = Regex.Replace(text, pattern.Value, pattern.Key, RegexOptions.IgnoreCase);
            }
            return text;
        }
    }
}

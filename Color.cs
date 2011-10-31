/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.opensource.org/licenses/ecl2.php
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the License is distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;

namespace MCForge
{
    public static class c
    {
        public const string black = "&0";
        public const string navy = "&1";
        public const string green = "&2";
        public const string teal = "&3";
        public const string maroon = "&4";
        public const string purple = "&5";
        public const string gold = "&6";
        public const string silver = "&7";
        public const string gray = "&8";
        public const string blue = "&9";
        public const string lime = "&a";
        public const string aqua = "&b";
        public const string red = "&c";
        public const string pink = "&d";
        public const string yellow = "&e";
        public const string white = "&f";

        public static string Parse(string str)
        {
            switch (str.ToLower())
            {
                case "black": return black;
                case "navy": return navy;
                case "green": return green;
                case "teal": return teal;
                case "maroon": return maroon;
                case "purple": return purple;
                case "gold": return gold;
                case "silver": return silver;
                case "gray": return gray;
                case "blue": return blue;
                case "lime": return lime;
                case "aqua": return aqua;
                case "red": return red;
                case "pink": return pink;
                case "yellow": return yellow;
                case "white": return white;
                default: return "";
            }
        }
        public static string Name(string str)
        {
            switch (str)
            {
                case black: return "black";
                case navy: return "navy";
                case green: return "green";
                case teal: return "teal";
                case maroon: return "maroon";
                case purple: return "purple";
                case gold: return "gold";
                case silver: return "silver";
                case gray: return "gray";
                case blue: return "blue";
                case lime: return "lime";
                case aqua: return "aqua";
                case red: return "red";
                case pink: return "pink";
                case yellow: return "yellow";
                case white: return "white";
                default: return "";
            }
        }

        public static byte MCtoIRC(string str)
        {
            switch (str)
            {
                case black: return 1;
                case navy: return 2;
                case green: return 3;
                case teal: return 10;
                case maroon: return 5;
                case purple: return 6;
                case gold: return 7;
                case silver: return 15;
                case gray: return 14;
                case blue: return 12;
                case lime: return 9;
                case aqua: return 11;
                case red: return 4;
                case pink: return 13;
                case yellow: return 8;
                case white: return 0;
                default: return 0;
            }
        }
        public static string IRCtoMC(byte str)
        {
            switch (str)
            {
                case 0: return white;
                case 1: return black;
                case 2: return navy;
                case 3: return green;
                case 4: return red;
                case 5: return maroon;
                case 6: return purple;
                case 7: return gold;
                case 8: return yellow;
                case 9: return lime;
                case 10: return teal;
                case 11: return aqua;
                case 12: return blue;
                case 13: return pink;
                case 14: return gray;
                case 15: return silver;
                default: return "";
            }
        }
    }
}
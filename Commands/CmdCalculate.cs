/*
	Copyright 2011 MCForge
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.osedu.org/licenses/ECL-2.0
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;
using System.IO;
using System.Threading;

namespace MCForge
{
    public class CmdCalculate : Command
    {
        public override string name { get { return "calculate"; } }
        public override string shortcut { get { return "calc"; } }
        public override string type { get { return "build"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message)
        {
            if(message == "")
            {
                Help(p);
                return;
            }

            var split = message.Split(' ');

            if (ValidChar(split[0]) && ValidChar(split[2]))
            {
                if (split[1] == "square" && split.Length == 2)
                {
                    float result = float.Parse(split[0]) * float.Parse(split[0]);
                    Player.SendMessage(p, "The answer: %aThe square of " + split[0] + Server.DefaultColor + " = %c" + result);
                    return;
                }
                if (split[1] == "root" && split.Length == 2)
                {
                    double result = Math.Sqrt(double.Parse(split[0]));
                    Player.SendMessage(p, "The answer: %aThe root of " + split[0] + Server.DefaultColor + " = %c" + result);
                    return;
                }
                if (split[1] == "cube" && split.Length == 2)
                {
                    float result = float.Parse(split[0]) * float.Parse(split[0]) + float.Parse(split[0]);
                    Player.SendMessage(p, "The answer: %aThe cube of " + split[0] + Server.DefaultColor + " = %c" + result);
                    return;
                }
                if (split[1] == "pi" && split.Length == 2)
                {
                    double result = int.Parse(split[0]) * Math.PI;
                    Player.SendMessage(p, "The answer: %a" + split[0] + " x PI" + Server.DefaultColor + " = %c" + result);
                    return;
                }
                if ((split[1] == "x" || split[1] == "*") && split.Length == 3)
                {
                    float result = float.Parse(split[0]) * float.Parse(split[2]);
                    Player.SendMessage(p, "The answer: %a" + split[0] + " x " + split[2] + Server.DefaultColor + " = %c" + result);
                    return;
                }
                if (split[1] == "+" && split.Length == 3)
                {
                    float result = float.Parse(split[0]) + float.Parse(split[2]);
                    Player.SendMessage(p, "The answer: %a" + split[0] + " + " + split[2] + Server.DefaultColor + " = %c" + result);
                    return;
                }
                if (split[1] == "-" && split.Length == 3)
                {
                    float result = float.Parse(split[0]) - float.Parse(split[2]);
                    Player.SendMessage(p, "The answer: %a" + split[0] + " - " + split[2] + Server.DefaultColor + " = %c" + result);
                    return;
                }
                if (split[1] == "/" && split.Length == 3)
                {
                    float result = float.Parse(split[0]) / float.Parse(split[2]);
                    Player.SendMessage(p, "The answer: %a" + split[0] + " / " + split[2] + Server.DefaultColor + " = %c" + result);
                    return;
                }
                else
                {
                    Player.SendMessage(p, "There is no such method");
                }
            }
            else
            {
                Player.SendMessage(p, "You can't calculate letters");
            }
        }
        public override void Help(Player p)
        {
            //Help message
            Player.SendMessage(p, "/calculate <num1> <method> <num2> - Calculates <num1> <method> <num2>");
            Player.SendMessage(p, "methods with 3 fillins: /, x, -, +");
            Player.SendMessage(p, "/calculate <num1> <method> - Calculates <num1> <method>");
            Player.SendMessage(p, "methods with 2 fillins: square, root, pi, cube");
        }
        public static bool ValidChar(string chr)
        {
            string allowedchars = "01234567890.,";
            foreach (char ch in chr) { if (allowedchars.IndexOf(ch) == -1) { return false; } } return true;
        }
    }
}
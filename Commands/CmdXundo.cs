using System;

namespace MCForge
{
    public class CmdXundo : Command
    {

        public override string name { get { return "xundo"; } }

        public override string shortcut { get { return ""; } }

        public override string type { get { return "other"; } }

        public override bool museumUsable { get { return false; } }

        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        public CmdXundo() { }
        public override void Use(Player p, string message)
        {

            if (message == "") { Help(p); return; }
            int number = message.Split(' ').Length;
            if (number != 1) { Help(p); return; }
         
            Player who = Player.Find(message);


            string error1 = "You are not allowed to use this command";
            string error2 = "You are not allowed to undo this player";

           
            if (p.group.Permission == LevelPermission.Operator)
            {
                if (who != null)
                {
                    if (who.group.Permission != LevelPermission.Operator)
                    {
                        p.group.Permission = LevelPermission.Admin;
                        Command.all.Find("undo").Use(p, who.name + " all");
                        p.group.Permission = LevelPermission.Operator;
                        return;
                    }
                    Player.SendMessage(p, error2);
                }
                else
                {
                    p.group.Permission = LevelPermission.Admin;
                    Command.all.Find("undo").Use(p, message + " all");
                    p.group.Permission = LevelPermission.Operator;
                    return;
                }
            }


            if (p.group.Permission > LevelPermission.Operator)
            {
                Command.all.Find("undo").Use(p, message + " all");
                return;
            }
            Player.SendMessage(p, error1);

        }



        public override void Help(Player p)
        {
            Player.SendMessage(p, "/xundo [name]  -  works as 'undo [name] all' but now Ops can use it");
        }
    }
}
using System;
using System.IO;

namespace MCForge
{
    public class CmdReferee : Command
    {
        public override string name { get { return "ref"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdReferee() { }
        public override void Use(Player p, string message)
        {
            if (p == null) { Player.SendMessage(p, "This command can only be used in-game!"); return; }
            if (p.referee)
            {
                p.referee = false;
                LevelPermission perm = Group.findPlayerGroup(name).Permission;
                Player.GlobalDie(p, false);
                Player.GlobalChat(p, p.color + p.name + Server.DefaultColor + " is no longer a referee", false);
                if (Server.infection)
                {
                    Player.GlobalDie(p, false);
                    Player.GlobalSpawn(p, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1], false);
                    if (!CmdZombieGame.infect.Contains(p) || CmdZombieGame.players.Contains(p))
                    {
                        CmdZombieGame.infect.Add(p);
                        CmdZombieGame.players.Remove(p);
                    }
                    p.color = c.red;
                }
                else
                {
                    Player.GlobalDie(p, false);
                    Player.GlobalSpawn(p, p.pos[0], p.pos[1], p.pos[2], p.rot[0], p.rot[1], false);
                    CmdZombieGame.infect.Remove(p);
                    CmdZombieGame.players.Add(p);
                    p.color = p.group.color;
                }
            }
            else
            {
                p.referee = true;
                Player.GlobalChat(p, p.color + p.name + Server.DefaultColor + " is now a referee", false);
                Player.GlobalDie(p, false);
                if (Server.infection)
                {
                    p.color = p.group.color;
                    try
                    {
                        CmdZombieGame.infect.Remove(p);
                        CmdZombieGame.players.Remove(p);
                    }
                    catch { }
                    CmdZombieGame.InfectedPlayerDC();
                }
                else
                {
                    CmdZombieGame.infect.Remove(p);
                    CmdZombieGame.players.Remove(p);
                    p.color = p.group.color;
                }
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/referee - Turns referee mode on/off.");
        }
    }
}
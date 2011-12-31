using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using MCForge;
namespace MCForge.CTF
{

    public class Setup : Plugin_Simple
    {
        Dictionary<Player, Data> cache = new Dictionary<Player, Data>();
        private Player.OnPlayerCommand _command; 

        public Player.OnPlayerCommand command
        {
            get
            {
                return _command;
            }
            set
            {
                _command = value;
            }
        }
        private Player.OnPlayerChat _chat; 

        public Player.OnPlayerChat chat
        {
            get
            {
                return _chat;
            }
            set
            {
                _chat = value;
            }
        }
        private Player.BlockchangeEventHandler2 _block; 

        public Player.BlockchangeEventHandler2 block
        {
            get
            {
                return _block;
            }
            set
            {
                _block = value;
            }
        }
        private Player.OnPlayerDisconnect _disconnect; 

        public Player.OnPlayerDisconnect disconnect
        {
            get
            {
                return _disconnect;
            }
            set
            {
                _disconnect = value;
            }
        }
        public override string creator
        {
            get { return "GamezGalaxy"; }
        }
        public override string MCForge_Version
        {
            get { return ""; }
        }
        public override string name
        {
            get { return "/ctfsetup"; }
        }
        public override void Load(bool startup)
        {
            block = new Player.BlockchangeEventHandler2(blockuse);
            command = new Player.OnPlayerCommand(commanduse);
            Player.PlayerCommand += command;
            chat = new Player.OnPlayerChat(chatuse);
            disconnect = new Player.OnPlayerDisconnect(disconnectuse);
            Player.PlayerChat += chat;
            Player.PlayerBlockChange += block;
            Player.PlayerDisconnect += disconnect;
        }
        public void disconnectuse(Player p, string reason)
        {
            if (cache.ContainsKey(p))
                cache.Remove(p);
        }
        public void blockuse(Player p, ushort x, ushort y, ushort z, byte type)
        {
            if (cache.ContainsKey(p))
            {
                switch (cache[p].s)
                {
                    case Step.GetBlueFlag:
                        cache[p].bx = x;
                        cache[p].by = y;
                        cache[p].bz = z;
                        cache[p].blue = p.level.GetTile(x, y, z);
                        Player.SendMessage(p, "Ok! I got the blue flag, now can you show me the red flag?");
                        Player.SendMessage(p, "Just hit it");
                        cache[p].s = Step.GetRedFlag;
                        break;
                    case Step.GetRedFlag:
                        cache[p].rx = x;
                        cache[p].ry = y;
                        cache[p].rz = z;
                        cache[p].red = p.level.GetTile(x, y, z);
                        Player.SendMessage(p, "Got it!");
                        Player.SendMessage(p, "Now I can do random spawns, or do you have a spawn in mind?");
                        Player.SendMessage(p, "Say - (Random/Set)");
                        cache[p].s = Step.RandomorSet;
                        break;
                }
            }
        }
        public void Finish(Player p, int bx, int by, int bz, int rx, int ry, int rz)
        {
            Player.SendMessage(p, "I'll set the tag points and capture points to there default");
            Player.SendMessage(p, "You can change them by going into CTF/<mapname>.config :)");
            List<string> config = new List<string>();
            config.Add("base.red.x=" + cache[p].rx);
            config.Add("base.red.y=" + cache[p].ry);
            config.Add("base.red.z=" + cache[p].rz);
            config.Add("base.blue.x=" + cache[p].bx);
            config.Add("base.blue.y=" + cache[p].by);
            config.Add("base.blue.z=" + cache[p].bz);
            config.Add("map.line.z=" + cache[p].middle);
            config.Add("base.red.block=" + cache[p].red);
            config.Add("base.blue.block=" + cache[p].blue);
            config.Add("game.maxpoints=3");
            config.Add("game.tag.points-gain=5");
            config.Add("game.tag.points-lose=5");
            config.Add("game.capture.points-gain=10");
            config.Add("game.capture.points-lose=10");
            if (bx != 0 && by != 0 && bz != 0 && rx != 0 && ry != 0 && rz != 0)
            {
                config.Add("base.blue.spawnx=" + bx);
                config.Add("base.blue.spawny=" + by);
                config.Add("base.blue.spawnz=" + bz);
                config.Add("base.red.spawnx=" + rx);
                config.Add("base.red.spawny=" + ry);
                config.Add("base.red.spawnz=" + rz);
            }
            File.WriteAllLines("CTF/" + cache[p].current.name + ".config", config.ToArray());
            config.Clear();
            if (File.Exists("CTF/maps.config"))
            {
                string[] temp = File.ReadAllLines("CTF/maps.config");
                foreach (string s in temp)
                    config.Add(s);
                temp = null;
            }
            config.Add(cache[p].current.name);
            File.WriteAllLines("CTF/maps.config", config.ToArray());
            config.Clear();
            if (!Directory.Exists("CTF/maps")) Directory.CreateDirectory("CTF/maps");
            if (File.Exists("CTF/maps/" + cache[p].current.name + ".lvl")) File.Delete("CTF/maps/" + cache[p].current.name + ".lvl");
            File.Copy("levels/" + cache[p].current.name + ".lvl", "CTF/maps/" + cache[p].current.name + ".lvl");
        }
        public void chatuse(Player p, string message)
        {
            if (message.ToLower(CultureInfo.CurrentCulture) == "random")
            {
                if (cache.ContainsKey(p))
                {
                    if (cache[p].s == Step.RandomorSet)
                    {
                        Player.SendMessage(p, "Ok random spawns it is!");
                        Finish(p, 0, 0, 0, 0, 0, 0);
                        cache.Remove(p);
                        Player.SendMessage(p, "Setup Complete!");
                    }
                }
            }
            if (message.ToLower(CultureInfo.CurrentCulture) == "set")
            {
                if (cache.ContainsKey(p))
                {
                    if (cache[p].s == Step.RandomorSet)
                    {
                        Player.SendMessage(p, "Ok, can you stand in the blue spawn and say \"continue\" (without the \" \")");

                    }
                }
            }
            if (message.ToLower(CultureInfo.CurrentCulture) == "continue")
            {
                if (cache.ContainsKey(p))
                {
                    switch (cache[p].s)
                    {
                        case Step.GetCenter:
                            cache[p].middle = p.pos[2] / 32;
                            Player.SendMessage(p, "I got " + cache[p].middle);
                            Player.SendMessage(p, "Ok, now I need to know where the blue flag is. Can you point me to it?");
                            Player.SendMessage(p, "Simply hit the block..");
                            cache[p].s = Step.GetBlueFlag;
                            break;
                        case Step.BlueSetSpawn:
                            cache[p].bluex = p.pos[0];
                            cache[p].bluey = p.pos[1];
                            cache[p].bluez = p.pos[2];
                            Player.SendMessage(p, "Ok, now can you stand in the red spawn and say \"conintue\"");
                            cache[p].s = Step.RedSetaSPawn;
                            break;
                        case Step.RedSetaSPawn:
                            Player.SendMessage(p, "ALMOST DONE!");
                            Finish(p, cache[p].bluex, cache[p].bluey, cache[p].bluez, p.pos[0], p.pos[1], p.pos[2]);
                            cache.Remove(p);
                            Player.SendMessage(p, "Setup Complete!");
                            break;
                    }
                }
            }
        }
        public void commanduse(string cmd, Player p, string message)
        {
            if (cmd.ToLower(CultureInfo.CurrentCulture) == "ctfsetup")
            {
                Level current = p.level;
//                int middle = p.level.height / 2 // COMMENTED BY CODEIT.RIGHT;
                Player.SendMessage(p, "%2Hello and welcome to the noob friendly CTF setup :D");
                if (!Directory.Exists("CTF")) Directory.CreateDirectory("CTF");
                Player.SendMessage(p, "I'll setup this map, but first can you stand in the middle of the map?");
                Player.SendMessage(p, "Once you get to the middle type \"continue\" in chat (without \" \")");
                Data d = new Data();
                d.s = Step.GetCenter;
                d.current = current;
                cache.Add(p, d);
            }
        }
        public override void Unload(bool shutdown)
        {
            Player.PlayerCommand -= command;
            Player.PlayerChat -= chat;
            Player.PlayerBlockChange -= block;
            Player.PlayerDisconnect -= disconnect;
        }
    }
    public class Data : IDisposable
    {
        private Step _s; 

        public Step s
        {
            get
            {
                return _s;
            }
            set
            {
                _s = value;
            }
        }
        private Level _current; 

        public Level current
        {
            get
            {
                return _current;
            }
            set
            {
                _current = value;
            }
        }
        private int _middle; 

        public int middle
        {
            get
            {
                return _middle;
            }
            set
            {
                _middle = value;
            }
        }
        private int _bx; 

        public int bx
        {
            get
            {
                return _bx;
            }
            set
            {
                _bx = value;
            }
        }
        private int _by; 

        public int by
        {
            get
            {
                return _by;
            }
            set
            {
                _by = value;
            }
        }
        private int _bz; 

        public int bz
        {
            get
            {
                return _bz;
            }
            set
            {
                _bz = value;
            }
        }
        private int _rx; 

        public int rx
        {
            get
            {
                return _rx;
            }
            set
            {
                _rx = value;
            }
        }
        private int _ry; 

        public int ry
        {
            get
            {
                return _ry;
            }
            set
            {
                _ry = value;
            }
        }
        private int _rz; 

        public int rz
        {
            get
            {
                return _rz;
            }
            set
            {
                _rz = value;
            }
        }
        private byte _blue; 

        public byte blue
        {
            get
            {
                return _blue;
            }
            set
            {
                _blue = value;
            }
        }
        private byte _red; 

        public byte red
        {
            get
            {
                return _red;
            }
            set
            {
                _red = value;
            }
        }
        private int _bluex; 

        public int bluex
        {
            get
            {
                return _bluex;
            }
            set
            {
                _bluex = value;
            }
        }
        private int _bluey; 

        public int bluey
        {
            get
            {
                return _bluey;
            }
            set
            {
                _bluey = value;
            }
        }
        private int _bluez; 

        public int bluez
        {
            get
            {
                return _bluez;
            }
            set
            {
                _bluez = value;
            }
        }
        public Data()
        {

        }

        #region IDisposable Implementation

        protected bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                // Do nothing if the object has already been disposed of.
                if (disposed)
                    return;

                if (disposing)
                {
                    // Release diposable objects used by this instance here.

                    if (current != null)
                        current.Dispose();
                }

                // Release unmanaged resources here. Don't access reference type fields.

                // Remember that the object has been disposed of.
                disposed = true;
            }
        }

        public virtual void Dispose()
        {
            Dispose(true);
            // Unregister object for finalization.
            GC.SuppressFinalize(this);
        }

        #endregion
    }
    public enum Step
    {
        GetCenter,
        GetBlueFlag,
        GetRedFlag,
        RandomorSet,
        BlueSetSpawn,
        RedSetaSPawn
    }
}

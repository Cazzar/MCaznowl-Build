using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge {
    class CmdServer : Command {
        public override string name { get { return "server"; } }
        public override string shortcut { get { return "serv"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public CmdServer() { }

        public override void Use(Player p, string message) {
            switch (message) {
                case "restart":
                    Command.all.Find("restart").Use(p, "");
                    break;
                case "update":
                    Command.all.Find("update").Use(p, "");
                    break;
                case "shutdown":
                    Command.all.Find("shutdown").Use(p, ""); //Will use other options later.
                    break;
                case "reset":
                case "reload":
                case "public":
                case "private":
                case "backup":
                case "restore":
                    Player.SendMessage(p, "'" + message + "' is currently unsupported.");
                    goto default;
                default:
                    Help(p);
                    break;
            }
        }

        public override void Help(Player p) {
            Player.SendMessage(p, "/server <reset|restart|reload|update|shutdown|public|private|backup|restore> - All server commands.");
            Player.SendMessage(p, "/server <reset>   - Reset everything to defaults.  WARNING: This will erase ALL properties.  Use with caution.");
            Player.SendMessage(p, "/server <restart> - Restart the sserver.");
            Player.SendMessage(p, "/server <reload>  - Reload the server files.");
            Player.SendMessage(p, "/server <update>  - Update the server");
            Player.SendMessage(p, "/server <update>  - Shutdown the server");
            Player.SendMessage(p, "/server <public>  - Make the server public. (Start listening for new connections.)");
            Player.SendMessage(p, "/server <private> - Make the server private. (Stop listening for new connections.)");
            Player.SendMessage(p, "Not yet implmented:");
            Player.SendMessage(p, "/server <backup>  - Make a complete backup of the server and all SQL data.");
            Player.SendMessage(p, "/server <restore> - Restore the server from a backup.");
        }
    }
}

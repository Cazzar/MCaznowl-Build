// shutdown was wrote by jordanneil23 with alot of help from TheMusiKid.
namespace MCForge
{
    using System;
    using System.IO;
    using System.Threading;
    public class CmdShutdown : Command
    {
        public override string name { get { return "shutdown"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override bool museumUsable { get { return true; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override void Help(Player p) { Player.SendMessage(p, "/shutdown [time] [message] - Shuts the server down"); }
        public override void Use(Player p, string message)
        {
            int secTime = 10;
            bool shutdown = true;
            string file = "stopShutdown";
            if (File.Exists(file)) { File.Delete(file); }
            if (message == "") { message = "Sever is going to shutdown in " + secTime + " seconds"; }
            else
            {
                if (message == "cancel") { File.Create(file).Close(); shutdown = false; message = "Shutdown cancelled"; }
                else
                {
                    if (!message.StartsWith("0"))
                    {
                        string[] split = message.Split(' ');
                        bool isNumber = false;
                        try { secTime = Convert.ToInt32(split[0]); isNumber = true; }
                        catch { secTime = 10; isNumber = false; }
                        if (split.Length > 1) { if (isNumber) { message = message.Substring(1 + split[0].Length); } }
                    }
                    else { Player.SendMessage(p, "Countdown time cannot be zero"); return; }
                }
            }
            if (shutdown)
            {
                Player.GlobalMessage(message);
                for (int t = secTime; t > 0; t = t - 1)
                {
                    if (!File.Exists(file)) { Player.GlobalMessage("Server shutdown in " + t + " seconds"); Thread.Sleep(1000); }
                    else { File.Delete(file); Player.GlobalMessage("Shutdown canceled"); return; }
                }
                if (!File.Exists(file)) { MCForge_.Gui.Program.ExitProgram(false); return; }
                else { File.Delete(file); Player.GlobalMessage("Shutdown canceled"); return; }
            }
            return;
        }
    }
}
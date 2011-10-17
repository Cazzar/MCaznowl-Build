using System;
using System.IO;
using System.Collections.Generic;

namespace MCForge
{
    public class CmdPCreate : Command
    {
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }
        public override bool museumUsable { get { return true; } }
        public override string name { get { return "pcreate"; } }
        public override string shortcut { get { return ""; } }
        public override string type { get { return "mod"; } }
        public override void Use(Player p, string message)
        {
            if (p != null) { Player.SendMessage(p, "Creating a plugin example source"); }
            else { Server.s.Log("Creating a plugin example source"); }

            string name;
            if (p != null) name = p.name;
            else name = Server.name;
            
            if (!Directory.Exists("plugin_source")) Directory.CreateDirectory("plugin_source");
            List<string> lines = new List<string>();
            lines.Add("//This is an example plugin source!");
            lines.Add("using System;");
            lines.Add("namespace MCForge");
            lines.Add("{");
            lines.Add("    public class " + message + " : Plugin");
            lines.Add("    {");
            lines.Add("        public override string name { get { return \"" + message + "\"; } }");
            lines.Add("        public override string website { get { return \"www.example.com\"; } }");
            lines.Add("        public override string MCForge_Version { get { return \"" + Server.Version + "\"; } }");
            lines.Add("        public override int build { get { return 100; } }");
            lines.Add("        public override string welcome { get { return \"Loaded Message!\"; } }");
            lines.Add("        public override string creator { get { return \"" + name + "\"; } }");
            lines.Add("        public override bool LoadAtStartup { get { return true; } }");
            lines.Add("        public override void Load(bool startup)");
            lines.Add("        {");
            lines.Add("            //LOAD YOUR PLUGIN WITH EVENTS OR OTHER THINGS!");
            lines.Add("        }");
            lines.Add("        public override void Unload(bool shutdown)");
            lines.Add("        {");
            lines.Add("            //UNLOAD YOUR PLUGIN BY SAVING FILES OR DISPOSING OBJECTS!");
            lines.Add("        }");
            lines.Add("        public override void Help(Player p) { //HELP INFO! }");
            lines.Add("    }}");
            lines.Add("}");
            File.WriteAllLines("plugin_source/" + message + ".cs", ListToArray(lines));
        }
        public override void Help(Player p)
        {
            if(p!=null)Player.SendMessage(p, "/pcreate <Plugin name> - Create a example .cs file!");
            else Server.s.Log("/pcreate <Plugin name> - Create a example .cs file!");
        }
        public CmdPCreate() { }
        public string[] ListToArray(List<string> list)
        {
            string[] temp = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
                temp[i] = list[i];
            return temp;
        }
    }
}

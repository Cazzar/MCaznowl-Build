/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
	
	Dual-licensed under the	Educational Community License, Version 2.0 and
	the GNU General Public License, Version 3 (the "Licenses"); you may
	not use this file except in compliance with the Licenses. You may
	obtain a copy of the Licenses at
	
	http://www.opensource.org/licenses/ecl2.php
	http://www.gnu.org/licenses/gpl-3.0.html
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the Licenses are distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the Licenses for the specific language governing
	permissions and limitations under the Licenses.
*/
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MCForge
{
    public sealed class CommandList
    {
        private List<Command> _commands = new List<Command>(); 

        public List<Command> commands
        {
            get
            {
                return _commands;
            }
            set
            {
                _commands = value;
            }
        }
        public CommandList() { }
        public void Add(Command cmd) { commands.Add(cmd); }
        public void AddRange(List<Command> listCommands)
        {
            listCommands.ForEach(delegate(Command cmd) { commands.Add(cmd); });
        }
        public List<string> commandNames()
        {
            List<string> tempList = new List<string>();

            commands.ForEach(delegate(Command cmd)
            {
                tempList.Add(cmd.name);
            });

            return tempList;
        }

        public bool Remove(Command cmd) { return commands.Remove(cmd); }
        public bool Contains(Command cmd) { return commands.Contains(cmd); }
        public bool Contains(string name)
        {
            name = name.ToLower(CultureInfo.CurrentCulture); foreach (Command cmd in commands)
            {
                if (cmd.name == name.ToLower(CultureInfo.CurrentCulture)) { return true; }
            } return false;
        }
        public Command Find(string name)
        {
            name = name.ToLower(CultureInfo.CurrentCulture); foreach (Command cmd in commands)
            {
                if (cmd.name == name || cmd.shortcut == name) { return cmd; }
            } return null;
        }

        public string FindShort(string shortcut)
        {
            if ((shortcut != null && String.IsNullOrEmpty(shortcut))) return "";

            shortcut = shortcut.ToLower(CultureInfo.CurrentCulture);
            foreach (Command cmd in commands)
            {
                if (cmd.shortcut == shortcut) return cmd.name;
            }
            return "";
        }

        public List<Command> All() { return new List<Command>(commands); }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge.Plugins
{
	public partial class Plugin
	{
        public static void CancelEvent(Events e) {
            switch (e)
            {
                case Events.BlockChange:
                    Player.cancelBlock = true;
                    break;
                case Events.Chat:
                    Player.cancelchat = true;
                    break;
                case Events.Command:
                    Player.cancelcommand = true;
                    break;
                case Events.LevelLoad:
                    Level.cancelsave = true;
                    break;
                case Events.LevelSave:
                    Level.cancelload = true;
                    break;
            }
        }
	}
}

using System;

namespace MCForge
{
    public partial class Player
    {
        public static bool cancelcommand = false;
        public static bool cancelchat = false;
        public static bool cancelBlock = false;
        /// <summary>
        /// BlockchangeEventHandler2 event is called when a player removes or places a block
        /// However, this event will due normal permission checking and normal block placing unless the event you cancel the event
        /// </summary>
        /// <param name="p">The player that placed the block</param>
        /// <param name="x">The x cord. of the block</param>
        /// <param name="y">The y cord. of the block</param>
        /// <param name="z">The z cord. of the block</param>
        /// <param name="type">The block the player is holding</param>
        public delegate void BlockchangeEventHandler2(Player p, ushort x, ushort y, ushort z, byte type);
        /// <summary>
        /// BlockchangeEventHandler event is called when a player removes, or places a block
        /// </summary>
        /// <param name="p">The player that removed or placed a block</param>
        /// <param name="x">The x cord. of the block</param>
        /// <param name="y">The y cord. of the block</param>
        /// <param name="z">The z cord. of the block</param>
        /// <param name="type">The block the player is holding</param>
        public delegate void BlockchangeEventHandler(Player p, ushort x, ushort y, ushort z, byte type);
        public event BlockchangeEventHandler Blockchange = null;
        /// <summary>
        /// Player Connect Event is called when a Player connects
        /// </summary>
        /// <param name="p">The player that connected</param>
        public delegate void OnPlayerConnect(Player p);
        public static event OnPlayerConnect PlayerConnect = null;
        /// <summary>
        /// Player Disconnect is called when a player disconnects
        /// </summary>
        /// <param name="p">The player that disconnected</param>
        /// <param name="reason">The reason (or kick message)</param>
        public delegate void OnPlayerDisconnect(Player p, string reason);
        public static event OnPlayerDisconnect PlayerDisconnect = null;
        /// <summary>
        /// OnPlayerCommand event is called when a player does a command
        /// However, the server will still look for another command unless you cancel the event
        /// </summary>
        /// <param name="cmd">The cmd the player used</param>
        /// <param name="p">The player that used it</param>
        /// <param name="message">The param.</param>
        public delegate void OnPlayerCommand(string cmd, Player p, string message);
        public static event OnPlayerCommand PlayerCommand = null;
        /// <summary>
        /// PlayerChat is event is called when a player chats on the server
        /// However the message will be sent unless you cancel the event
        /// </summary>
        /// <param name="p">The player that talked</param>
        /// <param name="message">The message that the user said</param>
        public delegate void OnPlayerChat(Player p, string message);
        public static event OnPlayerChat PlayerChat = null;
        /// <summary>
        /// The OnPlayerDeath event is called when...a player dies
        /// </summary>
        /// <param name="p">The player that died</param>
        /// <param name="deathblock">The block that killed him (in byte)</param>
        public delegate void OnPlayerDeath(Player p, byte deathblock);
        public static event OnPlayerDeath PlayerDeath = null;
        public static event BlockchangeEventHandler2 PlayerBlockChange = null;
        public event OnPlayerChat OnChat = null;
        public event OnPlayerCommand OnCommand = null;
        public event OnPlayerDeath OnDeath = null;
        public void ClearPlayerCommand() { OnCommand = null; }
        public void ClearPlayerChat() { OnChat = null; }
        public void ClearPlayerDeath() { OnDeath = null; }
        public void ClearBlockchange() { Blockchange = null; }
        public bool HasBlockchange() { return (Blockchange == null); }
        public object blockchangeObject = null;
    }
}

using System;

namespace MCForge
{
    public partial class Player
    {
        public bool cancelcommand = false;
        public bool cancelchat = false;
        public bool cancelmove = false;
        public bool cancelBlock = false;
        public bool cancelmysql = false;
        //Should people be able to cancel this event?
        public delegate void MOTDSent(Player p, byte[] buffer);
        public static event MOTDSent OnSendMOTD;
        //Should people be able to cancel this event?
        public delegate void MapSent(Player p, byte[] buffer);
        public static event MapSent OnSendMap;
        /// <summary>
        /// This is called when a player goes AFK
        /// </summary>
        /// <param name="p">The player that went AFK</param>
        public delegate void OnAFK(Player p);
        /// <summary>
        /// This event is triggered when a player goes AFK
        /// </summary>
        public static event OnAFK AFK;
        /// <summary>
        /// This event is triggered when a player goes AFK
        /// </summary>
        public event OnAFK ONAFK;
        /// <summary>
        /// This method is called whenever a players data is saved in mysql (even if disabled)
        /// </summary>
        /// <param name="p">The player</param>
        /// <param name="mysqlcommand">The MYSQL Command</param>
        public delegate void OnMySQLSave(Player p, string mysqlcommand);
        /// <summary>
        /// This event is called whenever a players data is saved in mysql (even if disabled) (Can be cancled)
        /// </summary>
        public static event OnMySQLSave MySQLSave;
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
        /// <summary>
        /// BlockChange event is called when a player removes or places a block
        /// </summary>
        public event BlockchangeEventHandler Blockchange = null;
        /// <summary>
        /// Player Connect Event is called when a Player connects
        /// </summary>
        /// <param name="p">The player that connected</param>
        public delegate void OnPlayerConnect(Player p);
        /// <summary>
        /// PlayerConnect event is called when a player connects (Player p)
        /// </summary>
        public static event OnPlayerConnect PlayerConnect = null;
        /// <summary>
        /// Player Disconnect is called when a player disconnects
        /// </summary>
        /// <param name="p">The player that disconnected</param>
        /// <param name="reason">The reason (or kick message)</param>
        public delegate void OnPlayerDisconnect(Player p, string reason);
        /// <summary>
        /// PlayerDisconnect event is called when a player disconnects (Player p, string reason)
        /// </summary>
        public static event OnPlayerDisconnect PlayerDisconnect = null;
        /// <summary>
        /// OnPlayerCommand event is called when a player does a command
        /// However, the server will still look for another command unless you cancel the event
        /// </summary>
        /// <param name="cmd">The cmd the player used</param>
        /// <param name="p">The player that used it</param>
        /// <param name="message">The param.</param>
        public delegate void OnPlayerCommand(string cmd, Player p, string message);
        /// <summary>
        /// PlayerCommand is called when a player does a command (string cmd, Player p, string message)
        /// </summary>
        public static event OnPlayerCommand PlayerCommand = null;
        /// <summary>
        /// PlayerChat is event is called when a player chats on the server
        /// However the message will be sent unless you cancel the event
        /// </summary>
        /// <param name="p">The player that talked</param>
        /// <param name="message">The message that the user said</param>
        public delegate void OnPlayerChat(Player p, string message);
        /// <summary>
        /// PlayerChat event is called when a player chats (Player p, string message)
        /// </summary>
        public static event OnPlayerChat PlayerChat = null;
        /// <summary>
        /// The OnPlayerDeath event is called when...a player dies
        /// </summary>
        /// <param name="p">The player that died</param>
        /// <param name="deathblock">The block that killed him (in byte)</param>
        public delegate void OnPlayerDeath(Player p, byte deathblock);
        /// <summary>
        /// This method is called when a player moves on the server
        /// </summary>
        /// <param name="p">The player that moved</param>
        /// <param name="x">The x cord.</param>
        /// <param name="y">The y cord.</param>
        /// <param name="z">The z cord.</param>
        public delegate void OnPlayerMove(Player p, ushort x, ushort y, ushort z);
        /// <summary>
        /// PlayerMove is called when a player moves (Player p, ushort x, ushort y, ushort z)
        /// </summary>
        public static event OnPlayerMove PlayerMove = null;
        /// <summary>
        /// OnMove is called when the player moves (Player p, ushort x, ushort y, ushort z)
        /// </summary>
        public event OnPlayerMove OnMove = null;
        /// <summary>
        /// PlayerDeath is called when the player dies (Player p, byte deathblock)
        /// </summary>
        public static event OnPlayerDeath PlayerDeath = null;
        /// <summary>
        /// PlayerBlockChange is called when a player places a block
        /// </summary>
        public static event BlockchangeEventHandler2 PlayerBlockChange = null;
        /// <summary>
        /// OnChat is called when the player chats (Player p, string message)
        /// </summary>
        public event OnPlayerChat OnChat = null;
        /// <summary>
        /// OnCommand is called when the player does a command (string cmd, Player p, string message)
        /// </summary>
        public event OnPlayerCommand OnCommand = null;
        /// <summary>
        /// OnDeath is called when the player dies (Player p, byte deathblock)
        /// </summary>
        public event OnPlayerDeath OnDeath = null;
        public void ClearPlayerCommand() { OnCommand = null; }
        public void ClearPlayerChat() { OnChat = null; }
        public void ClearPlayerDeath() { OnDeath = null; }
        public void ClearBlockchange() { Blockchange = null; }
        public bool HasBlockchange() { return (Blockchange == null); }
        public object blockchangeObject = null;

        //lolwut
        public delegate void BecomeBrony(Player p);
        public delegate void SonicRainboom(Player p);
        public static event BecomeBrony OnBecomeBrony;
        public static event SonicRainboom OnSonicRainboom;
    }
}

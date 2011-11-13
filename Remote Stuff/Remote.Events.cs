namespace MCForge.Remote
{
    public partial class Remote
    {
        /// <summary>
        /// This is called when a Remote successfully logs into the server
        /// </summary>
        /// <param name="remote">The Remote Object that is created</param>
        public delegate void RemoteLogin(Remote remote);
        /// <summary>
        /// This is called when a Remote successfully logs into the server
        /// </summary>
        public static event RemoteLogin OnRemoteLogin;



        /// <summary>
        /// This is called when the server sends data to the remote
        /// </summary>
        /// <param name="remote">The Remote Object that is created</param>
        /// <param name="size">The size of the byte array being sent</param>
        public delegate void RemoteSendData(Remote remote, short size);
        /// <summary>
        /// This is called when the server sends data to the remote
        /// </summary>
        public static event RemoteSendData OnRemoteSendData;



        /// <summary>
        /// This is called when the remote sends data to the server
        /// </summary>
        /// <param name="remote">The Remote Object that is created</param>
        /// <param name="size">The size of the byte array being recieved</param>
        public delegate void RemoteRecieveData(Remote remote, short size);
        /// <summary>
        /// This is called when the remote sends data to the server
        /// </summary>
        public static event RemoteRecieveData OnRemoteRecieveData;



        /// <summary>
        /// This is called when a Remote is kicked from the server, note: OnRemoteDisconnect will NOT be called.
        /// </summary>
        /// <param name="remote">The Remote Object</param>
        public delegate void RemoteKick(Remote remote);
        /// <summary>
        /// This is called when a Remote is kicked from the server
        /// </summary>
        public static event RemoteKick OnRemoteKick;



        /// <summary>
        /// This is called when a Remote disconnects from the server
        /// </summary>
        /// <param name="remote">The Remote Object</param>
        public delegate void RemoteDisconnect(Remote remote);
        /// <summary>
        /// This is called when a Remote is kicked from the server
        /// </summary>
        public static event RemoteDisconnect OnRemoteDisconnect;


        /// <summary>
        /// This is called when a log event is being sent to the remote
        /// </summary>
        /// <param name="remote">The Remote Object</param>
        /// <param name="p">The player that the remote is handling, Note player can be null(console)</param>
        /// <param name="message">the message being sent to the remote</param>
        public delegate void RemoteLog(Remote remote, Player p, string message);
        public static event RemoteLog OnRemoteLog;
        public static event RemoteLog OnRemoteOpLog;
        public static event RemoteLog OnRemoteAdminLog;
    }
         
}
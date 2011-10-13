using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace MCForge.Remote
{
    public class RemoteProperties
    {
        public static void Load(string fileName)
        {
            if (File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName);

                foreach (string line in lines)
                {
                    if (line != "")
                    {
                        string settingNode = line.Split('=')[0].Trim();
                        string value = "";
                        if (line.IndexOf('=') >= 0)
                            value = line.Substring(line.IndexOf('=') + 1).Trim();
                        switch (settingNode.ToLower())
                        {
                            case "RemoteUsername":

                                if (Properties.ValidString(value, "![]:.,{}~-+()?_/\\' "))
                                {
                                    RemoteServer.username = value;
                                }
                                else { Server.s.Log("Remote username invalid! setting to default."); }

                                break;
                            case "RemotePassword":

                                if (Properties.ValidString(value, "![]:.,{}~-+()?_/\\' "))
                                {
                                    RemoteServer.password = value;
                                }
                                else { Server.s.Log("Remote username invalid! setting to default."); }

                                break;
                            case "RemotePort":
                                try { RemoteServer.port = Convert.ToInt32(value); }
                                catch { Server.s.Log("Invalid Remote port! setting to default."); }
                                break;
                            case "UseRemote":
                                RemoteServer.enableRemote = (value.ToLower() == "true") ? true : false;
                                break;


                        }
                    }
                }
            }
        }
        public static void Save(string fileName)
        {

        }
        public class Crypto
        {
            // This is the base encryption salt! DO NOT CHANGE IT!!!
            private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");
            /// <summary>
            /// Encrypt the given string using AES.  The string can be decrypted using 
            /// DecryptStringAES().  The sharedSecret parameters must match.
            /// </summary>
            /// <param name="plainText">The text to encrypt.</param>
            /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
            /// <summary>
            /// Decrypt the given string.  Assumes the string was encrypted using 
            /// EncryptStringAES(), using an identical sharedSecret.
            /// </summary>
            /// <param name="cipherText">The text to decrypt.</param>
            /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
            public static string DecryptStringAES(string cipherText, string sharedSecret, Player who, string triedpass)
            {
                if (string.IsNullOrEmpty(cipherText))
                    throw new ArgumentNullException("cipherText");
                if (string.IsNullOrEmpty(sharedSecret))
                    throw new ArgumentNullException("sharedSecret");

                // Declare the RijndaelManaged object
                // used to decrypt the data.
                RijndaelManaged aesAlg = null;

                // Declare the string used to hold
                // the decrypted text.
                string plaintext = null;

                try
                {
                    // generate the key from the shared secret and the salt
                    Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                    // Create a RijndaelManaged object
                    // with the specified key and IV.
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    // Create the streams used for decryption.                
                    byte[] bytes = Convert.FromBase64String(cipherText);
                    using (MemoryStream msDecrypt = new MemoryStream(bytes))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
                finally
                {
                    // Clear the RijndaelManaged object.
                    if (aesAlg != null)
                        aesAlg.Clear();
                }
                //RemoteServer.password = plaintext;
                //gotpass = true;
                return plaintext;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MCForge
{
    public class Edit
    {
        /// <summary>
        /// Replaces a value in a line in a textfile
        /// </summary>
        /// <param name="path">Path of textfile</param>
        /// <param name="splitsearch">the messagesplit (' ') to search in</param>
        /// <param name="search">the string to search for in the msg split</param>
        /// <param name="splitedit">the messagesplit (' ') to edit</param>
        /// <param name="oldstring">the oldstring to be replaced</param>
        /// <param name="newstring">the replacement for oldstring</param>
        /// <returns></returns>
        public bool Replace(string path, int splitsearch, string search, int splitedit, string oldstring, string newstring)
        {
            bool succes = false;
            string end = "";
            if (File.Exists(path))
            {
                foreach (string line in File.ReadAllLines(path))
                {
                    if (line.Split(' ')[splitsearch] == search)
                    {
                        string newline = line.Split(' ')[splitedit].Replace(oldstring, newstring);
                        end = end + newline + "\r\n";
                        succes = true;
                    }
                    else
                    {
                        end = end + line + "\r\n";
                    }
                }
            }
            return succes;
        }
        /// <summary>
        /// Adds a line to a textfile
        /// </summary>
        /// <param name="path">the path to the textfile</param>
        /// <param name="line">the line to be added.</param>
        /// <returns></returns>
        public bool Add(string path, string line)
        {
            bool succes = false;
            if (File.Exists(path))
            {
                string alltext = File.ReadAllText(path);
                File.WriteAllText(path, alltext + line + "\r\n");
                succes = true;
            }
            return succes;
        }
        /// <summary>
        /// Deletes 1 line from a textfile, where the searchstring was found. Returns true if succesfull
        /// </summary>
        /// <param name="path">Path to file which has to get edited</param>
        /// <param name="search">The string to search for.</param>
        public bool Delete(string path, string search)
        {
            bool succes = false;
            string end = "";
            if (File.Exists(path))
            {
                foreach (string line in File.ReadAllLines(path))
                {
                    if (line.Contains(search))
                    {
                        succes = true;
                    }
                    else
                    {
                        end = end + line + "\r\n";
                    }
                }
            }
            return succes;
        }
        /// <summary>
        /// This searches for "search" string, in line.Split(' ')[split] in "path" file.
        /// </summary>
        /// <param name="path">The path to the file you're editing</param>
        /// <param name="split">The split number of the searchstring (split = (' '))</param>
        /// <param name="search">string to search for</param>
        /// <returns>Returns true if succes</returns>
        public bool Delete(string path, int split, string search)
        {
            bool succes = false;
            string end = "";
            if (File.Exists(path))
            {
                foreach (string line in File.ReadAllLines(path))
                {
                    if (line.Split(' ')[split] == search)
                    {
                        succes = true;
                    }
                    else
                    {
                        end = end + line + "\r\n";
                    }
                }
            }
            File.WriteAllText(path, end);
            return succes;
        }
    }
}

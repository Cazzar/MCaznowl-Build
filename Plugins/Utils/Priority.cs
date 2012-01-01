using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCForge
{
    /// <summary>
    /// Importance
    /// </summary>
    public enum Priority : byte
    {
        Low = 0,
        Normal = 1,
        High = 2,
        Critical = 3,
        System_Level = 4
    }
}

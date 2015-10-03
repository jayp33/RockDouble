using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockDouble
{
    class Song : IEquatable<Song>, IComparable
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string TimestampText { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return TimestampText + " " + Artist + " - " + Title;
        }

        public bool Equals(Song other)
        {
            return Timestamp.Equals(other.Timestamp);
        }

        public int CompareTo(object obj)
        {
            return Timestamp.CompareTo(((Song)obj).Timestamp);
        }
    }
}

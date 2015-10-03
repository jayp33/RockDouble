using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockDouble
{
    class Song
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Timestamp { get; set; }

        public override string ToString()
        {
            return Timestamp + " " + Artist + " - " + Title;
        }
    }
}

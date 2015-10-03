using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RockDouble
{
    [TestClass]
    public class SongTests
    {
        [TestMethod]
        public void UniqueSongCanOnlyBeAddedOnce()
        {
            var html = new HtmlAgilityPack.HtmlDocument();
            var path = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\..\TestData";
            html.Load(path + @"\ROCKLAND - Mach an und laut!.html");
            var parser = new RocklandParser();
            var songs = parser.GetSongs(html);
            var songDuplicate = new Song();
            songDuplicate.TimestampText = songs[5].TimestampText;
            songDuplicate.Timestamp = songs[5].Timestamp;
            songDuplicate.Artist = songs[5].Artist;
            songDuplicate.Title = songs[5].Title;
            if (!songs.Contains(songDuplicate))
                songs.Add(songDuplicate);
            Assert.AreEqual(8, songs.Count);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RockDouble
{
    [TestClass]
    public class RocklandParserTests
    {
        [TestMethod]
        public void RocklandWebsiteShouldContainROCKLAND_offline()
        {
            var html = new HtmlAgilityPack.HtmlDocument();
            var path = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\..\TestData";
            html.Load(path + @"\ROCKLAND - Mach an und laut!.html");
            Assert.IsTrue(html.DocumentNode.ChildNodes["html"].InnerText.Contains("ROCKLAND"));
        }

        [TestMethod]
        public void PlaylistWebsiteShouldContainPlaylistText()
        {
            var htmlWeb = new HtmlAgilityPack.HtmlWeb();
            var html = htmlWeb.Load("http://www.rockland.fm/start.php?playlist");
            Assert.IsTrue(html.DocumentNode.ChildNodes["html"].InnerText.Contains("Playlist - Was lief wann auf ROCKLAND?"));
        }

        [TestMethod]
        public void ParsePlaylistSongs_offline()
        {
            var html = new HtmlAgilityPack.HtmlDocument();
            var path = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\..\TestData";
            html.Load(path + @"\ROCKLAND - Mach an und laut!.html");
            var parser = new RocklandParser();
            var songs = parser.GetSongs(html);

            Assert.AreEqual(8, songs.Count);
            Assert.AreEqual("Metallica", songs[2].Artist);
            Assert.AreEqual("11:32 Uhr Russ Ballard - Voices",                             songs[0].ToString());
            Assert.AreEqual("11:26 Uhr Neil Young - Heart Of Gold",                        songs[1].ToString());
            Assert.AreEqual("11:21 Uhr Metallica - I Disappear",                           songs[2].ToString());
            Assert.AreEqual("11:18 Uhr Lenny Kravitz - Rock'n'Roll Is Dead",               songs[3].ToString());
            Assert.AreEqual("11:15 Uhr Hooters - Johnny B.",                               songs[4].ToString());
            Assert.AreEqual("11:11 Uhr Steve Harley &amp; Cockney Rebel - Make Me Smile ", songs[5].ToString());
            Assert.AreEqual("11:08 Uhr Foreigner - When It Comes To Love",                 songs[6].ToString());
            Assert.AreEqual("11:04 Uhr Glenn Frey - The Heat Is On ",                      songs[7].ToString());
        }

        [TestMethod]
        public void ParsePlaylistSongs()
        {
            var htmlWeb = new HtmlAgilityPack.HtmlWeb();
            var html = htmlWeb.Load("http://www.rockland.fm/start.php?playlist");
            var parser = new RocklandParser();
            var songs = parser.GetSongs(html);

            Assert.IsTrue(songs.Count > 0);
            foreach (var song in songs)
            {
                Assert.IsTrue(song.TimestampText.EndsWith("Uhr"));
            }
        }
    }
}

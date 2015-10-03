using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockDouble
{
    class RocklandParser
    {
        public List<Song> GetSongs(HtmlAgilityPack.HtmlDocument html)
        {
            var playlistTable = html.DocumentNode.SelectNodes(GetPlaylistXPath(html));
            //var playlistTable = html.DocumentNode.SelectNodes("/html[1]/body[1]/content[1]/div[2]/table[1]/tbody[1]/tr[1]/td[2]/table[1]/tbody[1]/tr[1]/td[1]/div[1]/div[1]/table[1]/tbody[1]");
            //var playlistTable = html.DocumentNode.SelectNodes("/html/body/content/form/div/table/tbody/tr/td[2]/table/tbody/tr/td/div[1]/div[1]/table/tbody");
            List<Song> songs = new List<Song>();
            foreach (HtmlAgilityPack.HtmlNode node in playlistTable.First().ChildNodes)
            {
                var subNodes = node.ChildNodes;
                if (subNodes.Count != 2)
                    continue;
                int startIndex = GetSongStartIndex(subNodes.Last().ChildNodes);
                Song song = new Song();
                song.Timestamp = subNodes.Last().ChildNodes.ElementAt(startIndex).InnerText;
                song.Artist = subNodes.Last().ChildNodes.ElementAt(startIndex + 2).InnerText;
                song.Title = subNodes.Last().ChildNodes.ElementAt(startIndex + 4).InnerText;
                songs.Add(song);
            }
            if (songs.Count == 0)
                throw new ArgumentException("No songs found");
            return songs;
        }

        public string GetPlaylistXPath(HtmlAgilityPack.HtmlDocument html)
        {
            var nodes = html.DocumentNode.Descendants();
            for (int i = nodes.Count() - 1; i > 0; i--)
                if (nodes.ElementAt(i).InnerText.Contains("Aktuelle Titelliste"))
                    for (int j = i; j < nodes.Count(); j++)
                        if (nodes.ElementAt(j).OriginalName.Equals("TABLE") ||
                            nodes.ElementAt(j).Name.Equals("tbody") /* tbody = for offline test */)
                            return nodes.ElementAt(j).XPath;
            throw new ArgumentException("Playlist XPath not found");
        }

        int GetSongStartIndex(HtmlAgilityPack.HtmlNodeCollection song)
        {
            for (int i = 0; i < song.Count; i++)
                if (System.Text.RegularExpressions.Regex.IsMatch(song.ElementAt(i).InnerText, "^\\d"))
                    return i;
            throw new ArgumentException("No timestamp found");
        }

    }
}

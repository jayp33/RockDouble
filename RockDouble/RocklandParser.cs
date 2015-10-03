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

        private string GetPlaylistXPath(HtmlAgilityPack.HtmlDocument html)
        {
            var nodes = html.DocumentNode.Descendants();
            for (int i = GetPlaylistNodeID(nodes); i < nodes.Count(); i++)
                if (nodes.ElementAt(i).OriginalName.Equals("TABLE") ||
                    nodes.ElementAt(i).Name.Equals("tbody") /* tbody = for offline test */)
                    return nodes.ElementAt(i).XPath;
            throw new ArgumentException("Playlist XPath not found");
        }

        private int GetPlaylistNodeID(IEnumerable<HtmlAgilityPack.HtmlNode> nodes)
        {
            for (int i = nodes.Count() - 1; i > 0; i--)
                if (nodes.ElementAt(i).InnerText.Contains("Aktuelle Titelliste"))
                    return i;
            throw new ArgumentException("Playlist Node ID not found");
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

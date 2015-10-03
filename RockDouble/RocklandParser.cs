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
                song.TimestampText = subNodes.Last().ChildNodes.ElementAt(startIndex).InnerText;
                song.Timestamp = GetDateTime(html, song.TimestampText);
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

        private string GetDateString(HtmlAgilityPack.HtmlDocument html)
        {
            string dateString = null;
            var nodes = html.DocumentNode.Descendants();
            for (int i = GetPlaylistNodeID(nodes); i > 0; i--)
                if (nodes.ElementAt(i).Name.Equals("h3"))
                {
                    dateString = nodes.ElementAt(i).InnerText.Split(' ')[1];
                    break;
                }
            var dateValues = dateString.Split('.');
            if (dateString.Length != 10 || dateValues.Length != 3)
                throw new ArgumentException("Invalid date");
            string day = dateValues[0];
            string month = dateValues[1];
            string year = dateValues[2];
            Int32.Parse(day);
            Int32.Parse(month);
            Int32.Parse(year);
            return dateString;
        }

        private string GetTimeString(string timestamp)
        {
            string time = timestamp.Split(' ')[0];
            var timeValues = time.Split(':');
            if (time.Length != 5 || timeValues.Length != 2)
                throw new ArgumentException("Invalid timestamp");
            Int32.Parse(timeValues[0]);
            Int32.Parse(timeValues[1]);
            return time;
        }

        private DateTime GetDateTime(HtmlAgilityPack.HtmlDocument html, string timestamp)
        {
            string date = GetDateString(html);
            string time = GetTimeString(timestamp);
            var dateValues = date.Split('.');
            var timeValues = time.Split(':');
            int day = Int32.Parse(dateValues[0]);
            int month = Int32.Parse(dateValues[1]);
            int year = Int32.Parse(dateValues[2]);
            int hour = Int32.Parse(timeValues[0]);
            int minute = Int32.Parse(timeValues[1]);
            return new DateTime(year, month, day, hour, minute, 0);
        }
    }
}

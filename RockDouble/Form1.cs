using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RockDouble
{
    public partial class Form1 : Form
    {
        Color m_originalColor;
        int defaultInterval;
        public Form1()
        {
            InitializeComponent();
            m_originalColor = this.BackColor;
            defaultInterval = timer1.Interval;
            textBoxInterval.Text = (timer1.Interval / 1000).ToString();
        }

        List<Song> m_songs = new List<Song>();
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!buttonStart.Text.Equals("Anhalten"))
            {
                buttonStart.Text = "Anhalten";
                listBoxSongs.Items.Clear();
                listBoxSongs.Items.Add("Loading songs...");
                Application.DoEvents();
                m_songs = new List<Song>();
                int interval = 0;
                if (Int32.TryParse(textBoxInterval.Text, out interval))
                    interval *= 1000;
                else
                    interval = defaultInterval;
                timer1.Interval = interval;
                timer1.Start();
                timer1_Tick(null, null);
            }
            else
            {
                buttonStart.Text = "Find RockDoubles";
                timer1.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var rockland = new RocklandParser();
            var songs = rockland.GetSongs(new Uri("http://www.rockland.fm/start.php?playlist"));
            if (songs.Count == 0)
                return;
            if (listBoxSongs.Items[0].Equals("Loading songs..."))
                listBoxSongs.Items.Clear();
            songs.Sort();
            foreach (var song in songs)
            {
                if (!m_songs.Contains(song))
                    m_songs.Add(song);
            }
            foreach (var m_song in m_songs)
            {
                if (!m_song.Added)
                {
                    listBoxSongs.Items.Insert(0, m_song);
                    m_song.Added = true;
                }
            }
            if (CheckForRockDouble())
            {
                this.BackColor = Color.Red;
                listBoxSongs.Items.Insert(0, "*** RockDouble found ***");
            }
        }

        private bool CheckForRockDouble()
        {
            if (m_songs.Last().Artist.Equals(m_songs.ElementAt(m_songs.Count - 2).Artist))
                return true;
            return false;
            //else
            //{
            //    string message = m_songs.ElementAt(m_songs.Count - 2).Artist + " <> " + m_songs.Last().Artist;
            //    if (!listBoxSongs.Items.Contains(message))
            //        listBoxSongs.Items.Add(message);
            //    return false;
            //}
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.BackColor = m_originalColor;
        }
    }
}

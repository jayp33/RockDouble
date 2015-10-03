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
        public Form1()
        {
            InitializeComponent();
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
                    listBoxSongs.Items.Add(m_song);
                    m_song.Added = true;
                }
            }
        }
    }
}

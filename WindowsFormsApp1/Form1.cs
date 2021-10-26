using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<Movie> PMovies = new List<Movie>(); // Global list so we can re-order it easily if wanted.

        string Link = "https://www.imdb.com/chart/top/?ref_=nv_mv_250"; // Imdb top 250 rated url 
        string Xpath = "/html/body/div[4]/div/div[2]/div[3]/div/div[1]/div/span/div/div/div[3]/table/tbody/tr[position()>0]"; // Xpath of the first node inside the parent list.


        /*these all are different xpaths to different child nodes of our tbody/tr parent. Only copy what comes after our parent node, otherwise 

         /html/body/div[4]/div/div[2]/div[3]/div/div[1]/div/span/div/div/div[3]/table/tbody/tr[1]/td[2]/a
         /html/body/div[4]/div/div[2]/div[3]/div/div[1]/div/span/div/div/div[3]/table/tbody/tr[1]/td[3]/strong
         /html/body/div[4]/div/div[2]/div[3]/div/div[1]/div/span/div/div/div[3]/table/tbody/tr[1]/td[1]/a/img

         what we use will be using here is 

        : td[2]/a
        : td[3]/strong
        : td[1]/a/img


        */ 

        


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadMovies(); // load when the application starts, if you do large amounts of scraping then the application will freeze. To fix this would be running for example LoadMovies() inside a thread. 
        }

        void LoadMovies()
        {
            var Movies = new List<Movie>(); // local list 

            var Web = new HtmlWeb(); // New html webclient 

            var Dock = Web.Load(Link); // Download and load the html of our url/link

            var Nodes = Dock.DocumentNode.SelectNodes(Xpath); // Get all of the nodes that has the same xpath as our variable, with the exception of 

            foreach(var Node in Nodes) // Extract information from each node <tr>
            {
                try
                {
                    var Movie = new Movie // Store locally 
                    {
                        Name = Node.SelectSingleNode("td[2]/a").InnerText, // title of movie 
                        Rating = Node.SelectSingleNode("td[3]/strong").InnerText, // rating of movie 
                        Img = Node.SelectSingleNode("td[1]/a/img").Attributes["src"].Value // image of movie (very low res) 47px
                    };

                    Movies.Add(Movie); // add to local list

                } catch { } // Easy handle of exception
            }

            // Loop through local list to add into datagridview 

            foreach (var Movie in Movies)
            {
                dataGridView1.Rows.Add(Movie.Name, Movie.Rating);
            }
            // add local list to global list,  I know it looks retarded.
            PMovies.Clear();
            PMovies.AddRange(Movies); 
        }
        

        class Movie
        {
            public string Name { get; set; }

            public string Rating { get; set; }

            public string Img { get; set; }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                pictureBox1.ImageLocation = PMovies[dataGridView1.CurrentCell.RowIndex].Img; // set the picturebox image to whatever the user clicks on.
            } catch { } // 
        }
    }
}

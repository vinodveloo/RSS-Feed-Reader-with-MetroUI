using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
//Library for reading the XML input
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceModel.Syndication;
using System.Data.SqlClient;



namespace RSS_Feed_Reader
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {

        //Connection String to DataSource

        SqlConnection con = new SqlConnection(@"Data Source=W10C42S6H2\SQLEXPRESS;Initial Catalog=RSSFeed;Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'rSSFeedDataSet.RSS_Link' table. You can move, or remove it, as needed.
            this.rSS_LinkTableAdapter.Fill(this.rSSFeedDataSet.RSS_Link);


            update_datagrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                // Initialization of XML reader

                XmlReader RSS_readerxml = XmlReader.Create(textBox1.Text);
                SyndicationFeed RSS_feed = SyndicationFeed.Load(RSS_readerxml);

                TabPage RSS_Tab = new TabPage(RSS_feed.Title.Text);
             

                //Adding new tab for each feed
                tabControl1.TabPages.Add(RSS_Tab);


                ListBox RSS_List = new ListBox();

                RSS_Tab.Controls.Add(RSS_List);

                RSS_List.Dock = DockStyle.Fill;

                RSS_List.HorizontalScrollbar = true;

                foreach(SyndicationItem RSS_Items in RSS_feed.Items)
                {
                    //Title
                    RSS_List.Items.Add(RSS_Items.Title.Text);
                     RSS_List.Items.Add("______________________");

                    //Summary 
                    RSS_List.Items.Add(RSS_Items.Summary.Text);
                    RSS_List.Items.Add("======================");




                }







            }
            

            catch(Exception ex)
            {
                MessageBox.Show( ex.Message);
            }




        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;

            //Save command
            cmd.CommandText = "insert into RSS_Link values ('" +textBox1.Text+ "')";
            cmd.ExecuteNonQuery();

            con.Close();
            update_datagrid();

            //User Alert
            MessageBox.Show("Updated Succesfully");
        }

        public void update_datagrid()
        {
            //To display the data on load or button click 

            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from RSS_Link";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

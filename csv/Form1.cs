using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Configuration;

namespace csv
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //ogranicz kol do 156?

        //sposób #1 - line by line
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            var plik = openFileDialog1.FileName;
            var wiersze = File.ReadAllLines(plik);
            var wierszeBezKolumn = wiersze.Skip(1);
            var znak = ';';
            var nazwyKolumn = wiersze.First().Split(znak);
            var tabela = new DataTable();
            
            foreach (var kol in nazwyKolumn)
            {
                tabela.Columns.Add(kol);
            }

            foreach (var dane in wierszeBezKolumn)
            {
                tabela.Rows.Add(dane.Split(znak));
            }
            
            dataGridView1.DataSource = tabela;
        }

        //sposób #2 - jeden string
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            var plik = openFileDialog1.FileName;
            var tabela = new DataTable();
            var znak = ';';

            var allData = File.ReadAllText(plik);
            var linieTekstu = Regex.Split(allData, "\r\n").ToList();
            var nazwyKolumn = linieTekstu.First().Split(znak);
            //var linie = allData.Split().SkipWhile(x => x.Equals("\r\n"));// && x != "\r\n");

            foreach (var s in nazwyKolumn)
            {
                tabela.Columns.Add(s);
            }

            linieTekstu.RemoveAt(0);
            foreach (var s in linieTekstu)
            {
                tabela.Rows.Add(s.Split(znak));
            }


            dataGridView1.DataSource = tabela;


        }

        //sposób 3
        private void button3_Click(object sender, EventArgs e)
        {

        }

        //'wyczyść' DataGridView
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            var plik = openFileDialog1.FileName;

            var allData = File.ReadAllText(plik);

            //App.config
            //var ustawienia = ConfigurationManager.ConnectionStrings["ConnStringODB1"];
            //using (var polacznie = new SqlConnection(ustawienia.ConnectionString))
            //SqlConnection con = new SqlConnection(@"Server=(local)\\sqlexpress;Database=OptimaDb;Trusted_Connection=True");

            //string connStr = @"Server=(local)\\sqlexpress;Database=OptimaDb;Trusted_Connection=True";
            string connStr = @"Data Source=(local)\sqlexpress;Initial Catalog=OptimaDb;Integrated Security=True;";
            string stworzTabele = @"CREATE TABLE PlikCSV (zawartosc varchar(255))";
            string plikDoTabeli = @"INSERT INTO PlikCSV VALUES (@zawartosc)";


            using (var polaczenie = new SqlConnection(connStr))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = polaczenie;
                    command.CommandText = stworzTabele;
                    command.CommandType = CommandType.Text;

                    try
                    {
                        polaczenie.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException exception)
                    {
                        MessageBox.Show(exception.Message.ToString());
                        
                    }
                }

                using (var command = new SqlCommand())
                {
                    command.Connection = polaczenie;
                    command.CommandText = plikDoTabeli;
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter(@"zawartosc", allData));


                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException exception)
                    {
                        MessageBox.Show(exception.Message.ToString());
                        
                    }
                }
            }
        }
    }
}

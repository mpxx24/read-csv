using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csv
{
    public partial class Form1 : Form
    {
        //private const string Path = @"C:/mpx/mojcsv.csv";
        public Form1()
        {
            InitializeComponent();
        }
        //ogranicz kol do 156
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

        private void button2_Click(object sender, EventArgs e)
        {

        }



    }
}

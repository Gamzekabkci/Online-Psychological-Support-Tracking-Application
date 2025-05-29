using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
namespace _1PsikolojikDestekApp
{
    public partial class Form9 : Form
    {
        
        private int aktifDanisanID;
        private string connStr;

        public Form9(int danisanID)
        {
            InitializeComponent();
            aktifDanisanID = danisanID;
        }

        private void Form9_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form10 seansForm = new Form10 (aktifDanisanID,connStr);
            seansForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form11 form11 = new Form11();
            form11.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form12 form12 = new Form12(aktifDanisanID, connStr);
            form12.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}

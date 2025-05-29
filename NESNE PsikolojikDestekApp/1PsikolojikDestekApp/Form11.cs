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
    public partial class Form11 : Form
    {
        private string connectionString = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";
        private int aktifDanisanID;
        

        public Form11(int danisanID, string connStr)
        {
            InitializeComponent();
            aktifDanisanID = danisanID;
            connectionString = connStr;
        }
        public Form11()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9 (aktifDanisanID);
            form9.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void Form11_Load(object sender, EventArgs e)
        {
            List<string> ilaclar = DanisanIlacBilgisi.IlaclariGetir(aktifDanisanID, connectionString);

            listBoxIlaclar.Items.Clear();
            foreach (string ilac in ilaclar)
            {
                listBoxIlaclar.Items.Add(ilac);
            }

            if (ilaclar.Count == 0)
                listBoxIlaclar.Items.Add("Kayıtlı ilaç bulunamadı.");
        }
    }
}

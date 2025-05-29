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
    public partial class Form10 : Form
    {
        private string connectionString = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";

        private int aktifDanisanID;

        public Form10 (int danisanID, string connStr)
        {
            InitializeComponent();
            aktifDanisanID = danisanID;
            connectionString = connStr;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9(aktifDanisanID);
            form9.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            List<Seans> seanslar = Seans.DanisanSeanslariniGetir(aktifDanisanID);

            listBoxSeanslar.Items.Clear();
            foreach (var seans in seanslar)
            {
                listBoxSeanslar.Items.Add($"{seans.Tarih:dd.MM.yyyy HH:mm} - {seans.Not}");
            }

            if (seanslar.Count == 0)
                listBoxSeanslar.Items.Add("Kayıtlı seans bulunamadı.");
        }

        private void listBoxSeanslar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

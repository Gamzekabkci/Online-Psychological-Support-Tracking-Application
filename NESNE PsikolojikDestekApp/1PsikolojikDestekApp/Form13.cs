using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1PsikolojikDestekApp
{
    public partial class Form13 : Form
    {
        private string connectionString = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";
        private int psikologID;
        public Form13(int gelenPsikologID)
        {
            InitializeComponent();
            psikologID = gelenPsikologID;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(psikologID);
            form4.Show();
            this.Hide();
        }

        private void Form13_Load(object sender, EventArgs e)
        {
            List<GeriBildirim> talepler = GeriBildirim.PsikologdanDanisanTalepleriGetir(psikologID);
            listBoxTalepler.Items.Clear();

            if (talepler.Count == 0)
            {
                listBoxTalepler.Items.Add("Randevu talebi yoktur.");
            }
            else
            {
                foreach (var geri in talepler)
                {
                    string adSoyad = $"{geri.DanisanAd} {geri.DanisanSoyad}";
                    listBoxTalepler.Items.Add($"{geri.Tarih.ToShortDateString()} - {adSoyad}: {geri.Icerik}");
                }
            }
        }
    }
}

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
    public partial class Form12 : Form
    {
        private string connectionString = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";
        private int aktifDanisanID;
        private int psikologID;


        public Form12(int danisanID, string connStr)
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

        private void Form12_Load(object sender, EventArgs e)
        {
            int psikologID;

            string connectionString = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sorgu = "SELECT PsikologID FROM Danisan WHERE DanisanID = @id";
                SqlCommand cmd = new SqlCommand(sorgu, conn);
                cmd.Parameters.AddWithValue("@id", aktifDanisanID);
                psikologID = Convert.ToInt32(cmd.ExecuteScalar());
            }

            Psikolog p = Psikolog.Getir(psikologID); 
            if (p != null)
            {
                lblAd1.Text = p.Ad;
                lblSoyad1.Text = p.Soyad;
                lblMail1.Text = p.Mail;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkBoxOnayli.Checked && !string.IsNullOrWhiteSpace(txtGeriBildirim.Text))
            {
                GeriBildirim yeni = new GeriBildirim
                {
                    DanisanID = aktifDanisanID,
                    Icerik = txtGeriBildirim.Text,
                    Tarih = DateTime.Now
                };

                GeriBildirim.Ekle(yeni);
                MessageBox.Show("Randevu talebiniz geri bildirim olarak gönderildi.", "Başarılı");
            }
            else
            {
                MessageBox.Show("Lütfen onay kutusunu işaretleyin ve bir not girin.", "Uyarı");
            }
        }
    }
}

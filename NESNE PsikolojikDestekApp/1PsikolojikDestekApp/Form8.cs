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
using System.Net.NetworkInformation;

namespace _1PsikolojikDestekApp
{
    public partial class Form8 : Form
    {

        public List<string> IlaclariGetir(int danisanID)
        {
            List<string> ilaclar = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Ilac FROM DanisanIlacBilgisi WHERE DanisanID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", danisanID);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ilaclar.Add(reader["Ilac"].ToString());
                }
            }

            return ilaclar;
        }
        private Danisan aktifDanisan;

        public Danisan DanisanGetir(string tc)
        {
            Danisan danisan = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Danisan WHERE TC = @tc AND PsikologID = @psikologID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@tc", tc);
                cmd.Parameters.AddWithValue("@psikologID", psikologID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    danisan = new Danisan
                    {
                        DanisanID = Convert.ToInt32(reader["DanisanID"]),
                        Ad = reader["Ad"].ToString(),
                        Soyad = reader["Soyad"].ToString(),
                        TC = reader["TC"].ToString(),
                        Mail = reader["Mail"].ToString(),
                        PsikologID = Convert.ToInt32(reader["PsikologID"])
                    };
                }
            }

            return danisan;
        }


        private string connectionString = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";
        private int psikologID;
        public Form8(int gelenPsikologID)
        {
            InitializeComponent();
            psikologID = gelenPsikologID;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(psikologID);
            form4.Show();
            this.Hide();
        }

        private void Form8_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string tc = txtTC.Text.Trim();
            aktifDanisan = DanisanGetir(tc);

            if (aktifDanisan != null)
            {
                lblAd.Text = aktifDanisan.Ad;
                lblSoyad.Text = aktifDanisan.Soyad;
                listBoxIlaclar.Items.Clear();

                List<string> ilaclar = IlaclariGetir(aktifDanisan.DanisanID);
                foreach (var ilac in ilaclar)
                {
                    listBoxIlaclar.Items.Add(ilac);
                }
            }
            else
            {
                MessageBox.Show("Danışan bulunamadı veya size ait değil.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtTC.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (aktifDanisan == null)
            {
                MessageBox.Show("Önce danışan seçmelisiniz.");
                return;
            }

            string ilacAdi = txtIlac.Text.Trim();
            if (string.IsNullOrEmpty(ilacAdi))
            {
                MessageBox.Show("İlaç adı boş olamaz.");
                return;
            }

            DanisanIlacBilgisi yeniIlac = new DanisanIlacBilgisi
            {
                DanisanID = aktifDanisan.DanisanID,
                Ilac = ilacAdi
            };

            bool basarili = yeniIlac.IlacEkle(connectionString);

            if (basarili)
            {
                listBoxIlaclar.Items.Add(ilacAdi);
                txtIlac.Clear();
                MessageBox.Show("İlaç eklendi.");
            }
            else
            {
                MessageBox.Show("İlaç eklenemedi.");
            }
        }
    }
}

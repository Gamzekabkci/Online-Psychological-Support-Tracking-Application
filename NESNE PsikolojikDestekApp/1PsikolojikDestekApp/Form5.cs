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
    public partial class Form5 : Form
    {
        private string connectionString = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";
        private int psikologID;
        public Form5(int gelenPsikologID)
        {
            InitializeComponent();
            psikologID = gelenPsikologID;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(psikologID);
            form4.Show();
            this.Hide();
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            Danisan danisan = new Danisan()
            {
                Ad = txtAd.Text.Trim(),
                Soyad = txtSoyad.Text.Trim(),
                TC = txtTC.Text.Trim(),
                Mail = txtMail.Text.Trim(),
                PsikologID = psikologID
            };

            Kullanici kullanici = new Kullanici()
            {
                KullaniciAdi = txtKullaniciAdi.Text.Trim(),
                Sifre = txtSifre.Text.Trim(),
                Rol = "danisan"
            };

            DanisanIlacBilgisi ilac = new DanisanIlacBilgisi()
            {
                Ilac = txtIlac.Text.Trim()
            };


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();

                try
                {
                 
                    string queryDanisan = "INSERT INTO Danisan (Ad, Soyad, TC, Mail, PsikologID) VALUES (@ad, @soyad, @tc, @mail, @psikologID); SELECT SCOPE_IDENTITY();";
                    SqlCommand cmdDanisan = new SqlCommand(queryDanisan, conn, tran);
                    cmdDanisan.Parameters.AddWithValue("@ad", danisan.Ad);
                    cmdDanisan.Parameters.AddWithValue("@soyad", danisan.Soyad);
                    cmdDanisan.Parameters.AddWithValue("@tc", danisan.TC);
                    cmdDanisan.Parameters.AddWithValue("@mail", danisan.Mail);
                    cmdDanisan.Parameters.AddWithValue("@psikologID", danisan.PsikologID);

                    danisan.DanisanID = Convert.ToInt32(cmdDanisan.ExecuteScalar());

                    
                    string queryKullanici = "INSERT INTO Kullanici (KullaniciAdi, Sifre, Rol) VALUES (@kadi, @sifre, @rol)";
                    SqlCommand cmdKullanici = new SqlCommand(queryKullanici, conn, tran);
                    cmdKullanici.Parameters.AddWithValue("@kadi", kullanici.KullaniciAdi);
                    cmdKullanici.Parameters.AddWithValue("@sifre", kullanici.Sifre);
                    cmdKullanici.Parameters.AddWithValue("@rol", kullanici.Rol);
                    cmdKullanici.ExecuteNonQuery();

                  
                    string queryIlac = "INSERT INTO DanisanIlacBilgisi (DanisanID, Ilac) VALUES (@danisanID, @ilac)";
                    SqlCommand cmdIlac = new SqlCommand(queryIlac, conn, tran);
                    cmdIlac.Parameters.AddWithValue("@danisanID", danisan.DanisanID);
                    cmdIlac.Parameters.AddWithValue("@ilac", ilac.Ilac);
                    cmdIlac.ExecuteNonQuery();

                    tran.Commit();
                    MessageBox.Show("Danışan başarıyla eklendi.");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show("Hata oluştu: " + ex.Message);
                }
            }

        }
    }
}

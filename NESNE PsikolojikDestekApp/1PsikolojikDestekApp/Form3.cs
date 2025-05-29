using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace _1PsikolojikDestekApp
{
    public partial class Form3 : Form
    {
        private string connectionString = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";
        

        public Form3()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            string kullaniciAdi = txtKullaniciAdi.Text.Trim();
            string sifre = txtSifre.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Kullanici WHERE KullaniciAdi = @kadi AND Sifre = @sifre";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@kadi", kullaniciAdi);
                cmd.Parameters.AddWithValue("@sifre", sifre);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string rol = reader["Rol"].ToString();
                        int kullaniciID = Convert.ToInt32(reader["KullaniciID"]);

                        MessageBox.Show("Giriş başarılı. Rol: " + rol);

                        if (rol == "psikolog")
                        {
                            reader.Close(); // yeni sorgu için kapat
                            string psikologQuery = "SELECT PsikologID FROM Psikolog WHERE KullaniciID = @kID";
                            SqlCommand psikologCmd = new SqlCommand(psikologQuery, conn);
                            psikologCmd.Parameters.AddWithValue("@kID", kullaniciID);

                            object result = psikologCmd.ExecuteScalar();

                            if (result != null)
                            {
                                int psikologID = Convert.ToInt32(result);
                                Form4 form = new Form4(psikologID);
                                form.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Psikolog bilgisi bulunamadı.");
                            }
                        }
                        else if (rol == "danisan")
                        {
                            reader.Close(); // yeni sorgu için kapat
                            string danisanQuery = "SELECT DanisanID FROM Danisan WHERE KullaniciID = @kID";
                            SqlCommand danisanCmd = new SqlCommand(danisanQuery, conn);
                            danisanCmd.Parameters.AddWithValue("@kID", kullaniciID);

                            object result = danisanCmd.ExecuteScalar();

                            if (result != null)
                            {
                                int danisanID = Convert.ToInt32(result);
                                Form9 form = new Form9(danisanID);
                                form.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Danışan bilgisi bulunamadı.");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı adı veya şifre hatalı.");
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bağlantı hatası: " + ex.Message);
                }
            }
            
        }
    }
}

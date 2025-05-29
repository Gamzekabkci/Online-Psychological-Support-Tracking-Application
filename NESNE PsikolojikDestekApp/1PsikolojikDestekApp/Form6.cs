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
   
   
    public partial class Form6 : Form
    {
        private string connectionString = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";
        private int psikologID;
        public Form6(int gelenPsikologID)
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

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tc = txtTC.Text.Trim();
            DateTime tarih = Tarih.Value;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                
                string danisanQuery = @"SELECT DanisanID FROM Danisan
                                WHERE TC = @tc AND PsikologID = @psikologID";

                SqlCommand cmd = new SqlCommand(danisanQuery, conn);
                cmd.Parameters.AddWithValue("@tc", tc);
                cmd.Parameters.AddWithValue("@psikologID", psikologID);

                object result = cmd.ExecuteScalar();
                try
                {
                    if (result != null)
                    {
                        int danisanID = Convert.ToInt32(result);


                        string insertQuery = @"INSERT INTO Randevu (DanisanID, Tarih)
                                   VALUES (@danisanID, @tarih)";

                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@danisanID", danisanID);
                        insertCmd.Parameters.AddWithValue("@tarih", tarih);

                        insertCmd.ExecuteNonQuery();

                        MessageBox.Show("Randevu başarıyla oluşturuldu.");
                        Form4 form4 = new Form4(psikologID);
                        form4.Show();
                        this.Hide();
                    }

                    else
                    {
                        MessageBox.Show("Bu TC'ye sahip danışan size ait değil veya bulunamadı.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }

            
            }
        }
    }
}

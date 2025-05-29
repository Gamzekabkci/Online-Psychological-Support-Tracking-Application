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
    public partial class Form7 : Form
    {
        private string connectionString = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";
        private int psikologID;
        public Form7(int gelenPsikologID)
        {
            InitializeComponent();
            psikologID = gelenPsikologID;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(psikologID);
            form4.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string tc = txtTC.Text.Trim();
            listBoxSeanslar.Items.Clear();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

               
                string danisanQuery = "SELECT DanisanID FROM Danisan WHERE TC = @tc AND PsikologID = @psikologID";

                SqlCommand danisanCmd = new SqlCommand(danisanQuery, conn);
                danisanCmd.Parameters.AddWithValue("@tc", tc);
                danisanCmd.Parameters.AddWithValue("@psikologID", psikologID); 

                object result = danisanCmd.ExecuteScalar();

                if (result != null)
                {
                    int danisanID = Convert.ToInt32(result);

                    
                    string seansQuery = "SELECT Tarih, Notlar FROM Seans WHERE DanisanID = @danisanID ORDER BY Tarih DESC";
                    SqlCommand seansCmd = new SqlCommand(seansQuery, conn);
                    seansCmd.Parameters.AddWithValue("@danisanID", danisanID);

                    SqlDataReader reader = seansCmd.ExecuteReader();

                    while (reader.Read())
                    {
                        DateTime tarih = Convert.ToDateTime(reader["Tarih"]);
                        string not = reader["Notlar"].ToString();

                        listBoxSeanslar.Items.Add($"{tarih:dd.MM.yyyy HH:mm} - {not}");
                    }

                    if (listBoxSeanslar.Items.Count == 0)
                    {
                        listBoxSeanslar.Items.Add("Bu danışana ait seans kaydı bulunamadı.");
                    }
                }
                else
                {
                    MessageBox.Show("Bu TC'ye sahip danışan size ait değil veya bulunamadı.");
                }
            }
        }
    }
}

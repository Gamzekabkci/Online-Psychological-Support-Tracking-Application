using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace _1PsikolojikDestekApp
{
    public class Kullanici
    {
        public int KullaniciID { get; set; } // Primary Key
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string Rol { get; set; } // 'psikolog' veya 'danisan'
    }
    public class Psikolog
    {
        public int PsikologID { get; set; } // Primary Key
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string TC { get; set; }
        public string Mail { get; set; }

        public static Psikolog Getir(int psikologID)
        {
            Psikolog psikolog = null;
            string connectionString = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Psikolog WHERE PsikologID = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", psikologID);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    psikolog = new Psikolog
                    {
                        PsikologID = psikologID,
                        Ad = reader["Ad"].ToString(),
                        Soyad = reader["Soyad"].ToString(),
                        Mail = reader["Mail"].ToString()
                    };
                }
            }

            return psikolog;
        }

    }
    public class Danisan
    {
        public int DanisanID { get; set; } // Primary Key
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string TC { get; set; }
        public string Mail { get; set; }
        public int PsikologID { get; set; }

    }
    public class DanisanIlacBilgisi
    {
        public int DanisanID { get; set; } // Foreign Key
        public string Ilac { get; set; }

        public bool IlacEkle(string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO DanisanIlacBilgisi (DanisanID, Ilac) VALUES (@id, @ilac)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", DanisanID);
                cmd.Parameters.AddWithValue("@ilac", Ilac);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public static List<string> IlaclariGetir(int danisanID, string connectionString)
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
    }
    public class Randevu
    {
        public int RandevuID { get; set; } // Primary Key
        public int DanisanID { get; set; } // Foreign Key
        public string DanisanTC { get; set; } // Foreign Key
        public DateTime Tarih { get; set; }
    }

    public class Seans
    {
        public int SeansID { get; set; } // Primary Key
        public int DanisanID { get; set; } // Foreign Key
        public DateTime Tarih { get; set; }
        public string Not { get; set; }
        
        public static List<Seans> DanisanSeanslariniGetir(int danisanID)
        {
            List<Seans> seanslar = new List<Seans>();

            string connectionString = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Seans WHERE DanisanID = @id ORDER BY Tarih DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", danisanID);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    seanslar.Add(new Seans
                    {
                        SeansID = Convert.ToInt32(reader["SeansID"]),
                        DanisanID = Convert.ToInt32(reader["DanisanID"]),
                        Tarih = Convert.ToDateTime(reader["Tarih"]),
                        Not = reader["Not"].ToString()
                    });
                }

                reader.Close();
            }

            return seanslar;
        }

    }

    public class GeriBildirim
    {
        public int GeriBildirimID { get; set; } // Primary Key
        public int DanisanID { get; set; } // Foreign Key
        public string Icerik { get; set; }
        public DateTime Tarih { get; set; }
        public string DanisanAd { get; set; }     
        public string DanisanSoyad { get; set; }  

        public static void Ekle(GeriBildirim geriBildirim)
        {
            string connectionString = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO GeriBildirim (DanisanID, Icerik, Tarih) VALUES (@danisanID, @icerik, @tarih)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@danisanID", geriBildirim.DanisanID);
                cmd.Parameters.AddWithValue("@icerik", geriBildirim.Icerik);
                cmd.Parameters.AddWithValue("@tarih", geriBildirim.Tarih);

                cmd.ExecuteNonQuery();
            }
        }

        public static List<GeriBildirim> PsikologdanDanisanTalepleriGetir(int psikologID)
        {
            List<GeriBildirim> liste = new List<GeriBildirim>();
            string connStr = "Server=GAMZE\\SQLEXPRESS;Database=PsikolojikDestekAppDB;Trusted_Connection=True;";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                string query = @"
                SELECT gb.GeriBildirimID, gb.DanisanID, gb.Icerik, gb.Tarih, d.Ad, d.Soyad
                FROM GeriBildirim gb
                INNER JOIN Danisan d ON gb.DanisanID = d.DanisanID
                WHERE d.PsikologID = @psikologID
                ORDER BY gb.Tarih DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@psikologID", psikologID);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    liste.Add(new GeriBildirim
                    {
                        GeriBildirimID = Convert.ToInt32(reader["GeriBildirimID"]),
                        DanisanID = Convert.ToInt32(reader["DanisanID"]),
                        Icerik = reader["Icerik"].ToString(),
                        Tarih = Convert.ToDateTime(reader["Tarih"]),
                        DanisanAd = reader["Ad"].ToString(),
                        DanisanSoyad = reader["Soyad"].ToString()
                    });
                }
            }

            return liste;
        }

    }



}

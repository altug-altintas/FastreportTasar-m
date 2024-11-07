using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using FastReport;
using FastReportCSharp;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;
using System.Data.SqlClient;

namespace FastReportCSharp
{
    public class Selectler
    {
        public static OdbcConnection con = new OdbcConnection();

        public static void UpdateRaporTasarim(string raporId, byte[] updatedRaporBinary)
        {
            try
            {
                Connection.OpenConnection(ref con);
                //   con.Open();
                OdbcCommand com = new OdbcCommand("UPDATE fast_rapor SET fst_dosya = ? WHERE fst_id = ?;", con);
                com.Parameters.AddWithValue("fst_dosya", updatedRaporBinary);
                com.Parameters.AddWithValue("fst_id", raporId);
                com.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                throw ex;
            }
        }
        public static void S_GetDosyaDetay(string fst_id, ref byte[] scndosya, ref string parametre, ref string hata)
        {
            try
            {
                Connection.OpenConnection(ref con);
                //  con.Open();
                OdbcCommand com = new OdbcCommand("Select fst_id,fst_rapor From fast_rapor WHERE etk_id = :fst_id ", con);
                com.Parameters.AddWithValue(":fst_id", fst_id);
                OdbcDataReader SDR = com.ExecuteReader();
                if (SDR.Read())
                {

                    if (SDR["fst_rapor"] != DBNull.Value)
                    {
                        scndosya = (byte[])SDR["fst_rapor"];
                    }
                    parametre = SDR["etk_parametre"].ToString();
                }
                con.Close();
            }
            catch (Exception c)
            {
                hata = c.Message;
                con.Close();
                return;
            }
        }
        public static void getdetay(string fst_id)
        {
            try
            {
                Connection.OpenConnection(ref con);
                // con.Open();
                OdbcCommand com = new OdbcCommand("Select fst_id,fst_ad,fst_dosya,fst_tip,fst_durum,fst_tanımı   From fast_rapor WHERE etk_id = :fst_id ", con);
                com.Parameters.AddWithValue(":fst_id", fst_id);
                OdbcDataReader SDR = com.ExecuteReader();
                if (SDR.Read())
                {
                }
                con.Close();
            }
            catch (Exception c)
            {
                con.Close();
                return;
            }
        }


        public byte[] GetRaporDosya(string raporId)
        {
            byte[] raporDosya = null;
            Connection.OpenConnection(ref con);
            // con.Open();
            try
            {
                string sorgu = "SELECT fst_dosya FROM fast_rapor WHERE fst_id = '" + raporId + "'";
                OdbcCommand com = new OdbcCommand(sorgu, con);
                OdbcDataReader reader = com.ExecuteReader();

                if (reader.Read())
                {
                    raporDosya = (byte[])reader["fst_dosya"];
                }

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
            return raporDosya;
        }


        public static bool I_DOSYAKAYIT(string fst_ad, byte[] fst_dosya, string fst_tip, string fst_tanımı, string fst_sira, string fst_srk_no,string fst_bcmno,string fst_tp_id)
         {
            bool durum = false;
            try
            {
                string param1 = fst_tip;
                string param2 = fst_tanımı;
                string param3 = fst_ad;

                if (param1 != "" && param2 != "" && param3 != "")
                {
                     Connection.OpenConnection(ref con);
                    // con.Open();
                     OdbcCommand com = new OdbcCommand("insert fast_rapor (fst_ad,fst_dosya,fst_tip,fst_tanımı,fst_durum,fst_sira,fst_srk_no,fst_bcmno,fst_tp_id) VALUES (:fst_ad,:fst_dosya,:fst_tip,:fst_tanımı,1 ,:fst_sira,:fst_srk_no,:fst_bcmno,:fst_tp_id);", con);
                    com.Parameters.AddWithValue(":fst_ad", fst_ad);
                    com.Parameters.AddWithValue(":fst_dosya", fst_dosya);
                    com.Parameters.AddWithValue(":fst_tur", fst_tip);
                    com.Parameters.AddWithValue(":fst_tanımı", fst_tanımı);
                    com.Parameters.AddWithValue(":fst_sira", fst_sira);
                    com.Parameters.AddWithValue(":fst_srk_no", fst_srk_no);
                    com.Parameters.AddWithValue(":fst_bcmno", fst_bcmno);
                    com.Parameters.AddWithValue(":fst_tp_id", fst_tp_id);


                    object x = com.ExecuteScalar();
                    con.Close();
                    durum = true;
                }
            }
            catch (Exception c)
            {
                durum = false;
                con.Close();
            }
            return durum;
        }

        //public static List<int> IsDuplicateFstSira(string fst_tip)

        public static string IsDuplicateFstSira(string fst_id)
        {
            string siraListesi = "";
            Connection.OpenConnection(ref con);
            OdbcCommand com = new OdbcCommand("SELECT fst_sira FROM fast_rapor WHERE fst_id = ?", con);
            com.Parameters.AddWithValue("@1", fst_id);
            OdbcDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                string fst_sira = reader["fst_sira"].ToString();
                siraListesi = fst_sira;
            }
            reader.Close();
            return siraListesi;
            //int count = 0;
            //Connection.OpenConnection(ref con);

            //OdbcCommand com = new OdbcCommand("SELECT fst_sira FROM fast_rapor WHERE fst_tip = ?", con);
            //com.Parameters.AddWithValue("@1", fst_tip);

            //object result = com.ExecuteScalar();// Eğer count 0'dan büyükse, aynı fst_sira değerine sahip bir kayıt var demektir
            //if (int.TryParse(result.ToString(), out count))
            //{
            //    // Dönüşüm başarılı
            //    return count;
            //}
            //return count;
        }


        public static DataTable RaporListeleme(string fst_tip, int fst_durum,string fst_srk_no, string fst_tp_id)
        {
            DataTable dt = new DataTable();
            Connection.OpenConnection(ref con);
            //  con.Open();
            try
            {
                string sorgu = "SELECT fst_tanımı,fst_id,fst_dosya,fst_durum,fst_ad,fst_sira,fst_srk_no,fst_tp_id FROM fast_rapor WHERE fst_durum= '" + fst_durum + "' and fst_tip = '" + fst_tip + "' and fst_srk_no= '"+fst_srk_no+ "' and fst_tp_id='"+ fst_tp_id + "' ";
                OdbcDataAdapter da = new OdbcDataAdapter(sorgu, con);
                da.Fill(dt);

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }
            return dt;
        }



        public static DataTable Sirket ()
        {
            DataTable dt = new DataTable();
            Connection.OpenConnection(ref con);
            //  con.Open();
            try
            {
                string sorgu = "SELECT skr_no,srk_ad FROM sirket ";
                OdbcDataAdapter da = new OdbcDataAdapter(sorgu, con);
                da.Fill(dt);

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }
            return dt;
        }






        public static DataTable RaporListelemeDKm(string fst_tip, int fst_durum,string fst_sira,string srk_no)
        {
            DataTable dt = new DataTable();
            Connection.OpenConnection(ref con);
            //  con.Open();
            try
            {
                string sorgu = "SELECT fst_tanımı,fst_id,fst_dosya,fst_durum,fst_ad,fst_sira FROM fast_rapor WHERE fst_durum= '" + fst_durum + "' and fst_tip = '" + fst_tip + "' and fst_sira= '"+fst_sira+"' and fst_srk_no = '"+srk_no+"'";
                OdbcDataAdapter da = new OdbcDataAdapter(sorgu, con);
                da.Fill(dt);

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }
            return dt;
        }


       


        public static DataTable RaporYazdır(string fst_tip, int fst_durum,string fst_sira)
        {
            DataTable dt = new DataTable();
            Connection.OpenConnection(ref con);
            //  con.Open();
            try
            {
                string sorgu = "SELECT fst_tanımı,fst_id,fst_dosya,fst_durum,fst_ad,fst_sira FROM fast_rapor WHERE fst_durum= '" + fst_durum + "' and fst_tip = '" + fst_tip + "'  and fst_sira='"+fst_sira+"'";
                OdbcDataAdapter da = new OdbcDataAdapter(sorgu, con);
                da.Fill(dt);

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }
            return dt;
        }


        public static bool Aktif_pasif(string id, int parametre)
        {
            bool durum = false;
            Connection.OpenConnection(ref con);
            // con.Open();
            try
            {
                string script = @" update fast_rapor set fst_durum='" + parametre + "' from dba.fast_rapor where fst_id=:ID";
                OdbcCommand com = new OdbcCommand(script, con);
                com.Parameters.AddWithValue(":ID", id);
                com.ExecuteNonQuery();

                durum = true;
                con.Close();
            }
            catch
            {
                con.Close();
                durum = false;
            }
            return durum;
        }

        public static bool sira_update(string id, string parametre)
        {
            bool durum = false;
            Connection.OpenConnection(ref con);
            // con.Open();
            try
            {
                string script = @" update fast_rapor set fst_sira='" + parametre + "' from dba.fast_rapor where fst_id=:ID";
                OdbcCommand com = new OdbcCommand(script, con);
                com.Parameters.AddWithValue(":ID", id);
                com.ExecuteNonQuery();

                durum = true;
                con.Close();
            }
            catch
            {
                con.Close();
                durum = false;
            }
            return durum;
        }
        public static DataTable sipTurList()
        {
            DataTable dt = new DataTable();
            Connection.OpenConnection(ref con);
            //  con.Open();
            try
            {
                string sorgu = " Select fst_tp_id,fst_tp_kod,fst_tp_ad,fst_tp_bcmno from fast_rapor_tip";
                OdbcDataAdapter da = new OdbcDataAdapter(sorgu, con);
                da.Fill(dt);

                con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                return null;
            }
            return dt;
        }


    }
}

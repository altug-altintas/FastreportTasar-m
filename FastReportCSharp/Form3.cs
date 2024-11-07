using FastReport;
using FastReport.Data;
using FastReportCSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastReportFormDkm
{
    public partial class Form3 : Form
    {
        private Form1 form1;
        public Form3(string an_primno, string form_tipi,string form_sira,string srk_no)
        {
            InitializeComponent();
            form1 = new Form1(Parametre.an_primno, Parametre.form_tipi,Parametre.form_sira,Parametre.srk_no);
            form1.FormClosed += Form1_FormClosed;
            this.ShowInTaskbar = false;

        }
        public Form3(Form1 form1)
        {
            InitializeComponent();
            this.form1 = form1;
            this.form1.FormClosed += Form1_FormClosed; // Form1'in kapatılma olayına abone ol
            this.ShowInTaskbar = false;
        }
        public static string connectionString = "DSN=alt;Uid=dba;encryptedpassword=" + Connection.Pass + ";";

        string selectedPrinter;
        private PaperSize selectedPaperSize;

        private PaperSource selectedPaperSource;

        private void Form3_Load(object sender, EventArgs e)
        {
            int durum = 1;
            DataTable raporTable = Selectler.RaporYazdır(Parametre.form_tipi, durum,Parametre.form_sira);

            if (raporTable.Rows.Count ==1 )
            {
                foreach (DataRow row in raporTable.Rows)
                {
                    string seciliRaporId = row["fst_id"].ToString();
                    Selectler selectlerInstance = new Selectler();
                    byte[] raporDosya = selectlerInstance.GetRaporDosya(seciliRaporId);


                    string tempFilePath = Path.GetTempFileName();
                    File.WriteAllBytes(tempFilePath, raporDosya);
                    FastReport.Report report2 = new FastReport.Report();
                    report2.Load(tempFilePath);
                    report2.SetParameterValue("<an_primno>", Parametre.an_primno);
                    report2.SetParameterValue("<an_yil>", Parametre.yıl);
                    report2.SetParameterValue("<an_dp_no>", Parametre.dp_no);
                    report2.SetParameterValue("<as_irs_seri>", Parametre.irs_seri);
                    report2.SetParameterValue("<an_irs_no>", Parametre.irs_no);
                    report2.SetParameterValue("<an_srk_no>", Parametre.srk_no);
                    report2.Show();
                    this.Close();
                }
            }
            else
            {
                if (raporTable.Rows.Count==0)
                {
                    MessageBox.Show("Rapor tasarımı mevcut değil");
                    this.Close();
                }
                else
                {
                    form1.Show();
                    this.Hide();
                }
               

                
            }

        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close(); // Form1 kapatıldığında Form3'ü de kapat
        }

    }
}

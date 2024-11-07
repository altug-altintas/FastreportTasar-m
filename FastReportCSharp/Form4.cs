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
    public partial class Form4 : Form
    {

        private Form1 form1;


        public Form4()
        {
            InitializeComponent();
            //form1 = new Form1(Parametre.an_primno, Parametre.form_tipi, Parametre.form_sira, Parametre.srk_no);
            //form1.FormClosed += Form1_FormClosed;
            //this.ShowInTaskbar = false;
        }


        //public Form4(Form1 form1)
        //{
        //    //InitializeComponent();
        //    //this.form1 = form1;
        //    //this.form1.FormClosed += Form1_FormClosed; // Form1'in kapatılma olayına abone ol
        //    //this.ShowInTaskbar = false;
        //}

        public static string connectionString = "DSN=alt;Uid=dba;encryptedpassword=" + Connection.Pass + ";";

        string selectedPrinter;
        private PaperSize selectedPaperSize;

        private PaperSource selectedPaperSource;


        private void Form4_Load(object sender, EventArgs e)
        {
            
             int durum = 1;
        //   DataTable raporTable = Selectler.RaporYazdır(Parametre.form_tipi, durum, Parametre.form_sira);

          
                    string seciliRaporId = Parametre.srk_no;
                    Selectler selectlerInstance = new Selectler();
                    byte[] raporDosya = selectlerInstance.GetRaporDosya(seciliRaporId);


                    string tempFilePath = Path.GetTempFileName();
                    File.WriteAllBytes(tempFilePath, raporDosya);
                    FastReport.Report report2 = new FastReport.Report();
                    report2.Load(tempFilePath);                    
                    report2.Show();
                    this.Close();
            
            
          

        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close(); // Form1 kapatıldığında Form3'ü de kapat
        }
    }
}

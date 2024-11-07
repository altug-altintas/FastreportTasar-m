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

namespace FastReportCSharp
{
    public partial class Form1 : Form
    {
        public Form1(string an_primno, string form_tipi,string form_sira, string srk_no)
        {
            InitializeComponent();
        }
        public static string connectionString = "DSN=alt;Uid=dba;encryptedpassword=" + Connection.Pass + ";";

        string selectedPrinter;
        private PaperSize selectedPaperSize;

        private PaperSource selectedPaperSource;
        
        private void Form1_Load_1(object sender, EventArgs e)
        {


            int durum = 1;
            DataTable raporTable = Selectler.RaporListelemeDKm(Parametre.form_tipi,durum,Parametre.form_sira,Parametre.srk_no);

            if (raporTable != null)
            {
                if (raporTable != null)
                {
                    comboBox1.DisplayMember = "fst_tanımı";
                    comboBox1.ValueMember = "fst_tanımı";
                    comboBox1.DataSource = raporTable;
                    
                }
            }
            else
            {
                MessageBox.Show("Raporlar yüklenirken bir hata oluştu. Program kapatılacak.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit(); 
            }
        }


        bool RaporTasariMMi = false;
       
        private void button1_Click(object sender, EventArgs e)
        {
            int kopyasay = Convert.ToInt32(numericUpDown1.Value.ToString());
            DataRowView selectedRapor = (DataRowView)comboBox1.SelectedItem;

            string seciliRaporId = selectedRapor["fst_id"].ToString();

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

            report2.PrintSettings.Copies = kopyasay;



            report2.Show();
            File.Delete(tempFilePath);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.PrinterName = printDialog.PrinterSettings.PrinterName;
                selectedPrinter = printDialog.PrinterSettings.PrinterName;

                PageSettings pageSettings = printDialog.PrinterSettings.DefaultPageSettings;
                selectedPaperSize = pageSettings.PaperSize;
                selectedPaperSource = pageSettings.PaperSource;

            }

        }

      

        private void btnStok_Click_1(object sender, EventArgs e)
        {

            int kopyasay = Convert.ToInt32(numericUpDown1.Value.ToString());
            DataRowView selectedRapor = (DataRowView)comboBox1.SelectedItem;

            string seciliRaporId = selectedRapor["fst_id"].ToString();

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


            // report2.PrintSettings
            report2.PrintSettings.ShowDialog = false;
            report2.PrintSettings.Copies = kopyasay;
            if (selectedPrinter != null)
            {
                report2.PrintSettings.Printer = selectedPrinter;

            }
            report2.Print();
            File.Delete(tempFilePath);

        }

       
    }
}

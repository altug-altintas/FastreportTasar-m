using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace FastReportCSharp
{
    public partial class Form2 : Form
    {
        public Form2(string app_ac ,string fst_srk_no)
        {
            InitializeComponent();
        }
       // public static string connectionString = "DSN="+Connection.Database+"Uid=dba;encryptedpassword=" + Connection.Pass + ";";


        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {


        }
       
        private string selectedDosyaAdi;
        private byte[] selectedDosyaBinary;
        private byte[] updateDosyaBinary;
        private string _selectedFstSira;
        int eskiComboBox4Degeri = -1;
        private bool comboBox3Changing = false;
        DataTable dtSipTur;


        private void Form2_Load(object sender, EventArgs e)
        {

            dtSipTur = Selectler.sipTurList();
            comboBox1.ValueMember = "fst_tp_kod";
            comboBox1.DisplayMember = "fst_tp_ad";
            comboBox2.ValueMember = "fst_tp_kod";
            comboBox2.DisplayMember = "fst_tp_ad";

            comboBox1.ValueMember = "fst_tp_id";
            comboBox2.ValueMember = "fst_tp_id";


            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            foreach (DataRow row in dtSipTur.Rows)
            {
                comboBox1.Items.Add(row["fst_tp_ad"]);
                comboBox2.Items.Add(row["fst_tp_ad"]);
            }

            comboBox5.SelectedIndex = 0;

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int selectedIndex = comboBox1.SelectedIndex;
            DataRow selectedRow = dtSipTur.Rows[selectedIndex];
            string fst_tp_kod = selectedRow["fst_tp_kod"].ToString();
            string fst_tp_bcmno = selectedRow["fst_tp_bcmno"].ToString();
            string fst_tp_id = selectedRow["fst_tp_id"].ToString();



            string selectedİndex_tur = comboBox5.SelectedItem.ToString();

            string rapor_tanımı = textBox1.Text;
           

            

            bool durum = Selectler.I_DOSYAKAYIT(selectedDosyaAdi, selectedDosyaBinary, fst_tp_kod, rapor_tanımı, selectedİndex_tur,Parametre.srk_no, fst_tp_bcmno,fst_tp_id);
            if (durum == true && rapor_tanımı != "" && fst_tp_kod != "")
            {
                MessageBox.Show("Raporunuz kayıt edildi.");
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Hata oluştu! Rapor adını veya Tasarımı kontrol ediniz");
            }
           


        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = comboBox2.SelectedIndex;
            DataRow selectedRow = dtSipTur.Rows[selectedIndex];
            string fst_tp_kod = selectedRow["fst_tp_kod"].ToString();
            string fst_tp_id = selectedRow["fst_tp_id"].ToString();



            int rapor_durum = checkBox1.Checked ? 0 : 1;


            DataTable raporlar = Selectler.RaporListeleme(fst_tp_kod, rapor_durum,Parametre.srk_no, fst_tp_id);

            if (raporlar != null && raporlar.Rows.Count > 0)
            {
                comboBox2.Refresh();
                comboBox3.DisplayMember = "fst_tanımı";
                comboBox3.ValueMember = "fst_tanımı";
                comboBox3.DataSource = raporlar;


                string fstSiraValue = raporlar.Rows[0]["fst_sira"].ToString();
                int fstsira = Convert.ToInt32(fstSiraValue);

                eskiComboBox4Degeri = comboBox2.SelectedIndex;

                comboBox4.SelectedItem = fstsira;
                comboBox4.Refresh();

            }
            else
            {

                MessageBox.Show("Raporlar listesi boş.");
                if (eskiComboBox4Degeri != -1)
                {
                    comboBox2.SelectedIndex = eskiComboBox4Degeri;
                }

            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            DataRowView selectedRapor = (DataRowView)comboBox3.SelectedItem;
            string seciliRaporId = selectedRapor["fst_id"].ToString();
            string seciliRaporAd = selectedRapor["fst_ad"].ToString();
            Selectler selectlerInstance = new Selectler();
            byte[] raporDosya = selectlerInstance.GetRaporDosya(seciliRaporId);
            // Geçici dosyayı oluştur
            string tempFilePath = Path.GetTempFileName();

            // Raporu geçici dosyaya kaydet
            File.WriteAllBytes(tempFilePath, raporDosya);

            // Raporu yükle ve tasarım modunu aç
            FastReport.Report report2 = new FastReport.Report();
            report2.Load(tempFilePath);
            string connectionString = "DSN=" + Connection.Database + ";Uid=dba;pwd=" + Connection.Password + ";";

             report2.Dictionary.Connections[0].ConnectionString = connectionString;

            report2.Design();
            // Geçici dosyayı .frx olarak kaydet            
            string raporad = seciliRaporAd.TrimEnd(".frx".ToCharArray());
            string yeniFrxDosyaYolu = Path.Combine(Path.GetDirectoryName(tempFilePath), raporad + ".frx");
            if (File.Exists(yeniFrxDosyaYolu))
            {
                File.Delete(yeniFrxDosyaYolu);
            }
            File.Move(tempFilePath, yeniFrxDosyaYolu);
            byte[] dosyaBinary = File.ReadAllBytes(yeniFrxDosyaYolu);
            updateDosyaBinary = dosyaBinary;

            Selectler.UpdateRaporTasarim(seciliRaporId, updateDosyaBinary);

            // Rapor dosyasını hedef klasöre taşı
            string hedefKlasor = Path.Combine(Application.StartupPath, "Rapor");
            string hedefDosyaYolu = Path.Combine(hedefKlasor, $"{raporad}.frx");

            if (File.Exists(hedefDosyaYolu))
            {
                File.Delete(hedefDosyaYolu);
            }
            File.Move(yeniFrxDosyaYolu, hedefDosyaYolu);

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "FRX Files (*.frx)|*.frx";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string dosyaYolu = openFileDialog.FileName;
                    string dosyaAdi = Path.GetFileName(dosyaYolu);
                    byte[] dosyaBinary = File.ReadAllBytes(dosyaYolu);
                    selectedDosyaAdi = dosyaAdi;
                    selectedDosyaBinary = dosyaBinary;
                    label2.Text = dosyaAdi;
                }
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {
                checkBox2.Visible = true;
                checkBox2.Text = "Aktif Yap";
            }
            if (checkBox1.CheckState == CheckState.Unchecked)
            {
                checkBox2.Visible = true;
                checkBox2.Text = "Pasif Yap";
            }
            int selectedIndex = comboBox2.SelectedIndex;
            DataRow selectedRow = dtSipTur.Rows[selectedIndex];
            string fst_tp_kod = selectedRow["fst_tp_kod"].ToString();
            string fst_tp_id = selectedRow["fst_tp_id"].ToString();



            string rapor_tip = fst_tp_kod;
            int rapor_durum;
            if (checkBox1.CheckState == CheckState.Checked)
            {
                comboBox2.Refresh();
                DataTable raporlar = Selectler.RaporListeleme(rapor_tip, rapor_durum = 0,Parametre.srk_no, fst_tp_id);
                comboBox3.DisplayMember = "fst_tanımı";
                comboBox3.ValueMember = "fst_tanımı";
                comboBox3.DataSource = raporlar;
            }
            else
            {
                comboBox2.Refresh();
                DataTable raporlar = Selectler.RaporListeleme(rapor_tip, rapor_durum = 1,Parametre.srk_no, fst_tp_id);
                comboBox3.DisplayMember = "fst_tanımı";
                comboBox3.ValueMember = "fst_tanımı";
                comboBox3.DataSource = raporlar;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.CheckState == CheckState.Checked)
            {
                button4.Visible = true;
            }
            if (checkBox2.CheckState == CheckState.Unchecked)
            {
                button4.Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataRowView selectedRapor = (DataRowView)comboBox3.SelectedItem;
            DialogResult result = MessageBox.Show("Seçili raporda işlem yapmak  istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string seciliRaporId = selectedRapor["fst_id"].ToString();
                string seciliRaporDurum = selectedRapor["fst_durum"].ToString();
                int İnt_durume = Convert.ToInt32(seciliRaporDurum);


                int parametre = İnt_durume switch
                {
                    1 => 0,
                    0 => 1,
                    _ => 2
                };

                int selectedIndex = comboBox2.SelectedIndex;
                DataRow selectedRow = dtSipTur.Rows[selectedIndex];
                string fst_tp_kod = selectedRow["fst_tp_kod"].ToString();
                string fst_tp_id = selectedRow["fst_tp_id"].ToString();


                int rapor_durum = checkBox1.Checked ? 0 : 1;
                string siraListesi = Selectler.IsDuplicateFstSira(seciliRaporId);

                if (checkBox2.Checked)
                {
                    Selectler.Aktif_pasif(seciliRaporId, parametre);


                }

                else if (siraListesi != comboBox4.SelectedItem)
                {
                    Selectler.sira_update(seciliRaporId, comboBox4.SelectedItem.ToString());
                }
                else
                {

                }


                DataTable raporlar = Selectler.RaporListeleme(fst_tp_kod, rapor_durum,Parametre.srk_no, fst_tp_id);
                comboBox3.Items.Remove(seciliRaporId); // Seçilen öğeyi kaldır
                comboBox3.DisplayMember = "fst_tanımı";
                comboBox3.ValueMember = "fst_tanımı";
                comboBox3.DataSource = raporlar;

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && comboBox1.Text != "")
            {
                string uygulamaKlasoru = Path.GetDirectoryName(Application.ExecutablePath);

                int selectedIndex = comboBox1.SelectedIndex;
                DataRow selectedRow = dtSipTur.Rows[selectedIndex];
                string fst_tp_kod = selectedRow["fst_tp_kod"].ToString();

                string index = "";
               // string kaynakDosyaYolu = "";

                string frxadı = $"{fst_tp_kod}.frx";
                string kaynakDosyaYolu = Path.Combine(uygulamaKlasoru, "BaseRapor", frxadı);

               
                string rapor_tip = index;

                string hedefDosyaYolu = Path.Combine(uygulamaKlasoru, "Rapor", $"{textBox1.Text}.frx");

                if (File.Exists(hedefDosyaYolu))
                {
                    DialogResult result = MessageBox.Show("Aynı isimle form mevcut lütfen başka bir form adı deneyiniz ", "Onay"); //MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        // File.Delete(hedefDosyaYolu);
                    }
                }
                else
                {
                    File.Copy(kaynakDosyaYolu, hedefDosyaYolu, true);
                    MessageBox.Show("Yeni form oluşturuldu. Lütfen tasarım ekranını bekleyiniz", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    string dosyaAdi = Path.GetFileName(hedefDosyaYolu);
                    label2.Text = dosyaAdi;
                    Process process = new Process();
                    process.StartInfo.FileName = hedefDosyaYolu;
                    process.EnableRaisingEvents = true;
                    process.Exited += (sender, e) =>
                    {
                        string dosyaAdi = Path.GetFileName(hedefDosyaYolu);
                        byte[] dosyaBinary = File.ReadAllBytes(hedefDosyaYolu);
                        selectedDosyaBinary = dosyaBinary;
                        selectedDosyaAdi = dosyaAdi;
                    };
                    try
                    {


                        process.Start();
                        process.WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Rapor adı alanı boş");
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3Changing = true;

            DataRowView selectedRow = comboBox3.SelectedItem as DataRowView;

            string selectedFstSira = selectedRow["fst_sira"].ToString();

            comboBox4.SelectedItem = selectedFstSira;

            _selectedFstSira = selectedFstSira.ToString();

            comboBox3Changing = false;
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex == -1 || comboBox3Changing)
                return;

            string selectedValue = comboBox4.SelectedItem.ToString();

            if (selectedValue != _selectedFstSira)
            {
                button4.Visible = true;
            }
            else
            {
                button4.Visible = false;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

using FastReport.Data;
using FastReport.DevComponents.DotNetBar.Controls;
using FastReportFormDkm;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastReportCSharp
{

    class Parametre
    {
        public static string an_primno;
        public static string form_tipi;
        public static string app_tipi;
        public static string form_sira;
        public static string app_ac;
        public static string srk_no;
        public static string bcm_no;
        public static string dp_no;
        public static string irs_seri;
        public static string irs_no;
        public static string yıl;
        
        


    }

    static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main(string[] args)//
        {
            //Parametre.app_ac = "FST1";
            //Parametre.srk_no = "1";

            //Parametre.bcm_no = "100";

            //Parametre.an_primno = "26820";
            //Parametre.form_tipi = "stkf_b0200_cek";
            //Parametre.app_tipi = "F";
            //Parametre.form_sira = "1";
            //Parametre.yıl = "2024";
            //Parametre.dp_no = "35";
            //Parametre.irs_seri = "BKS";
            //Parametre.irs_no = "141";

            Parametre.app_ac = args.Length > 0 ? args[0] : null;
            Parametre.srk_no = args.Length > 1 ? args[1] : null;
            Parametre.an_primno = args.Length > 2 ? args[2] : null;
            Parametre.form_tipi = args.Length > 3 ? args[3] : null;
            Parametre.form_sira = args.Length > 4 ? args[4] : null;
            Parametre.app_tipi = args.Length > 5 ? args[5] : null;
            Parametre.yıl = args.Length > 6 ? args[6] : null;
            Parametre.dp_no = args.Length > 7 ? args[7] : null;
            Parametre.irs_seri = args.Length > 8 ? args[8] : null;
            Parametre.irs_no = args.Length > 9 ? args[9] : null;





            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Parametre.app_tipi == "F")
            {
                if (Parametre.app_ac == "FST1")
                {
                    Application.Run(new Form3(Parametre.an_primno, Parametre.form_tipi, Parametre.form_sira,Parametre.srk_no));

                }
                else
                {
                    MessageBox.Show("Hata: Lütfen Osoft ile iletişime geçiniz. Program sonlandırılıyor.");
                    return;
                }


            }
           else
            {
                if (Parametre.app_ac == "FST1")
                {
                    Application.Run(new Form2(Parametre.app_ac, Parametre.srk_no));

                }
                else if(Parametre.app_ac == "FST2")
                {
                    Application.Run(new Form4());
                    //Parametre.app_ac, Parametre.srk_no)
                }
                else
                {
                    MessageBox.Show("Hata: Lütfen Osoft ile iletişime geçiniz. Program sonlandırılıyor.");
                    return;
                }

            }
            
        }


    }
}

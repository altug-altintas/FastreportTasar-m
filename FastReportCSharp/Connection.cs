using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastReportCSharp
{
    public class Connection
    {
        private static string stringconn = "";
        public static string Datemode = "";
        public static string server = "";
        public static string Database = "";
        public static string User = "";
        public static string Password = "";
        public static string Pass = "";
        public static OdbcConnection con = null;
        public static void ConnectionString()
        {
            try
            {
                IniFiles inifiles = new IniFiles();
                inifiles.IniFile = Application.StartupPath + @"\DataBase.ini";
                Database = inifiles.ReadIni("DB", "Database").ToString();
                Pass = inifiles.ReadIni("DB", "Pass").ToString();
                User = "dba";
                Password = Encrypt.Config_Decrypt(Pass);
                stringconn = "Dsn=" + Database + ";Uid=" + User + ";Pwd=" + Password + ";";
            }
            catch
            {
                return;
            }
        }
        public static IDbConnection OpenConnection(ref OdbcConnection oConn)
        {
            try
            {
                ConnectionString();
                oConn = new OdbcConnection(stringconn);
                if (oConn.State != ConnectionState.Open)
                {
                    oConn.Open();
                }
            }
            catch (Exception ex)
            {

            }
            return oConn;
        }

    }
}

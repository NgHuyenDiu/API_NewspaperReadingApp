using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace api_test.helper
{
    public class SqlHelper
    {
        public static SqlConnection conn = new SqlConnection();
        public static String connstr;
        public static SqlDataReader myReader;
        public static String servername = "DESKTOP-J64E1ON";
        public static String username = "";
        public static String mlogin = "sa";
        public static String password = "123";
        public static String database = "NewspaperReadingApp";
       
        public static int KetNoi()
        {
            if (SqlHelper.conn != null && SqlHelper.conn.State == ConnectionState.Open)
                SqlHelper.conn.Close();
            try
            {
                SqlHelper.connstr = "Data Source=" + SqlHelper.servername + ";Initial Catalog=" +
                      SqlHelper.database + ";User ID=" +
                      SqlHelper.mlogin + ";password=" + SqlHelper.password;
                Console.WriteLine(SqlHelper.connstr);
                SqlHelper.conn.ConnectionString = SqlHelper.connstr;
                SqlHelper.conn.Open();
                return 1;
            }

            catch (Exception e)
            {
              
                return 0;
            }
        }
        public static SqlDataReader ExecSqlDataReader(String strLenh)
        {
            SqlDataReader myreader;
            SqlCommand sqlcmd = new SqlCommand(strLenh, SqlHelper.conn);
            sqlcmd.CommandType = CommandType.Text;
            if (SqlHelper.conn.State == ConnectionState.Closed) SqlHelper.conn.Open();
            try
            {
                myreader = sqlcmd.ExecuteReader(); return myreader;
            }
            catch (SqlException ex)
            {
                SqlHelper.conn.Close();
             
                return null;
            }
        }

        public static int ExecSqlNonQuery(String strLenh)
        {
            SqlCommand sqlcmd = new SqlCommand(strLenh, SqlHelper.conn);
            sqlcmd.CommandType = CommandType.Text;
            sqlcmd.CommandTimeout = 600;
            if (SqlHelper.conn.State == ConnectionState.Closed) SqlHelper.conn.Open();
            try
            {
                sqlcmd.ExecuteNonQuery();
                SqlHelper.conn.Close();
                return 0;
            }
            catch (SqlException e)
            {


                SqlHelper.conn.Close();
                return e.State;// trạng thái lỗi gởi từ raiseError trogn sql server qua
            }

        }
        public static DataTable ExecSqlDataTable(String cmd)
        {
            DataTable dt = new DataTable();
            if (SqlHelper.conn.State == ConnectionState.Closed) SqlHelper.conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn);
            da.Fill(dt);
            conn.Close();
            return dt;
        }
    }
}

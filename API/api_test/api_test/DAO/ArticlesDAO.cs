using api_test.helper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace api_test.DAO
{
    public class ArticlesDAO
    {
        public static int taoMa()
        {
            int maxMa = 0;
            // mo ket noi
            String lenh = String.Format("EXEC sp_timmaxIdArticles");
            using (SqlConnection connection = new SqlConnection(SqlHelper.connstr))
            {
                connection.Open();
                SqlCommand sqlcmt = new SqlCommand(lenh, connection);
                sqlcmt.CommandType = CommandType.Text;
                try
                {
                    maxMa = (Int32)sqlcmt.ExecuteScalar();
                }
                catch
                {

                }
            }
            return maxMa + 1;
        }

        public static DataTable getAll()
        {
            String lenh = String.Format("EXEC getListArticles ");
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(SqlHelper.connstr);
            try
            {
                SqlCommand cmd = new SqlCommand(lenh, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            cn.Close();
            return dt;
        }

        public static DataTable getTopView()
        {
            String lenh = String.Format("EXEC getTopView ");
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(SqlHelper.connstr);
            try
            {
                SqlCommand cmd = new SqlCommand(lenh, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            cn.Close();
            return dt;
        }

        public static DataTable getPageList(int page, int pagesize)
        {
            String lenh = String.Format("EXEC getListArticlesPageSize {0},{1}", page, pagesize);
            DataTable dt = new DataTable();
            SqlConnection cn = new SqlConnection(SqlHelper.connstr);
            try
            {
                SqlCommand cmd = new SqlCommand(lenh, cn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            cn.Close();
            return dt;
        }
    }
}

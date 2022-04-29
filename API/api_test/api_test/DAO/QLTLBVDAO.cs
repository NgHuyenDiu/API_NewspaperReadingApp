using api_test.helper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace api_test.DAO
{
    public class QLTLBVDAO
    {
        public static int taoMa()
        {
            int maxMa = 0;
            // mo ket noi
            String lenh = String.Format("EXEC sp_timmaxIdQLTLBV");
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
    }
}

using api_test.helper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace api_test.DAO
{
    public class HistoryDAO
    {
        public static void deleteAllHistory(int id)
        {

            String lenh = String.Format("EXEC deleteHistory {0}", id);
            using (SqlConnection connection = new SqlConnection(SqlHelper.connstr))
            {
                connection.Open();
                SqlCommand sqlcmt = new SqlCommand(lenh, connection);

                sqlcmt.CommandType = CommandType.Text;
                try
                {
                    sqlcmt.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                connection.Close();
            }
        }

        public static void deleteHistoryById(int id_user, int id_articles , DateTime date)
        {

            String lenh = String.Format(" EXEC deleteHistoryById {0}, {1}, N'{2}' ", id_user, id_articles, date.ToString());
           
            using (SqlConnection connection = new SqlConnection(SqlHelper.connstr))
            {
                connection.Open();
                SqlCommand sqlcmt = new SqlCommand(lenh, connection);

                sqlcmt.CommandType = CommandType.Text;
                try
                {
                    sqlcmt.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                connection.Close();
            }
        }
    }
}

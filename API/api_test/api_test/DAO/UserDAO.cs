﻿using api_test.helper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace api_test.DAO
{
    public class UserDAO
    {
        public static int taoMa()
        {
            int maxMa = 0;
            // mo ket noi
            String lenh = String.Format("EXEC sp_timmaxIdUser ");
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
            String lenh = String.Format("EXEC getListUser ");
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
            String lenh = String.Format("EXEC getListUserPageSize {0},{1}", page, pagesize);
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
        public static DataTable getListHistory(int id)
        {
            String lenh = String.Format("EXEC getHistoryArticles {0}", id);
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

        public static DataTable getListAuthorFavorite(int id_user)
        {
            String lenh = String.Format("EXEC list_author_favourite {0}", id_user);
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


using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EverFresh
{
    public class SqlDataObject
    {
        private string _sqlconn;
        public string SqlConn
        {
            get { return _sqlconn; }
            set { _sqlconn = value; }
        }

        private string _sqlcomm;
        public string SqlComm
        {
            get { return _sqlcomm; }
            set { _sqlcomm = value; }
        }

        public SqlDataObject()
        {
            _sqlconn = ConfigurationManager.ConnectionStrings["database"].ToString();
        }

        public SqlDataObject(string conn)
        {
            _sqlconn = conn;
        }

        public DataTable GetDataTable(params SqlParameter[] param)
        {
            return GetDataTable(CommandType.Text, param);
        }

        public DataTable GetDataTable(CommandType type, params SqlParameter[] param)
        {
            DataTable dt = new DataTable();
            GetDataTable(dt, type, param);
            return dt;
        }

        public void GetDataTable(DataTable dt, params SqlParameter[] param)
        {
            GetDataTable(dt, CommandType.Text, param);
        }

        public void GetDataTable(DataTable dt, CommandType type, params SqlParameter[] param)
        {
            SqlConnection connection = new SqlConnection(_sqlconn);
            SqlDataAdapter adpter = new SqlDataAdapter(_sqlcomm, connection);
            adpter.SelectCommand.CommandType = type;
            adpter.SelectCommand.CommandTimeout = 300;
            if (param != null)
            {
                foreach (SqlParameter item in param)
                {
                    adpter.SelectCommand.Parameters.Add(item);
                }
            }
            adpter.Fill(dt);
        }

        private void BuildSqlCommand(string filter)
        {
            string sql = _sqlcomm;
            if (filter != null && filter.Trim() != "" &&
                sql.Trim().ToUpper().LastIndexOf("WHERE") < sql.Trim().ToUpper().LastIndexOf("FROM"))
            {
                sql += " WHERE " + filter;
            }
            else if (filter != null && filter.Trim() != "")
            {
                sql += " AND " + filter;
            }
            _sqlcomm = sql;
        }

        public DataTable GetFilteredDataTable(string filter)
        {
            return GetFilteredDataTable(filter, null);
        }

        public DataTable GetFilteredDataTable(string filter, params SqlParameter[] param)
        {
            BuildSqlCommand(filter);
            return GetDataTable(param);
        }

        public void GetFilteredDataTable(DataTable dt, string filter)
        {
            GetFilteredDataTable(dt, filter, null);
        }

        public void GetFilteredDataTable(DataTable dt, string filter, params SqlParameter[] param)
        {
            BuildSqlCommand(filter);
            GetDataTable(dt, param);
        }


        public void GetSchema(DataTable dt)
        {
            SqlConnection connection = new SqlConnection(_sqlconn);
            SqlDataAdapter adpter = new SqlDataAdapter(_sqlcomm, connection);
            adpter.FillSchema(dt, SchemaType.Source);
        }

        public int ExecuteNonQuery(params SqlParameter[] param)
        {
            return ExecuteNonQuery(CommandType.Text, param);
        }

        public int ExecuteNonQuery(CommandType type, params SqlParameter[] param)
        {
            int i = 0;
            SqlConnection connection = new SqlConnection(_sqlconn);
            SqlCommand command = new SqlCommand(_sqlcomm, connection);
            command.CommandType = type;
            if (param != null)
            {
                foreach (SqlParameter item in param)
                {
                    command.Parameters.Add(item);
                }
            }
            connection.Open();
            i = command.ExecuteNonQuery();
            connection.Close();
            return i;
        }


        public int Update(DataTable dt)
        {
            int i = 0;
            SqlConnection connection = new SqlConnection(_sqlconn);
            SqlDataAdapter adapter = new SqlDataAdapter(_sqlcomm, connection);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            builder.QuotePrefix = "[";
            builder.QuoteSuffix = "]";
            i = adapter.Update(dt);
            return i;
        }

        public object GetObject(params SqlParameter[] param)
        {
            object obj;
            SqlConnection connection = new SqlConnection(_sqlconn);
            SqlCommand command = new SqlCommand(_sqlcomm, connection);
            if (param != null)
            {
                foreach (SqlParameter item in param)
                {
                    command.Parameters.Add(item);
                }
            }
            connection.Open();
            obj = command.ExecuteScalar();
            connection.Close();
            return obj;
        }

        public static int GetIdentity(string tablename)
        {
            SqlDataObject dbo = new SqlDataObject();
            dbo.SqlComm = "SELECT IDENT_CURRENT('" + tablename + "')";
            return Convert.ToInt32(dbo.GetObject());
        }
    }
}
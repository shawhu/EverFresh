using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

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
        
        public DataTable GetDataTable(params MySqlParameter[] param)
        {
            return GetDataTable(CommandType.Text, param);
        }

        public DataTable GetDataTable(CommandType type, params MySqlParameter[] param)
        {
            DataTable dt = new DataTable();
            GetDataTable(dt, type, param);
            return dt;
        }

        public void GetDataTable(DataTable dt, params MySqlParameter[] param)
        {
            GetDataTable(dt, CommandType.Text, param);
        }

        public void GetDataTable(DataTable dt, CommandType type, params MySqlParameter[] param)
        {
            MySqlConnection connection = new MySqlConnection(_sqlconn);
            MySqlDataAdapter adpter = new MySqlDataAdapter(_sqlcomm, connection);
            adpter.SelectCommand.CommandType = type;
            adpter.SelectCommand.CommandTimeout = 300;
            if (param != null)
            {
                foreach (MySqlParameter item in param)
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

        public DataTable GetFilteredDataTable(string filter, params MySqlParameter[] param)
        {
            BuildSqlCommand(filter);
            return GetDataTable(param);
        }

        public void GetFilteredDataTable(DataTable dt, string filter)
        {
            GetFilteredDataTable(dt, filter, null);
        }

        public void GetFilteredDataTable(DataTable dt, string filter, params MySqlParameter[] param)
        {
            BuildSqlCommand(filter);
            GetDataTable(dt, param);
        }


        public void GetSchema(DataTable dt)
        {
            MySqlConnection connection = new MySqlConnection(_sqlconn);
            MySqlDataAdapter adpter = new MySqlDataAdapter(_sqlcomm, connection);
            adpter.FillSchema(dt, SchemaType.Source);
        }

        public int ExecuteNonQuery(params MySqlParameter[] param)
        {
            return ExecuteNonQuery(CommandType.Text, param);
        }

        public int ExecuteNonQuery(CommandType type, params MySqlParameter[] param)
        {
            int i = 0;
            MySqlConnection connection = new MySqlConnection(_sqlconn);
            MySqlCommand command = new MySqlCommand(_sqlcomm, connection);
            command.CommandType = type;
            if (param != null)
            {
                foreach (MySqlParameter item in param)
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
            MySqlConnection connection = new MySqlConnection(_sqlconn);
            MySqlDataAdapter adapter = new MySqlDataAdapter(_sqlcomm, connection);
            MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
            builder.QuotePrefix = "[";
            builder.QuoteSuffix = "]";
            i = adapter.Update(dt);
            return i;
        }

        public object GetObject(params MySqlParameter[] param)
        {
            object obj;
            MySqlConnection connection = new MySqlConnection(_sqlconn);
            MySqlCommand command = new MySqlCommand(_sqlcomm, connection);
            if (param != null)
            {
                foreach (MySqlParameter item in param)
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
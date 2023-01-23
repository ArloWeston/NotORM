using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace NotORM
{
    public class SqlQryBuilder
    {
        private string _connStr;
        //private string _sql;
        private List<SqlParameter> _sqlParams;
        private int _rtnVal;

        public string SQL { get; set; }

        public SqlQryBuilder(string connStr)
        {
            _connStr = connStr;
            _sqlParams = new List<SqlParameter>();
            SQL = "";
        }

        public SqlQryBuilder AddSQLString(string sql)
        {
            SQL = sql;
            return this;
        }

        public SqlQryBuilder AddParameter(string column, string value)
        {
            SqlParameter sParam;
            if (value == null)
            {
                sParam = new SqlParameter(column, DBNull.Value);
            }
            else
            {
                sParam = new SqlParameter(column, value);
            }

            _sqlParams.Add(sParam);
            return this;
        }


        public SqlQryBuilder AddParameter(string column, DateTime value)
        {
            SqlParameter dateParam = new SqlParameter(column, System.Data.SqlDbType.DateTime);
            if (value == DateTime.MinValue)
            {
                dateParam.Value = DBNull.Value;
            }
            else
            {
                dateParam.Value = value;
            }
            _sqlParams.Add(dateParam);
            return this;
        }

        public SqlQryBuilder AddParameter(string column, int value)
        {
            SqlParameter dateParam = new SqlParameter(column, System.Data.SqlDbType.Int);
            dateParam.Value = value;
            _sqlParams.Add(dateParam);
            return this;
        }

        public SqlQryBuilder AddParameter(string column, int? value)
        {
            SqlParameter dateParam = new SqlParameter(column, System.Data.SqlDbType.Int);
            if (!value.HasValue)
            {
                dateParam.Value = DBNull.Value;
            }
            else
            {
                dateParam.Value = value.Value;
            }
            _sqlParams.Add(dateParam);
            return this;
        }

        public SqlQryBuilder AddParameter(string column, decimal? value)
        {
            SqlParameter dateParam = new SqlParameter(column, System.Data.SqlDbType.Decimal);
            if (!value.HasValue)
            {
                dateParam.Value = DBNull.Value;
            }
            else
            {
                dateParam.Value = value.Value;
            }
            _sqlParams.Add(dateParam);
            return this;
        }

        public SqlQryBuilder AddParameter(string column, DateTime? value)
        {
            SqlParameter dateParam = new SqlParameter(column, System.Data.SqlDbType.DateTime);
            if (!value.HasValue)
            {
                dateParam.Value = DBNull.Value;
            }
            else
            {
                dateParam.Value = value.Value;
            }
            _sqlParams.Add(dateParam);
            return this;
        }

        public SqlQryBuilder AddParameter(string column, bool? value)
        {
            SqlParameter bitParam = new SqlParameter(column, System.Data.SqlDbType.Bit);
            if (!value.HasValue)
            {
                bitParam.Value = DBNull.Value;
            }
            else
            {
                bitParam.Value = value;
            }
            
            _sqlParams.Add(bitParam);
            return this;
        }



        public SqlQryBuilder ClearParameterList()
        {
            _sqlParams = new List<SqlParameter>();
            return this;
        }

        public int BuildNonQuery()
        {
            _rtnVal = 0;

            string sConnection = _connStr;
            using (SqlConnection conn = new SqlConnection(sConnection))
            {
                SqlCommand command = new SqlCommand(SQL, conn);

                if (_sqlParams.Count > 0)
                {
                    foreach (SqlParameter p in _sqlParams)
                    {
                        command.Parameters.Add(p);
                    }
                }

                conn.Open();
                _rtnVal = command.ExecuteNonQuery();

            }

            return _rtnVal;
        }


        public async Task<int> BuildNonQueryAsync()
        {
            _rtnVal = 0;

            string sConnection = _connStr;
            using (SqlConnection conn = new SqlConnection(sConnection))
            {
                SqlCommand command = new SqlCommand(SQL, conn);

                if (_sqlParams.Count > 0)
                {
                    foreach (SqlParameter p in _sqlParams)
                    {
                        command.Parameters.Add(p);
                    }
                }

                conn.Open();
                _rtnVal = await command.ExecuteNonQueryAsync();

            }

            return _rtnVal;
        }

        public async Task<List<string>> BuildListStringQueryAsync()
        {
            List<string> rtnList = new List<string>();

            string sConnection = _connStr;
            using (SqlConnection conn = new SqlConnection(sConnection))
            {
                SqlCommand command = new SqlCommand(SQL, conn);

                if (_sqlParams.Count > 0)
                {
                    foreach (SqlParameter p in _sqlParams)
                    {
                        command.Parameters.Add(p);
                    }
                }

                conn.Open();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    string val = reader[0].ToString();
                    rtnList.Add(val);
                }
            }

            return rtnList;
        }

    }

}

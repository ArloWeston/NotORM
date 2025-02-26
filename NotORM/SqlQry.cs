using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotORM
{

    

    public class SqlQry : ISqlQry
    {
        public string ConnStr { get; set; }
        public string SQL { get; set; }
        public List<SqlParameter> SqlParams { get; set; }
        public string ClassMappingErrors { get; set; }
        public string WhereCl { get; set; }
        public string OrderByCl { get; set; }

        public SqlQry(string connStr) 
        {
            ConnStr = connStr;
            SqlParams = new List<SqlParameter>();
            SQL = string.Empty;
            ClassMappingErrors = string.Empty;
            WhereCl = string.Empty;
            OrderByCl = string.Empty;
        }


        public void AddParameter(string column, string value)
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

            SqlParams.Add(sParam);
            
        }

        public void AddParameter(string column, DateTime value)
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
            SqlParams.Add(dateParam);

        }

        public void AddParameter(string column, int value)
        {
            SqlParameter iParam = new SqlParameter(column, System.Data.SqlDbType.Int);
            iParam.Value = value;
            SqlParams.Add(iParam);

        }

        public void AddParameter(string column, int? value)
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

            SqlParams.Add(sParam);

        }

        public void AddParameter(string column, decimal? value)
        {
            SqlParameter sParam = new SqlParameter(column, System.Data.SqlDbType.Decimal);
            if (!value.HasValue)
            {
                sParam.Value = DBNull.Value;
            }
            else
            {
                sParam.Value = value.Value;
            }
            SqlParams.Add(sParam);

        }

        public void AddParameter(string column, DateTime? value)
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

            SqlParams.Add(dateParam);

        }

        public void AddParameter(string column, bool? value)
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
            SqlParams.Add(bitParam);

        }


        public void AddParameter(string column, byte[] value)
        {
            SqlParameter bitParam = new SqlParameter(column, System.Data.SqlDbType.VarBinary);
            if (value == null)
            {
                bitParam.Value = DBNull.Value;
            }
            else
            {
                bitParam.Value = value;
            }
            SqlParams.Add(bitParam);

        }


        public void ClearParameterList()
        {
            SqlParams = new List<SqlParameter>();

        }

        public int NonQuery()
        {
            int rtnVal = 0;

            string sConnection = ConnStr;
            using (SqlConnection conn = new SqlConnection(sConnection))
            {
                if (WhereCl.Trim().Length > 0)
                {
                    SQL += " " + WhereCl;
                }
                if (OrderByCl.Trim().Length > 0)
                {
                    SQL += " " + OrderByCl;
                }

                SqlCommand command = new SqlCommand(SQL, conn);

                if (SqlParams.Count > 0)
                {
                    foreach (SqlParameter p in SqlParams)
                    {
                        command.Parameters.Add(p);
                    }
                }

                conn.Open();
                rtnVal = command.ExecuteNonQuery();

            }

            return rtnVal;
        }

        public async Task<int> NonQueryAsync()
        {
            int rtnVal = 0;

            string sConnection = ConnStr;
            using (SqlConnection conn = new SqlConnection(sConnection))
            {
                if (WhereCl.Trim().Length > 0)
                {
                    SQL += " " + WhereCl;
                }
                if (OrderByCl.Trim().Length > 0)
                {
                    SQL += " " + OrderByCl;
                }

                SqlCommand command = new SqlCommand(SQL, conn);

                if (SqlParams.Count > 0)
                {
                    foreach (SqlParameter p in SqlParams)
                    {
                        command.Parameters.Add(p);
                    }
                }

                conn.Open();
                rtnVal = await command.ExecuteNonQueryAsync();

            }

            return rtnVal;
        }

        public async Task<List<string>> ListStringQueryAsync()
        {
            List<string> rtnList = new List<string>();

            string sConnection = ConnStr;
            using (SqlConnection conn = new SqlConnection(sConnection))
            {
                if (WhereCl.Trim().Length > 0)
                {
                    SQL += " " + WhereCl;
                }
                if (OrderByCl.Trim().Length > 0)
                {
                    SQL += " " + OrderByCl;
                }

                SqlCommand command = new SqlCommand(SQL, conn);

                if (SqlParams.Count > 0)
                {
                    foreach (SqlParameter p in SqlParams)
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

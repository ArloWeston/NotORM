using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace NotORM
{
    public class SqlQryBuilder<T> where T : class, new()
    {
        private string _connStr;
        //private string _sql;
        private List<SqlParameter> _sqlParams;
        private List<T>? _rtnList;
        public string ClassMappingErrors { get; set; }


        public string SQL { get; set; }

        public SqlQryBuilder(string connStr)
        {
            _connStr = connStr;
            ClassMappingErrors = "";
            _sqlParams = new List<SqlParameter>();
            SQL = "";
        }

        public SqlQryBuilder<T> AddSQLString(string sql)
        {
            SQL = sql;
            return this;
        }

        public SqlQryBuilder<T> AddParameter(string column, string value)
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

        public SqlQryBuilder<T> AddParameter(string column, DateTime value)
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

        public SqlQryBuilder<T> AddParameter(string column, int value)
        {
            SqlParameter dateParam = new SqlParameter(column, System.Data.SqlDbType.Int);
            dateParam.Value = value;
            _sqlParams.Add(dateParam);
            return this;
        }

        public SqlQryBuilder<T> AddParameter(string column, int? value)
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

        public SqlQryBuilder<T> AddParameter(string column, decimal? value)
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

        public SqlQryBuilder<T> AddParameter(string column, DateTime? value)
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

        public SqlQryBuilder<T> AddParameter(string column, bool value)
        {
            SqlParameter bitParam = new SqlParameter(column, System.Data.SqlDbType.Bit);
            bitParam.Value = value;
            _sqlParams.Add(bitParam);
            return this;
        }

        public SqlQryBuilder<T> ClearParameterList()
        {
            _sqlParams = new List<SqlParameter>();
            return this;
        }

        public List<T> Build()
        {
            _rtnList = new List<T>();

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
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            if (reader[prop.Name] != DBNull.Value)
                            {
                                PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);

                                if (propertyInfo.PropertyType.Name == "Nullable`1")
                                {
                                    // code take from: https://stackoverflow.com/questions/3531318/convert-changetype-fails-on-nullable-types
                                    Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                                    object safeValue = (reader[prop.Name] == null) ? null : Convert.ChangeType(reader[prop.Name], t);
                                    propertyInfo.SetValue(obj, safeValue, null);
                                    //var temp = Convert.ChangeType(reader[prop.Name], propertyInfo.PropertyType);
                                    //propertyInfo.SetValue(obj, temp, null);
                                }
                                else
                                {
                                    propertyInfo.SetValue(obj, Convert.ChangeType(reader[prop.Name], propertyInfo.PropertyType), null);
                                }

                            }

                        }
                        catch (Exception e)
                        {
                            ClassMappingErrors += "  " + prop.Name + " Error: " + e.Message;
                        }
                    }

                    _rtnList.Add(obj);

                }

            }

            return _rtnList;
        }




        public async Task<List<T>> BuildAsync()
        {
            _rtnList = new List<T>();

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
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            if (reader[prop.Name] != DBNull.Value)
                            {
                                PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);

                                if (propertyInfo.PropertyType.Name == "Nullable`1")
                                {
                                    // code taken from: https://stackoverflow.com/questions/3531318/convert-changetype-fails-on-nullable-types
                                    Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                                    object safeValue = (reader[prop.Name] == null) ? null : Convert.ChangeType(reader[prop.Name], t);
                                    propertyInfo.SetValue(obj, safeValue, null);
                                }
                                else
                                {
                                    propertyInfo.SetValue(obj, Convert.ChangeType(reader[prop.Name], propertyInfo.PropertyType), null);
                                }

                            }

                        }
                        catch (Exception e)
                        {
                            ClassMappingErrors += "  " + prop.Name + " Error: " + e.Message;
                        }
                    }

                    _rtnList.Add(obj);
                }
            }

            return _rtnList;
        }


    }
}

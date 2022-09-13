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
        private string _database;
        private string _sql;
        //private Dictionary<string, string> _params;
        //private Dictionary<string, DateTime> _paramsDate;
        private List<SqlParameter> _sqlParams;
        private List<T> _rtnList;
        //private string _errors;
        public string Errors { get; set; }
        //public readonly string CONST_DB_NULL = "AddDbNull";

        //public readonly string CONST_DB_NULL2 { get; }


        public SqlQryBuilder(string database)
        {
            _database = database;
            Errors = "";
            _sqlParams = new List<SqlParameter>();
        }

        public SqlQryBuilder<T> AddSQLString(string sql)
        {
            _sql = sql;
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

        public SqlQryBuilder<T> RunQuery()
        {
            _rtnList = new List<T>();
            try
            {

                string sConnection = DbConnStr.RtnStr(_database);
                using (SqlConnection conn = new SqlConnection(sConnection))
                {
                    SqlCommand command = new SqlCommand(_sql, conn);
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
                                Errors += " class mapping error: " + e.Message;
                            }
                        }

                        _rtnList.Add(obj);

                    }

                }
            }
            catch (Exception e)
            {
                Errors += " " + e.Message;
            }
            return this;
        }




        public async Task<List<T>> RunQueryAsync()
        {
            _rtnList = new List<T>();
            try
            {

                string sConnection = DbConnStr.RtnStr(_database);
                using (SqlConnection conn = new SqlConnection(sConnection))
                {
                    SqlCommand command = new SqlCommand(_sql, conn);

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
                                Errors += " class mapping error: " + e.Message;
                            }
                        }

                        _rtnList.Add(obj);
                    }
                }
            }
            catch (Exception e)
            {
                Errors += " " + e.Message;
            }
            return _rtnList;
        }



        public List<T> ReturnResult()
        {
            return _rtnList;
        }

        public SqlQryBuilder<T> GetErrors(out string errors)
        {
            errors = Errors;
            return this;
        }



        //public SqlQryBuilder<T> LogErrors(Logger logger)
        //{
        //    //log any errors here...
        //}

    }


    public class SqlQryBuilder
    {
        private string _database;
        private string _sql;
        private List<SqlParameter> _sqlParams;
        private int _rtnVal;
        public string Errors { get; set; }


        public SqlQryBuilder(string database)
        {
            _database = database;
            Errors = "";
            _sqlParams = new List<SqlParameter>();
        }

        public SqlQryBuilder AddSQLString(string sql)
        {
            _sql = sql;
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

        public SqlQryBuilder ClearParameterList()
        {
            _sqlParams = new List<SqlParameter>();
            return this;
        }

        public SqlQryBuilder RunNonQuery()
        {
            _rtnVal = 0;
            try
            {

                string sConnection = DbConnStr.RtnStr(_database);
                using (SqlConnection conn = new SqlConnection(sConnection))
                {
                    SqlCommand command = new SqlCommand(_sql, conn);

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
            }
            catch (Exception e)
            {
                Errors += " " + e.Message;
            }
            return this;
        }


        public async Task<int> RunNonQueryAsync()
        {
            _rtnVal = 0;
            try
            {

                string sConnection = DbConnStr.RtnStr(_database);
                using (SqlConnection conn = new SqlConnection(sConnection))
                {
                    SqlCommand command = new SqlCommand(_sql, conn);

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
            }
            catch (Exception e)
            {
                Errors += " " + e.Message;
            }
            return _rtnVal;
        }

        public async Task<List<string>> RunQueryAsync()
        {
            List<string> rtnList = new List<string>();
            try
            {

                string sConnection = DbConnStr.RtnStr(_database);
                using (SqlConnection conn = new SqlConnection(sConnection))
                {
                    SqlCommand command = new SqlCommand(_sql, conn);

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
            }
            catch (Exception e)
            {
                Errors += " " + e.Message;
            }
            return rtnList;
        }

        public int ReturnResult()
        {
            return _rtnVal;
        }

        public SqlQryBuilder GetErrors(out string errors)
        {
            errors = Errors;
            return this;
        }

        //public SqlQryBuilder<T> LogErrors(Logger logger)
        //{
        //    //log any errors here...
        //}
    }






    public static class ErrorHelper
    {
        //not sure if I'll ever use this...

        public static string JsonErr(string error)
        {
            string rtnVal = string.Empty;
            if (error != string.Empty)
            {
                var e = new { Error = error };
                rtnVal = JsonConvert.SerializeObject(e);
            }
            return rtnVal;
        }

        public static string JsonErr(string error, Object result)
        {
            string rtnVal = string.Empty;
            if (error != string.Empty)
            {
                var e = new { Error = error };
                rtnVal = JsonConvert.SerializeObject(e);
            }
            else if (result == null)
            {
                var e = new { Error = "No Data Here!" };
                rtnVal = JsonConvert.SerializeObject(e);
            }
            return rtnVal;
        }


        public static string JsonErr(string error, int rowCount)
        {
            string rtnVal = string.Empty;
            if (error != string.Empty)
            {
                var e = new { Error = error };
                rtnVal = JsonConvert.SerializeObject(e);
            }
            else
            {
                var temp = new { rowCount = rowCount };
                rtnVal = JsonConvert.SerializeObject(temp);
            }
            return rtnVal;
        }

        public static string JsonReturnObject(string error, int rowCount, object objResult)
        {
            string rtnVal = string.Empty;
            ReturnObject ro = new ReturnObject
            {
                ErrorStr = error,
                RowCount = rowCount,
                RtnObjJsonStr = JsonConvert.SerializeObject(objResult)
            };
            if (ro.ErrorStr != string.Empty)
            {
                rtnVal = JsonConvert.SerializeObject(ro);
            }
            else
            {
                var temp = new { rowCount = rowCount };
                rtnVal = JsonConvert.SerializeObject(temp);
            }
            return rtnVal;
        }

    }

    public class ReturnObject
    {
        public string ErrorStr { get; set; }
        public int RowCount { get; set; }
        public string RtnObjJsonStr { get; set; }

        public ReturnObject()
        {
            ErrorStr = "";
            RowCount = 0;
            RtnObjJsonStr = "";
        }

    }







}

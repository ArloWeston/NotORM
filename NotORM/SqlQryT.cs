using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NotORM
{
    public class SqlQry<T> : SqlQry where T : class, new()
    {
        private List<T> _rtnList;

        public SqlQry(string connStr) :base(connStr)
        {
            _rtnList = new List<T>();
        }

        public List<T> GetData()
        {
            _rtnList = new List<T>();

            string sConnection = this.ConnStr;
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
                if (this.SqlParams.Count > 0)
                {
                    foreach (SqlParameter p in this.SqlParams)
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


        public async Task<List<T>> GetDataAsync()
        {
            _rtnList = new List<T>();
            if (WhereCl.Trim().Length > 0)
            {
                SQL += " " + WhereCl;
            }
            if (OrderByCl.Trim().Length > 0)
            {
                SQL += " " + OrderByCl;
            }
            string sConnection = this.ConnStr;
            using (SqlConnection conn = new SqlConnection(sConnection))
            {
                SqlCommand command = new SqlCommand(SQL, conn);

                if (this.SqlParams.Count > 0)
                {
                    foreach (SqlParameter p in this.SqlParams)
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

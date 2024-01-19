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
        private SqlQry _sqlQry;
        //private List<SqlParameter> _sqlParams;
        //private int _rtnVal;

        public string SQL 
        { 
            get => _sqlQry.SQL;
            set 
            {
                _sqlQry.SQL = value;   
            } 
        }

        public string ClassMappingErrors
        {
            get => _sqlQry.ClassMappingErrors;
            set
            {
                _sqlQry.ClassMappingErrors = value;
            }
        }

        /// <summary>
        /// Optional place to put the where clause if need to get it seperate from SQL
        /// </summary>
        public string WhereCl
        {
            get => _sqlQry.WhereCl;
            set
            {
                _sqlQry.WhereCl = value;
            }
        }

        /// <summary>
        /// Optional place to put the order by clause if need to get it seperate from SQL
        /// </summary>
        public string OrderByCl
        {
            get => _sqlQry.OrderByCl;
            set
            {
                _sqlQry.OrderByCl = value;
            }
        }

        public SqlQryBuilder(string connStr)
        {
            _sqlQry = new SqlQry(connStr);
            
        }

        public SqlQryBuilder AddSQLString(string sql)
        {
            _sqlQry.SQL = sql;
            return this;
        }

        public SqlQryBuilder AddParameter(string column, string value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }


        public SqlQryBuilder AddParameter(string column, DateTime value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }

        public SqlQryBuilder AddParameter(string column, int value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }

        public SqlQryBuilder AddParameter(string column, int? value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }

        public SqlQryBuilder AddParameter(string column, decimal? value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }

        public SqlQryBuilder AddParameter(string column, DateTime? value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }

        public SqlQryBuilder AddParameter(string column, bool? value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }

        public SqlQryBuilder AddWhereCls(string whereCls)
        {
            this.WhereCl = whereCls;
            return this;
        }

        public SqlQryBuilder AddOrderBy(string orderByCls)
        {
            this.OrderByCl = orderByCls;
            return this;
        }


        public SqlQryBuilder ClearParameterList()
        {
            _sqlQry.ClearParameterList();
            return this;
        }

        public int BuildNonQuery()
        {
            return _sqlQry.NonQuery();
        }


        public async Task<int> BuildNonQueryAsync()
        {
            return await _sqlQry.NonQueryAsync();
        }

        public async Task<List<string>> BuildListStringQueryAsync()
        {
            return await _sqlQry.ListStringQueryAsync();
        }

    }

}

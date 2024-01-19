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
        private SqlQry<T> _sqlQry;

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
            _sqlQry = new SqlQry<T>(connStr);
        }

        public SqlQryBuilder<T> AddSQLString(string sql)
        {
            _sqlQry.SQL = sql;
            return this;
        }

        public SqlQryBuilder<T> AddParameter(string column, string value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }

        public SqlQryBuilder<T> AddParameter(string column, DateTime value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }

        public SqlQryBuilder<T> AddParameter(string column, int value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }

        public SqlQryBuilder<T> AddParameter(string column, int? value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }

        public SqlQryBuilder<T> AddParameter(string column, decimal? value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }

        public SqlQryBuilder<T> AddParameter(string column, DateTime? value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }

        public SqlQryBuilder<T> AddParameter(string column, bool value)
        {
            _sqlQry.AddParameter(column, value);
            return this;
        }

        public SqlQryBuilder<T> AddWhereCls(string whereCls)
        {
            this.WhereCl = whereCls;
            return this;
        }

        public SqlQryBuilder<T> AddOrderBy(string orderByCls)
        {
            this.OrderByCl = orderByCls;
            return this;
        }

        public SqlQryBuilder<T> ClearParameterList()
        {
            _sqlQry.ClearParameterList();
            return this;
        }

        public List<T> Build()
        {
            return _sqlQry.GetData();

        }


        public async Task<List<T>> BuildAsync()
        {
            return await _sqlQry.GetDataAsync();
        }


    }
}

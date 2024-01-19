using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotORM
{
    public interface ISqlQry
    {
        public string SQL { get; set; }
        public List<SqlParameter> SqlParams { get; set; }
        public string ClassMappingErrors { get; set; }
        public string WhereCl { get; set; }
        public string OrderByCl { get; set; }

        public void AddParameter(string column, string value);
        public void AddParameter(string column, DateTime value);
        public void AddParameter(string column, int value);
        public void AddParameter(string column, int? value);
        public void AddParameter(string column, decimal? value);
        public void AddParameter(string column, DateTime? value);
        public void AddParameter(string column, bool? value);
        public void ClearParameterList();
    }
}

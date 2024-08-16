using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotORM
{
    public interface ISqlQry
    {
        string SQL { get; set; }
        List<SqlParameter> SqlParams { get; set; }
        string ClassMappingErrors { get; set; }
        string WhereCl { get; set; }
        string OrderByCl { get; set; }

        void AddParameter(string column, string value);
        void AddParameter(string column, DateTime value);
        void AddParameter(string column, int value);
        void AddParameter(string column, int? value);
        void AddParameter(string column, decimal? value);
        void AddParameter(string column, DateTime? value);
        void AddParameter(string column, bool? value);
        void ClearParameterList();
    }
}

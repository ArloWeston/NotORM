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
    }
}

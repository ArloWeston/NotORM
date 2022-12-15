# Not ORM

Not really an ORM, just a simple mapping from a SQL Server query to a class.  Written in C#, uses the builder pattern.

## Example
### Let's say we have this class:
    public class Customer
    {
        public string CustomerID { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
    }

### I want to get data from the database and use it as a Customer class:
    SqlQryBuilder<Customer> sqlb = new SqlQryBuilder<Customer>(sqlConnection);

    List<Customer> CustomersInCity =
        sqlb
        .AddSQLString(@"
            SELECT dbID as CustomerID, dbCompany as Company, dbCity as City
            FROM CustomerMasterTable
            WHERE dbCity = @dbCity
            ")
        .AddParameter("dbCity", "New York")
        .Build();
        
### Now I have a list of the Customer class to be used how ever I want!

### You can do Insert and Update queries too:
    SqlQryBuilder sqlb = new SqlQryBuilder(sqlConnection);

    int success = 
        sqlb
        .AddSQLString(@"
            UPDATE CustomerMasterTable
            SET boolTblVal = @boolTblVal
            WHERE CustomerID = @CustomerID")
        .AddParameter("boolTblVal", true) 
        .AddParameter("CustomerID", 12345678)
        .BuildNonQuery();

### It has async methods too, like BuildAsync and BuildNonQueryAsync





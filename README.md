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

### Use like this:
    SqlQryBuilder<Customer> sqlb = new SqlQryBuilder<Customer>(sqlConnection);

    List<Customer> CustomersInCity =
        sqlb
        .AddSQLString(@"
            SELECT dbID as CustomerID, dbCompany as Company, dbCity as City
            FROM CustomerMasterTable
            WHERE dbCity = @dbCity
            ")
        .AddParameter("dbCity", "New York City")
        .Build();
        return CustomersInCity;
### Now I have a list of the Customer class to be used how ever I want!







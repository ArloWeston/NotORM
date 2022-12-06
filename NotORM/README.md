# Not ORM

C#
This project mapps a class to a SQL Server query.  Uses builder pattern.  

## Example:
We have this class:
`
	 public class Customer
    {
        public string CustomerID { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
    }
`
### Use like this:
`
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

                //Now I have a list of customers I can use how ever I want!

                return CustomersInCity;

`





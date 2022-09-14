using NotORM;

namespace NotORM_Tests
{
    public class SqlQryBuilder_Tests
    {
        private string _sqlConnection = "server=DBS;Integrated Security=True; TrustServerCertificate=True; database=CUST_TESTBED;";


        [Fact]
        public void BuildAQueryTest()
        {

            List<CustomerMasterCustom> expected = new List<CustomerMasterCustom>();
            expected.Add(new CustomerMasterCustom
            {
                cmCompany = "Market Services, Inc. ",
                cmCustID = "444444",
                cmDateSetUp = new DateTime(1983, 01, 05)
            });


            SqlQryBuilder<CustomerMasterCustom> builder =
                new SqlQryBuilder<CustomerMasterCustom>(_sqlConnection);


            List<CustomerMasterCustom> actual = builder.AddSQLString(@"
                        SELECT cmCustID, cmCompany, cmDateSetUp
                        FROM CustomerMaster
                        WHERE cmCustID = @cmCustID
                    ")
              .AddParameter("cmCustID", "444444")
              .RunQuery()
              .ReturnResult();

            Assert.Equal(expected[0].cmCompany, actual[0].cmCompany);
            Assert.Equal(expected[0].cmCustID, actual[0].cmCustID);
            Assert.Equal(expected[0].cmDateSetUp, actual[0].cmDateSetUp);

        }

        [Fact]
        public void BuildAQueryTest2()
        {

            List<CustomerMasterCustom> expected = new List<CustomerMasterCustom>();
            expected.Add(new CustomerMasterCustom
            {
                cmCompany = "Market Services, Inc. ",
                cmCustID = "444444",
                cmDateSetUp = new DateTime(1983, 01, 05)
            });

            CustomerMasterCustom actual =
                new SqlQryBuilder<CustomerMasterCustom>(_sqlConnection)
                .AddSQLString(@"
                        SELECT cmCustID, cmCompany, cmDateSetUp
                        FROM CustomerMaster
                        WHERE cmCustID = @cmCustID
                    ")
                .AddParameter("cmCustID", "444444")
                .RunQuery()
                .ReturnResult()[0];

            Assert.Equal(expected[0].cmCompany, actual.cmCompany);
            Assert.Equal(expected[0].cmCustID, actual.cmCustID);
            Assert.Equal(expected[0].cmDateSetUp, actual.cmDateSetUp);

        }


        [Fact]
        public void BuildAQueryTestErrors()
        {
            string expectedErr = " class mapping error: cmCustID";
            string actualErr = "";

            CustomerMasterCustom actual =
                new SqlQryBuilder<CustomerMasterCustom>(_sqlConnection)
                .AddSQLString(@"
                        SELECT cmCustID as hereistheErr, cmCompany, cmDateSetUp
                        FROM CustomerMaster
                        WHERE cmCustID = @cmCustID
                    ")
                .AddParameter("cmCustID", "444444")
                .RunQuery()
                .GetErrors(out actualErr)
                .ReturnResult()[0];

            Assert.Equal(expectedErr, actualErr);

        }

        [Fact]
        public void BuildAQueryTestErrors2()
        {
            string expectedErr = " Cannot open database \"CUST_TESTBEDhereistheErr\"";
            string actualErr = "";

            string badConn = _sqlConnection.Replace("CUST_TESTBED", "CUST_TESTBEDhereistheErr");

            List<CustomerMasterCustom> actual =
                new SqlQryBuilder<CustomerMasterCustom>(badConn)
                .AddSQLString(@"
                        SELECT cmCustID, cmCompany, cmDateSetUp
                        FROM CustomerMaster
                        WHERE cmCustID = @cmCustID
                    ")
                .AddParameter("cmCustID", "444444")
                .RunQuery()
                .GetErrors(out actualErr)
                .ReturnResult();
            actualErr = actualErr.Substring(0, 48);
            Assert.Equal(expectedErr, actualErr);

        }


        [Fact]
        public void BuildAQueryTestErrors3()
        {
            string expectedErr = " Must declare the scalar variable \"@cmCustID\".";
            string actualErr = "";

            List<CustomerMasterCustom> actual =
                new SqlQryBuilder<CustomerMasterCustom>(_sqlConnection)
                .AddSQLString(@"
                        SELECT cmCustID, cmCompany, cmDateSetUp
                        FROM CustomerMaster
                        WHERE cmCustID = @cmCustID
                    ")
                .AddParameter("cmCustIDhereistheErr", "444444")
                .RunQuery()
                .GetErrors(out actualErr)
                .ReturnResult();

            Assert.Equal(expectedErr, actualErr);

        }

        [Fact]
        public void BuildAQueryWithNoParametersTest()
        {

            SqlQryBuilder<CustomerMasterCustom> builder =
                new SqlQryBuilder<CustomerMasterCustom>(_sqlConnection);


            List<CustomerMasterCustom> actual = builder.AddSQLString(@"
                        SELECT cmCustID, cmCompany, cmDateSetUp
                        FROM CustomerMaster
                    ")
              .RunQuery()
              .ReturnResult();
            Assert.True(actual.Count > 0);

        }

    }


    public class CustomerMasterCustom
    {
        public string cmCustID { get; set; }
        public string cmCompany { get; set; }
        public DateTime cmDateSetUp { get; set; }
    }
}
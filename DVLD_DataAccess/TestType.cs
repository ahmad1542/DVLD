using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess {
    public class TestType {

        public static bool GetTestTypeInfoByID(int TestTypeID,
            ref string TestTypeTitle, ref string TestTypeDescription, ref float TestTypeFees) {

            bool isFound = false;

            string query = "select * from TestTypes where TestTypeID = @TestTypeID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            TestTypeTitle = (string)reader["TestTypeTitle"];
                            TestTypeDescription = (string)reader["TestTypeDescription"];
                            TestTypeFees = Convert.ToSingle(reader["TestTypeFees"]);
                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static DataTable GetAllTestTypes() {
            DataTable dt = new DataTable();

            string query = "select * from TestTypes order by TestTypeID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.HasRows) {
                            dt.Load(reader);
                        }
                    }
                }
            } catch { }
            return dt;
        }

        public static int AddNewTestType(string Title, string Description, float Fees) {
            int testID = -1;

            string query = @"insert into TestTypes (TestTypeTitle, TestTypeDescription, TestTypeFees)
                             values (@Title, @Description, @Fees)
                             
                             select SCOPE_IDENTITY();";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@Fees", Fees);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID)) {
                        testID = insertedID;
                    }
                }
            } catch { }
            return testID;
        }

        public static bool UpdateTestType(int TestTypeID, string Title, string Description, float Fees) {
            int rowsAffected = 0;

            string query = @"update TestTypes
                             set TestTypeTitle = @Title,
                             TestTypeDescription = @Description,
                             TestTypeFees = @Fees
                             where TestTypeID = @TestTypeID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Description", Description);
                    command.Parameters.AddWithValue("@Fees", Fees);

                    rowsAffected = command.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffected > 0);
            }
            return (rowsAffected > 0);
        }

    }
}

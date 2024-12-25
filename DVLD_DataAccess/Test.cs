using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess {
    public class Test {

        public static bool GetTestInfoByID(int TestID,
            ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID) {
            bool isFound = false;

            string query = "select * from Tests where TestID = @TestID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@TestID", TestID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestResult = (bool)reader["TestResult"];

                            if (reader["Notes"] == DBNull.Value)
                                Notes = "";
                            else
                                Notes = (string)reader["Notes"];

                            CreatedByUserID = (int)reader["CreatedByUserID"];
                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(int PersonID, int LicenseClassID, int TestTypeID, ref int TestID,
            ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID) {
            bool isFound = false;

            string query = @"select  top 1 Tests.TestID, Tests.TestAppointmentID, Tests.TestResult, 
			                 Tests.Notes, Tests.CreatedByUserID, Applications.ApplicantPersonID
                             from LocalDrivingLicenseApplications inner join
                             Tests inner join
                             TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID inner join
                             Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                             where (Applications.ApplicantPersonID = @PersonID) 
                             AND (LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID)
                             AND ( TestAppointments.TestTypeID=@TestTypeID)
                             order by Tests.TestAppointmentID desc";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            TestID = (int)reader["TestID"];
                            TestAppointmentID = (int)reader["TestID"];
                            TestResult= (bool)reader["TestID"];

                            if (reader[Notes] == DBNull.Value)
                                Notes = "";
                            else
                                Notes = (string)reader["TestID"];
                            
                            CreatedByUserID = (int)reader["TestID"];
                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static DataTable GetAllTests() {

            DataTable dt = new DataTable();

            string query = "select * from Tests order by TestID";

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
            } catch {

            }
            return dt;
        }

        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID) {
            int TestID = -1;

            string query = @"insert into Tests (TestAppointmentID,TestResult, Notes,   CreatedByUserID)
                            values (@TestAppointmentID,@TestResult, @Notes,   @CreatedByUserID);
                            update TestAppointments 
                            set IsLocked = 1
                            where TestAppointmentID = @TestAppointmentID;

                            select SCOPE_IDENTITY();";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestResult", TestResult);
                    command.Parameters.AddWithValue("@Notes", Notes);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID)) {
                        TestID = insertedID;
                    }
                }
            } catch {

            }
            return TestID;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID) {

            int rowsAffected = 0;

            string query = @"update  Tests  
                             set TestAppointmentID = @TestAppointmentID,
                                 TestResult=@TestResult,
                                 Notes = @Notes,
                                 CreatedByUserID=@CreatedByUserID
                                 where TestID = @TestID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestResult", TestResult);
                    command.Parameters.AddWithValue("@Notes", Notes);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@TestID", TestID);

                    rowsAffected = command.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffected > 0);
            }
            return (rowsAffected > 0);
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID) {
            byte PassedTestCount = 0;

            string query = @"select PassedTestCount = count(TestTypeID)
                             from Tests inner join
                             TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
						     where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestResult = 1";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    object result = command.ExecuteScalar();

                    if (result != null && byte.TryParse(result.ToString(), out byte ptCount)) {
                        PassedTestCount = ptCount;
                    }
                }
            } catch {

            }
            return PassedTestCount;
        }
    }
}

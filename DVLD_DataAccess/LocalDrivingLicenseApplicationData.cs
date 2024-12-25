using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess {
    public class LocalDrivingLicenseApplicationData {

        public static bool GetLocalDrivingLicenseApplicationInfoByID(
            int LocalDrivingLicenseApplicationID, ref int ApplicationID,
            ref int LicenseClassID) {
            bool isFound = false;

            string query = @"select * from LocalDrivingLicenseApplications
                             where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            ApplicationID = (int)reader["ApplicationID"];
                            LicenseClassID = (int)reader["LicenseClassID"];

                        } else
                            isFound = false;
                    }
                }
            } catch { 
                isFound=false;
            }
            return isFound;
        }

        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationID(
         int ApplicationID, ref int LocalDrivingLicenseApplicationID,
         ref int LicenseClassID) {
            bool isFound = false;

            string query = "select * from LocalDrivingLicenseApplications where ApplicationID = @ApplicationID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                            LicenseClassID = (int)reader["LicenseClassID"];
                        } else
                            isFound = false;

                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications() {
            DataTable dt = new DataTable();

            string query = @"select * from LocalDrivingLicenseApplications_View
                             order by ApplicationDate desc";

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

        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID) {

            int LocalDrivingLicenseApplicationID = -1;

            string query = @"insert into LocalDrivingLicenseApplications (ApplicationID,LicenseClassID)
                             values (@ApplicationID,@LicenseClassID);
                             select SCOPE_IDENTITY();";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID)) {
                        LocalDrivingLicenseApplicationID = insertedID;
                    }
                }
            } catch {

            }
            return LocalDrivingLicenseApplicationID;
        }

        public static bool UpdateLocalDrivingLicenseApplication(
            int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID) {
            int rowsAffectes = 0;

            string query = @"update LocalDrivingLicenseApplications
                             set ApplicationID = @ApplicationID, 
                             LicenseClassID = @LicenseClassID
                             where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    rowsAffectes = command.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffectes > 0);
            }
            return (rowsAffectes > 0);
        }

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID) {
            int rowsAffected = 0;

            string query = @"delete LocalDrivingLicenseApplications 
                             where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    rowsAffected = command.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffected > 0);
            }
            return (rowsAffected > 0);
        }

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID) {
            bool Result = false;

            string query = @"select top 1 TestResult
                             from LocalDrivingLicenseApplications inner join
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID inner join
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             where
                             (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                             AND (TestAppointments.TestTypeID = @TestTypeID)
                             order by TestAppointments.TestAppointmentID desc";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    object result = command.ExecuteScalar();

                    if (result != null && bool.TryParse(result.ToString(), out bool returnedResult)) { 
                        Result = returnedResult;
                    }
                }
            } catch {

            }
            return Result;
        }

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID) {
            bool IsFound = false;

            string query = @"select top 1 Found=1
                             from LocalDrivingLicenseApplications join
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID inner join
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             where
                             (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                             AND(TestAppointments.TestTypeID = @TestTypeID)
                             order by TestAppointments.TestAppointmentID desc";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    object result = command.ExecuteScalar();

                    if (result != null) { 
                        IsFound = true;
                    }
                }
            } catch {

            }
            return IsFound;
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID) {
            byte TotalTrialsPerTest = 0;

            string query = @"select TotalTrialsPerTest = count(TestID)
                             from LocalDrivingLicenseApplications inner join
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID inner join
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             where
                             (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                             AND (TestAppointments.TestTypeID = @TestTypeID)";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    object result = command.ExecuteScalar();

                    if (result != null && byte.TryParse(result.ToString(), out byte Trials)) {
                        TotalTrialsPerTest = Trials;
                    }
                }
            } catch {

            }
            return TotalTrialsPerTest;
        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID) {
            bool Result = false;

            string query = @"select top 1 Found=1
                             from LocalDrivingLicenseApplications inner join
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                             where
                             (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)  
                             AND (TestAppointments.TestTypeID = @TestTypeID) and isLocked=0
                             order by TestAppointments.TestAppointmentID desc";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    object result = command.ExecuteScalar();

                    if (result != null) {
                        Result = true;
                    }
                }
            } catch {

            }
            return Result;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess {
    internal class TestAppointment {

        public static bool GetTestAppointmentInfoByID(int TestAppointmentID,
            ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
            ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID) {
            bool isFound = false;

            string query = "select * from TestAppointments where TestAppointmentID = @TestAppointmentID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            TestTypeID = (int)reader["TestTypeID"];
                            LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                            AppointmentDate = (DateTime)reader["AppointmentDate"];
                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsLocked = (bool)reader["IsLocked"];

                            if (reader["RetakeTestApplicationID"] == DBNull.Value)
                                RetakeTestApplicationID = -1;
                            else
                                RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];

                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static bool GetLastTestAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID,
            ref int TestAppointmentID, ref DateTime AppointmentDate,
            ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID) {
            bool isFound = false;

            string query = @"select top 1 *
                             from TestAppointments
                             where (TestTypeID = @TestTypeID) 
                             AND (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                             order by TestAppointmentID desc";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = reader.HasRows;

                            TestAppointmentID = (int)reader["TestAppointmentID"];
                            AppointmentDate = (DateTime)reader["AppointmentDate"];
                            PaidFees = (int)reader["PaidFees"];
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsLocked = (bool)reader["IsLocked"];

                            if (reader["RetakeTestApplicationID"] == DBNull.Value)
                                RetakeTestApplicationID = -1;
                            else
                                RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];

                        } else 
                            isFound = false;
                    }
                }
            } catch {
                isFound=false;
            }
            return isFound;
        }

        public static DataTable GetAllTestAppointments() {

            DataTable dt = new DataTable();

            string query = @"select * from TestAppointments_View
                             order by AppointmentDate desc";

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

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID) {
            DataTable dt = new DataTable();

            string query = @"select TestAppointmentID, AppointmentDate,PaidFees, IsLocked
                             from TestAppointments
                             where (TestTypeID = @TestTypeID) 
                             AND (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                             order by TestAppointmentID desc;";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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

        public static int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID,
            DateTime AppointmentDate, float PaidFees, int CreatedByUserID, int RetakeTestApplicationID) {
            int TestAppointmentID = -1;

            string query = @"insert into TestAppointments (TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,CreatedByUserID,IsLocked,RetakeTestApplicationID)
                            values (@TestTypeID,@LocalDrivingLicenseApplicationID,@AppointmentDate,@PaidFees,@CreatedByUserID,0,@RetakeTestApplicationID);
                
                            select SCOPE_IDENTITY();";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    if (RetakeTestApplicationID == -1)
                        command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID)) {
                        TestAppointmentID = insertedID;
                    }
                }
            } catch {

            }
            return TestAppointmentID;
        }

        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID,
            DateTime AppointmentDate, float PaidFees,
            int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID) {

            int rowsAffected = 0;

            string query = @"update TestAppointments
                             set TestTypeID = @TestTypeID,
                                 LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                 AppointmentDate = @AppointmentDate,
                                 PaidFees = @PaidFees,
                                 CreatedByUserID = @CreatedByUserID,
                                 IsLocked=@IsLocked,
                                 RetakeTestApplicationID=@RetakeTestApplicationID
                             where TestAppointmentID = @TestAppointmentID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@IsLocked", IsLocked);

                    if (RetakeTestApplicationID == -1)
                        command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
                    
                    rowsAffected = command.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffected > 0);
            }
            return (rowsAffected > 0);
        }

        public static int GetTestID(int TestAppointmentID) {
            int TestID = -1;

            string query = @"select TestID from Tests where TestAppointmentID=@TestAppointmentID;";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        TestID = insertedID;
                }
            } catch {

            }
            return TestID;
        }


    }
}
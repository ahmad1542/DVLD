using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DVLD_DataAccess {
    internal class DetainedLicense {

        public static bool GetDetainedLicenseInfoByID(int DetainID,
            ref int LicenseID, ref DateTime DetainDate,
            ref float FineFees, ref int CreatedByUserID,
            ref bool IsReleased, ref DateTime ReleaseDate,
            ref int ReleasedByUserID, ref int ReleaseApplicationID) {

            bool isFound = false;

            string query = "select * from DetainedLicenses where DetainID = @DetainID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();
                    command.Parameters.AddWithValue("@DetainID", DetainID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            LicenseID = (int)reader["LicenseID"];
                            DetainDate = (DateTime)reader["DetainDate"];
                            FineFees = Convert.ToSingle(reader["FineFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsReleased = (bool)reader["IsReleased"];
                            if (reader["ReleaseDate"] == DBNull.Value)
                                ReleaseDate = DateTime.MaxValue;
                            else
                                ReleaseDate = (DateTime)reader["ReleaseDate"];

                            if (reader["ReleasedByUserID"] == DBNull.Value)
                                ReleasedByUserID = -1;
                            else
                                ReleasedByUserID = (int)reader["ReleasedByUserID"];

                            if (reader["ReleaseApplicationID"] == DBNull.Value)
                                ReleaseApplicationID = -1;
                            else
                                ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID,
         ref int DetainID, ref DateTime DetainDate,
         ref float FineFees, ref int CreatedByUserID,
         ref bool IsReleased, ref DateTime ReleaseDate,
         ref int ReleasedByUserID, ref int ReleaseApplicationID) {

            bool isFound = false;

            string query = "select top 1 * from DetainedLicenses where LicenseID = @LicenseID order by DetainID desc";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            DetainID = (int)reader["DetainID"];
                            DetainDate = (DateTime)reader["DetainDate"];
                            FineFees = Convert.ToSingle(reader["FineFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsReleased = (bool)reader["IsReleased"];
                            if (reader["ReleaseDate"] == DBNull.Value)
                                ReleaseDate = DateTime.MaxValue;
                            else
                                ReleaseDate = (DateTime)reader["ReleaseDate"];

                            if (reader["ReleasedByUserID"] == DBNull.Value)
                                ReleasedByUserID = -1;
                            else
                                ReleasedByUserID = (int)reader["ReleasedByUserID"];

                            if (reader["ReleaseApplicationID"] == DBNull.Value)
                                ReleaseApplicationID = -1;
                            else
                                ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static DataTable GetAllDetainedLicenses() {
            DataTable dt = new DataTable();

            string query = "select * from detainedLicenses_View order by IsReleased ,DetainID;";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.HasRows)
                            dt.Load(reader);
                    }
                }
            } catch {

            }
            return dt;
        }

        public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate,
            float FineFees, int CreatedByUserID) {

            int DetainID = -1;

            string query = @"insert into dbo.DetainedLicenses
                               (LicenseID,
                               DetainDate,
                               FineFees,
                               CreatedByUserID,
                               IsReleased)
                            values
                               (@LicenseID,
                               @DetainDate, 
                               @FineFees, 
                               @CreatedByUserID,
                               0);
                            
                            select SCOPE_IDENTITY();";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    command.Parameters.AddWithValue("@DetainDate", DetainDate);
                    command.Parameters.AddWithValue("@FineFees", FineFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID)) {
                        DetainID = insertedID;
                    }

                }
            } catch {

            }
            return DetainID;
        }

        public static bool UpdateDetainedLicense(int DetainID,
            int LicenseID, DateTime DetainDate,
            float FineFees, int CreatedByUserID) {

            int rowsAffected = 0;

            string query = @"update dbo.DetainedLicenses
                              set LicenseID = @LicenseID, 
                              DetainDate = @DetainDate, 
                              FineFees = @FineFees,
                              CreatedByUserID = @CreatedByUserID,   
                              where DetainID=@DetainID;";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    command.Parameters.AddWithValue("@DetainDate", DetainDate);
                    command.Parameters.AddWithValue("@FineFees", FineFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@DetainID", DetainID);

                    rowsAffected = command.ExecuteNonQuery();
                }
            } catch {
                return false;
            }
            return (rowsAffected > 0);
        }

        public static bool ReleaseDetainedLicense(int DetainID,
                 int ReleasedByUserID, int ReleaseApplicationID) {

            int rowsAffected = 0;

            string query = @"update dbo.DetainedLicenses
                              set IsReleased = 1, 
                              ReleaseDate = @ReleaseDate, 
                              ReleaseApplicationID = @ReleaseApplicationID   
                              where DetainID=@DetainID;";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();
                    command.Parameters.AddWithValue("@LicenseID", DetainID);
                    command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
                    command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
                    command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);

                    rowsAffected = command.ExecuteNonQuery();

                }
            } catch { return false; }
            return (rowsAffected > 0);
        }

        public static bool IsLicenseDetained(int LicenseID) {
            bool IsDetained = false;

            string query = @"select IsDetained=1 
                            from detainedLicenses 
                            where 
                            LicenseID=@LicenseID 
                            and IsReleased=0;";
            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    object result = command.ExecuteScalar();
                    if (result != null) {
                        IsDetained = Convert.ToBoolean(result);
                    }

                }

            } catch {
            
            }
            return IsDetained;
        }
    }
}

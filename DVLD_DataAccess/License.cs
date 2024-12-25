using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess {
    public class License {

        public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass,
            ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes,
            ref float PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID) {
            
            bool isFound = false;

            string query = "select * from Licenses where LicenseID = @LicenseID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            ApplicationID = (int)reader["ApplicationID"];
                            DriverID = (int)reader["DriverID"];
                            LicenseClass = (int)reader["LicenseClass"];
                            IssueDate = (DateTime)reader["IssueDate"];
                            ExpirationDate = (DateTime)reader["ExpirationDate"];

                            if (reader["Notes"] == DBNull.Value)
                                Notes = "";
                            else
                                Notes = (string)reader["Notes"];

                            PaidFees = Convert.ToSingle(reader["PaidFees"]);
                            IsActive = (bool)reader["IsActive"];
                            IssueReason = (byte)reader["IssueReason"];
                            CreatedByUserID = (int)reader["DriverID"];
                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound=false;
            }
            return isFound;
        }

        public static DataTable GetAllLicenses() {
            DataTable dt = new DataTable();

            string query = "select * from Licenses";

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

        public static DataTable GetDriverLicenses(int DriverID) {
            DataTable dt = new DataTable();

            string query = @"select     
                           Licenses.LicenseID,
                           ApplicationID,
		                   LicenseClasses.ClassName, Licenses.IssueDate, 
		                   Licenses.ExpirationDate, Licenses.IsActive
                           from Licenses join
                                LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            where DriverID=@DriverID
                            order by IsActive desc, ExpirationDate desc";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@DriverID", DriverID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.HasRows) {
                            dt.Load(reader);
                        }
                    }
                }
            } catch {}
            return dt;
        }

        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass,
             DateTime IssueDate, DateTime ExpirationDate, string Notes,
             float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID) {
            int LicenseID = -1;

            string query = @"insert into Licenses (ApplicationID,
                                DriverID,
                                LicenseClass,
                                IssueDate,
                                ExpirationDate,
                                Notes,
                                PaidFees,
                                IsActive,IssueReason,
                                CreatedByUserID)
                         values (@ApplicationID,
                               @DriverID,
                               @LicenseClass,
                               @IssueDate,
                               @ExpirationDate,
                               @Notes,
                               @PaidFees,
                               @IsActive,@IssueReason, 
                               @CreatedByUserID);
                            select SCOPE_IDENTITY();";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                    if (Notes == "")
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Notes", Notes);

                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@IssueReason", IssueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID)) {
                        LicenseID = insertedID;
                    }
                }
            } catch { }

            return LicenseID;
        }

        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
             DateTime IssueDate, DateTime ExpirationDate, string Notes,
             float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID) {
            int rowsAffected = 0;

            string query = @"update Licenses
                             set ApplicationID=@ApplicationID, DriverID = @DriverID,
                              LicenseClass = @LicenseClass,
                              IssueDate = @IssueDate,
                              ExpirationDate = @ExpirationDate,
                              Notes = @Notes,
                              PaidFees = @PaidFees,
                              IsActive = @IsActive,IssueReason=@IssueReason,
                              CreatedByUserID = @CreatedByUserID
                             where LicenseID=@LicenseID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                    command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                    if (Notes == "")
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    else
                        command.Parameters.AddWithValue("@Notes", Notes);

                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@IsActive", IsActive);
                    command.Parameters.AddWithValue("@IssueReason", IssueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    rowsAffected = command.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffected > 0);
            }
            return (rowsAffected > 0);
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID) {
            int LicenseID = -1;

            string query = @"select Licenses.LicenseID
                            from Licenses 
                            join Drivers ON Licenses.DriverID = Drivers.DriverID
                            where Licenses.LicenseClass = @LicenseClass 
                              and Drivers.PersonID = @PersonID
                              and IsActive=1;";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID)) {
                        LicenseID = insertedID;
                    }
                }
            } catch { }
            return LicenseID;

        }

        public static bool DeactivateLicense(int LicenseID) {
            int rowsAffected = 0;

            string query = @"update Licenses
                             set IsActive = 0
                             where LicenseID=@LicenseID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();
                
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    rowsAffected = command.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffected > 0);
            }
            return (rowsAffected > 0);
        }

    }
}

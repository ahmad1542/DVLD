using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess {
    public class LicenseClassData {
        public static bool GetLicenseClassInfoByID(int LicenseClassID,
            ref string ClassName, ref string ClassDescription, ref byte MinimumAllowedAge,
            ref byte DefaultValidityLength, ref float ClassFees) {
            bool isFound = false;

            string query = "select * from LicenseClasses where LicenseClassID = @LicenseClassID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString)) 
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            ClassName = (string)reader["ClassName"];
                            ClassDescription = (string)reader["ClassDescription"];
                            MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                            DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                            ClassFees = Convert.ToSingle(reader["ClassFees"]);
                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static bool GetLicenseClassInfoByClassName(string ClassName, ref int LicenseClassID,
            ref string ClassDescription, ref byte MinimumAllowedAge,
           ref byte DefaultValidityLength, ref float ClassFees) {
            bool isFound = false;

            string query = "select * from LicenseClasses where ClassName = @ClassName";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@ClassName", ClassName);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            LicenseClassID = (int)reader["LicenseClassID"];
                            ClassDescription = (string)reader["ClassDescription"];
                            MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                            DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                            ClassFees = Convert.ToSingle(reader["ClassFees"]);
                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static DataTable GetAllLicenseClasses() {
            DataTable dt = new DataTable();

            string query = "select * from LicenseClasses order by ClassName";

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
            } catch {}
            return dt;
        }

        public static int AddNewLicenseClass(string ClassName, string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees) {

            int LicenseClassID = -1;

            string query = @"insert into LicenseClasses (ClassName,
                                ClassDescription,
                                MinimumAllowedAge, 
                                DefaultValidityLength,
                                ClassFees)
                            values (@ClassName,
                                @ClassDescription,
                                @MinimumAllowedAge, 
                                @DefaultValidityLength,
                                @ClassFees)
                            where LicenseClassID = @LicenseClassID;
                            select SCOPE_IDENTITY();";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();
            
                    command.Parameters.AddWithValue("@ClassName", ClassName);
                    command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                    command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                    command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                    command.Parameters.AddWithValue("@ClassFees", ClassFees);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID)) {
                        LicenseClassID = insertedID;
                    }
                }
            } catch { }
            return LicenseClassID;
        }

        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName,
            string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees) {

            int rowsAffected = 0;

            string query = @"Update  LicenseClasses  
                            set ClassName = @ClassName,
                                ClassDescription = @ClassDescription,
                                MinimumAllowedAge = @MinimumAllowedAge,
                                DefaultValidityLength = @DefaultValidityLength,
                                ClassFees = @ClassFees
                                where LicenseClassID = @LicenseClassID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                    command.Parameters.AddWithValue("@ClassName", ClassName);
                    command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                    command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                    command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                    command.Parameters.AddWithValue("@ClassFees", ClassFees);

                    rowsAffected = command.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffected > 0);
            }
            return (rowsAffected > 0);
        }

    }
}

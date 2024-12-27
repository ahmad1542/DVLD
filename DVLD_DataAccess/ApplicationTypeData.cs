using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess {
    public class ApplicationTypeData {

        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID,
            ref string ApplicationTypeTitle, ref float ApplicationFees) {
            bool isFound = false;

            string query = "select * from ApplicationTypes where ApplicationTypeID = @ApplicationTypeID";


            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn)) {

                    cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {

                        if (reader.Read()) {
                            isFound = true;

                            ApplicationTypeID = (int)reader["ApplicationTypeID"];
                            ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                            ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);
                        } else {
                            isFound = false;
                        }
                    }
                }
            } catch {
                return false;
            }
            return isFound;
        }

        public static DataTable GetAllApplicationTypes() {
            DataTable dt = new DataTable();

            string query = "select * from ApplicationTypes order by ApplicationTypeTitle";

            try {
                using (SqlConnection connection =  new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                using (SqlDataReader reader = cmd.ExecuteReader()) {
                    if (reader.HasRows)
                        dt.Load(reader);
                } 
            } catch {
                return dt;
            }

            return dt;
        }

        public static int AddNewApplicationType(string ApplicationTypeTitle, float ApplicationFees) {
            int appTypeID = -1;

            string query = @"insert into ApplicationTypes (ApplicationTypeTitle, ApplicationFees)
                             values (@ApplicationTypeTitle,@ApplicationFees);
                             select SCOPE_IDENTITY();";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection))
                using (SqlDataReader reader = cmd.ExecuteReader()) {
                    connection.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID)) {
                        appTypeID = insertedID;
                    }
                }
            } catch {
                return appTypeID;
            }

            return appTypeID;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string Title, float Fees) {
            int rowsAffected = 0;

            string query = @"Update  ApplicationTypes  
                            set ApplicationTypeTitle = @Title,
                                ApplicationFees = @Fees
                                where ApplicationTypeID = @ApplicationTypeID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, connection)) {
                    connection.Open();
                    cmd.Parameters.AddWithValue("@Title", Title);
                    cmd.Parameters.AddWithValue("@Fees", Fees);
                    cmd.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                    rowsAffected = cmd.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffected > 0);    
            }
            
            return (rowsAffected > 0);    
        }
    }
}

using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;

namespace DVLD_DataAccess {
    public class CountryData {

        public static bool GetCountryInfoByID(int ID, ref string CountryName) {

            bool isFound = false;

            string query = "select * from Countries where CountryID = @ID";

            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn)) {

                    cmd.Parameters.AddWithValue("@ID", ID);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            CountryName = (string)reader["CountryName"];
                        } else {
                            isFound = false;
                        }
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static bool GetCountryInfoByName(string CountryName, ref int ID) {
            bool isFound = false;

            string query = "select * from Countries where CountryName = @CountryName";

            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn)) {

                    cmd.Parameters.AddWithValue("@CountryName", CountryName);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            ID = (int)reader["CountryID"];
                        } else {
                            isFound = false;
                        }
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static DataTable GetAllCountries() {
            DataTable dt = new DataTable();

            string query = "select * from Countries order by CountryName";

            try {
                using (SqlConnection conn = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn)) {

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader()) {
                        if (reader.HasRows)
                            dt.Load(reader);
                    }
                }
            } catch {

            }
            return dt;
        }

    }
}

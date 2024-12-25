using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess {
    public class UserData {

        public static bool GetUserInfoByUserID(int UserID, ref int PersonID, ref string UserName,
            ref string Password, ref bool IsActive) {
            bool isFound = false;

            string query = "select * from Users where UserID = @UserID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@UserID", UserID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            PersonID = (int)reader["PersonID"];
                            UserName = (string)reader["UserName"];
                            Password = (string)reader["Password"];
                            IsActive = (bool)reader["IsActive"];
                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static bool GetUserInfoByPersonID(int PersonID, ref int UserID, ref string UserName,
          ref string Password, ref bool IsActive) {
            bool isFound = false;

            string query = "select * from Users where PersonID = @PersonID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            UserID = (int)reader["UserID"];
                            UserName = (string)reader["UserName"];
                            Password = (string)reader["Password"];
                            IsActive = (bool)reader["IsActive"];
                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static bool GetUserInfoByUsernameAndPassword(string UserName, string Password,
            ref int UserID, ref int PersonID, ref bool IsActive) {
            bool isFound = false;

            string query = "select * from Users where UserName = @UserName and Password = @Password";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@Password", Password);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            UserID = (int)reader["UserID"];
                            PersonID = (int)reader["PersonID"];
                            IsActive = (bool)reader["IsActive"];
                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static int AddNewUser(int PersonID, string UserName,
             string Password, bool IsActive) {
            int userID = -1;

            string query = @"insert into Users (PersonID, UserName, Password, IsActive)
                             values (@PersonID, @UserName, @Password, @IsActive)

                             select SCOPE_IDENTITY();";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@IsActive", IsActive);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID)) {
                        userID = insertedID;
                    }
                }
            } catch { }
            return userID;
        }

        public static bool UpdateUser(int UserID, int PersonID, string UserName,
             string Password, bool IsActive) {
            int rowsAffected = -1;

            string query = @"update Users
                             set PersonID = @PersonID,
                             UserName = @UserName,
                             Password = @Password,
                             IsActive = @IsActive

                             where UserID = @UserID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@UserName", UserName);
                    command.Parameters.AddWithValue("@Password", Password);
                    command.Parameters.AddWithValue("@IsActive", IsActive);

                    rowsAffected = command.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffected > 0);
            }
            return (rowsAffected > 0);

        }

        public static DataTable GetAllUsers() {
            DataTable dt = new DataTable();

            string query = @"select Users.UserID, Users.PersonID,
                             FullName = People.FirstName + ' ' + People.SecondName + ' ' + ISNULL( People.ThirdName,'') +' ' + People.LastName,
                             Users.UserName, Users.IsActive
                             from Users join
                             People ON Users.PersonID = People.PersonID";

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

        public static bool DeleteUser(int UserID) {
            int rowsAffected = 0;

            string query = @"delete Users
                             where UserID = @UserID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@UserID", UserID);

                    rowsAffected = command.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffected > 0);
            }
            return (rowsAffected > 0);
        }

        public static bool IsUserExist(int UserID) {
            bool isFound = false;

            string query = "select Found = 1 from Users where UserID = @UserID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@UserID", UserID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        isFound = reader.HasRows;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static bool IsUserExist(string UserName) {
            bool isFound = false;

            string query = "select Found = 1 from Users where UserName = @UserName";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@UserName", UserName);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        isFound = reader.HasRows;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static bool IsUserExistForPersonID(int PersonID) {
            bool isFound = false;

            string query = "select Found = 1 from Users where PersonID = @PersonID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        isFound = reader.HasRows;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static bool DoesPersonHaveUser(int PersonID) {
            bool isFound = false;

            string query = "select Found = 1 from Users where PersonID = @PersonID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        isFound = reader.HasRows;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }

        public static bool ChangePassword(int UserID, string NewPassword) {
            int rowsAffected = 0;

            string query = @"update Users
                             set Password = @NewPassword
                             where UserID= @UserID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@NewPassword", NewPassword);

                    rowsAffected = command.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffected > 0);
            }
            return (rowsAffected > 0);
        }

    }
}

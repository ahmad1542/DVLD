using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess {
    internal class Person {

        public static bool GetPersonInfoByID(int PersonID, ref string FirstName, ref string SecondName,
          ref string ThirdName, ref string LastName, ref string NationalNo, ref DateTime DateOfBirth,
           ref short Gender, ref string Address, ref string Phone, ref string Email,
           ref int NationalityCountryID, ref string ImagePath) {
            bool isFound = false;

            string query = "select * from People where PersonID = @PersonID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            FirstName = (string)reader["FirstName"];
                            SecondName = (string)reader["SecondName"];

                            if (reader["ThirdName"] != DBNull.Value) {
                                ThirdName = (string)reader["ThirdName"];
                            } else {
                                ThirdName = "";
                            }
                            
                            LastName = (string)reader["LastName"];
                            NationalNo = (string)reader["NationalNo"];
                            DateOfBirth = (DateTime)reader["DateOfBirth"];
                            Gender = (short)reader["Gender"];
                            Address = (string)reader["Address"];
                            Phone = (string)reader["Phone"];

                            if (reader["Email"] != DBNull.Value) {
                                Email = (string)reader["Email"];
                            } else {
                                Email = "";
                            }

                            NationalityCountryID = (int)reader["NationalityCountryID"];

                            if (reader["ImagePath"] != DBNull.Value) {
                                ImagePath = (string)reader["ImagePath"];
                            } else {
                                ImagePath = "";
                            }

                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }


        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
        ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
         ref short Gender, ref string Address, ref string Phone, ref string Email,
         ref int NationalityCountryID, ref string ImagePath) {
            bool isFound = false;

            string query = "select * from People where NationalNo = @NationalNo";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@NationalNo", NationalNo);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        if (reader.Read()) {
                            isFound = true;

                            PersonID = (int)reader["PersonID"];
                            FirstName = (string)reader["FirstName"];
                            SecondName = (string)reader["SecondName"];

                            if (reader["ThirdName"] != DBNull.Value) {
                                ThirdName = (string)reader["ThirdName"];
                            } else {
                                ThirdName = "";
                            }

                            LastName = (string)reader["LastName"];
                            DateOfBirth = (DateTime)reader["DateOfBirth"];
                            Gender = (short)reader["Gender"];
                            Address = (string)reader["Address"];
                            Phone = (string)reader["Phone"];

                            if (reader["Email"] != DBNull.Value) {
                                Email = (string)reader["Email"];
                            } else {
                                Email = "";
                            }

                            NationalityCountryID = (int)reader["NationalityCountryID"];

                            if (reader["ImagePath"] != DBNull.Value) {
                                ImagePath = (string)reader["ImagePath"];
                            } else {
                                ImagePath = "";
                            }

                        } else
                            isFound = false;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }



        public static int AddNewPerson(string FirstName, string SecondName,
           string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
           short Gender, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath) {

            int PersonID = -1;


            string query = @"insert into People (FirstName, SecondName, ThirdName,LastName,NationalNo,
                                                   DateOfBirth,Gender,Address,Phone, Email, NationalityCountryID,ImagePath)
                             values (@FirstName, @SecondName,@ThirdName, @LastName, @NationalNo,
                                     @DateOfBirth,@Gender,@Address,@Phone, @Email,@NationalityCountryID,@ImagePath);
                             select SCOPE_IDENTITY();";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);

                    if (ThirdName != "" && ThirdName != null)
                        command.Parameters.AddWithValue("@ThirdName", ThirdName);
                    else
                        command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gender", Gender);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Phone", Phone);

                    if (Email != "" && Email != null)
                        command.Parameters.AddWithValue("@Email", Email);
                    else
                        command.Parameters.AddWithValue("@Email", System.DBNull.Value);

                    command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

                    if (ImagePath != "" && ImagePath != null)
                        command.Parameters.AddWithValue("@ImagePath", ImagePath);
                    else
                        command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID)) {
                        PersonID = insertedID;                        
                    }
                }
            } catch {

            }
            return PersonID;
        }



        public static bool UpdatePerson(int PersonID, string FirstName, string SecondName,
           string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
           short Gender, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath) {

            int rowsAffected = 0;

            string query = @"update  People  
                            set FirstName = @FirstName,
                                SecondName = @SecondName,
                                ThirdName = @ThirdName,
                                LastName = @LastName, 
                                NationalNo = @NationalNo,
                                DateOfBirth = @DateOfBirth,
                                Gender=@Gender,
                                Address = @Address,  
                                Phone = @Phone,
                                Email = @Email, 
                                NationalityCountryID = @NationalityCountryID,
                                ImagePath =@ImagePath
                                where PersonID = @PersonID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    
                    if (ThirdName != "" && ThirdName != null)
                        command.Parameters.AddWithValue("@ThirdName", ThirdName);
                    else
                        command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gender", Gender);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Phone", Phone);

                    if (Email != "" && Email != null)
                        command.Parameters.AddWithValue("@Email", Email);
                    else
                        command.Parameters.AddWithValue("@Email", System.DBNull.Value);

                    command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

                    if (ImagePath != "" && ImagePath != null)
                        command.Parameters.AddWithValue("@ImagePath", ImagePath);
                    else
                        command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

                    rowsAffected = command.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffected > 0);
            }
            return (rowsAffected > 0);
        }


        public static DataTable GetAllPeople() {

            DataTable dt = new DataTable();

            string query =
              @"select People.PersonID, People.NationalNo,
                People.FirstName, People.SecondName, People.ThirdName, People.LastName,
			    People.DateOfBirth, People.Gender,  
				    CASE
                    WHEN People.Gender = 0 THEN 'Male'

                    ELSE 'Female'

                    END as GenderCaption ,
			    People.Address, People.Phone, People.Email, 
                People.NationalityCountryID, Countries.CountryName, People.ImagePath
                from People inner join
                Countries ON People.NationalityCountryID = Countries.CountryID
                order by People.FirstName";

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

        public static bool DeletePerson(int PersonID) {

            int rowsAffected = 0;

            string query = @"delete People 
                                where PersonID = @PersonID";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    rowsAffected = command.ExecuteNonQuery();
                }
            } catch {
                return (rowsAffected > 0);
            }
            return (rowsAffected > 0);
        }

        public static bool IsPersonExist(int PersonID) {
            bool isFound = false;

            string query = "select Found=1 from People where PersonID = @PersonID";

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

        public static bool IsPersonExist(string NationalNo) {
            bool isFound = false;

            string query = "select Found=1 from People where NationalNo = @NationalNo";

            try {
                using (SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection)) {
                    connection.Open();

                    command.Parameters.AddWithValue("@NationalNo", NationalNo);

                    using (SqlDataReader reader = command.ExecuteReader()) {
                        isFound = reader.HasRows;
                    }
                }
            } catch {
                isFound = false;
            }
            return isFound;
        }



    }
}

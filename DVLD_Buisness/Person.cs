using System;
using System.Data;
using DVLD_DataAccess;
using static DVLD_DataAccess.CountryData;

namespace DVLD_Buisness {
    public class Person {

        public enum enMode {AddNew = 0, Update = 1};
        public enMode Mode = enMode.AddNew;

        public int PersonID {  get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; } }
        public DateTime DateOfBirth { get; set; }
        public short Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }

        public Country CountryInfo;

        public Person() {
            this.PersonID = -1;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";

            Mode = enMode.AddNew;
        }

        public Person(int PersonID, string FirstName, string SecondName, string ThirdName,
            string LastName, string NationalNo, DateTime DateOfBirth, short Gender,
            string Address, string Phone, string Email, int NationalityCountryID, string ImagePath) {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.NationalNo = NationalNo;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
            this.CountryInfo = Country.Find(NationalityCountryID);
            Mode = enMode.Update;
        }

        public static Person Find(int PersonID) {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", NationalNo = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            short Gender = 0;

            if (PersonData.GetPersonInfoByID(PersonID, ref FirstName, ref SecondName, ref ThirdName,
                ref LastName, ref NationalNo, ref DateOfBirth,
                ref Gender, ref Address, ref Phone, ref Email, 
                ref NationalityCountryID, ref ImagePath)) {
                return new Person(PersonID, FirstName, SecondName, ThirdName, LastName,
                          NationalNo, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            } else 
                return null;
        }

        public static Person Find(string NationalNo) {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1, PersonID = -1;
            short Gender = 0;

            if (PersonData.GetPersonInfoByNationalNo(
                NationalNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName,
                ref LastName, ref DateOfBirth,
                ref Gender, ref Address, ref Phone, ref Email,
                ref NationalityCountryID, ref ImagePath)) {
                return new Person(PersonID, FirstName, SecondName, ThirdName, LastName,
                          NationalNo, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            } else
                return null;
        }

        private bool _AddNewPerson() {
            this.PersonID = PersonData.AddNewPerson(this.FirstName, this.SecondName, this.ThirdName,
                this.LastName, this.NationalNo,
                this.DateOfBirth, this.Gender, this.Address, this.Phone, this.Email,
                this.NationalityCountryID, this.ImagePath);
            return (this.PersonID != -1);
        }

        private bool _UpdatePerson() {
            return PersonData.UpdatePerson(this.PersonID, this.FirstName, this.SecondName, this.ThirdName,
                this.LastName, this.NationalNo, this.DateOfBirth, this.Gender,
                this.Address, this.Phone, this.Email,
                  this.NationalityCountryID, this.ImagePath);
        }

        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewPerson()) {
                        Mode = enMode.AddNew;
                        return true;
                    } else
                        return false;
                case enMode.Update:
                    return _UpdatePerson();
            }
            return false;
        }
        
        public static DataTable GetAllPeople() {
            return PersonData.GetAllPeople();
        }

        public static bool DeletePerson(int PersonID) {
            return PersonData.DeletePerson(PersonID);
        }

        public static bool IsPersonExist(int PersonID) {
            return PersonData.IsPersonExist(PersonID);
        }
        
        public static bool IsPersonExist(string NationalNo) {
            return PersonData.IsPersonExist(NationalNo);
        }

    }
}

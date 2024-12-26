using System;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Buisness {
    public class Country {

        public int CountryID { get; set; }
        public string CountryName { get; set; }

        public Country() {
            this.CountryID = -1;
            this.CountryName = "";
        }

        private Country(int countryID, string countryName) {
            this.CountryID = countryID;
            this.CountryName = countryName;
        }

        public static Country Find(int countryID) {
            string countryName = "";
            if (CountryData.GetCountryInfoByID(countryID, ref countryName))
                return new Country(countryID, countryName);
            else
                return null;
        }

        public static Country Find(string countryName) {
            int countryID = -1;
            if (CountryData.GetCountryInfoByName(countryName, ref countryID))
                return new Country(countryID, countryName);
            else
                return null;
        }

        public static DataTable GetAllCountries() {
            return CountryData.GetAllCountries();
        }

    }
}

using System;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Buisness {
    public class Driver {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public Person PersonInfo;

        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public Driver() {
            DriverID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.Now;

            Mode = enMode.AddNew;
        }
        public Driver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate) {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;
            this.PersonInfo = Person.Find(PersonID);

            Mode = enMode.Update;
        }

        public static Driver FindByDriverID(int DriverID) {
            int PersonID = -1, CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            if (DriverData.GetDriverInfoByDriverID(DriverID, ref PersonID,
                ref CreatedByUserID, ref CreatedDate))
                return new Driver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            else
                return null;
        }
        
        public static Driver FindByPersonID(int PersonID) {
            int DriverID = -1, CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            if (DriverData.GetDriverInfoByPersonID(PersonID, ref DriverID,
                ref CreatedByUserID, ref CreatedDate))
                return new Driver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            else
                return null;
        }

        public static DataTable GetAllDrivers() {
            return DriverData.GetAllDrivers();
        }

        private bool _AddNewDriver() {
            this.DriverID = DriverData.AddNewDriver(PersonID, CreatedByUserID);
            return (this.DriverID != -1);
        }
        
        private bool _UpdateDriver() {
            return DriverData.UpdateDriver(this.DriverID, this.PersonID, this.CreatedByUserID);
        }

        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewDriver()) {
                        Mode = enMode.Update;
                        return true;
                    } else
                        return false;
                case enMode.Update:
                    return _UpdateDriver();
            }
            return false;
        }



    }
}

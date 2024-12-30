using System;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Buisness {
    public  class DetainedLicense {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int DetainID {  get; set; }
        public int LicenseID {  get; set; }
        public int CreatedByUserID {  get; set; }
        public User CreatedByUserInfo { set; get; }

        public int ReleasedByUserID {  get; set; }
        public User ReleasedByUserInfo { set; get; }

        public int ReleaseApplicationID {  get; set; }
        public DateTime DetainDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsReleased { get; set; }
        public float FineFees { get; set; }

        public DetainedLicense() {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MaxValue;
            this.ReleasedByUserID = 0;
            this.ReleaseApplicationID = -1;

            Mode = enMode.AddNew;
        }

        public DetainedLicense(int DetainID, int LicenseID, DateTime DetainDate,
            float FineFees, int CreatedByUserID,
            bool IsReleased, DateTime ReleaseDate,
            int ReleasedByUserID, int ReleaseApplicationID) {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = User.FindByUserID(this.CreatedByUserID);
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;
            this.ReleasedByUserInfo = User.FindByPersonID(this.ReleasedByUserID);

            Mode = enMode.Update;
        }

        public static DetainedLicense Find(int DetainID) {
            int LicenseID = -1, CreatedByUserID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
            float FineFees = 0;
            bool IsReleased = false;

            if (DetainedLicenseData.GetDetainedLicenseInfoByID(DetainID, ref LicenseID, ref DetainDate,
            ref FineFees, ref CreatedByUserID,
            ref IsReleased, ref ReleaseDate,
            ref ReleasedByUserID, ref ReleaseApplicationID))
                return new DetainedLicense(DetainID, LicenseID, DetainDate,
                    FineFees, CreatedByUserID,
                    IsReleased, ReleaseDate,
                    ReleasedByUserID, ReleaseApplicationID);
            else
                return null;
        }
        
        public static DetainedLicense FindByLicenseID(int LicenseID) {
            int DetainID = -1, CreatedByUserID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
            float FineFees = 0;
            bool IsReleased = false;

            if (DetainedLicenseData.GetDetainedLicenseInfoByID(LicenseID, ref DetainID, ref DetainDate,
            ref FineFees, ref CreatedByUserID,
            ref IsReleased, ref ReleaseDate,
            ref ReleasedByUserID, ref ReleaseApplicationID))
                return new DetainedLicense(DetainID, LicenseID, DetainDate,
                    FineFees, CreatedByUserID,
                    IsReleased, ReleaseDate,
                    ReleasedByUserID, ReleaseApplicationID);
            else
                return null;
        }

        public static DataTable GetAllDetainedLicenses() {
            return DetainedLicenseData.GetAllDetainedLicenses();
        }

        private bool _AddNewDetainedLicense() {
            this.DetainID = DetainedLicenseData.AddNewDetainedLicense(this.LicenseID, this.DetainDate,
                this.FineFees, this.CreatedByUserID);
            return (this.DetainID != -1);
        }

        private bool _UpdateDetainedLicense() {
            return DetainedLicenseData.UpdateDetainedLicense(this.DetainID, this.LicenseID,
                this.DetainDate, this.FineFees, this.CreatedByUserID);
        }

        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewDetainedLicense()) {

                        Mode = enMode.Update;
                        return true;
                    } else
                        return false;
                case enMode.Update:
                    return _UpdateDetainedLicense();
            }
            return false;
        }

        public static bool IsLicenseDetained(int LicenseID) {
            return DetainedLicenseData.IsLicenseDetained(LicenseID);
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID) {
            return DetainedLicenseData.ReleaseDetainedLicense(this.DetainID, ReleasedByUserID, ReleaseApplicationID);
        }

    }
}

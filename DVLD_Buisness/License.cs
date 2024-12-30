using System;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Buisness {
    public class License {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };

        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public Driver DriverInfo;

        public int LicenseClass { get; set; }
        public LicenseClass LicenseClassInfo;
        public int CreatedByUserID { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public string Notes { get; set; }

        public float PaidFees { set; get; }
        public bool IsActive { set; get; }

        public enIssueReason IssueReason { set; get; }
        public string IssueReasonText {
            get {
                return GetIssueReasonText(this.IssueReason);
            }
        }

        public DetainedLicense DetainedInfo { set; get; }
        public bool IsDetained {
            get { return DetainedLicense.IsLicenseDetained(this.LicenseID); }
        }


        public License() {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClass = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;

        }

        public License(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
            DateTime IssueDate, DateTime ExpirationDate, string Notes,
            float PaidFees, bool IsActive, enIssueReason IssueReason, int CreatedByUserID) {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClass = LicenseClass;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;

            this.DriverInfo = Driver.FindByDriverID(this.DriverID);
            this.LicenseClassInfo = DVLD_Buisness.LicenseClass.Find(this.LicenseClass);
            this.DetainedInfo = DetainedLicense.FindByLicenseID(this.LicenseID);

            Mode = enMode.Update;
        }

        private bool _AddNewLicense() {
            this.LicenseID = LicenseData.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClass,
               this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees,
               this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);
            return (this.LicenseID != -1);
        }

        private bool _UpdateLicense() {
            return LicenseData.UpdateLicense(this.ApplicationID, this.LicenseID, this.DriverID, this.LicenseClass,
               this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees,
               this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);
        }

        public static License Find(int LicenseID) {
            int ApplicationID = -1; int DriverID = -1; int LicenseClass = -1, CreatedByUserID = 1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0;
            bool IsActive = true;
            byte IssueReason = 1;
            if (LicenseData.GetLicenseInfoByID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass,
                ref IssueDate, ref ExpirationDate, ref Notes,
                ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
                return new License(LicenseID, ApplicationID, DriverID, LicenseClass,
                    IssueDate, ExpirationDate, Notes,
                    PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            else
                return null;
        }

        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewLicense()) {
                        Mode = enMode.Update;
                        return true;
                    } else
                        return false;
                case enMode.Update:
                    return _UpdateLicense();
            }
            return true;
        }

        public static string GetIssueReasonText(enIssueReason IssueReason) {
            switch (IssueReason) {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replacement for Damaged";
                case enIssueReason.LostReplacement:
                    return "Replacement for Lost";
                default:
                    return "First Time";
            }
        }

        public static DataTable GetAllLicenses() {
            return LicenseData.GetAllLicenses();

        }

        public static DataTable GetDriverLicenses(int DriverID) {
            return LicenseData.GetDriverLicenses(DriverID);
        }

        public bool DeactivateCurrentLicense() {
            return (LicenseData.DeactivateLicense(this.LicenseID));
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID) {
            return LicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        }

        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID) {
            return (GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1);
        }

        public bool IsLicenseExpired() {
            return (this.ExpirationDate < DateTime.Now);
        }

        public int Detain(float FineFees, int CreatedByUserID) {

            DetainedLicense detainedLicense = new DetainedLicense();
            detainedLicense.LicenseID = this.LicenseID;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = Convert.ToSingle(FineFees);
            detainedLicense.CreatedByUserID = CreatedByUserID;

            if (!detainedLicense.Save()) {

                return -1;
            }
            return detainedLicense.DetainID;
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID, ref int ApplicationID) {

            Application Application = new Application();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)Application.enApplicationType.ReleaseDetainedDrivingLicense;
            Application.ApplicationStatus = Application.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = ApplicationType.Find((int)Application.enApplicationType.ReleaseDetainedDrivingLicense).Fees;
            Application.CreatedByUserID = ReleasedByUserID;

            if (!Application.Save()) {
                ApplicationID = -1;
                return false;
            }

            ApplicationID = Application.ApplicationID;
            return this.DetainedInfo.ReleaseDetainedLicense(ReleasedByUserID, Application.ApplicationID);
        }
        
        public License RenewLicense(string Notes, int CreatedByUserID) {

            Application Application = new Application();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)Application.enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatus = Application.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = ApplicationType.Find((int)Application.enApplicationType.RenewDrivingLicense).Fees;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save()) {
                return null;
            }

            License NewLicense = new License();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;

            int DefaultValidityLength = this.LicenseClassInfo.DefaultValidityLength;

            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = License.enIssueReason.Renew;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (!NewLicense.Save()) {
                return null;
            }

            DeactivateCurrentLicense(); // we must deactivate the old license.

            return NewLicense;
        }

        public License Replace(enIssueReason IssueReason, int CreatedByUserID) {

            Application Application = new Application();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;

            Application.ApplicationTypeID = (IssueReason == enIssueReason.DamagedReplacement) ?
                (int)Application.enApplicationType.ReplaceDamagedDrivingLicense :
                (int)Application.enApplicationType.ReplaceLostDrivingLicense;

            Application.ApplicationStatus = Application.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = ApplicationType.Find(Application.ApplicationTypeID).Fees;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save()) {
                return null;
            }

            License NewLicense = new License();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.Notes = this.Notes;
            /* there is two type of fees the first one for
             * the application and the second one for making
             * the license but for the replacment the driver
             * just pay the application fees */
            NewLicense.PaidFees = 0;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;



            if (!NewLicense.Save()) {
                return null;
            }

            DeactivateCurrentLicense();

            return NewLicense;
        }


    }
}

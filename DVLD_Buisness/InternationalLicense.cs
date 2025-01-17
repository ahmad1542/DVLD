using System;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Buisness {
    public class InternationalLicense : Application {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int InternationalLicenseID { get; set; }
        public int DriverID { get; set; }
        public Driver DriverInfo { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }

        public InternationalLicense() {
            
            this.ApplicationTypeID = (int)Application.enApplicationType.NewInternationalLicense;

            this.InternationalLicenseID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = true;

            Mode = enMode.AddNew;
        }

        private InternationalLicense(int ApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID,
             int InternationalLicenseID, int DriverID, int IssuedUsingLocalLicenseID,
            DateTime IssueDate, DateTime ExpirationDate, bool IsActive) {

            base.ApplicationID = ApplicationID;
            base.ApplicantPersonID = ApplicantPersonID;
            base.ApplicationDate = ApplicationDate;
            base.ApplicationTypeID = (int)Application.enApplicationType.NewInternationalLicense;
            base.ApplicationStatus = ApplicationStatus;
            base.LastStatusDate = LastStatusDate;
            base.PaidFees = PaidFees;
            base.CreatedByUserID = CreatedByUserID;

            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.CreatedByUserID = CreatedByUserID;

            this.DriverInfo = Driver.FindByDriverID(this.DriverID);

            Mode = enMode.Update;
        }

        public static InternationalLicense Find(int InternationalLicenseID) {
            int ApplicationID = -1, DriverID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID = 1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = true;

            if (InternationalLicenseData.GetInternationalLicenseInfoByID(InternationalLicenseID, ref ApplicationID, ref DriverID,
                ref IssuedUsingLocalLicenseID,
                ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID)) {

                Application Application = Application.FindBaseApplication(ApplicationID);


                return new InternationalLicense(Application.ApplicationID, Application.ApplicantPersonID, Application.ApplicationDate,
                    (enApplicationStatus)Application.ApplicationStatus, Application.LastStatusDate,
                    Application.PaidFees, Application.CreatedByUserID,
                    InternationalLicenseID, DriverID, IssuedUsingLocalLicenseID,
                    IssueDate, ExpirationDate, IsActive);

            } else
                return null;
        }

        private bool _AddNewInternationalLicense() {

            this.InternationalLicenseID = InternationalLicenseData.AddNewInternationalLicense(this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID,
                this.IssueDate, this.ExpirationDate,
                this.IsActive, this.CreatedByUserID);

            return (this.InternationalLicenseID != -1);
        }

        private bool _UpdateInternationalLicense() {
            return InternationalLicenseData.UpdateInternationalLicense(this.InternationalLicenseID, this.ApplicationID,
                this.DriverID, this.IssuedUsingLocalLicenseID,
                this.IssueDate, this.ExpirationDate,
                this.IsActive, this.CreatedByUserID);
        }

        public bool Save() {

            base.Mode = (Application.enMode)Mode;
            if (!base.Save())
                return false;

            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewInternationalLicense()) {

                        Mode = enMode.Update;
                        return true;
                    } else
                        return false;

                case enMode.Update:

                    return _UpdateInternationalLicense();

            }

            return false;
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID) {
            return InternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID) {
            return InternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }
        
        public static DataTable GetAllInternationalLicenses() {
            return InternationalLicenseData.GetAllInternationalLicenses();
        }

    }
}

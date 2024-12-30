using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness {
    public class Application {

        public enum enMode { AddNew = 0, Update = 1 };
        public enum enApplicationType {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicense = 5, NewInternationalLicense = 6, RetakeTest = 7
        };
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 };

        public enMode Mode = enMode.AddNew;

        public int ApplicationID { set; get; }
        public int ApplicantPersonID { set; get; }
        public string ApplicantFullName {
            get {
                return Person.Find(ApplicantPersonID).FullName;
            }
        }
        public DateTime ApplicationDate { set; get; }
        public int ApplicationTypeID { set; get; }

        public ApplicationType ApplicationTypeInfo;
        public enApplicationStatus ApplicationStatus { set; get; }
        public string StatusText {
            get {

                switch (ApplicationStatus) {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }

        }
        public DateTime LastStatusDate { set; get; }
        public float PaidFees { set; get; }
        public int CreatedByUserID { set; get; }

        public User CreatedByUserInfo;

        public Application() {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = enApplicationStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;

        }

        private Application(int ApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate, int ApplicationTypeID,
            enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
            float PaidFees, int CreatedByUserID) {

            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeInfo = ApplicationType.Find(ApplicationTypeID);
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = User.FindByUserID(CreatedByUserID);

            Mode = enMode.Update;
        }

        public static Application FindBaseApplication(int ApplicationID) {
            int ApplicantPersonID = -1, ApplicationTypeID = -1, CreatedByUserID = -1;
            DateTime ApplicationDate = DateTime.Now, LastStatusDate = DateTime.Now;
            byte ApplicationStatus = 1;
            float PaidFees = 0;

            if (ApplicationData.GetApplicationInfoByID(ApplicationID, ref ApplicantPersonID, ref ApplicationDate, ref ApplicationTypeID,
                ref ApplicationStatus, ref LastStatusDate, ref PaidFees, ref CreatedByUserID))
                return new Application(ApplicationID, ApplicantPersonID,
                                       ApplicationDate, ApplicationTypeID,
                                       (enApplicationStatus)ApplicationStatus, LastStatusDate,
                                       PaidFees, CreatedByUserID);
            else
                return null;

        }

        public static DataTable GetAllApplications() {
            return ApplicationData.GetAllApplications();
        }

        private bool _AddNewApplication() {

            this.ApplicationID = ApplicationData.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate,
            this.ApplicationTypeID, (byte)this.ApplicationStatus,
            this.LastStatusDate, this.PaidFees, this.CreatedByUserID);

            return (this.ApplicationID != -1);
        }

        private bool _UpdateApplication() {
            return ApplicationData.UpdateApplication(this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate,
                this.ApplicationTypeID, (byte)this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
        }

        public bool Cancel() {
            return ApplicationData.UpdateStatus(this.ApplicationID, 2);
        }

        public bool SetComplete() {
            return ApplicationData.UpdateStatus(this.ApplicationID, 3);
        }

        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewApplication()) {

                        Mode = enMode.Update;
                        return true;
                    } else
                        return false;
                case enMode.Update:
                    return _UpdateApplication();
            }
            return false;
        }

        public bool Delete() {
            return ApplicationData.DeleteApplication(this.ApplicationID);
        }

        public static bool IsApplicationExist(int applicationID) {
            return ApplicationData.IsApplicationExist(applicationID);
        }

        public static int GetActiveApplicationID(int PersonID, Application.enApplicationType ApplicationTypeID) {
            return ApplicationData.GetActiveApplicationID(PersonID, (int)ApplicationTypeID);
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, Application.enApplicationType ApplicationTypeID, int LicenseClassID) {
            return ApplicationData.GetActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }

        public int GetActiveApplicationID(Application.enApplicationType ApplicationTypeID) {
            return GetActiveApplicationID(this.ApplicantPersonID, ApplicationTypeID);
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID) {
            return ApplicationData.DoesPersonHaveActiveApplication(PersonID, ApplicationTypeID);
        }

        public bool DoesPersonHaveActiveApplication(int ApplicationTypeID) {
            return DoesPersonHaveActiveApplication(this.ApplicantPersonID, ApplicationTypeID);
        }

    }
}

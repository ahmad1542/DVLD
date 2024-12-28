using System;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Buisness {
    public class LocalDrivingLicenseApplication : Application {
        
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int LocalDrivingLicenseApplicationID { get; set; }
        public int LicenseClassID { get; set; }
        public LicenseClass LicenseClassInfo;

        public string PersonFullName {
            get {
                return Person.Find(ApplicantPersonID).FullName;
            }
        }

        public LocalDrivingLicenseApplication() {
            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;

            Mode = enMode.AddNew;
        }

        private LocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate, int ApplicationTypeID,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID, int LicenseClassID) {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID; ;
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = (int)ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = LicenseClass.Find(LicenseClassID);

            Mode = enMode.Update;
        }

        public static LocalDrivingLicenseApplication FindByLocalDrivingAppLicenseID(int LocalDrivingLicenseApplicationID) {
            int ApplicationID = -1, LicenseClassID = -1;

            if (LocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByID(LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseClassID)) {

                Application Application = Application.FindBaseApplication(ApplicationID);

                return new LocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, Application.ApplicationID,
                    Application.ApplicantPersonID,
                    Application.ApplicationDate, Application.ApplicationTypeID,
                    (enApplicationStatus)Application.ApplicationStatus, Application.LastStatusDate,
                    Application.PaidFees, Application.CreatedByUserID, LicenseClassID);
            } else
                return null;
        }

        public static LocalDrivingLicenseApplication FindByApplicationID(int ApplicationID) {
            int LocalDrivingLicenseApplicationID = -1, LicenseClassID = -1;

            if (LocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByApplicationID(ApplicationID,
                ref LocalDrivingLicenseApplicationID, ref LicenseClassID)) {

                Application Application = Application.FindBaseApplication(ApplicationID);

                return new LocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, Application.ApplicationID,
                    Application.ApplicantPersonID,
                    Application.ApplicationDate, Application.ApplicationTypeID,
                    (enApplicationStatus)Application.ApplicationStatus, Application.LastStatusDate,
                    Application.PaidFees, Application.CreatedByUserID, LicenseClassID);
            } else
                return null;
        }

        private bool _AddNewLocalDrivingLicenseApplication() {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(this.ApplicationID, this.LicenseClassID);

            return (this.LocalDrivingLicenseApplicationID != -1);
        }

        private bool _UpdateLocalDrivingLicenseApplication() {
            return LocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID,
                this.ApplicationID, this.LicenseClassID);

        }

        public bool Save() {

            base.Mode = (Application.enMode)Mode;
            if (!base.Save())
                return false;

            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewLocalDrivingLicenseApplication()) {

                        Mode = enMode.Update;
                        return true;
                    } else
                        return false;

                case enMode.Update:
                    return _UpdateLocalDrivingLicenseApplication();
            }
            return false;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications() {
            return LocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications();
        }

        public bool Delete() {
            bool isLocalDrivingApplicationDeleted = false;
            bool isBaseApplicationDeleted = false;

            isLocalDrivingApplicationDeleted = LocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID);

            if (!isLocalDrivingApplicationDeleted)
                return false;

            isBaseApplicationDeleted = base.Delete();
            return isBaseApplicationDeleted;

        }

        public bool DoesPassTestType(TestType.enTestType TestTypeID) {
            return LocalDrivingLicenseApplicationData.DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool DoesPassPreviousTest(TestType.enTestType CurrentTestType) {

            switch (CurrentTestType) {
                case TestType.enTestType.VisionTest:
                    return true;

                case TestType.enTestType.WrittenTest:
                    return this.DoesPassTestType(TestType.enTestType.VisionTest); // here he should be passed the vision test

                case TestType.enTestType.PracticalTest:
                    // here he should be passed 2 tests and if he passed the written test then he already passed the vision test
                    return this.DoesPassTestType(TestType.enTestType.WrittenTest);

                default:
                    return false;
            }
        }

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, TestType.enTestType TestTypeID) {
            return LocalDrivingLicenseApplicationData.DoesPassTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool DoesAttendTestType(TestType.enTestType TestTypeID) {
            return LocalDrivingLicenseApplicationData.DoesAttendTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, TestType.enTestType TestTypeID) {

            return LocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool IsThereAnActiveScheduledTest(TestType.enTestType TestTypeID) {

            return LocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public byte TotalTrialsPerTest(TestType.enTestType TestTypeID) {
            return LocalDrivingLicenseApplicationData.TotalTrialsPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, TestType.enTestType TestTypeID) {
            return LocalDrivingLicenseApplicationData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static bool AttendedTest(int LocalDrivingLicenseApplicationID, TestType.enTestType TestTypeID) {
            return (LocalDrivingLicenseApplicationData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID) > 0);
        }

        public bool AttendedTest(TestType.enTestType TestTypeID) {
            return (LocalDrivingLicenseApplicationData.TotalTrialsPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID) > 0);
        }



    }
}

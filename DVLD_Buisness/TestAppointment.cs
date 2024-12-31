using System;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Buisness {
    public class TestAppointment {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestAppointmentID {  get; set; }
        public TestType.enTestType TestTypeID { get; set; }

        public int LocalDrivingLicenseApplicationID { set; get; }
        public int CreatedByUserID { set; get; }
        public int RetakeTestApplicationID { set; get; }
        public Application RetakeTestAppInfo { set; get; }

        public DateTime AppointmentDate {  set; get; }
        public float PaidFees { set; get; }
        public bool IsLocked { set; get; }

        public int TestID {
            get {
                return _GetTestID();
            }
        }

        public TestAppointment() {
            this.TestAppointmentID = -1;
            this.TestTypeID = TestType.enTestType.VisionTest;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.RetakeTestApplicationID = -1;

            Mode = enMode.AddNew;
        }

        private TestAppointment(int TestAppointmentID, TestType.enTestType TestTypeID,
            int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees,
            int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID) {

            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
            this.RetakeTestAppInfo = Application.FindBaseApplication(RetakeTestApplicationID);

            Mode = enMode.Update;
        }

        public static TestAppointment Find(int TestAppointmentID) {
            int TestTypeID = 1; int LocalDrivingLicenseApplicationID = -1, RetakeTestApplicationID = -1, CreatedByUserID = -1;
            DateTime AppointmentDate = DateTime.Now;
            float PaidFees = 0;
            bool IsLocked = false;

            if (TestAppointmentData.GetTestAppointmentInfoByID(TestAppointmentID, ref TestTypeID,
                ref LocalDrivingLicenseApplicationID,
                ref AppointmentDate, ref PaidFees, ref CreatedByUserID,
                ref IsLocked, ref RetakeTestApplicationID))

                return new TestAppointment(TestAppointmentID, (TestType.enTestType)TestTypeID,
                    LocalDrivingLicenseApplicationID,
                    AppointmentDate, PaidFees, CreatedByUserID,
                    IsLocked, RetakeTestApplicationID);
            else
                return null;
        }

        public static TestAppointment GetLastTestAppointment(int LocalDrivingLicenseApplicationID, TestType.enTestType TestTypeID) {
            int TestAppointmentID = -1, CreatedByUserID = -1, RetakeTestApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            float PaidFees = 0;
            bool IsLocked = false;

            if (TestAppointmentData.GetLastTestAppointment(LocalDrivingLicenseApplicationID, (int)TestTypeID,
                ref TestAppointmentID, ref AppointmentDate, ref PaidFees,
                ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

                return new TestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID,
                    AppointmentDate, PaidFees, CreatedByUserID,
                    IsLocked, RetakeTestApplicationID);
            else
                return null;
        }

        public static DataTable GetAllTestAppointments() {
            return TestAppointmentData.GetAllTestAppointments();
        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, TestType.enTestType TestTypeID) {
            return TestAppointmentData.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public DataTable GetApplicationTestAppointmentsPerTestType(TestType.enTestType TestTypeID) {
            return TestAppointmentData.GetApplicationTestAppointmentsPerTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);

        }

        private bool _AddNewTestAppointment() {

            this.TestAppointmentID = TestAppointmentData.AddNewTestAppointment((int)this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate, this.PaidFees,
                this.CreatedByUserID, this.RetakeTestApplicationID);

            return (this.TestAppointmentID != -1);
        }

        private bool _UpdateTestAppointment() {
            return TestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, (int)this.TestTypeID,
                this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID,
                this.IsLocked, this.RetakeTestApplicationID);
        }

        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewTestAppointment()) {

                        Mode = enMode.Update;
                        return true;
                    } else
                        return false;
                case enMode.Update:
                    return _UpdateTestAppointment();
            }
            return false;
        }

        private int _GetTestID() {
            return TestAppointmentData.GetTestID(this.TestAppointmentID);
        }

    }
}

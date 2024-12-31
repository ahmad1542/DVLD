using System;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Buisness {
    public class Test {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestID {  get; set; }
        public int TestAppointmentID {  get; set; }
        public TestAppointment TestAppointmentInfo;
        public int CreatedByUserID {  get; set; }

        public bool TestResult { set; get; }
        public string Notes { set; get; }

        public Test() {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.CreatedByUserID = -1;
            this.TestResult = false;
            this.Notes = "";

            Mode = enMode.AddNew;
        }

        public Test(int TestID, int TestAppointmentID, int CreatedByUserID, bool TestResult, string Notes) {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestAppointmentInfo = TestAppointment.Find(this.TestAppointmentID);
            this.CreatedByUserID = CreatedByUserID;
            this.TestResult = TestResult;
            this.Notes = Notes;

            Mode = enMode.Update;
        }

        public static Test Find(int TestID) {
            int TestAppointmentID = -1, CreatedByUserID = -1;
            bool TestResult = false;
            string Notes = "";

            if (TestData.GetTestInfoByID(TestID,
            ref TestAppointmentID, ref TestResult,
            ref Notes, ref CreatedByUserID))
                return new Test(TestID, TestAppointmentID, CreatedByUserID, TestResult, Notes);
            else
                return null;

        }

        public static Test FindLastTestPerPersonAndLicenseClass(int PersonID, int LicenseClassID, TestType.enTestType TestTypeID) {
            int TestID = -1, CreatedByUserID = -1, TestAppointmentID = -1;
            bool TestResult = false;
            string Notes = "";

            if (TestData.GetLastTestByPersonAndTestTypeAndLicenseClass(PersonID, LicenseClassID, (int)TestTypeID,
                ref TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))
                return new Test(TestID, TestAppointmentID, CreatedByUserID, TestResult, Notes);
            else
                return null;
        }

        public static DataTable GetAllTests() {
            return TestData.GetAllTests();
        }

        private bool _AddNewTest() {
            this.TestID = TestData.AddNewTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);

            return (this.TestID != -1);
        }

        private bool _UpdateTest() {
            return TestData.UpdateTest(this.TestID, this.TestAppointmentID,
                this.TestResult, this.Notes, this.CreatedByUserID);
        }

        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewTest()) {

                        Mode = enMode.Update;
                        return true;
                    } else
                        return false;
                case enMode.Update:
                    return _UpdateTest();
            }
            return false;
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID) {
            return TestData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID) {
            return GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3;
            // Because the whole tests is 3 which are vision test, writen test and practical test.
        }

    }
}

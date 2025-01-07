using System;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Buisness {
    public class TestType {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enTestType { VisionTest = 1, WrittenTest = 2, PracticalTest = 3 };

        public enTestType TestTypeID {  get; set; }
        public string TestTypeTitle {  get; set; }
        public string TestTypeDescription {  get; set; }
        public float TestTypeFees {  get; set; }

        public TestType() {
            this.TestTypeID = enTestType.VisionTest;
            this.TestTypeTitle = "";
            this.TestTypeDescription = "";
            this.TestTypeFees = 0;
            Mode = enMode.AddNew;
        }

        private TestType(enTestType TestTypeID, string TestTypeTitle, string TestTypeDescription, float TestTypeFees) {
            this.TestTypeID = TestTypeID;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.TestTypeFees = TestTypeFees;
            Mode = enMode.Update;
        }

        public static TestType Find(enTestType TestTypeID) {
            string TestTypeTitle = "", TestTypeDescription = "";
            float TestTypeFees = 0;
            if (TestTypeData.GetTestTypeInfoByID((int)TestTypeID, ref TestTypeTitle,
                ref TestTypeDescription, ref TestTypeFees)) {
                return new TestType(TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);
            } else
                return null;
        }

        public static DataTable GetAllTestTypes() {
            return TestTypeData.GetAllTestTypes();
        }

        private bool _AddNewTestType() {
            this.TestTypeID = (enTestType) TestTypeData.AddNewTestType(this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);

            return (this.TestTypeTitle != "");
        }

        private bool _UpdateTestType() {
            return TestTypeData.UpdateTestType((int)this.TestTypeID, this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
        }

        public bool Save() {
            switch (this.Mode) {
                case enMode.AddNew:
                    if (_AddNewTestType()) {

                        Mode = enMode.Update;
                        return true;
                    } else
                        return false;
                case enMode.Update:
                    return _UpdateTestType();
            }
            return false;
        }
    }
}

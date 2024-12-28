using System;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Buisness {
    public class LicenseClass {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int LicenseClassID { set; get; }
        public string ClassName { set; get; }
        public string ClassDescription { set; get; }
        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public float ClassFees { set; get; }

        public LicenseClass() {
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 18;
            this.DefaultValidityLength = 10;
            this.ClassFees = 0;

            Mode = enMode.AddNew;
        }
        
        private LicenseClass(int ID, string ClassName, string ClassDes,
            byte MinAllowedAge, byte DefValLength, float ClassFees) {
            this.LicenseClassID = ID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDes;
            this.MinimumAllowedAge = MinAllowedAge;
            this.DefaultValidityLength = DefValLength;
            this.ClassFees = ClassFees;

            Mode = enMode.Update;
        }

        public static LicenseClass Find(int LicenseClassID) {
            string ClassName = "", ClassDescription = "";
            byte DefaultValidityLength = 10, MinimumAllowedAge = 18;
            float ClassFees = 0;

            if (LicenseClassData.GetLicenseClassInfoByID(LicenseClassID, ref ClassName, ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
                return new LicenseClass(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);
            else
                return null;
        }
        
        public static LicenseClass Find(string ClassName) {
            int LicenseClassID = -1;
            string ClassDescription = "";
            byte DefaultValidityLength = 10, MinimumAllowedAge = 18;
            float ClassFees = 0;

            if (LicenseClassData.GetLicenseClassInfoByClassName(ClassName, ref LicenseClassID, ref ClassDescription, ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))
                return new LicenseClass(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);
            else
                return null;
        }

        public static DataTable GetAllLicenseClasses() {
            return LicenseClassData.GetAllLicenseClasses();
        }

        private bool _AddNewLicenseClass() {
            this.LicenseClassID = LicenseClassData.AddNewLicenseClass(this.ClassName, this.ClassDescription, this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);
            return (this.LicenseClassID != -1);
        }
        
        private bool _UpdateLicenseClass() {
            return LicenseClassData.UpdateLicenseClass(this.LicenseClassID, this.ClassName, this.ClassDescription, this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);
        }

        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewLicenseClass()) {
                        Mode = enMode.Update;
                        return true;
                    } else
                        return false;
                case enMode.Update:
                    return _UpdateLicenseClass();
            }
            return false;
        }

    }
}

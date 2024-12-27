using System;
using System.Data;
using System.Reflection.Emit;
using System.Xml.Linq;
using DVLD_DataAccess;

namespace DVLD_Buisness {
    public class ApplicationType {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { set; get; }
        public string Title { set; get; }
        public float Fees { set; get; }

        public ApplicationType() {
            this.ID = -1;
            this.Title = "";
            this.Fees = 0;
            Mode = enMode.AddNew;

        }

        private ApplicationType(int ID, string ApplicationTypeTitle, float ApplicationTypeFees) {
            this.ID = ID;
            this.Title = ApplicationTypeTitle;
            this.Fees = ApplicationTypeFees;
            Mode = enMode.Update;
        }

        public static ApplicationType Find(int ID) {
            string ApplicationTypeTitle = "";
            float ApplicationFees = 0;
            if (ApplicationTypeData.GetApplicationTypeInfoByID(ID, ref ApplicationTypeTitle, ref ApplicationFees))
                return new ApplicationType(ID, ApplicationTypeTitle, ApplicationFees);
            else
                return null;
        }

        private bool _AddNewApplicationType() {
            this.ID = ApplicationTypeData.AddNewApplicationType(Title, Fees);
            return (this.ID != -1);
        }
        
        private bool _UpdateApplicationType() {
            return ApplicationTypeData.UpdateApplicationType(this.ID, this.Title, this.Fees);
        }

        public static DataTable GetAllApplicationTypes() {
            return ApplicationTypeData.GetAllApplicationTypes();
        }

        public bool Save() {
            switch (Mode) {
                case enMode.AddNew:
                    if (_AddNewApplicationType()) {

                        Mode = enMode.Update;
                        return true;
                    } else 
                        return false;

                case enMode.Update:
                    return _UpdateApplicationType();

            }
            return false;
        }

    }
}

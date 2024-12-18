using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess {
    internal class LocalDrivingLicenseApplicationData {

        public static bool GetLocalDrivingLicenseApplicationInfoByID(
            int LocalDrivingLicenseApplicationID, ref int ApplicationID,
            ref int LicenseClassID) {
            bool isFound = false;

            string query = @"select * from LocalDrivingLicenseApplications
                             where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
        }

        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationID(
         int ApplicationID, ref int LocalDrivingLicenseApplicationID,
         ref int LicenseClassID) {
            bool isFound = false;

            string query = "select * from LocalDrivingLicenseApplications where ApplicationID = @ApplicationID";
        }

        public static DataTable GetAllLocalDrivingLicenseApplications() {
            DataTable dt = new DataTable();

            string query = @"select * from LocalDrivingLicenseApplications_View
                             order by ApplicationDate desc";
        }

        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID) {

            int LocalDrivingLicenseApplicationID = -1;

            string query = @"insert into LocalDrivingLicenseApplications (ApplicationID,LicenseClassID)
                             values (@ApplicationID,@LicenseClassID);
                             select SCOPE_IDENTITY();";

        }

        public static bool UpdateLocalDrivingLicenseApplication(
            int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID) {
            int rowsAffectes = 0;

            string query = @"update LocalDrivingLicenseApplications
                             set ApplicationID = @ApplicationID, 
                             LicenseClassID = @LicenseClassID
                             where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";


        }

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID) {

        }

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID) {

        }

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID) {

        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID) {

        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID) {

        }

    }
}

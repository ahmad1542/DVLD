using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Buisness;


namespace DVLD.Classes {
    internal static class Global {
        public static DVLD_Buisness.User CurrentUser;

        public static bool RememberUsernameAndPassword(string Username, string Password) {

            //File Handling in C#
            try {
                //this will get the current project directory folder.
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                // Define the path to the text file where you want to save the data
                string filePath = currentDirectory + "\\data.txt";

                //incase the username is empty, delete the file
                if (Username == "" && File.Exists(filePath)) {
                    File.Delete(filePath);
                    return true;
                }

                // concatenate username and passwrod with seperator.
                string dataToSave = Username + "#//#" + Password;

                // Create a StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter(filePath)) {
                    // Write the data to the file
                    writer.WriteLine(dataToSave);

                    return true;
                }
            } catch (Exception ex) {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

        }

        public static bool GetStoredCredential(ref string Username, ref string Password) {

            //this will get the stored username and password and will return true if found and false if not found.
            try {
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                string filePath = currentDirectory + "\\data.txt";

                if (File.Exists(filePath)) {
                    using (StreamReader reader = new StreamReader(filePath)) {

                        string line;
                        while ((line = reader.ReadLine()) != null) {
                            Console.WriteLine(line);
                            string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.None);

                            Username = result[0];
                            Password = result[1];
                        }
                        return true;
                    }
                } else
                    return false;

            } catch (Exception ex) {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

        }


    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CrossDock.Parameters
{
    public class ParametersExportImport
    {

        public bool ExportParameters(string fileName)
        {
            try
            {
                // Create directory string for saving parameters settings
                string jsonParameters = JsonConvert.SerializeObject(ParametersValues.Instance);
                string baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saved_files", "parameters_settings");
                string fileDirectory = Path.Combine(baseDirectory, fileName);


                // Create directory for saving settings, if doesn't exists
                if (!Directory.Exists(baseDirectory))
                {
                    DirectoryInfo di = Directory.CreateDirectory(baseDirectory);
                    Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(baseDirectory));
                }

                // Write paremeters settings to file
                using StreamWriter writer = new StreamWriter(fileDirectory);
                writer.WriteLine(jsonParameters);
            }
            catch(JsonException e)
            {
                Console.WriteLine("Json exception: " + e.Message);
                return false;
            }
            catch (DirectoryNotFoundException dirEx)
            {
                // Let the user know that the directory did not exist.
                Console.WriteLine("Directory not found: " + dirEx.Message);
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine("What error???: " + ex.Message);
                return false;
            }

            return true;
        }

        public bool ImportParameters(string filePath)
        {
            string jsonParameters = new string("");
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                        jsonParameters += line;
                }
                Console.WriteLine(jsonParameters);
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                return false;
            }

            try
            {
                ParametersValues.Instance = JsonConvert.DeserializeObject<ParametersValues>(jsonParameters);
            }
            catch (Exception e)
            {
                Console.WriteLine("Deserialization failed:");
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }
    }
}

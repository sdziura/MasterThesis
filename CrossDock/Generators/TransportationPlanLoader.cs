using CrossDock.Models;
using CrossDock.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
//using System.Text.Json;

namespace CrossDock.Generators
{
    public class TransportationPlanLoader
    {
        public bool ExportPlan(string fileName)
        {
            try
            {
                // Create directory string for saving parameters settings
                //string jsonParameters = JsonSerializer.Serialize(MainWindow.TransportationPlan);
                //string baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saved_files", "transportation_plan");
                //string fileDirectory = Path.Combine(baseDirectory, fileName);

                // Create directory string for saving parameters settings
                string jsonParameters = JsonConvert.SerializeObject(MainWindow.TransportationPlan);
                string baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saved_files", "transportation_plan");
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
            catch (JsonException e)
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
            catch (Exception ex)
            {
                Console.WriteLine("What error???: " + ex.Message);
                return false;
            }

            return true;
        }

        public bool ImportPlan(string filePath)
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
                MainWindow.TransportationPlan = JsonConvert.DeserializeObject<TransportationPlan>(jsonParameters);
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

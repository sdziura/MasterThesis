using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace CrossDock.Parameteres
{
    public class ParametersExportImport
    {

        public bool ExportParameters(string fileName)
        {
            Console.WriteLine("hej");
            try
            {
                string jsonParameters = JsonSerializer.Serialize(Parameters.Instance);
                string directory = AppDomain.CurrentDomain.BaseDirectory;
                directory = Path.Combine(directory, "saved_files", "parameters_settings", fileName);
                using StreamWriter writer = new StreamWriter(directory);
                writer.WriteLine(jsonParameters);
            }
            catch(JsonException e)
            {
                Console.WriteLine(e.Message);
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
            }


            return true;
        }

        public bool ImportParameters(string fileName)
        {
            return true;
        }
    }
}

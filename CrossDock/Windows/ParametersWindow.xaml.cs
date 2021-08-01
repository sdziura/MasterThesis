using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CrossDock.Parameters;

namespace CrossDock.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy ParametersWindow.xaml
    /// </summary>
    public partial class ParametersWindow : Window
    {
        ParametersExportImport parametersExportImport = new ParametersExportImport();
        public ParametersWindow()
        {
            InitializeComponent();
        }

        private void LoadParametersButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON Files (*.json)|*.json|TXT Files (*.txt)|*.txt|ALL Files |*";
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string fileName = dlg.FileName;
                parametersExportImport.ImportParameters(fileName);
            }
        }

        private void SaveParametersButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON Files (*.json)|*.json|TXT Files (*.txt)|*.txt|ALL Files (*)|*";
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string fileName = dlg.FileName;
                parametersExportImport.ExportParameters(fileName);
            }
        }
    }
}

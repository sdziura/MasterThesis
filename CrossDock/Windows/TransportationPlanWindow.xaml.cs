using CrossDock.Generators;
using CrossDock.Models;
using CrossDock.Parameters;
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

namespace CrossDock.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy TransportationPlanWindow.xaml
    /// </summary>
    public partial class TransportationPlanWindow : Window
    {
        TransportationPlanLoader loader = new TransportationPlanLoader();
        
        public TransportationPlanWindow()
        {
            InitializeComponent();
            
            var colId = new DataGridTextColumn();
            var bindingId = new Binding("Id");
            colId.Binding = bindingId;
            colId.Header = new string("Id");
            colId.IsReadOnly = true;
            DemandGrid.Columns.Add(colId); 
            for ( int i = 0; i < ParametersValues.Instance.NumberOfInboundTrucks; i++)
            {
                var col = new DataGridTextColumn();
                var binding = new Binding("Demand[" + i + "]");
                col.Binding = binding;
                col.Header = new string(i.ToString());
                DemandGrid.Columns.Add(col);
            }
            ArrivalTimesGrid.ItemsSource = MainWindow.TransportationPlan.UnloadingTasks;
            DemandGrid.ItemsSource = MainWindow.TransportationPlan.LoadingTasks;
        }

        private void GenerateDataButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateDataWindow generateDataWindow = new GenerateDataWindow();
            generateDataWindow.Show();
            //TEST
            //HelperWindow helper = new HelperWindow(MainWindow.TransportationPlan.ArrivalTimes[0]);
            //helper.Show();
            //TEST
        }

        private void SaveDataButton_Click(object sender, RoutedEventArgs e)
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
                loader.ExportPlan(fileName);
            }
        }

        private void LoadDataButton_Click(object sender, RoutedEventArgs e)
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
                loader.ImportPlan(fileName);
            }

            ArrivalTimesGrid.ItemsSource = MainWindow.TransportationPlan.UnloadingTasks;
            DemandGrid.ItemsSource = MainWindow.TransportationPlan.LoadingTasks;
        }
    }
}

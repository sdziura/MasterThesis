using CrossDock.Models;
using CrossDock.NeighborhoodSearch;
using CrossDock.Schedulers;
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
    /// Logika interakcji dla klasy RunAlgoWindow.xaml
    /// </summary>
    public partial class RunAlgoWindow : Window
    {
        BeeColony colony = new BeeColony(MainWindow.TransportationPlan, new NeighborhoodSearchWorker(), new CompareTaskTime(), new CompareTaskRandom<LoadingTask>());
        public RunAlgoWindow()
        {
            InitializeComponent();
        }

        private void RunStaticButon_Click(object sender, RoutedEventArgs e)
        {
            colony = new BeeColony(MainWindow.TransportationPlan, new NeighborhoodSearchWorker(), new CompareTaskTime(), new CompareTaskRandom<LoadingTask>());
            int a = colony.Colony[ParametersValues.Instance.ScoutBeesNumber - 1].TimeOfWork;
            StaticWorstBlock.DataContext = a;
            int b = colony.BestBee.TimeOfWork;
            StaticInitBlock.DataContext = b;

            colony.AllGenerations();
            /*var colId = new DataGridTextColumn();
            var bindingId = new Binding("Id");
            colId.Binding = bindingId;
            colId.Header = new string("Id");
            colId.IsReadOnly = true;
            DemandGrid.Columns.Add(colId);
            for (int i = 0; i < ParametersValues.Instance.NumberOfInboundTrucks; i++)
            {
                var col = new DataGridTextColumn();
                var binding = new Binding("Demand[" + i + "]");
                col.Binding = binding;
                col.Header = new string(i.ToString());
                DemandGrid.Columns.Add(col);
            }
            ArrivalTimesGrid.ItemsSource = MainWindow.TransportationPlan.UnloadingTasks;
            DemandGrid.ItemsSource = MainWindow.TransportationPlan.LoadingTasks;
            StaticGrid.ItemsSource = colony.*/

            StaticResultBlock.DataContext = colony.BestBee.TimeOfWork;
        }

        private void RunDynamicButon_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

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
using System.Linq;

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
            colony = new BeeColony(MainWindow.TransportationPlan, new NeighborhoodSearchWorker(), new CompareTaskRandom<UnloadingTask>(), new CompareTaskRandom<LoadingTask>());
            int a = colony.Colony[ParametersValues.Instance.ScoutBeesNumber - 1].TimeOfWork;
            StaticWorstBlock.DataContext = a;
            int b = colony.BestBee.TimeOfWork;
            StaticInitBlock.DataContext = b;
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var generationResults = colony.AllGenerations();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

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
            StaticGenResultsBox.Text = string.Join("\n", generationResults);
            //Clipboard.SetDataObject(generationResults.ToString());
            //StaticResultsList.DataContext = generationResults;
            StaticResultBlock.DataContext = colony.BestBee.TimeOfWork;
            StaticTimeBlock.DataContext = elapsedMs;
        }

        private void RunDynamicButon_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(LateTruckBox.Text, out int lateTruck)) lateTruck = 0;
            if (!int.TryParse(LatenessBox.Text, out int lateness)) lateness = 0;
            if (!int.TryParse(TimeOfChangeBox.Text, out int timeOfChange)) timeOfChange = 0;
            if (timeOfChange == 0) timeOfChange = colony.BestBee.Plan.UnloadingTasks[lateTruck].ArrivalTime;
            if (timeOfChange <= colony.BestBee.Plan.UnloadingTasks[lateTruck].ArrivalTime)
            {
                Bee lateBee = colony.BestBee.Clone();
                FifoScheduler lateSchedule = new FifoScheduler(lateBee.Plan);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                lateBee.Late(lateTruck, lateness);
                lateBee = lateSchedule.DynamicReschedule(lateBee, timeOfChange);
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                DynamicResultBlock.DataContext = lateBee.TimeOfWork;
                DynamicTimeBlock.DataContext = elapsedMs;
            }
        }

        private void RunExactButon_Click(object sender, RoutedEventArgs e)
        {
            DepthFirstScheduler depth = new DepthFirstScheduler(MainWindow.TransportationPlan);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Bee bee = depth.Schedule();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            ExactResultBlock.DataContext = bee.TimeOfWork;
            ExactTimeBlock.DataContext = elapsedMs;
        }

        private void RunMultitButon_Click(object sender, RoutedEventArgs e)
        {

            if (!int.TryParse(MultiBox.Text, out int multi)) return;
            var results = new int[multi];
            var times = new long[multi];
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < multi; i++)
            {
                colony = new BeeColony(MainWindow.TransportationPlan, new NeighborhoodSearchWorker(), new CompareTaskRandom<UnloadingTask>(), new CompareTaskRandom<LoadingTask>());
                watch = System.Diagnostics.Stopwatch.StartNew();
                colony.AllGenerations();
                watch.Stop();
                results[i] = colony.BestBee.TimeOfWork;
                times[i] = watch.ElapsedMilliseconds;
            }
            StaticMultiResultsBox.Text = string.Join("\n", results);
            StaticMultiResultsBox.Text += "\nTimes:\n";
            StaticMultiResultsBox.Text += string.Join("\n", times);
        }
    }
}

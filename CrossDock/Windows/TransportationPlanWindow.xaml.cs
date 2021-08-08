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
        private TransportationPlan _transportationPlan;
        //private int[] _arrivalTimes = new int[ParametersValues.Instance.NumberOfInboundTrucks];
        //private int[,] _demand = new int[ParametersValues.Instance.NumberOfOutboundTrucks, ParametersValues.Instance.NumberOfInboundTrucks];
        //private LoadingTask[] _demand = new LoadingTask[ParametersValues.Instance.NumberOfOutboundTrucks];


        public TransportationPlanWindow(TransportationPlan transportationPlan)
        {
            InitializeComponent();
            //TransportationPlan = transportationPlan;
            //ArrivalTimes = transportationPlan.ArrivalTimes;
            //Demand = transportationPlan.LoadingTasks;
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
            ArrivalTimesGrid.ItemsSource = transportationPlan.UnloadingTasks;
            DemandGrid.ItemsSource = transportationPlan.LoadingTasks;
        }

        //public int[] ArrivalTimes { get => _arrivalTimes; set => _arrivalTimes = value; }
        //public int[,] Demand { get => _demand; set => _demand = value; }
       // public LoadingTask[] Demand { get => _demand; set => _demand = value; }

        public TransportationPlan TransportationPlan { get => _transportationPlan; set => _transportationPlan = value; }
    }
}

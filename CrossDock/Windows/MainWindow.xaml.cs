﻿using CrossDock.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CrossDock.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TransportationPlan transportationPlan = new TransportationPlan();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ParameteresButton_Click(object sender, RoutedEventArgs e)
        {
            ParametersWindow parametersWindow = new ParametersWindow();
            parametersWindow.Show();
        }

        private void TransportationPlanButton_Click(object sender, RoutedEventArgs e)
        {
            TransportationPlanWindow transportationPlanWindow = new TransportationPlanWindow(transportationPlan);
            transportationPlanWindow.Show();
        }
    }
}

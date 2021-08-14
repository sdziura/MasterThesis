using CrossDock.Generators;
using CrossDock.Models;
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
    /// Logika interakcji dla klasy GenerateDataWindow.xaml
    /// </summary>
    public partial class GenerateDataWindow : Window
    {
        private TestDataGenerator _testDataGenerator = new TestDataGenerator();
        public GenerateDataWindow()
        {
            InitializeComponent();
        }

        internal TestDataGenerator TestDataGenerator { get => _testDataGenerator; set => _testDataGenerator = value; }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(maxArrivalTimesBox.Text, out int maxTime)) maxTime = 0;
            if (!int.TryParse(maxProductDemandBox.Text, out int maxProductAmount)) maxProductAmount = 0;
            if (!int.TryParse(avgPrecentageOfProductTypesBox.Text, out int precentageChanceOfProduct)) precentageChanceOfProduct = 0;

            MainWindow.TransportationPlan = TestDataGenerator.GenerateTransportationPlan(maxTime, maxProductAmount, precentageChanceOfProduct);
            this.Close();
        }
    }
}

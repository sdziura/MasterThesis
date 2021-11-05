using CrossDock.Models;
using CrossDock.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrossDock.Generators
{
    public class TestDataGenerator
    {
        public TransportationPlan GenerateTransportationPlan(int maxArrivalTimes, int maxProductDemand, int avgPrecentageOfProductTypes)
        {
            Random random = new Random();
            int[] arrivalTimes = new int[ParametersValues.Instance.NumberOfInboundTrucks];
            int[,] demand = new int[ParametersValues.Instance.NumberOfOutboundTrucks, ParametersValues.Instance.NumberOfInboundTrucks];
            for (int i = 0; i < ParametersValues.Instance.NumberOfInboundTrucks; i++)
                arrivalTimes[i] = random.Next(maxArrivalTimes);
            for (int i = 0; i < ParametersValues.Instance.NumberOfOutboundTrucks; i++)
            {
                int sum = 0;
                for (int j = 0; j < ParametersValues.Instance.NumberOfInboundTrucks; j++)
                {
                    if (random.Next(100) < avgPrecentageOfProductTypes)
                        demand[i, j] = random.Next(maxProductDemand) + 1;
                    sum += demand[i, j];
                }
                if (sum == 0)
                    demand[i, random.Next(ParametersValues.Instance.NumberOfInboundTrucks)] = random.Next(maxProductDemand);
            }

            return new TransportationPlan(arrivalTimes, demand);
        }
    }
}
    

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CrossDock.Parameters;

namespace CrossDock.Models
{
    public class LoadingTask
    {
        private int _id;
        private int[] _demand;


        public LoadingTask(int id, int[] demand)
        {
            if (demand.Length == ParametersValues.Instance.NumberOfInboundTrucks)
            {
                _id = id;
                _demand = demand;
            }
            else Console.WriteLine("Wrong length of demand array");
        }

        public int Id { get => _id; }
        public int[] Demand { get => _demand; set => _demand = value; }
        public int ProductsAmount { get => _demand.Sum(); }
        public LoadingTask Clone()
        {
            return new LoadingTask(Id, (int[])Demand.Clone());
        }
    }
}

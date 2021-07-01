using System;
using System.Collections.Generic;
using System.Text;

namespace CrossDock.Models
{
    public class UnloadingTask
    {
        private int _id;
        private int _arrivalTime;
        private int _productsAmount;

        //static int _currentTasksAmount;

        public UnloadingTask(int id, int arrivalTime, int productsAmount)
        {
            _id = id;
            ArrivalTime = arrivalTime;
            ProductsAmount = productsAmount;
        }

        public int Id { get => _id; }
        public int ArrivalTime { get => _arrivalTime; set => _arrivalTime = value; }
        public int ProductsAmount { get => _productsAmount; set => _productsAmount = value; }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using CrossDock.Parameters;

namespace CrossDock.Models
{
    public class Bee
    {
        // Rows number: number of tasks
        // Columns number : 4 (0:dock ID, 1:worker team ID, 2:start time, 3:end time)
        private int[,] _scheduleUnloading;
        private int[,] _scheduleLoading;
        private TransportationPlan _plan;
        private bool isSolutionFound = true;

        public Bee()
        {
            ScheduleUnloading = new int[ParametersValues.Instance.NumberOfInboundTrucks, 4];
            ScheduleLoading = new int[ParametersValues.Instance.NumberOfOutboundTrucks, 4];
        }
        public Bee(bool k)
        {
            IsSolutionFound = k;
        }
        public Bee(TransportationPlan plan, int[,] scheduleUnloading, int[,] scheduleLoading)
        {
            _scheduleUnloading = scheduleUnloading;
            _scheduleLoading = scheduleLoading;
            _plan = plan;
        }

        public int[,] ScheduleUnloading { get => _scheduleUnloading; set => _scheduleUnloading = value; }
        public int[,] ScheduleLoading { get => _scheduleLoading; set => _scheduleLoading = value; }
        public TransportationPlan Plan { get => _plan; set => _plan = value; }
        public int TimeOfWork { get => IsSolutionFound? _TimeOfWork(): Int32.MaxValue;}
        public bool IsSolutionFound { get => isSolutionFound; set => isSolutionFound = value; }

        private int _TimeOfWork()
        {
            int maxTime = 0;
            //for(int i = 0; i < ParametersValues.Instance.NumberOfInboundTrucks; i++)
             //   if (ScheduleUnloading[i, 3] > maxTime) 
             //       maxTime = ScheduleUnloading[i, 3];

            for (int i = 0; i < ParametersValues.Instance.NumberOfOutboundTrucks; i++)
                if (ScheduleLoading[i, 3] > maxTime)
                    maxTime = ScheduleLoading[i, 3];

            return maxTime;

        }
        public bool CheckStorage()
        {
            int currentStorageOccupation = 0;
            StorageAction[] storageAction = new StorageAction[ParametersValues.Instance.NumberOfInboundTrucks + ParametersValues.Instance.NumberOfOutboundTrucks];

            // List the effects of unloading tasks on storage
            for(int i = 0; i < ParametersValues.Instance.NumberOfInboundTrucks; i++)
            {
                storageAction[i].actionTime = ScheduleUnloading[i, 2];
                storageAction[i].actionAmount = Plan.UnloadingTasks[i].ProductsAmount;
            }

            // List the effects of loading tasks on storage
            for (int i = 0; i < ParametersValues.Instance.NumberOfOutboundTrucks; i++)
            {
                storageAction[i + ParametersValues.Instance.NumberOfInboundTrucks].actionTime = ScheduleLoading[i, 3];
                storageAction[i + ParametersValues.Instance.NumberOfInboundTrucks].actionAmount = -Plan.LoadingTasks[i].ProductsAmount;
            }

            // Sort actions on storage
            Array.Sort(storageAction, new CompareStorageActions());

            // Check if storage overloaded in any moment
            for(int i = 0; i < (ParametersValues.Instance.NumberOfInboundTrucks + ParametersValues.Instance.NumberOfOutboundTrucks); i++)
            {
                currentStorageOccupation += storageAction[i].actionAmount;
                if (currentStorageOccupation > ParametersValues.Instance.MaxStorageCapacity)
                    return false;
            }

            return true;

        }
        public void PrintSchedule()
        {
            Console.WriteLine("\nUnloading schedule:\n");
            for (int i = 0; i < ParametersValues.Instance.NumberOfInboundTrucks; i++)
            {
                for (int j = 0; j < 4; j++)
                    Console.Write(ScheduleUnloading[i, j] + " ");
                Console.WriteLine();
            }

            Console.WriteLine("\nLoading schedule:\n");
            for (int i = 0; i < ParametersValues.Instance.NumberOfOutboundTrucks; i++)
            {
                for (int j = 0; j < 4; j++)
                    Console.Write(ScheduleLoading[i, j] + " ");
                Console.WriteLine();
            }

        }
        public void Late(int truckId, int lateness)
        {
            _plan.UnloadingTasks[truckId].ArrivalTime += lateness;
        }
        public Bee Clone()
        {
            return new Bee(_plan.Clone(), (int[,])ScheduleUnloading.Clone(), (int[,])ScheduleLoading.Clone());
        }
    }

    struct StorageAction
    {
        public int actionTime;
        public int actionAmount;
    }

    public class CompareStorageActions : IComparer
    {
        // Compare for which task the inbound truck comes first.
        int IComparer.Compare(Object x, Object y)
        {
            StorageAction a = (StorageAction)x;
            StorageAction b = (StorageAction)y;
            if (a.actionTime < b.actionTime)
                return -1;
            else if (a.actionTime > b.actionTime)
                return 1;
            else
            {
                if (a.actionAmount > b.actionAmount)
                    return -1;
                else if (a.actionAmount < b.actionAmount)
                    return 1;
                else
                    return 0;
            }        
        }
    }
}

TransportationPlan
	Array of UnloadingTask
	Array of LoadingTask



Bee
	2D array ScheduleUnloading
	2D array ScheduleLoading

	Columns : 4 (dock ID, worker team ID, start time, end time) 
	Rows : one for each task. Row index = taskID
		Problem: Other ideas: 
			a) To use objects holding this information, easier sorting and standard functions 
			that could help in some operations.
			b) Multidimensional cube- each dimension would be different resource. 
			Easier data manipulation, but very data consuming.

	methods:
	To get the TimeOfWork, the time when the last task is finished is taken into account. 
	To check if storage is not overloaded




FIFOScheduler
	
	Problem - the idea was to find the earliest empty time window for given resources, 
	but this process would be very complex and time consuming. Instead, the latest time
	when all given resources are finishing all tasks that were scheduled until now.
	
	Scheduler schedule loading tasks as soon as they are available
	and can use different comprarers to select the queue for unloading tasks.




Neighourhood
 
    1) Function for finiding neighorhood takes the actual solution and the transportation plan (object with inforamtion about arrival of trucks and the orders).    

 
    2) Draw a random resource (depending on type of Neigborhood search it can be worker ID or dock ID)


    3) Unshedules tasks from given resource. 

 
    4) Schedule it back to this resource, but with random choice of order and other resources 
        (eg. if we uscheduled tasks with workerID = 3, then we schedule them back to this worker but with random dockID).
    
    5) First schedules unloading task from the queue, then (after each schedulued unloading task) checks if loading task can be scheduled:
     - if yes- it is shceduled and the next loading task is checked.
     - if not- it goes to back to unloading tasks (next loading task can't be scheduled, until preceeding task wasn't, unlike in Schedule function).
    
	 
	 
	
	

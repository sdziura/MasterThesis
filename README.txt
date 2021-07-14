TransportationPlan
	Array of UnloadingTask
	Array of LoadingTask

Bee
	2D array ScheduleUnloading
	2D array ScheduleLoading

	Columns : 4 (dock ID, worker team ID, start time, end time)
	Rows : one for each task. Row index = taskID


FIFOScheduler
	
	Problem - the idea was to find the earliest empty time window for given resources, 
	but this process would be very complex and time consuming. Instead, the latest time
	when all given resources are finishing all tasks that were scheduled until now.
	
	Scheduler schedule loading tasks as soon as they are available
	and can use different comprarers to select the queue for unloading tasks.

Neighourhood

	Unshedule tasks from given resource and schedule it back to this resource, but with random choice of order and other resources.
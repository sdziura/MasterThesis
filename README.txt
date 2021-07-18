TransportationPlan
	Array of UnloadingTask
	Array of LoadingTask

Bee
	2D array ScheduleUnloading
	2D array ScheduleLoading

	Columns : 4 (dock ID, worker team ID, start time, end time)
	Rows : one for each task. Row index = taskID

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

	Unshedules tasks from given resource and schedule it back to this resource, but with random choice of order and other resources.
	First schedules unloading task from the queue, then checks if loading task can be scheduled:
	 - if yes- it is shceduled and the next loading task is checked.
	 - if not- it goes to back to unloading tasks (next loading task can't be scheduled, until preceeding task wasn't, unlike in Scedule function).
	*Still, rescheduled tasks are put after the time when resources are free.

BeeColony!!!
	
	Looking for each neighbour lasts until there will be the one which has CheckStorage = 1. (get rid of infinite loop!!!) 

	
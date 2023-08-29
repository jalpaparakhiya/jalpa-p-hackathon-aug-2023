using TaskExecutor.Models;

namespace TaskExecutor
{
	public class TaskQueueManager
	{
		public Queue<TaskItem> TaskQueue { get; }

		public TaskQueueManager()
		{
			Console.WriteLine("TaskQueueManager constructor called.");
			TaskQueue = new Queue<TaskItem>();
		}
	}
}

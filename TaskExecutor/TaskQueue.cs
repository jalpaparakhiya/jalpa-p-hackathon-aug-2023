using TaskExecutor.Models;

namespace TaskExecutor
{
	public class TaskQueue
	{
		private Queue<TaskItem> _queue = new Queue<TaskItem>();
		public void AddTask(TaskItem task)
		{
			Console.WriteLine("Task Added in task queue...");
			_queue.Enqueue(task);
			
		}

		public TaskItem? GetTask(string taskId)
		{
			if (_queue.Count > 0)
				return _queue.Where(s => s.Id.Equals(taskId)).FirstOrDefault();
			return null;
		}

		public List<TaskItem> GetTasksByStatus(TaskExecutor.Models.TaskStatus status)
		{
			return _queue.Where(t => t.Status == status).ToList();
		}

		public TaskItem? Dequeue()
		{
			return _queue.Dequeue();
		}

		public void RemoveTaskFromQueue( string taskId)
		{
			Queue<TaskItem> queue = _queue;
			Queue<TaskItem> tempQueue = new Queue<TaskItem>();

			while (queue.Count > 0)
			{
				TaskItem task = queue.Dequeue();
				if (task.Id != taskId)
				{
					tempQueue.Enqueue(task);
				}
			}

			foreach (var task in tempQueue)
			{
				queue.Enqueue(task);
			}
		}
	}
}

using TaskExecutor.Models;

namespace TaskExecutor
{
	public class TaskQueue
	{
		private Queue<TaskItem> _queue = new Queue<TaskItem>();

		public void Enqueue(TaskItem task)
		{
			_queue.Enqueue(task);
		}

		public TaskItem? Dequeue()
		{
			if (_queue.Count > 0)
				return _queue.Dequeue();
			return null;
		}
	}
}

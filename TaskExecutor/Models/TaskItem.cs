namespace TaskExecutor.Models
{
	public class TaskItem
	{
		public string Id { get; set; }
		public TaskStatus Status { get; set; }

		public bool Nsfw { get; set; }
		public string Url { get; set; }
		public DateTime Executed { get; set; }
		public string TaskStatus => Status.ToString();
	}

	public enum TaskStatus
	{
		Pending,
		Running,
		Completed,
		Failed
	}
}

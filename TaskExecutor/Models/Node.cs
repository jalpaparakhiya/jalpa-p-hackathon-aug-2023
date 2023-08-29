namespace TaskExecutor.Models
{
	public class Node
	{
		public string Name { get; set; }
		public NodeStatus Status { get; set; }
		public string Address { get; set; }
		public string MemesPath { get; set; }
		public DateTime LastUpdated { get; set; }

		public string NodeStatus => Status.ToString();

	}

	public enum NodeStatus
	{
		Available,
		Busy,
		Offline
	}
}

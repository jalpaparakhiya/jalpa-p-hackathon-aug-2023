namespace TaskExecutor.Models
{
	public class Node
	{
		public string Name { get; set; }
		public NodeStatus Status { get; set; }
		public string Address { get; set; }
	}

	public enum NodeStatus
	{
		Available,
		Busy,
		Offline
	}
}

using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;
using TaskExecutor.Models;
using TaskStatus = TaskExecutor.Models.TaskStatus;

namespace TaskExecutor
{
	public class TaskAllocator
	{
		private TaskQueue _taskQueue = new TaskQueue();
		private NodeManager _nodeManager = new NodeManager();
		private TaskExecutor _taskExecutor = new TaskExecutor();

		public async Task SubmitTask(TaskItem task, string memePath, NodeRegistrationRequest node )
		{
			_nodeManager.RegisterNode(new Node()
			{
				Address = node.Address,
				Name = node.Name,
				Status = NodeStatus.Available
			});
			_taskQueue.Enqueue(task);
			
			await AllocateTasks(memePath);
		}

		public async Task UnregisterNode(string name)
		{
			await _nodeManager.UnregisterNode(name);
		}

		private async Task AllocateTasks(string memePath)
		{
			while (true)
			{
				Console.WriteLine("Allocating tasks...");
				var task = _taskQueue.Dequeue();
				if (task == null)
					break;

				var availableNode = _nodeManager.GetAvailableNode();
				Console.WriteLine($"Available node:{availableNode.Name}");
				if (availableNode == null)
					break;

				task.Status = TaskStatus.Running;
				availableNode.Status = NodeStatus.Busy;

				await _taskExecutor.ExecuteMemeTask(task, memePath, availableNode);

				availableNode.Status = NodeStatus.Available;
			}
		}

	}
}

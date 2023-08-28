using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;
using TaskExecutor.Models;
using TaskStatus = TaskExecutor.Models.TaskStatus;

namespace TaskExecutor
{
	public class TaskAllocator
	{
		private readonly TaskQueue _taskQueue;
		private readonly NodeManager _nodeManager;
		private readonly TaskExecution _taskExecutor;

		public TaskAllocator(TaskQueue taskQueue, NodeManager nodeManager, TaskExecution taskExecutor)
		{
			_taskExecutor = taskExecutor;
			_nodeManager =	nodeManager;
			_taskQueue = taskQueue;	
		}
		public async Task SubmitTask(TaskItem task)
		{
			_taskQueue.Enqueue(task);
			
			await AllocateTasks();
		}

		public void RegisterNode(NodeRegistrationRequest node)
		{
			_nodeManager.RegisterNode(new Node()
			{
				Address = node.Address,
				Name = node.Name,
				Status = NodeStatus.Available,
				MemesPath = node.MemesPath
			});
		}


		public async Task UnregisterNode(string name)
		{
			await _nodeManager.UnregisterNode(name);
		}

		private async Task AllocateTasks()
		{
			while (true)
			{
				Console.WriteLine("Allocating tasks...");
				var task = _taskQueue.Dequeue();
				if (task == null)
					break;

				var availableNode = _nodeManager.GetAvailableNode();
				if (availableNode == null)
				{
					Console.WriteLine("No any node available....");

					break;
				}
				Console.WriteLine($"Available node:{availableNode.Name}");

				task.Status = TaskStatus.Running;
				availableNode.Status = NodeStatus.Busy;

				await _taskExecutor.ExecuteMemeTask(task, availableNode);

				availableNode.Status = NodeStatus.Available;
			}
		}

	}
}

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
			_nodeManager = nodeManager;
			_taskQueue = taskQueue;
			this.AllocateTaskByNode();
		}
		public async Task SubmitTask(TaskItem task)
		{
			_taskQueue.AddTask(task);

			await AllocateTasks(task);
		}

		public List<TaskItem> GetTaskByStatus(TaskStatus status)
		{
			return _taskQueue.GetTasksByStatus(status);
		}

		private async Task AllocateTasks(TaskItem t)
		{

			var availableNode = _nodeManager.GetAvailableNode();
			if (availableNode == null)
			{
				Console.WriteLine("No any node available....");
				return;
			}
			var task = _taskQueue.GetTask(t.Id);
			if (task == null)
				return;

			task.Status = TaskStatus.Running;
			availableNode.Status = NodeStatus.Busy;

			Console.WriteLine($"Allocating tasks to node: {availableNode.Name} and TaskId : {task.Id}({task.Status})");


			await _taskExecutor.ExecuteMemeTask(task, availableNode);

			task.Executed = DateTime.Now;
			availableNode.Status = NodeStatus.Available;
			availableNode.LastUpdated = DateTime.Now;
			if(task.Status == TaskStatus.Completed || task.Status == TaskStatus.Failed)
			{
				_taskQueue.RemoveTaskFromQueue(task.Id);
			}
			Console.WriteLine($"Now node {availableNode.Name} free.");
		}

		public void AllocateTaskByNode()
		{
			Timer timer = new Timer(async _ => await this.AllocateTasks(), null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

		}
	}
}

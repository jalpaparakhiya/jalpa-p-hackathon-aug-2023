using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskExecutor.Models;
using TaskStatus = TaskExecutor.Models.TaskStatus;

namespace TaskExecutor.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class NodesController : ControllerBase
	{

		private readonly TaskAllocator _taskAllocate;
		private readonly NodeManager _nodeManager;

		public NodesController(TaskAllocator taskAllocator, NodeManager nodeManager)
		{
			_taskAllocate = taskAllocator;
			_nodeManager = nodeManager;
		}

		[HttpPost]
		[Route("register")]
		public IActionResult RegisterNode([FromBody] NodeRegistrationRequest node)
		{

			_nodeManager.RegisterNode(new Node()
			{
				Address = node.Address,
				Name = node.Name,
				Status = NodeStatus.Available,
				MemesPath = node.MemesPath,
				LastUpdated = DateTime.Now
			});
			Console.WriteLine($"Register node {node.Name}...");

			return Ok();
		}

		[HttpDelete]
		[Route("unregister/{name}")]
		public async Task<IActionResult> RegisterNode(string name)
		{
			Console.WriteLine($"Unregistering node {name}...");
			await _nodeManager.UnregisterNode(name);

			return Ok();
		}

		[HttpGet]
		[Route("nodes")]
		public List<Node> GetNodes()
		{
			return _nodeManager.GetNodes();
		}

		[HttpGet]
		[Route("task-add")]
		public async Task<IActionResult> TaskAdd()
		{

			TaskItem task = new TaskItem
			{
				Id = Guid.NewGuid().ToString(),
				Status = TaskStatus.Pending,
				Executed = DateTime.Now
			};


			await _taskAllocate.SubmitTask(task);

			Console.WriteLine($"Task Id: {task.Id}({task.Status})");
			
			return Ok();
		}

		[HttpGet]
		[Route("tasks/{status}")]
		public List<TaskItem> TaskGetByStatus(TaskStatus status)
		{

			return _taskAllocate.GetTaskByStatus(status);

		}

	}
}

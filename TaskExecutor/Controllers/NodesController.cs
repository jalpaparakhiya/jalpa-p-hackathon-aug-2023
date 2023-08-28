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

		public NodesController(TaskAllocator taskAllocator)
		{
			_taskAllocate = taskAllocator;
		}

		[HttpPost]
		[Route("register")]
		public IActionResult RegisterNode([FromBody] NodeRegistrationRequest node)
		{

			_taskAllocate.RegisterNode(node);
			Console.WriteLine($"Register node {node.Name}...");

			return Ok();
		}

		[HttpDelete]
		[Route("unregister/{name}")]
		public async Task<IActionResult> RegisterNode(string name)
		{
			Console.WriteLine($"Unregistering node {name}...");
			await _taskAllocate.UnregisterNode(name);

			return Ok();
		}

		[HttpGet]
		[Route("task-add")]
		public async Task<IActionResult> TaskAdd()
		{

			TaskItem task = new TaskItem
			{
				Id = Guid.NewGuid().ToString(),
				Status = TaskStatus.Pending
			};


			await _taskAllocate.SubmitTask(task);

			Console.WriteLine($"Task Id: {task.Id}");
			Console.WriteLine($"Task Status: {task.Status}");

			return Ok();
		}

	}
}

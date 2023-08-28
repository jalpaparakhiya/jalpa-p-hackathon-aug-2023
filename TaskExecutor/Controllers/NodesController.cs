using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

		private TaskAllocator _taskAllocate;

		public NodesController()
		{
			_taskAllocate = new TaskAllocator();
		}

		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> RegisterNode([FromBody] NodeRegistrationRequest node)
		{

			TaskItem task = new TaskItem
			{
				Id = Guid.NewGuid().ToString(),
				Status = TaskStatus.Pending
			};


			await _taskAllocate.SubmitTask(task, node.MemesPath, node);

			Console.WriteLine($"Task Id: {task.Id}");
			Console.WriteLine($"Task Status: {task.Status}");

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
	}
}

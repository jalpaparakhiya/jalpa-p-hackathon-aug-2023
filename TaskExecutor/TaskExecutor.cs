using System.Net;
using TaskExecutor.Models;
using TaskStatus = TaskExecutor.Models.TaskStatus;

namespace TaskExecutor
{
	public class TaskExecutor
	{
		private static readonly HttpClient _httpClient = new HttpClient();

		public async Task ExecuteMemeTask(TaskItem memeTask, string memePath, Node node)
		{
			try
			{
				Console.WriteLine($"Executing task {memeTask.Id} on node {node.Name}...");
				memeTask.Status = TaskStatus.Running;

				// Fetch meme data from the API
				HttpResponseMessage response = await _httpClient.GetAsync("https://meme-api.com/gimme/wholesomememes");
				if (response.IsSuccessStatusCode)
				{
					string responseBody = await response.Content.ReadAsStringAsync();
					TaskItem responseMeme = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskItem>(responseBody);

					if (responseMeme.Nsfw)
					{
						memeTask.Status = TaskStatus.Failed;
					}
					else
					{
						// Download and save meme image
						using (WebClient webClient = new WebClient())
						{
							string memeFileName = $"{memePath}\\meme_{memeTask.Id}.jpg";
							webClient.DownloadFile(responseMeme.Url, memeFileName);
							memeTask.Status = TaskStatus.Completed;
						}
					}
				}
				else
				{
					memeTask.Status = TaskStatus.Failed;
				}
			}
			catch (Exception)
			{
				memeTask.Status = TaskStatus.Failed;
			}
		}
	}
}

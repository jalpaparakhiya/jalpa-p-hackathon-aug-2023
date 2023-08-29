using System.Net;
using TaskExecutor.Models;
using TaskStatus = TaskExecutor.Models.TaskStatus;

namespace TaskExecutor
{
	public class TaskExecution
	{
		private static readonly HttpClient _httpClient = new HttpClient();

		public async Task ExecuteMemeTask(TaskItem task, Node node)
		{
			try
			{
				task.Status = TaskStatus.Running;

				// Fetch meme data from the API
				HttpResponseMessage response = await _httpClient.GetAsync("https://meme-api.com/gimme/wholesomememes");
				if (response.IsSuccessStatusCode)
				{
					string responseBody = await response.Content.ReadAsStringAsync();
					TaskItem responseMeme = Newtonsoft.Json.JsonConvert.DeserializeObject<TaskItem>(responseBody);

					if (responseMeme.Nsfw)
					{
						task.Status = TaskStatus.Failed;
					}
					else
					{
						// Download and save meme image
						using (WebClient webClient = new WebClient())
						{
							string memeFileName = $"{node.MemesPath}\\meme_{task.Id}.jpg";
							webClient.DownloadFile(responseMeme.Url, memeFileName);
							var randomNumber = new Random().Next(1, 4);
							Thread.Sleep(randomNumber * 10000);
							task.Status = TaskStatus.Completed;
						}
					}
				}
				else
				{
					task.Status = TaskStatus.Failed;
				}

			}
			catch (Exception)
			{
				task.Status = TaskStatus.Failed;
			}
		}
	}
}

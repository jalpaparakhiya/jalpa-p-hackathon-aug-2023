namespace Worker;

public class WorkerRegister
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<WorkerRegister> _logger;
    private readonly string _allocatorUri;

    public WorkerRegister(IConfiguration configuration, ILogger<WorkerRegister> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _allocatorUri = configuration.GetValue<string>("AllocatorUri");
    }

    public WorkerInfo GetWorkerInfo()
    {
        return new WorkerInfo(
            _configuration.GetValue<string>("name"),
            _configuration.GetValue<int>("port")
        );
    }

    public async Task RegisterWorkerAsync()
    {
        var worker = GetWorkerInfo();
        Console.WriteLine($"{_allocatorUri}/api/nodes/register");
        var client = new HttpClient();
        var response = await client.PostAsJsonAsync($"{_allocatorUri}/api/nodes/register", new
        {
            worker.Name,
            Address = $"http://localhost:{worker.Port}",
            MemesPath = worker.WorkDir
        });
        
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Worker registered with name: {WorkerName}", worker.Name);
    }

    public async Task UnRegisterWorkerAsync()
    {
		var worker = GetWorkerInfo();
		Console.WriteLine($"{_allocatorUri}/api/nodes/unregister/{worker.Name}");
		
		var client = new HttpClient();
		var response = await client.DeleteAsync($"{_allocatorUri}/api/nodes/unregister/{worker.Name}");

		response.EnsureSuccessStatusCode();

		_logger.LogInformation("Worker registered with name: {WorkerName}", worker.Name);
	}
}
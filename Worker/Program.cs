using Worker;

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("Worker Start!");


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<WorkerRegister>();
builder.Services.AddSingleton<WorkerInfo>(provider => provider.GetRequiredService<WorkerRegister>().GetWorkerInfo());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var provider = app.Services;
var worker = provider.GetRequiredService<WorkerInfo>();

app.Urls.Add($"http://0.0.0.0:{worker.Port}");

var registerNode = provider.GetRequiredService<WorkerRegister>();
await registerNode.RegisterWorkerAsync();

Console.WriteLine($"Worker started, listening on port: {worker.Port}. Memes will be saved at: {worker.WorkDir}");

provider.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping.Register(async () =>
{
	Console.WriteLine("Worker stopping...");
	await registerNode.UnRegisterWorkerAsync();
});
app.Run();





using FunctionApp.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;




string connectionString = Environment.GetEnvironmentVariable("AzureSqlDatabase");

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
    })
    .Build();



host.Run();
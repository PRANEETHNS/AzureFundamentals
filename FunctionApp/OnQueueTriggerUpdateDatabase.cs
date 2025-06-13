using System;
using Azure.Storage.Queues.Models;
using FunctionApp.Data;
using FunctionApp.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp;

public class OnQueueTriggerUpdateDatabase
{
    private readonly ILogger<OnQueueTriggerUpdateDatabase> _logger;

    private readonly ApplicationDbContext _dbContext;

    public OnQueueTriggerUpdateDatabase(ILogger<OnQueueTriggerUpdateDatabase> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }


    //The connection can be left blank and azure will know to take the default name "AzureWebJobsStorage"
    [Function(nameof(OnQueueTriggerUpdateDatabase))]
    public void Run([QueueTrigger("SalesRequestInBound", Connection = "AzureWebJobsStorage")] QueueMessage message)
    {
        string messageBody = message.Body.ToString();

        SalesRequest salesRequest = JsonConvert.DeserializeObject<SalesRequest>(messageBody);
        salesRequest.Status = "";
        _dbContext.SalesRequests.Add(salesRequest);
        _dbContext.SaveChanges();

        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);
    }
}
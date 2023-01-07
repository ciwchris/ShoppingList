using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Api.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ShoppingListApp.Shared;

namespace ShoppingListApp;

public class AddItem
{
    private readonly ILogger logger;
    private readonly ShoppingListsService shoppingListsService;
    private readonly JsonSerializerOptions serializeOptions;

    public AddItem(ILoggerFactory loggerFactory, ShoppingListsService shoppingListsService)
    {
        logger = loggerFactory.CreateLogger<AddShoppingList>();
        this.shoppingListsService = shoppingListsService;
        serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    [Function(nameof(AddItem))]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = nameof(AddItem) + "/{listId}")] HttpRequestData req,
        Guid listId)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var shoppingListItem = JsonSerializer.Deserialize<ShoppingListItem>(requestBody, serializeOptions);
        logger.LogInformation("Add item {ItemId} to list {ListId}", shoppingListItem.Id, listId);

        var result = shoppingListsService.AddItem(listId, shoppingListItem);

        var response = req.CreateResponse(HttpStatusCode.OK);
        _=response.WriteAsJsonAsync(result);
        return response;
    }
}

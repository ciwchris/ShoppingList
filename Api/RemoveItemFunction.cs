using System;
using System.Net;
using Api.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ShoppingListApp;

public class RemoveItem
{
    private readonly ILogger logger;
    private readonly ShoppingListsService shoppingListsService;

    public RemoveItem(ILoggerFactory loggerFactory, ShoppingListsService shoppingListsService)
    {
        logger = loggerFactory.CreateLogger<RemoveShoppingList>();
        this.shoppingListsService = shoppingListsService;
    }

    [Function(nameof(RemoveItem))]
    public HttpResponseData Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = nameof(RemoveItem) + "/{listId}/{itemId}")] HttpRequestData req,
        Guid listId,
        Guid itemId)
    {
        logger.LogInformation("Removing item {ItemId} from list {ListId}", itemId, listId);

        var result = shoppingListsService.RemoveItem(listId, itemId);

        var response = req.CreateResponse(HttpStatusCode.OK);
        _=response.WriteAsJsonAsync(result);
        return response;
    }
}

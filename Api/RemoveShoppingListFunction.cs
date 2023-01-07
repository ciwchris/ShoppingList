using System;
using System.Net;
using Api.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ShoppingListApp;

public class RemoveShoppingList
{
    private readonly ILogger logger;
    private readonly ShoppingListsService shoppingListsService;

    public RemoveShoppingList(ILoggerFactory loggerFactory, ShoppingListsService shoppingListsService)
    {
        logger = loggerFactory.CreateLogger<RemoveShoppingList>();
        this.shoppingListsService = shoppingListsService;
    }

    [Function(nameof(RemoveShoppingList))]
    public HttpResponseData Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = nameof(RemoveShoppingList) + "/{id}")] HttpRequestData req,
        Guid id)
    {
        logger.LogInformation("Removing list {ListId}", id);

        var result = shoppingListsService.RemoveShoppingList(id);

        var response = req.CreateResponse(HttpStatusCode.OK);
        _=response.WriteAsJsonAsync(result);
        return response;
    }
}

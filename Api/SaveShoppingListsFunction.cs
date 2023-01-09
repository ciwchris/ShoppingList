using System.Net;
using System.Threading.Tasks;
using Api.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ShoppingListApp;

public class SaveShoppingLists
{
    private readonly ILogger logger;
    private readonly ShoppingListsService shoppingListsService;

    public SaveShoppingLists(ILoggerFactory loggerFactory, ShoppingListsService shoppingListsService)
    {
        logger = loggerFactory.CreateLogger<SaveShoppingLists>();
        this.shoppingListsService = shoppingListsService;
    }

    [Function(nameof(SaveShoppingLists))]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        logger.LogInformation("Saving lists");

        await shoppingListsService.SaveShoppingLists();

        return req.CreateResponse(HttpStatusCode.OK);
    }
}

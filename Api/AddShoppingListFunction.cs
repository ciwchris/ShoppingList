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

public class AddShoppingList
{
    private readonly ILogger logger;
    private readonly ShoppingListsService shoppingListsService;
    private readonly JsonSerializerOptions serializeOptions;

    public AddShoppingList(ILoggerFactory loggerFactory, ShoppingListsService shoppingListsService)
    {
        logger = loggerFactory.CreateLogger<AddShoppingList>();
        this.shoppingListsService = shoppingListsService;
        serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    [Function(nameof(AddShoppingList))]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
    {
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var shoppingList = JsonSerializer.Deserialize<ShoppingList>(requestBody, serializeOptions);
        logger.LogInformation("Add list {ListId}", shoppingList.Id);

        var result = shoppingListsService.AddShoppingList(shoppingList);

        var response = req.CreateResponse(HttpStatusCode.OK);
        _=response.WriteAsJsonAsync(result);
        return response;
    }
}

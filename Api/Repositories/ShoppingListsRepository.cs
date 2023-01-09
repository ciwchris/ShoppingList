using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using ShoppingListApp.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Repositories;

public class ShoppingListsRepository
{
    private readonly string connectionString;
    private List<ShoppingList> shoppingLists = new();

    public ShoppingListsRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetValue<string>("BlobConnectionString");
    }

    public List<ShoppingList> Get() => shoppingLists;

    public List<ShoppingList> Remove(Guid listId)
    {
        var listToRemove = shoppingLists.Single(list => list.Id == listId);
        shoppingLists.Remove(listToRemove);
        return shoppingLists;
    }

    public List<ShoppingList> Add(ShoppingList shoppingList)
    {
        shoppingLists.Add(shoppingList);
        return shoppingLists;
    }

    public List<ShoppingListItem> AddItem(Guid listId, ShoppingListItem itemToAdd)
    {
        var listToAddItemTo = shoppingLists.Single(list => list.Id == listId);
        listToAddItemTo.ShoppingListItems.Add(itemToAdd);
        return listToAddItemTo.ShoppingListItems;
    }

    public List<ShoppingListItem> RemoveItem(Guid listId, Guid itemId)
    {
        var listToRemoveItemFrom = shoppingLists.Single(list => list.Id == listId);
        var itemToRemove = listToRemoveItemFrom.ShoppingListItems
            .Single(item => item.Id == itemId);
        listToRemoveItemFrom.ShoppingListItems.Remove(itemToRemove);
        return listToRemoveItemFrom.ShoppingListItems;
    }

    public async Task<List<ShoppingList>> SaveToStorage()
    {
        var blobClient = new BlobClient(connectionString, "shopping-lists", "shopping-list");

        var json = System.Text.Json.JsonSerializer.Serialize(shoppingLists);
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
        {
            await blobClient.UploadAsync(ms, overwrite: true);
        }

        return shoppingLists;
    }

    public async Task<List<ShoppingList>> RetrieveFromStorage()
    {
        var blobClient = new BlobClient(connectionString, "shopping-lists", "shopping-list");

        if (await blobClient.ExistsAsync())
        {
            var blob = await blobClient.DownloadContentAsync();
            var json = blob.Value.Content.ToString();
            shoppingLists = System.Text.Json.JsonSerializer.Deserialize<List<ShoppingList>>(json);
        }

        return shoppingLists;
    }
}

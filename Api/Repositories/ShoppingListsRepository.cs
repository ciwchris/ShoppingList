using ShoppingListApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Repositories;

public class ShoppingListsRepository
{
    private readonly List<ShoppingList> shoppingLists;

    public ShoppingListsRepository()
    {
        shoppingLists = new()
        {
            { new ShoppingList { Id = Guid.NewGuid(), Name = "Grocery" } },
            { new ShoppingList { Id = Guid.NewGuid(), Name = "Wholesale" } }
        };
    }

    public List<ShoppingList> Get() => shoppingLists;

    public List<ShoppingList> Remove(Guid listId)
    {
        var listToRemove = shoppingLists.Single(list => list.Id == listId);
        shoppingLists.Remove(listToRemove);
        return shoppingLists;
    }

    internal List<ShoppingList> Add(ShoppingList shoppingList)
    {
        shoppingLists.Add(shoppingList);
        return shoppingLists;
    }

    internal List<ShoppingListItem> AddItem(Guid listId, ShoppingListItem itemToAdd)
    {
        var listToAddItemTo = shoppingLists.Single(list => list.Id == listId);
        listToAddItemTo.ShoppingListItems.Add(itemToAdd);
        return listToAddItemTo.ShoppingListItems;
    }

    internal List<ShoppingListItem> RemoveItem(Guid listId, Guid itemId)
    {
        var listToRemoveItemFrom = shoppingLists.Single(list => list.Id == listId);
        var itemToRemove = listToRemoveItemFrom.ShoppingListItems
            .Single(item => item.Id == itemId);
        listToRemoveItemFrom.ShoppingListItems.Remove(itemToRemove);
        return listToRemoveItemFrom.ShoppingListItems;
    }
}

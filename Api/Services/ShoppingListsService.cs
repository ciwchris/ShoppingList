using Api.Repositories;
using ShoppingListApp.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services
{
    public class ShoppingListsService
    {
        private readonly ShoppingListsRepository shoppingListsRepository;

        public ShoppingListsService(ShoppingListsRepository shoppingListsRepository) =>
            this.shoppingListsRepository = shoppingListsRepository;

        public async Task<List<ShoppingList>> GetShoppingLists()
        {
            var shoppingLists = shoppingListsRepository.Get();
            if (shoppingLists.Count == 0) shoppingLists = await shoppingListsRepository.RetrieveFromStorage();
            return shoppingLists;
        }

        public List<ShoppingList> RemoveShoppingList(Guid listId) =>
            shoppingListsRepository.Remove(listId);

        public List<ShoppingList> AddShoppingList(ShoppingList shoppingList) =>
            shoppingListsRepository.Add(shoppingList);

        public List<ShoppingListItem> AddItem(Guid listId, ShoppingListItem itemToAdd) =>
            shoppingListsRepository.AddItem(listId, itemToAdd);

        public List<ShoppingListItem> RemoveItem(Guid listId, Guid itemId) =>
            shoppingListsRepository.RemoveItem(listId, itemId);

        internal async Task<List<ShoppingList>> SaveShoppingLists() =>
            await shoppingListsRepository.SaveToStorage();
    }
}

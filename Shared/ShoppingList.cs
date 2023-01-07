using System;
using System.Collections.Generic;

namespace ShoppingListApp.Shared
{
    public class ShoppingList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ShoppingListItem> ShoppingListItems { get; set; } = new List<ShoppingListItem>();
    }
}

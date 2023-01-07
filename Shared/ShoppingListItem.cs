using System;

namespace ShoppingListApp.Shared
{
    public class ShoppingListItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
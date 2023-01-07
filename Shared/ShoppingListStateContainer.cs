namespace ShoppingListApp.Shared
{
    using System;

    public class ShoppingListStateContainer
    {
        public ShoppingList Value { get; set; }

        public event Action OnStateChange;

        public void SetValue(ShoppingList value)
        {
            Value = value;
            NotifyStateChange();
        }

        private void NotifyStateChange() => OnStateChange?.Invoke();
    }
}

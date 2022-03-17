using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PathOfExileShoppingTool.Helpers;
using PathOfExileShoppingTool.Modal;

namespace PathOfExileShoppingTool.Pages
{
    public class ShopListComponent : ComponentBase
    {
        [Inject]
        public ISyncLocalStorageService LocalStorageService { get; set; }

        [Inject]
        public SaveLocal SaveLocal { get; set; }

        public List<ShopListItem> Items { get; set; } = new List<ShopListItem>();

        protected string ItemTitle;
        protected ItemCost ItemCost;
        protected int EstimatedCost;
        protected Importancy Importancy;
        protected string Description;
        protected string TradeUrl;
        

        protected void CreateShoppingItem()
        {
            var shopItem = new ShopListItem
            {
                Title = ItemTitle,
                ItemCost = ItemCost,
                EstimatedCost = EstimatedCost,
                Importancy = Importancy,
                Description = Description,
                TradeLink = new Uri(TradeUrl),
                ItemId = Guid.NewGuid()
            };
            Items.Add(shopItem);
            SaveItemArray(shopItem);
        }

        private void SaveItemArray(ShopListItem shopItem)
        {
            if (shopItem != null)
            {
                SaveLocal.SaveLocalArray(Items, "Items", LocalStorageService, shopItem);
            }
        }
    }
}

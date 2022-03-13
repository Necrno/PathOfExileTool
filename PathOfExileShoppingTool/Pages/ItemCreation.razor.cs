using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PathOfExileShoppingTool.Modal;

namespace PathOfExileShoppingTool.Pages
{
    public class ShopListComponent : ComponentBase
    {
        [Inject]
        public ISyncLocalStorageService LocalStorageService { get; set; }

        public List<ShopListItem> Items { get; set; } = new List<ShopListItem>();

        protected string ItemTitle;
        protected ItemCost ItemCost;
        protected int EstimatedCost;
        protected Importancy Importancy;
        protected string Description;
        

        protected void CreateShoppingItem()
        {
            var shopItem = new ShopListItem
            {
                Title = ItemTitle,
                ItemCost = ItemCost,
                EstimatedCost = EstimatedCost,
                Importancy = Importancy,
                Description = Description,
                ItemId = Guid.NewGuid()
            };
            Items.Add(shopItem);
            SaveItemArray(shopItem);
        }

        private void SaveItemArray(ShopListItem shopItem)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };


            if (shopItem != null)
            {
                var currArr = LocalStorageService.GetItemAsString("Items");
                if(currArr != null)
                {
                    var des = JsonSerializer.Deserialize<List<ShopListItem>>(currArr);
                    des.Add(shopItem);
                    var serializedArr = JsonSerializer.Serialize(des, options);
                    LocalStorageService.SetItemAsString("Items", serializedArr);
                }
                else
                {                   
                    var serialzedItem = JsonSerializer.Serialize(Items);
                    LocalStorageService.SetItemAsString("Items", serialzedItem);
                }
            }
        }
    }
}

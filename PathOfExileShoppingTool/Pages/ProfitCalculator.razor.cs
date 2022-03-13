using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PathOfExileShoppingTool.Modal;

namespace PathOfExileShoppingTool.Pages
{
    public class ProfitCalculatorComponent : ComponentBase
    {
        [Inject]
        public ISyncLocalStorageService LocalStorageService { get; set; }

        public List<ProfitItem> ProfitItems { get; set; } = new List<ProfitItem>();

        protected string BoughtItemTitle;
        protected int BoughtItemPrice;
        protected int QuantityBoughtItem;
        protected ItemCost ItemCost;

        protected void AddToBoughtItems()
        {
            var profitItem = new ProfitItem
            {
                ItemID = Guid.NewGuid(),
                ItemTitle = BoughtItemTitle,
                ItemPrice = BoughtItemPrice,
                ItemCost = ItemCost,
                ItemQuantity = QuantityBoughtItem
            };

            if (profitItem.ItemTitle != String.Empty && profitItem.ItemPrice != 0)
            {
                if (profitItem.ItemQuantity == 0)
                {
                    profitItem.ItemPrice = 1;
                }

                ProfitItems.Add(profitItem);

                // Get array from localstorage
                var currentItem = LocalStorageService.GetItemAsString("ProfitItems");
                if (currentItem != null)
                {
                    var currentDeserializedArray = JsonSerializer.Deserialize<List<ProfitItem>>(currentItem);
                    if (currentDeserializedArray != null)
                    {
                        currentDeserializedArray.Add(profitItem);
                        var serializerArray = JsonSerializer.Serialize(currentDeserializedArray);
                        LocalStorageService.SetItemAsString("ProfitItems", serializerArray);
                    }
                }
                else
                {
                    var newProfitItemsArray = JsonSerializer.Serialize(ProfitItems);
                    LocalStorageService.SetItemAsString("ProfitItems", newProfitItemsArray);
                }
            }
        }
    }
}

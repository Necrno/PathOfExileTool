using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PathOfExileShoppingTool.Modal;

namespace PathOfExileShoppingTool.Pages
{
    public class ItemsOverviewComponent : ComponentBase
    {
        [Inject]
        public ISyncLocalStorageService LocalStorageService { get; set; }

        public List<ShopListItem> Items { get; set; } = new List<ShopListItem>();

        protected bool Loading;
        protected int CostOfTotalItemsChaos = 0;
        protected int CostOfTotalItemsExalts = 0;
        protected int CostOfTotalItemsMirrors = 0;

        protected override void OnInitialized()
        {
            var items = LocalStorageService.GetItemAsString("Items");
            if (items != null)
            {
                var x = JsonSerializer.Deserialize<List<ShopListItem>>(items);
                if (x != null)
                {
                    Items = x;
                }
            }
        }

        protected void DeleteItem(Guid id)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            var item = Items.Find(x => x.ItemId == id);
            if (item != null)
            {
                Items.Remove(item);
            }
            var serArr = JsonSerializer.Serialize(Items);
            LocalStorageService.SetItemAsString("Items", serArr);

        }

        protected void TotalCost()
        {
            foreach(var item in Items)
            {
                if(item.ItemCost == ItemCost.Chaos)
                {
                    CostOfTotalItemsChaos += item.EstimatedCost;
                }
                else if(item.ItemCost == ItemCost.Exatls)
                {
                    CostOfTotalItemsExalts += item.EstimatedCost;
                }
                else
                {
                    CostOfTotalItemsMirrors += item.EstimatedCost;
                }
            }
        }
    }
}

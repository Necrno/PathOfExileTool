using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using PathOfExileShoppingTool.Helpers;
using PathOfExileShoppingTool.Modal;

namespace PathOfExileShoppingTool.Pages
{
    public class ProfitCalculatorComponent : ComponentBase
    {
        [Inject]
        public ISyncLocalStorageService LocalStorageService { get; set; }

        [Inject]
        public SaveLocal SaveLocal { get; set; }

        public List<ProfitItem> ProfitItems { get; set; } = new List<ProfitItem>();

        public List<CurrencyRow> CurrencyRows { get; set; } = new List<CurrencyRow>();

        protected int totalPriceChaos;
        protected int totalPriceExalts;
        protected int totalPriceMirros;

        // Variables below are used for binding.
        protected string BoughtItemTitle;
        protected int BoughtItemPrice;
        protected int QuantityBoughtItem;
        protected ItemCost ItemCost;

        protected int madeChaos;
        protected int madeExalts;
        protected int madeMirrors;

        protected override void OnInitialized()
        {
            var profitItemsArray = LocalStorageService.GetItemAsString("ProfitItems");
            var currencyRowsArray = LocalStorageService.GetItemAsString("CurrencyRows");

            if (profitItemsArray != null)
            {
                var profList = JsonSerializer.Deserialize<List<ProfitItem>>(profitItemsArray);
                if (profList != null)
                {
                    ProfitItems = profList;
                }
            }
            if (currencyRowsArray != null)
            {
                var curList = JsonSerializer.Deserialize<List<CurrencyRow>>(currencyRowsArray);
                if (curList != null)
                {
                    CurrencyRows = curList;
                }
            }
        }

        protected void DeleteTableRow(Guid id)
        {
            var itemToDelete = ProfitItems.Find(x => x.ItemID == id);
            var curRowToDelete = CurrencyRows.Find(x => x.RowID == id);
            if (itemToDelete != null)
            {
                ProfitItems.Remove(itemToDelete);
                if (itemToDelete.ItemCost == ItemCost.Chaos)
                {
                    totalPriceChaos -= itemToDelete.ItemPrice * itemToDelete.ItemQuantity;
                }
                else if (itemToDelete.ItemCost == ItemCost.Exatls)
                {
                    totalPriceExalts -= itemToDelete.ItemPrice * itemToDelete.ItemQuantity;
                }
                else
                {
                    totalPriceMirros -= itemToDelete.ItemPrice * itemToDelete.ItemQuantity;
                }
                var newArr = JsonSerializer.Serialize(ProfitItems);
                LocalStorageService.SetItemAsString("ProfitItems", newArr);

            }

            if (curRowToDelete != null)
            {
                CurrencyRows.Remove(curRowToDelete);
                var newCurrencyArr = JsonSerializer.Serialize(CurrencyRows);
                LocalStorageService.SetItemAsString("CurrencyRows", newCurrencyArr);
            }
        }

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

                if (profitItem.ItemCost == ItemCost.Chaos)
                {
                    totalPriceChaos += profitItem.ItemPrice * profitItem.ItemQuantity;
                }
                else if (profitItem.ItemCost == ItemCost.Exatls)
                {
                    totalPriceExalts += profitItem.ItemPrice * profitItem.ItemQuantity;
                }
                else
                {
                    totalPriceMirros += profitItem.ItemPrice * profitItem.ItemQuantity;
                }

                ProfitItems.Add(profitItem);
                SaveLocal.SaveLocalArray(ProfitItems, "ProfitItems", LocalStorageService, profitItem);
            }
        }

        protected void AddMadeCurrency()
        {
            var currencyRow = new CurrencyRow
            {
                RowID = Guid.NewGuid(),
                Chaos = madeChaos,
                Exalts = madeExalts,
                Mirrors = madeMirrors,
            };

            CurrencyRows.Add(currencyRow);
            SaveLocal.SaveLocalArray(CurrencyRows, "CurrencyRows", LocalStorageService, currencyRow);
        }
    }
}

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

        public List<CurrencyRow> CurrencyRows { get; set; } = new List<CurrencyRow>();

        protected string BoughtItemTitle;
        protected int BoughtItemPrice;
        protected int QuantityBoughtItem;
        protected ItemCost ItemCost;

        protected int totalPriceChaos;
        protected int totalPriceExalts;
        protected int totalPriceMirros;

        protected int madeChaos;
        protected int madeExalts;
        protected int madeMirrors;

        protected override void OnInitialized()
        {
            var profitItemsArray = LocalStorageService.GetItemAsString("ProfitItems");
            var currencyRowsArray = LocalStorageService.GetItemAsString("CurrencyRows");
            if (profitItemsArray != null && currencyRowsArray != null)
            {
                ProfitItems = JsonSerializer.Deserialize<List<ProfitItem>>(profitItemsArray);
                CurrencyRows = JsonSerializer.Deserialize<List<CurrencyRow>>(currencyRowsArray);
            }
            CalculateCost(ProfitItems);
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
                else if(itemToDelete.ItemCost == ItemCost.Exatls)
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

            if(curRowToDelete != null)
            {
                CurrencyRows.Remove(curRowToDelete);
                var newCurrencyArr = JsonSerializer.Serialize(CurrencyRows);
                LocalStorageService.SetItemAsString("CurrencyRows", newCurrencyArr);
            }
            CalculateCost(ProfitItems);
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
            CalculateCost(ProfitItems);
        }

        protected void CalculateCost(List<ProfitItem> list)
        {
            foreach (var item in list)
            {
                switch (item.ItemCost)
                {
                    case ItemCost.Chaos:
                        totalPriceChaos += item.ItemPrice * item.ItemQuantity;
                        break;
                    case ItemCost.Exatls:
                        totalPriceExalts += item.ItemPrice * item.ItemQuantity;
                        break;
                    case ItemCost.Mirrors:
                        totalPriceMirros += item.ItemPrice * item.ItemQuantity;
                        break;
                }
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
            var currentCurrencyRows = LocalStorageService.GetItemAsString("CurrencyRows");
            if(currentCurrencyRows != null)
            {
                var desCurArr = JsonSerializer.Deserialize<List<CurrencyRow>>(currentCurrencyRows);
                desCurArr.Add(currencyRow);
                var serCurArr = JsonSerializer.Serialize(desCurArr);
                LocalStorageService.SetItemAsString("CurrencyRows", serCurArr);
            }
            else
            {
                var newCurArr = JsonSerializer.Serialize(CurrencyRows);
                LocalStorageService.SetItemAsString("CurrencyRows", newCurArr);
            }
        }
    }
}

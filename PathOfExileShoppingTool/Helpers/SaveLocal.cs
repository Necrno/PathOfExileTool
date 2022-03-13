using System.Text.Json;
using Blazored.LocalStorage;

namespace PathOfExileShoppingTool.Helpers
{
    public class SaveLocal
    {
        public void SaveLocalArray<T>(List<T> list, string key, ISyncLocalStorageService localStorageService, T item)
        {
            var currArr = localStorageService.GetItemAsString(key);
            if (currArr != null)
            {
                var curDesArr = JsonSerializer.Deserialize<List<T>>(currArr);
                if (curDesArr != null)
                {
                    curDesArr.Add(item);
                    var serArr = JsonSerializer.Serialize(curDesArr);
                    localStorageService.SetItemAsString(key, serArr);
                }
            }
            else
            {
                var newArr = JsonSerializer.Serialize(list);
                localStorageService.SetItemAsString(key, newArr);
            }
        }
    }
}

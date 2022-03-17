namespace PathOfExileShoppingTool.Modal
{
    public class ShopListItem
    {
        public Guid ItemId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int EstimatedCost { get; set; }
        public Uri TradeLink { get; set; }
        public ItemCost ItemCost { get; set; }
        public Importancy Importancy { get; set; }
    }

    public enum Importancy
    {
        High,
        Medium,
        Low
    }

    public enum ItemCost
    {
        Exatls,
        Chaos,
        Mirrors
    }
}

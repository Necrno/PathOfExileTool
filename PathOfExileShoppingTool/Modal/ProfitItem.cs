namespace PathOfExileShoppingTool.Modal
{
    public class ProfitItem
    {
        public Guid ItemID { get; set; }
        public string ItemTitle{ get; set; }
        public int ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
        public ItemCost ItemCost { get; set; }
    }
}

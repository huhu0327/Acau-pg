namespace Acau_Playground.Models
{
    public class Food
    {
        public string Type { get; set; }
        public IEnumerable<Content> Contents { get; set; }
    }

    public class Content
    {
        public string Name { get; set; }
        public int ShopPrice { get; set; }
        public int PurchasePrice { get; set; }
        public int SellPrice { get; set; }
    }

}

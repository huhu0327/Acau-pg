namespace Acau_Playground.Models
{
    public record class Food(string Type, IEnumerable<Content> Contents);
    public record class Content(string Name, int ShopPrice, int PurchasePrice);
}

namespace Acau_Playground.Models
{
    public class Job
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<Food> Foods { get; set; } = Enumerable.Empty<Food>();
    }

    public class Food
    {
        public string Name { get; set; } = string.Empty;
        public int ShopPrice { get; set; }
        public int PurchasePrice { get; set; }
        public int Box { get; set; }
        public int Set { get; set; }
        public int Num { get; set; }
        public int Sum => (Num + (Set * 64) + (Box * 64 * 27)) * (ShopPrice + PurchasePrice);

        public void Init()
        {
            Box = 0; Set = 0; Num = 0;
        }
    }
}

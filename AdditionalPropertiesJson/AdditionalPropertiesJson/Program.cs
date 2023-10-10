namespace AdditionalPropertiesJson
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var response = new InheritsOrdetItem
            {
                InheritsOrdetItems = new Dictionary<string, ItemDetail>
                {
                    { "SampleKey01", new ItemDetail { ItemName = "りんご", ItemPrice = 250 } },
                    { "SampleKey02", new ItemDetail { ItemName = "バナナ", ItemPrice = 150 } },
                    { "SampleKey03", new ItemDetail { ItemName = "オレンジ", ItemPrice = 200 } }
                }
            };

            // 値が正しくセットされているか確認するための出力
            foreach (var item in response.InheritsOrdetItems)
            {
                Console.WriteLine($"{item.Key} -> Name: {item.Value.ItemName}, Price: {item.Value.ItemPrice}");
            }
        }
    }

    public class InheritsOrdetItem
    {
        public required Dictionary<string, ItemDetail> InheritsOrdetItems { get; set; }
    }

    public class ItemDetail
    {
        public required string ItemName { get; set; }
        public int ItemPrice { get; set; }
    }
}
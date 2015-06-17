namespace UIC
{
    public interface IInventoryItem
    {
        string id { get; set; }
        string Name { set; }
        string Type { set; }
        int Count { set; }
        int Price { set; }
    }
}

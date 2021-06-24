namespace DCx.Webshop.Models
{
    public class Quantity
    {
        public int Value { get; set; }
        public string Name { get; set; }
        public Quantity(string name, int value)
        {
            Name = name;
            Value = value;
        }
        public Quantity() { }
    }
}

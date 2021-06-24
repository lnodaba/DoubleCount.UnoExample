namespace DCx.Webshop.Models
{
    public class Hosting
    {
        public static Hosting Desktop = new Hosting("Desktop");
        public static Hosting Lan = new Hosting("Lan");
        public static Hosting TSOpen = new Hosting("TS Open");
        public static Hosting TSMax3 = new Hosting("TS Max 3");
        public static Hosting RentAsp = new Hosting("Rent Asp");
        public static Hosting RentSingle = new Hosting("Rent Single");
        public string Name { get; set; }
        public Hosting(string name)
        {
            Name = name;
        }
        public Hosting() { }
    }
}

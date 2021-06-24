namespace DCx.Webshop.Models
{
    public class LicenseOption
    {
        public static LicenseOption License = new LicenseOption("License CHF", false);
        public static LicenseOption Update = new LicenseOption("Update CHF", true);
        public static LicenseOption RentMonth = new LicenseOption("Rent Month CHF", true);
        public static LicenseOption RentYear = new LicenseOption("Rent Year CHF", true);

        public string Name { get; set; }
        public bool IsProrata { get; set; }
        public LicenseOption(string name, bool isprorata)
        {
            Name = name;
            IsProrata = isprorata;
        }
        public LicenseOption() { }
    }


}

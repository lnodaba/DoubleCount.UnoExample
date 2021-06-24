using DCx.Webshop.Models;
using System.Collections.Generic;
using System.Linq;

namespace DCx.Webshop.Products
{
    public class ProductRepo : IProductRepo
    {
        public List<ProductItem> GetProducts() => new List<ProductItem>()
        {
            new ProductItem(1, "Payroll(Lohn)")
            {
                Quantities = new List<Quantity>()
                {
                    new Quantity("up to 5 employees",5),
                    new Quantity("up to 10 employees",10),
                    new Quantity("up to 25 employees",25),
                    new Quantity("up to 50 employees",50),
                    new Quantity("up to 100 employees",100),
                    new Quantity("up to 250 employees",250),
                    new Quantity("up to 500 employees",500),
                    new Quantity("up to 999 employees",999)
                },
                Hostings = new List<Hosting>()
                {
                    Hosting.Desktop,
                    Hosting.Lan,
                    Hosting.TSOpen,
                    Hosting.TSMax3,
                    Hosting.RentAsp,
                    Hosting.RentSingle
                }
            },
            new ProductItem(2, "Tax(Steuern)")
            {
                Quantities = new List<Quantity>()
                {
                    new Quantity("up to 25 Ste",25),
                    new Quantity("up to 50 Ste",50),
                    new Quantity("up to 100 Ste",100),
                    new Quantity("up to 250 Ste",250),
                    new Quantity("up to 500 Ste",500),
                    new Quantity("up to 999 Ste",999)
                },
                Hostings = new List<Hosting>()
                {
                    Hosting.Desktop,
                    Hosting.Lan,
                    Hosting.TSOpen,
                }
            }
        };
    }
}

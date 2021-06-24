using DCx.Webshop.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DCx.Webshop.Models
{
    public class ProductItem
    {
        public ProductItem() { }

        public ProductItem(int id, string name)
        {
            ID      = id;
            Name    = name;
        }

        public int              ID          { get; set; }
        public string           Name        { get; set; }
        
        public Quantity         Quantity    = new Quantity();
        public List<Quantity>   Quantities { get; set; } = new List<Quantity>();
        
        public Hosting          Hosting     = new Hosting("");
        public List<Hosting>    Hostings    { get; set; } = new List<Hosting>();

        public List<Price>      Prices   => PriceRepo.Instance.Calculate(ID, Hosting, Quantity.Value);




    }
}

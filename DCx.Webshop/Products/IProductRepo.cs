using System;
using System.Collections.Generic;
using DCx.Webshop.Models;

namespace DCx.Webshop.Products
{
    public interface IProductRepo
    {
        List<ProductItem>   GetProducts();
    }
}

using DCx.Webshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.Webshop.Products
{
    public sealed class PriceRepo
    {
        private PriceRepo()
        {
        }

        private static readonly Lazy<PriceRepo> lazy = new Lazy<PriceRepo>(() => new PriceRepo());

        public static PriceRepo Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private static List<Price> _prices = new List<Price>()
        {
            //Tax-Buy Desktop-RentYear
            new Price(2,Hosting.Desktop,LicenseOption.RentYear,25,290),
            new Price(2,Hosting.Desktop,LicenseOption.RentYear,50,490),
            new Price(2,Hosting.Desktop,LicenseOption.RentYear,100,690),
            new Price(2,Hosting.Desktop,LicenseOption.RentYear,250,1090),
            new Price(2,Hosting.Desktop,LicenseOption.RentYear,500,1090-1),
            new Price(2,Hosting.Desktop,LicenseOption.RentYear,999,1590-2),
            //Tax-Buy Lan-RentYear
            new Price(2,Hosting.Lan,LicenseOption.RentYear,25,490),
            new Price(2,Hosting.Lan,LicenseOption.RentYear,50,790),
            new Price(2,Hosting.Lan,LicenseOption.RentYear,100,990),
            new Price(2,Hosting.Lan,LicenseOption.RentYear,250,1390),
            new Price(2,Hosting.Lan,LicenseOption.RentYear,500,1990-1),
            new Price(2,Hosting.Lan,LicenseOption.RentYear,999,1990-2),
            //Tax-TS Open-RentYear
            new Price(2,Hosting.TSOpen,LicenseOption.RentYear,25,490),
            new Price(2,Hosting.TSOpen,LicenseOption.RentYear,50,790),
            new Price(2,Hosting.TSOpen,LicenseOption.RentYear,100,990),
            new Price(2,Hosting.TSOpen,LicenseOption.RentYear,250,1390),
            new Price(2,Hosting.TSOpen,LicenseOption.RentYear,500,1990-1),
            new Price(2,Hosting.TSOpen,LicenseOption.RentYear,999,1990-2),
            //Lohn-Buy Desktop-License
            new Price(1,Hosting.Desktop,LicenseOption.License,5,550),
            new Price(1,Hosting.Desktop,LicenseOption.License,10,950),
            new Price(1,Hosting.Desktop,LicenseOption.License,25,1350),
            new Price(1,Hosting.Desktop,LicenseOption.License,50,2150),
            new Price(1,Hosting.Desktop,LicenseOption.License,100,2950),
            new Price(1,Hosting.Desktop,LicenseOption.License,250,3850),
            new Price(1,Hosting.Desktop,LicenseOption.License,500,4850),
            new Price(1,Hosting.Desktop,LicenseOption.License,999,5650),
            //Lohn-Buy Desktop-Update
            new Price(1,Hosting.Desktop,LicenseOption.Update,5,240),
            new Price(1,Hosting.Desktop,LicenseOption.Update,10,420),
            new Price(1,Hosting.Desktop,LicenseOption.Update,25,540),
            new Price(1,Hosting.Desktop,LicenseOption.Update,50,780),
            new Price(1,Hosting.Desktop,LicenseOption.Update,100,1140),
            new Price(1,Hosting.Desktop,LicenseOption.Update,250,1380),
            new Price(1,Hosting.Desktop,LicenseOption.Update,500,1740),
            new Price(1,Hosting.Desktop,LicenseOption.Update,999,2100),
            //Lohn-Buy Lan-License
            new Price(1,Hosting.Lan,LicenseOption.License,5,1050),
            new Price(1,Hosting.Lan,LicenseOption.License,10,1650),
            new Price(1,Hosting.Lan,LicenseOption.License,25,2450),
            new Price(1,Hosting.Lan,LicenseOption.License,50,3350),
            new Price(1,Hosting.Lan,LicenseOption.License,100,4350),
            new Price(1,Hosting.Lan,LicenseOption.License,250,5350),
            new Price(1,Hosting.Lan,LicenseOption.License,500,6450),
            new Price(1,Hosting.Lan,LicenseOption.License,999,7350),
            //Lohn-Buy Lan-Update
            new Price(1,Hosting.Lan,LicenseOption.Update,5,480),
            new Price(1,Hosting.Lan,LicenseOption.Update,10,780),
            new Price(1,Hosting.Lan,LicenseOption.Update,25,1020),
            new Price(1,Hosting.Lan,LicenseOption.Update,50,1380),
            new Price(1,Hosting.Lan,LicenseOption.Update,100,1860),
            new Price(1,Hosting.Lan,LicenseOption.Update,250,2220),
            new Price(1,Hosting.Lan,LicenseOption.Update,500,2700),
            new Price(1,Hosting.Lan,LicenseOption.Update,999,2940),
            //Lohn-Buy TS Open-License
            new Price(1,Hosting.TSOpen,LicenseOption.License,5,1050),
            new Price(1,Hosting.TSOpen,LicenseOption.License,10,1650),
            new Price(1,Hosting.TSOpen,LicenseOption.License,25,2450),
            new Price(1,Hosting.TSOpen,LicenseOption.License,50,3350),
            new Price(1,Hosting.TSOpen,LicenseOption.License,100,4350),
            new Price(1,Hosting.TSOpen,LicenseOption.License,250,5350),
            new Price(1,Hosting.TSOpen,LicenseOption.License,500,6450),
            new Price(1,Hosting.TSOpen,LicenseOption.License,999,7350),
            //Lohn-Buy TS Open-Update
            new Price(1,Hosting.TSOpen,LicenseOption.Update,5,480),
            new Price(1,Hosting.TSOpen,LicenseOption.Update,10,780),
            new Price(1,Hosting.TSOpen,LicenseOption.Update,25,1020),
            new Price(1,Hosting.TSOpen,LicenseOption.Update,50,1380),
            new Price(1,Hosting.TSOpen,LicenseOption.Update,100,1860),
            new Price(1,Hosting.TSOpen,LicenseOption.Update,250,2220),
            new Price(1,Hosting.TSOpen,LicenseOption.Update,500,2700),
            new Price(1,Hosting.TSOpen,LicenseOption.Update,999,2940),
            //Lohn-TS max3 -Licens
            new Price(1,Hosting.TSMax3,LicenseOption.License,5,750),
            new Price(1,Hosting.TSMax3,LicenseOption.License,10,1150),
            new Price(1,Hosting.TSMax3,LicenseOption.License,25,1650),
            new Price(1,Hosting.TSMax3,LicenseOption.License,50,2550),
            new Price(1,Hosting.TSMax3,LicenseOption.License,100,3550),
            new Price(1,Hosting.TSMax3,LicenseOption.License,250,4550),
            new Price(1,Hosting.TSMax3,LicenseOption.License,500,5650),
            new Price(1,Hosting.TSMax3,LicenseOption.License,999,6550),
            //Lohn-TS max3-Update
            new Price(1,Hosting.TSMax3,LicenseOption.Update,5,360),
            new Price(1,Hosting.TSMax3,LicenseOption.Update,10,540),
            new Price(1,Hosting.TSMax3,LicenseOption.Update,25,720),
            new Price(1,Hosting.TSMax3,LicenseOption.Update,50,960),
            new Price(1,Hosting.TSMax3,LicenseOption.Update,100,1380),
            new Price(1,Hosting.TSMax3,LicenseOption.Update,250,1620),
            new Price(1,Hosting.TSMax3,LicenseOption.Update,500,2040),
            new Price(1,Hosting.TSMax3,LicenseOption.Update,999,2280),
            //Lohn-RentAsp-Rent Month
            new Price(1,Hosting.RentAsp,LicenseOption.RentYear,5,40*12),
            new Price(1,Hosting.RentAsp,LicenseOption.RentYear,10,65*12),
            new Price(1,Hosting.RentAsp,LicenseOption.RentYear,25,85*12),
            new Price(1,Hosting.RentAsp,LicenseOption.RentYear,50,125*12),
            new Price(1,Hosting.RentAsp,LicenseOption.RentYear,100,185*12),
            new Price(1,Hosting.RentAsp,LicenseOption.RentYear,250,245*12),
            new Price(1,Hosting.RentAsp,LicenseOption.RentYear,500,315*12),
            new Price(1,Hosting.RentAsp,LicenseOption.RentYear,999,365*12),
            //Lohn-RentSingle- Rent Year
            new Price(1,Hosting.RentSingle,LicenseOption.RentYear,5,360),
            new Price(1,Hosting.RentSingle,LicenseOption.RentYear,10,600),
            new Price(1,Hosting.RentSingle,LicenseOption.RentYear,25,780),
            new Price(1,Hosting.RentSingle,LicenseOption.RentYear,50,1140),
            new Price(1,Hosting.RentSingle,LicenseOption.RentYear,100,1680),
            new Price(1,Hosting.RentSingle,LicenseOption.RentYear,250,2040),
            new Price(1,Hosting.RentSingle,LicenseOption.RentYear,500,2580),
            new Price(1,Hosting.RentSingle,LicenseOption.RentYear,999,3120)
        };

        public List<Price> Get() => _prices;

        public Price Calculate(int productId, Hosting hosting, int quantity, LicenseOption license)
            => Calculate(productId, hosting, quantity)
            .FirstOrDefault(x => x.licenseOption.Name == license.Name);

        public List<Price> Calculate(int productId, Hosting hosting, int quantity)
            => Get()
            .Where(x => x.ProductID == productId && x.Quantity == quantity && x.Hosting.Name == hosting.Name)
            .ToList();
    }
}

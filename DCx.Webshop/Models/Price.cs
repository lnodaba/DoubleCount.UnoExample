using System;

namespace DCx.Webshop.Models
{
    public class Price
    {
        public Price(int productID, Hosting hosting, LicenseOption licenseOption, int quantity, float value)
        {
            ProductID = productID;
            Hosting = hosting;
            Quantity = quantity;
            Value = value;
            this.licenseOption = licenseOption;
        }

        public int ProductID { get; set; }
        public Hosting Hosting { get; set; }
        public int Quantity { get; set; }
        public float Value { get; set; }
        public float MonthlyValue 
        { 
            get => Value / 12; 
        }
        public LicenseOption licenseOption { get; set; } = new LicenseOption();
        public float ProRata
        {
            get => licenseOption.IsProrata ? calculateProRata() : Value;
        }

        public string Period
        {
            get => getPeriod(); 
        }

        public bool HasPeriod 
        {
            get => this.licenseOption.IsProrata;
        }


        /// <summary>
        /// In december full year (for next year)
        /// Otherwise the remaining months from th year, but based on where we are in the month that can be included as well before the 15th (including)
        /// </summary>
        /// <returns></returns>
        private float calculateProRata()
        {
            return MonthlyValue * getProRataMultiplier();
        }

        private int getProRataMultiplier()
        {
            if (DateTime.Now.Month == 12)
                return 12;

            var remainingMonths = 12 - DateTime.Now.Month;
            
            if (DateTime.Now.Day <= 15)
                ++remainingMonths;

            return remainingMonths;
        }

        private string getPeriod()
        {
            var currentDate = DateTime.Now;
            var startMonth = currentDate.Day > 15 ? currentDate.Month + 1 : currentDate.Month;
            var endMonth = 12;
            var year = currentDate.Year;

            if (currentDate.Month == 12)
            {
                year += 1;
                startMonth = 1;
            }

            return $"{year}.{startMonth}-{endMonth}";
        }
    }
}

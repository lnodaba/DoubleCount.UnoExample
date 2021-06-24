using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DCx.CsvStore;


namespace DCx.CsvIdentityStore.Models
{
    public class UserProfile : CsvModel
    {
        protected enum Fields
        {
            Id = 0,
            Email,
            Company,
            FirstName,
            LastName,
            Address,
            Zip,
            City
        }

        private const int fieldCount = (int)Fields.City + 1;

        public UserProfile() : base(fieldCount) { }

        public int Id
        {
            get => this.GetFldInteger(Fields.Id);
            set => this.SetFldInteger(Fields.Id, value);
        }

        [EmailAddress]
        public string Email
        {
            get => this.GetFldString(Fields.Email);
            set => this.SetFldString(Fields.Email, value.AsEmail());
        }

        public string Company
        {
            get => this.GetFldString(Fields.Company);
            set => this.SetFldString(Fields.Company, value);
        }

        public string FirstName
        {
            get => this.GetFldString(Fields.FirstName);
            set => this.SetFldString(Fields.FirstName, value);
        }

        public string LastName
        {
            get => this.GetFldString(Fields.LastName);
            set => this.SetFldString(Fields.LastName, value);
        }

        public string Address
        {
            get => this.GetFldString(Fields.Address);
            set => this.SetFldString(Fields.Address, value);
        }

        public string Zip
        {
            get => this.GetFldString(Fields.Zip);
            set => this.SetFldString(Fields.Zip, value);
        }
        public string City
        {
            get => this.GetFldString(Fields.City);
            set => this.SetFldString(Fields.City, value);
        }

        public override int GetFldCount() => fieldCount;
        public override int GetKeyFldIdx() => (int)Fields.Id;
        public override string GetHeader() => String.Join(';', Enum.GetNames(typeof(Fields)));
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

using DCx.CsvStore;

namespace DCx.CsvIdentityStore.Models
{
    public class AppUser : CsvModel
    {
        protected enum Fields
        {
            Id = 0,
            SubjectId,
            UserName,
            Email,
            Password,
            ProviderName,
            ProviderSubjectId,
            Role
        }

        private const int fieldCount = (int)Fields.Role + 1;

        public AppUser() : base(fieldCount)
        {
        }

        public int Id
        {
            get => this.GetFldInteger(Fields.Id);
            set => this.SetFldInteger(Fields.Id, value);
        }

        public string SubjectId
        {
            get => this.GetFldString(Fields.SubjectId);
            set => this.SetFldString(Fields.SubjectId, value);
        }

        public string UserName
        {
            get => this.GetFldString(Fields.UserName);
            set => this.SetFldString(Fields.UserName, value);
        }

        [EmailAddress]
        public string Email
        {
            get => this.GetFldString(Fields.Email);
            set => this.SetFldString(Fields.Email, value.AsEmail());
        }

        public string Password
        {
            get => this.GetFldString(Fields.Password);
            set => this.SetFldString(Fields.Password, value);
        }

        public string ProviderName
        {
            get => this.GetFldString(Fields.ProviderName);
            set => this.SetFldString(Fields.ProviderName, value);
        }

        public string ProviderSubjectId
        {
            get => this.GetFldString(Fields.ProviderSubjectId);
            set => this.SetFldString(Fields.ProviderSubjectId, value);
        }
        public string Role
        {
            get => this.GetFldString(Fields.Role);
            set => this.SetFldString(Fields.Role, value);
        }

        public override int GetFldCount() => fieldCount;
        public override int GetKeyFldIdx() => (int)Fields.Id;
        public override string GetHeader() => String.Join(';', Enum.GetNames(typeof(Fields)));
    }
}

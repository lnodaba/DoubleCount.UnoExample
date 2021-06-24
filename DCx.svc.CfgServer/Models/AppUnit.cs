using DCx.CsvStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.svc.CfgServer.Models
{
    public class AppUnit : CsvModel
    {
        protected enum Fields
        {
            Id = 0,
            UnitId,
            UnitName,
            UnitNumber,
            UnitCountry,
            RegNo,
            AdmUser,
            InvUser,
            InvMail,
            InvAddress,
            Members,
        }

        private const int fieldCount = (int)Fields.Members + 1;

        public AppUnit() : base(fieldCount)
        {
        }

        public int Id
        {
            get => this.GetFldInteger(Fields.Id);
            set => this.SetFldInteger(Fields.Id, value);
        }

        public Guid UnitId
        {
            get => this.GetFldGuid(Fields.UnitId);
            set => this.SetFldGuid(Fields.UnitId, value);
        }

        public string UnitName
        {
            get => this.GetFldString(Fields.UnitName);
            set => this.SetFldString(Fields.UnitName, value);
        }

        public string UnitNumber
        {
            get => this.GetFldString(Fields.UnitNumber);
            set => this.SetFldString(Fields.UnitNumber, value);
        }

        public string UnitCountry
        {
            get => this.GetFldString(Fields.UnitCountry);
            set => this.SetFldString(Fields.UnitCountry, value);
        }

        public string RegNo
        {
            get => this.GetFldString(Fields.RegNo);
            set => this.SetFldString(Fields.RegNo, value);
        }

        public Guid AdmUser
        {
            get => this.GetFldGuid(Fields.AdmUser);
            set => this.SetFldGuid(Fields.AdmUser, value);
        }

        public Guid InvUser
        {
            get => this.GetFldGuid(Fields.InvUser);
            set => this.SetFldGuid(Fields.InvUser, value);
        }

        public string InvMail
        {
            get => this.GetFldString(Fields.InvMail);
            set => this.SetFldString(Fields.InvMail, value);
        }

        public string InvAddress
        {
            get => this.GetFldString(Fields.InvAddress);
            set => this.SetFldString(Fields.InvAddress, value);
        }

        public Guid[] Members
        {
            get => this.GetFldString(Fields.Members)
                .Split(",")
                .Select(x => x.ToGuid())
                .ToArray();
            set => this.SetFldString(Fields.Members, string.Join(",", value.Select(x => x.ToString())));
        }

        public override int GetFldCount() => fieldCount;

        public override string GetHeader() => String.Join(';', Enum.GetNames(typeof(Fields)));

        public override int GetKeyFldIdx() => (int)Fields.Id;
    }
}

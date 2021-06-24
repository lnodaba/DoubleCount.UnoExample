using DCx.CsvStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.svc.CfgServer.Models
{
    public class LicUnit : CsvModel
    {
        protected enum Fields
        {
            Id = 0,
            LicId,
            LicOwner,
            LicProduct,
            LicStore,
            LicYear,
            LicData,
            InvOwner,
            InvNumber,
            InvDate,
            IsPayed
        }

        private const int fieldCount = (int)Fields.IsPayed + 1;

        public LicUnit() : base(fieldCount)
        {
        }

        public int Id
        {
            get => this.GetFldInteger(Fields.Id);
            set => this.SetFldInteger(Fields.Id, value);
        }
        public Guid LicId
        {
            get => this.GetFldGuid(Fields.LicId);
            set => this.SetFldGuid(Fields.LicId, value);
        }
        public Guid LicOwner
        {
            get => this.GetFldGuid(Fields.LicOwner);
            set => this.SetFldGuid(Fields.LicOwner, value);
        }
        public int LicProduct
        {
            get => this.GetFldInteger(Fields.LicProduct);
            set => this.SetFldInteger(Fields.LicProduct, value);
        }
        public string LicStore
        {
            get => this.GetFldString(Fields.LicStore);
            set => this.SetFldString(Fields.LicStore, value);
        }
        public int LicYear
        {
            get => this.GetFldInteger(Fields.LicYear);
            set => this.SetFldInteger(Fields.LicYear, value);
        }
        public string LicData
        {
            get => this.GetFldString(Fields.LicData);
            set => this.SetFldString(Fields.LicData, value);
        }
        public Guid InvOwner
        {
            get => this.GetFldGuid(Fields.InvOwner);
            set => this.SetFldGuid(Fields.InvOwner, value);
        }

        public int InvNumber
        {
            get => this.GetFldInteger(Fields.InvNumber);
            set => this.SetFldInteger(Fields.InvNumber, value);
        }

        public bool IsPayed
        {
            get => this.GetFldBoolean(Fields.IsPayed);
            set => this.SetFldBoolean(Fields.IsPayed, value);
        }


        public DateTime InvDate
        {
            get => this.GetFldDateTime(Fields.IsPayed);
            set => this.SetFldDateTime(Fields.IsPayed, value);
        }

        public override int GetFldCount() => fieldCount;

        public override string GetHeader() => String.Join(';', Enum.GetNames(typeof(Fields)));

        public override int GetKeyFldIdx() => (int)Fields.Id;
    }
}

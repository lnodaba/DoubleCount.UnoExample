using DCx.Messaging;
using DCx.svc.CfgServer.Models;
using DCx.CsvStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DCx.svc.CfgServer.Services
{
    public class LicenseService
    {
        private ICsvTable<LicUnit> _licenseUnits;
        private ICsvTable<AppUnit> _appUnits;


        public LicenseService(ITableMgr tableManager)
        {
            tableManager.LoadTable<LicUnit>();
            tableManager.LoadTable<AppUnit>();

            _licenseUnits = tableManager.GetTable<LicUnit>();
            _appUnits = tableManager.GetTable<AppUnit>();
        }

        public WebBagBinary GetUnitLic(byte[] rawData)
        {
            var result = default(byte[]);
            var error = string.Empty;
            try
            {
                var licenseQuery = rawData.FromUTF8().Split(";"); // "unitNo;regNo;licYear;"
                var args = new
                {
                    UnitNo  = licenseQuery[0],
                    RegNo   = licenseQuery[1],
                    LicYear = int.Parse(licenseQuery[2])
                };

                var appUnit = _appUnits.Records.ToList()
                    .Where(appUnit => appUnit.UnitNumber == args.UnitNo && appUnit.RegNo == args.RegNo)
                    .FirstOrDefault();

                var licenses = _licenseUnits.Records.ToList()
                    .Where(unit => unit.LicOwner == appUnit.AdmUser && unit.LicYear == args.LicYear)
                    .Select(unit => unit)
                    .ToList();

                var resultItems = licenses.Select(license => license.WriteLine(false)).ToArray();
                
                result = string.Join("|", resultItems).ToUTF8();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new WebBagBinary(HttpStatusCode.OK, result, error);
        }
    }
}
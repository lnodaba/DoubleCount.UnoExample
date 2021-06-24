using DCx.IniData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCx.svc.CfgServer.Setup
{
    public class Resources
    {
        #region (vars)
        private static IDxIniData sIniData { get; set; }
        #endregion

        #region (ctor)
        public static void Init(string iniFile)
        {
            sIniData = DxIniFile.GetIniData(iniFile, false);
        }
        #endregion

        #region (func) - GetIniValue
        public static string GetIniValue(string section, string param)
            => sIniData.IniSections.GetValue(section).GetValue(param);
        #endregion
    }
}

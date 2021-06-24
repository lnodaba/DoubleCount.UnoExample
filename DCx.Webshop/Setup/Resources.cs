using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DCx.IniData;
using DCx.Webshop.Config;

namespace DCx.Webshop.Setup
{
    public class Resources
    {
        #region (vars)
        private static IDxIniData   sIniData { get; set; }
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

        #region (func) - GetMailConfig
        public static MailConfig GetMailConfig() 
            => new MailConfig(sIniData.IniSections.GetValue("Mail"));
        #endregion

        #region (func) - GetOicdConfig
        public static OicdConfig GetOicdConfig() 
            => new OicdConfig(sIniData.IniSections.GetValue("oicd"));
        #endregion
    }
}

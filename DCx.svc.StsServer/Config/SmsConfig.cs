using System;
using DCx.IniData;


namespace DCx.StsServer.Config
{
    public class SmsConfig
    {
        #region vars
        public string   EndPoint    { get; init; }
        public string   ApiUsr      { get; init; }
        public string   ApiPwd      { get; init; }

        #endregion

        #region gets
        public bool     IsValid     =>  this.EndPoint.IsUsed() && this.ApiUsr.IsUsed() && this.ApiPwd.IsUsed();

        #endregion

        #region ctor
        public SmsConfig(IDxIniSection iniSection)
        {
            this.EndPoint   = iniSection.GetValue(nameof(this.EndPoint));
            this.ApiUsr     = iniSection.GetValue(nameof(this.ApiUsr));
            this.ApiPwd     = iniSection.GetValue(nameof(this.ApiPwd));
        }
        #endregion
    }
}

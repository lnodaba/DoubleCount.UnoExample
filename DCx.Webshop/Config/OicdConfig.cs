using System;
using DCx.IniData;


namespace DCx.Webshop.Config
{
    public class OicdConfig
    {
        #region vars
        public string   Authority       { get; init; }
        public string   ClientId        { get; init; }
        public string[] DefaultScopes   { get; init; }
        public string   PostLogoutUri   { get; init; }
        public string   ResponseType    { get; init; }

        #endregion

        #region ctor
        public OicdConfig(IDxIniSection iniSection)
        {
            this.Authority      = iniSection.GetValue(nameof(this.Authority));
            this.ClientId       = iniSection.GetValue(nameof(this.ClientId));
            this.DefaultScopes  = iniSection.GetValue(nameof(this.DefaultScopes)).Split(';');
            this.PostLogoutUri  = iniSection.GetValue(nameof(this.PostLogoutUri));
            this.ResponseType   = iniSection.GetValue(nameof(this.ResponseType));
        }
        #endregion
    }
}

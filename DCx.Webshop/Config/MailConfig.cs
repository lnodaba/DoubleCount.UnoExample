using System;
using DCx.IniData;


namespace DCx.Webshop.Config
{
    public class MailConfig
    {
        #region vars
        public  string  Host        { get; init; }
        public  int     Port        { get; init; }
        public  string  Username    { get; init; }
        public  string  Password    { get; init; }
        public  bool    UseSSL      { get; init; }

        public  string  NameFrom    { get; init; }
        public  string  AdrCC       { get; init; }
        public  string  AdrBCC      { get; init; }

        public string ImapServer { get; init; }
        public int ImapPort { get; init; }


        #endregion

        #region gets
        public bool     IsValid     =>  this.Host.IsUsed() && this.Port != 0 && this.Username.IsUsed() && this.Password.IsUsed();

        #endregion

        #region ctor
        public MailConfig(IDxIniSection iniSection)
        {
            this.Host       = iniSection.GetValue(nameof(this.Host));
            this.Port       = iniSection.GetValue(nameof(this.Port)).ToInteger();
            this.Username   = iniSection.GetValue(nameof(this.Username));
            this.Password   = iniSection.GetValue(nameof(this.Password));
            this.UseSSL     = iniSection.GetValue(nameof(this.UseSSL)).ToBoolean();
            this.NameFrom   = iniSection.GetValue(nameof(this.NameFrom));
            this.AdrCC      = iniSection.GetValue(nameof(this.AdrCC));
            this.AdrBCC     = iniSection.GetValue(nameof(this.AdrBCC));
            this.ImapServer  = iniSection.GetValue(nameof(this.ImapServer));
            this.ImapPort    = iniSection.GetValue(nameof(this.ImapPort)).ToInteger();
        }
        #endregion

        #region over - ToString

        public override string ToString()   =>  $"{this.Host} {this.Port} {this.Username} {this.Password} {this.UseSSL} {this.AdrCC} {this.AdrBCC}";

        #endregion
    }
}

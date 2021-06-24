using DCx.List;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DCx.IniData
{
    public class DxIniValue : IDxIniValue
    {
        public DxIniValue(bool splitValues, string txtValue, byte[] rawValue)
        {
            if (txtValue.IsUsed())
            {
                this.TxtValue = txtValue.Trim();

                if (splitValues)
                {
                    this.SubValues = txtValue.Split(';');
                }
            }
            else
            {
                this.RawValue = rawValue;
            }
        }

        public DxIniValue(string iniValue, string[] subValues)
        {
            this.TxtValue   = iniValue.Trim();
            this.SubValues  = subValues;
        }


        public  string          TxtValue        { get; private set; }   = null;
        public  string[]        SubValues       { get; private set; }   = null;
        public  bool            IsList          => (this.SubValues?.Length > 1);

        public  byte[]          RawValue        { get; private set; }   = null;
       

        public string GetString()
        {
            if (this.IsList)
            {
                return String.Join(";", this.SubValues);
            }
            else
            {
                return this.TxtValue.Trim();
            }
        }
    }
}

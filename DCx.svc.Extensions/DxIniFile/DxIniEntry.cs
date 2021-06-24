using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using DCx.List;

namespace DCx.IniData
{
    public class DxIniEntry : IDxIniEntry
    {
        #region (static) ParseLine
        internal static void ParseLine (bool splitValues, string iniLine,
                                        Action<string>      doAddSection, 
                                        Action<IDxIniEntry> doAddEntry)
        {
            if (iniLine?.Length > 2)
            {
                bool        isCompressed = false;
                string      keyPart      = null;
                string      valPart      = null;
                IDxIniEntry iniEntry     = null;


                switch(iniLine[0])
                {
                    //  commentLine
                    case ';':
                    case '*':
                    case '#':   
                        return;

                    //  sectionName
                    case '[':   
                        doAddSection?.Invoke(iniLine.Substring(1, (iniLine.Length-2))); 
                        return;

                    //  isCompressed
                    case '§':
                        isCompressed = true;
                        break;
                }

                if (isCompressed && iniLine.Length>36)
                {
                    keyPart = iniLine.Substring(2,32);
                    valPart = iniLine.Substring(35).Trim();

                    if (keyPart.IsUsed() && valPart.IsUsed())
                    {
                        iniEntry = new DxIniEntry(splitValues, keyPart, null, valPart.ToUTF8());
                    }
                }
                else
                {
                    var posSign = iniLine.IndexOf('=');
                
                    if (posSign < 2)
                    {
                        return;
                    }

                    keyPart = iniLine.Left(posSign).Trim();
                    valPart = iniLine.Substring(posSign+1).Trim();

                    if (keyPart.IsUsed() && valPart.IsUsed())
                    {
                        iniEntry = new DxIniEntry(splitValues, keyPart, valPart, null);
                    }
                }

                if (iniEntry?.IniValue != null)
                {
                    doAddEntry?.Invoke(iniEntry);
                }
            }
        }
        #endregion

        #region members / properties
        public  string          EntryName       { get; set; }   = null;
        public  IDxIniValue     IniValue        { get; set; }   = null;

        #endregion


        #region ctors
        public DxIniEntry(bool splitValues, string entryName, string iniValue, byte[] rawValue)
        {
            this.EntryName  = entryName;
            this.IniValue   = new DxIniValue(splitValues, iniValue, rawValue);
        }

        public DxIniEntry(string entryName, string iniValue, string[] subValues)
        {
            this.EntryName  = entryName;
            this.IniValue   = new DxIniValue(iniValue, subValues);
        }
        #endregion


        #region GetString
        public string GetString(bool isCompressed)
        {
            if (this.IniValue != null)
            {
                if (isCompressed)
                {
                    return $"§-{this.EntryName}-{this.IniValue.RawValue.FromUTF8()}";
                }
                else
                {
                    return $"{this.EntryName} = {this.IniValue.GetString()}";
                }
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}

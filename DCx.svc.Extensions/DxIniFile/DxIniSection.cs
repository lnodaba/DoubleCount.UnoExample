using DCx.List;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DCx.IniData
{
    public class DxIniSection : IDxIniSection
    {
        private readonly bool   m_isCompressed = false;

        public  DxIniSection(bool splitValues, string sectionName, bool isCompressed)
        {
            this.SplitValues    = splitValues;
            this.SectionName    = sectionName;
            this.m_isCompressed = isCompressed;
        }


        public  string      GetValue (string entryName)
            => this.IniEntries.GetValue(entryName)?.IniValue.TxtValue;


        public  string[]    GetValues(string entryName)
            => this.IniEntries.GetValue(entryName)?.IniValue.SubValues;


        public  void AddValue (string entryName, string txtValue) 
            => this.IniEntries.MapValue(entryName, new DxIniEntry(this.SplitValues, entryName, txtValue, null));

        public  void AddValue (string entryName, byte[] rawValue) 
            => this.IniEntries.MapValue(entryName, new DxIniEntry(this.SplitValues, entryName, null, rawValue));

        public  bool                        SplitValues     { get; private set; }

        public  string                      SectionName     { get; private set; }   = null;
        public  DictTxtOf<IDxIniEntry>      IniEntries      { get; private set; }   = new DictTxtOf<IDxIniEntry>();

        public string GetString()
        {
            string result = null;

            if (this.IniEntries.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"[{this.SectionName}]");

                foreach(IDxIniEntry iniEntry in this.IniEntries.ValueList)
                {
                    sb.AppendLine(iniEntry.GetString(this.m_isCompressed));
                }

                result = sb.ToString();
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using DCx.List;


namespace DCx.IniData
{
    public interface IDxIniData
    {
        void                        ParseString     (bool splitValues, string iniValue);
        
        void                        AddComment      (string commentLine);
        IDxIniSection               AddSection      (string sectionName, bool isCompressed);
        IDxIniSection               AddSection      (bool splitValues, string sectionName, bool isCompressed);
        DictTxtOf<IDxIniSection>    IniSections     { get; }

        string                      GetString       ();
    }

    public interface IDxIniSection
    {
        string                      GetValue        (string entryName);
        string[]                    GetValues       (string entryName);
        void                        AddValue        (string entryName, string txtValue);
        void                        AddValue        (string entryName, byte[] rawValue);
        string                      SectionName     { get; }
        DictTxtOf<IDxIniEntry>      IniEntries      { get; }

        string                      GetString       ();
    }

    public interface IDxIniEntry
    {
        string                      EntryName       { get; set; }
        IDxIniValue                 IniValue        { get; set; }

        string                      GetString       (bool isCompressed);
    }

    public interface IDxIniValue
    {
        string                      TxtValue        { get; }
        string[]                    SubValues       { get; }
        bool                        IsList          { get; }

        byte[]                      RawValue        { get; }

        string                      GetString       ();
    }

}

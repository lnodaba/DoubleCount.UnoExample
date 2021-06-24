using System;

namespace DCx.IniData
{
    public interface IDxIniStore
    {
        string          FilePath        { get; }

        void            InitStore       (string filePath, bool splitValues);
        bool            ExistsIniData   ();
        string          RetrieveIniData ();
        void            PersistIniData  (string iniValue);
        void            RemoveIniData   ();

        IDxIniData      GetIniData      (string iniValue);
    }
}

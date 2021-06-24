using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DCx.IniData
{
    public class DxIniFile : DxIniStore<DxIniData>
    {
        #region over - InitStore

        public override void InitStore(string filePath, bool splitValues)
        {
            this.FilePath       = filePath;
            this.SplitValues    = splitValues;
        }
        #endregion

        #region over
        
        public override bool    ExistsIniData()                     =>  File.Exists      (this.FilePath);
        public override string  RetrieveIniData()                   =>  File.ReadAllText (this.FilePath, Encoding.Default);
        public override void    PersistIniData(string iniValue)     =>  File.WriteAllText(this.FilePath, iniValue, Encoding.Default);
        public override void    RemoveIniData()                     =>  File.Delete      (this.FilePath);

        #endregion

        #region (func) GetIniData
        public static IDxIniData GetIniData(string filePath, bool splitValues)
        {
            var iniStore    = new DxIniFile();
                iniStore    .InitStore(filePath, splitValues);

            var iniContent  = iniStore.RetrieveIniData();

            return iniStore.GetIniData(iniContent);
        }
        #endregion

    }
}
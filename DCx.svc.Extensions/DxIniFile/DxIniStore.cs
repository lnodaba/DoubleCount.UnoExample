using DCx.Defines;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DCx.IniData
{
    public abstract class DxIniStore<TSource> : IDxIniStore
        where TSource: IDxIniData, new()
    {
        public  string          FilePath        { get; set; }
        public  bool            SplitValues     { get; set; }

        public  virtual void    InitStore       (string filePath, 
                                                 bool splitValues)  =>  throw new NotImplementedException();
        public  virtual bool    ExistsIniData   ()                  =>  throw new NotImplementedException();
        public  virtual string  RetrieveIniData ()                  =>  throw new NotImplementedException();
        public  virtual void    PersistIniData  (string iniValue)   =>  throw new NotImplementedException();
        public  virtual void    RemoveIniData   ()                  =>  throw new NotImplementedException();

        #region GetIniData

        public IDxIniData GetIniData(string iniValue)
        {
            var iniData = new TSource();
                iniData.ParseString(this.SplitValues, iniValue);
            return iniData;
        }
        #endregion
    }
}

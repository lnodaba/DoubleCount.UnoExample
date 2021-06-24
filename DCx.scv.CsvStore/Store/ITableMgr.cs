using System;
using System.Collections.Generic;
using System.Text;

namespace DCx.CsvStore
{
    public interface ITableMgr
    {
        public  void            LoadTable<T>()  where T : ICsvModel, new();
        public  ICsvTable<T>    GetTable<T> ()  where T : ICsvModel, new();
    }
}

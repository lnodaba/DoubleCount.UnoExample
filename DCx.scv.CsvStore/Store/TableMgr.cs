using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCx.CsvStore
{
    public class TableMgr : ITableMgr
    {
        #region vars

        private WaitTimer                       m_timer         = default;
        private Dictionary<string, ICsvTable>   m_dictTables    = default;
        private string                          m_baseFolder    = default;

        #endregion

        #region sets

        public  void    SetDirty()  =>  this.m_timer.Trigger();

        #endregion

        #region ctor
        public TableMgr(string baseFolder)
        {
            this.m_timer        = new WaitTimer(5000, this.AutoPersist);
            this.m_dictTables   = new Dictionary<string, ICsvTable>();
            this.m_baseFolder   = baseFolder;
        }
        #endregion

        //  ITableMgr
        
        #region void - LoadTable
        public void LoadTable<T>()
            where T : ICsvModel, new()
        {

            var tbl = this.GetTable<T>();
                tbl.Fetch(this.m_baseFolder);
          
        }
        #endregion

        #region func - GetTable
        public ICsvTable<T> GetTable<T>() where T : ICsvModel, new()
        {
            var key = typeof(T).Name;
            var tbl = default(ICsvTable);

            if (!this.m_dictTables.TryGetValue(key, out tbl))
            { 
                var newTable = (ICsvTable<T>)new CsvTable<T>(this.SetDirty);
                this.m_dictTables[key] = tbl = (ICsvTable)newTable;
                return newTable;
            }
            else
            {
                return tbl as ICsvTable<T>;
            }
        }
        #endregion

        //  internal

        #region void - AutoPersist

        private void AutoPersist()
        {
            Task.Run( () =>
                        { 
                            foreach (var csvTable in this.m_dictTables.Values)
                            {
                                csvTable.Commit(this.m_baseFolder, false);
                            }
                        });
        }
        #endregion
    }
}

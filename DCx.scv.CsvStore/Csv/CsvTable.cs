using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DCx.CsvStore
{
    public class CsvTable<T> : ICsvTable<T>
        where T : ICsvModel, new()
    {
       
        #region vars    

        private Action                  m_setDirty  =   default;
        private Dictionary<string, T>   m_dictItems =   default;
        private string                  m_fileName  =   default;

        #endregion

        #region gets

        public  IEnumerable<T>  Records =>  this.m_dictItems.Values;
        public  int             Count   =>  this.m_dictItems.Count;

        #endregion

        #region sets
        public  void            SetDirty()  =>  this.m_setDirty?.Invoke();

        #endregion

        #region ctor
        public CsvTable(Action setDirty)
        {
            this.m_setDirty     = setDirty;
            this.m_dictItems    = new Dictionary<string, T>();
            this.m_fileName     = $"{typeof(T).Name}.csv";
        }
        #endregion


        /*  ICsvTableT */

        #region void - Empty
        
        public void Empty()
        {
            this.m_dictItems.Clear();
        }
        #endregion

        #region void - Fetch

        public void Fetch(string baseFolder)
        {
            var filePath = $@"{baseFolder}\{this.m_fileName}";
            try
            {
                var csvLines = File.ReadAllLines(filePath);

                //  first row is Header
                for (int i = 1; i < csvLines?.Length; i++)
                {
                    var record = new T();
                    record.ReadLine(csvLines[i]);
                    this.m_dictItems.Add(record.Key, record);
                }
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(baseFolder);
                Commit(baseFolder, true);
                Fetch(baseFolder);
            }
            catch (FileNotFoundException)
            {
                Commit(baseFolder, true);
                Fetch(baseFolder);
            }
        }
        #endregion

        #region void - Commit

        public void Commit(string baseFolder, bool force)
        {
            var filePath = $@"{baseFolder}\{this.m_fileName}";
            var isDirty  = force;

            if (!isDirty)
            {
                foreach (T record in this.Records)
                {
                    isDirty = isDirty || record.IsDirty;
                }

            }

            if (isDirty)
            {
                var csvLines = new List<string>(this.m_dictItems.Count);
                    csvLines.Add(new T().GetHeader());

                foreach (T record in this.Records)
                {
                    csvLines.Add(record.WriteLine(force));
                }
            
                File.WriteAllLines(filePath, csvLines);
            }
        }
        #endregion


        #region void - GetItem

        public T GetItem(string itmKey)
        {
            var itm = default(T);
            this.m_dictItems.TryGetValue(itmKey, out itm);
            return itm;
        }
        #endregion

        #region void - AddItem

        public T AddItem(T newItem, bool autoKey)
        {
            var itmKey = default(string);

            if (autoKey)
            {
                var numKey = this.m_dictItems.Count > 0 ? this.m_dictItems.Values.Max( x => x.Key.ToInteger()) : 0;
                    numKey++;
                newItem.Key = itmKey = numKey.ToString();
            }
            else
            {
                itmKey = newItem.Key;
            }

            this.m_dictItems[itmKey] = newItem;
            this.SetDirty();

            return newItem;
        }
        #endregion

        #region void - UpdItem

        public T UpdItem(T updItem) => this.AddItem(updItem, false);

        #endregion

        #region void - DelItem

        public void DelItem(string itmKey)
        {
            if (this.m_dictItems.ContainsKey(itmKey))
            {
                this.m_dictItems.Remove(itmKey);
                this.SetDirty();
            }
        }
        #endregion
    }
}

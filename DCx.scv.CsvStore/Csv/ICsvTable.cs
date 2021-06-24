using System.Collections.Generic;

namespace DCx.CsvStore
{
    public interface ICsvTable
    {
        public int Count { get; }
        public void Empty();
        public void Fetch(string baseFolder);
        public void Commit(string baseFolder, bool force);
    }

    public interface ICsvTable<T> : ICsvTable
        where T : ICsvModel
    {
        public IEnumerable<T> Records { get; }

        public T GetItem(string itmKey);
        public T AddItem(T newItem, bool autoKey);
        public T UpdItem(T updItem);
        public void DelItem(string itmKey);
    }
}

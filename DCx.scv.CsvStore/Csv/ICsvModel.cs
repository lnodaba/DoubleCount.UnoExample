using System;
using System.Collections.Generic;
using System.Text;

namespace DCx.CsvStore
{
    public interface ICsvModel
    {
        public  bool    IsDirty     { get; }

        public  string  Key         { get; set; }

        public  int     GetFldCount ();
        public  int     GetKeyFldIdx();
        public  string  GetHeader   ();

        public  void    ReadLine    (string recLine);
        public  string  WriteLine   (bool force);

    }
}

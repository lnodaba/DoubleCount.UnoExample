using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCx.Mvvm;

namespace DCx.CsvStore
{
    public abstract class CsvModel : PropNotify, ICsvModel
    {
        #region vars

        private object      m_csvLock       = default;
        private string      m_recLine       = default;
        private string[]    m_fldValues     = default;

        public  bool        IsDirty         { get; private set; }

        #endregion
        
        #region gets

        public string   GetFldString    (object fldIndex)               =>  this.GetFldValue(fldIndex);
        public Guid     GetFldGuid      (object fldIndex)               =>  this.GetFldValue(fldIndex).ConvertToGuid();
        public int      GetFldInteger   (object fldIndex)               =>  this.GetFldValue(fldIndex).ToInteger();
        public double   GetFldDouble    (object fldIndex)               =>  this.GetFldValue(fldIndex).ToDouble();
        public bool     GetFldBoolean   (object fldIndex)               =>  this.GetFldValue(fldIndex).ToBool();
        public DateTime GetFldDateTime  (object fldIndex)               =>  this.GetFldValue(fldIndex).ToDateTime();

        #endregion

        #region sets
        public bool     SetFldString    (object fldIndex, string val)   =>  this.SetFldValue(fldIndex, val);
        public bool     SetFldGuid(object fldIndex, Guid val)           =>  this.SetFldValue(fldIndex, val.ToString());
        public bool     SetFldInteger   (object fldIndex, int    val)   =>  this.SetFldValue(fldIndex, val.ToString());
        public bool     SetFldDouble    (object fldIndex, double val)   =>  this.SetFldValue(fldIndex, val.ToString());
        public bool     SetFldDateTime  (object fldIndex, DateTime val)   => this.SetFldValue(fldIndex, val.ToUniversalTime().Ticks.ToString());
        public bool     SetFldBoolean   (object fldIndex, bool   val)   =>  this.SetFldValue(fldIndex, val ? "1" : "0");
        #endregion


        #region ctor
        public CsvModel(int fieldCount)
        {
            this.m_csvLock  = new object();
            this.m_fldValues = new string[fieldCount];
        }
        #endregion


        //  ICsvModel

        #region virtual

        public abstract int     GetFldCount();
        public abstract int     GetKeyFldIdx();
        public abstract string  GetHeader();

        #endregion

        #region prop - Key

        public string Key
        {
            get =>  this.GetFldValue(this.GetKeyFldIdx());
            set =>  this.SetFldValue(this.GetKeyFldIdx(), value);
        }

        #endregion


        #region void - ReadLine
        public void ReadLine(string recLine)
        {
            if (recLine.IsUsed())
            {
                this.m_recLine      = recLine;
                this.m_fldValues    = recLine.Split(';');

                if (this.GetFldCount() != this.m_fldValues?.Length)
                {
                    throw new IndexOutOfRangeException("wrong fldCount");
                }
            }
            else
            {
                this.m_fldValues    = new string[this.GetFldCount()];
                this.IsDirty        = true;
            }
           
        }
        #endregion

        #region void - WriteLine
        public string WriteLine(bool force)
        {
            if (this.IsDirty || force)
            {
                lock (this.m_csvLock)
                {
                    this.m_recLine = String.Join(';', this.m_fldValues);
                }
                this.IsDirty = false;
            }
            return this.m_recLine;
        }
        #endregion


        //  internal

        #region func - GetFldValue

        protected string GetFldValue(object fldIndex)
        {
            var idx = (int)fldIndex;
            if (this.m_fldValues == null)
                return string.Empty;

            return this.m_fldValues[idx];
        }
        #endregion

        #region func - SetFldValue
        private bool SetFldValue(object fldIndex, string value)
        {
            var idx     = (int)fldIndex;
            var newVal  = value.AsCsvField();
            var changed = this.m_fldValues[idx] != newVal;

            if (changed)
            {
                lock (this.m_csvLock)
                {
                    this.m_fldValues[idx] = newVal;
                }
                this.IsDirty = true;
                this.RaisePropChanged(fldIndex.ToString());
            }
            return changed;
        }

        #endregion
    }
}

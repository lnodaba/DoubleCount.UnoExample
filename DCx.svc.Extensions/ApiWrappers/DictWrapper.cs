using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DCx.List
{
    public class DictIntOf<TValue> : Dict<int, TValue>
    { 
        public DictIntOf(bool useLock = false) : base(useLock) {}
        public DictIntOf(int capacity) : base(capacity) {}
    }

    public class DictTxtOf<TValue> : Dict<string, TValue>
    { 
        public DictTxtOf(bool useLock = false) : base(useLock) {}
        public DictTxtOf(int capacity) : base(capacity) {}
        public DictTxtOf(IEqualityComparer<string> comparer) : base(comparer)  { }
    }

    public class DictGuidOf<TValue> : Dict<Guid, TValue>
    {
        public DictGuidOf(bool useLock = false) : base(useLock) {}
        public DictGuidOf(int capacity) : base(capacity) {}
    }

    public class DictTypeOf<TValue> : Dict<Type, TValue>
    {
        public DictTypeOf(bool useLock = false) : base(useLock) {}
        public DictTypeOf(int capacity) : base(capacity) {}
    }



    public class Dict<TKey, TValue> : Dictionary<TKey, TValue>
    { 
        #region members/properties

        private bool    m_isProcessing  = false;
        private string  m_logProcessing = null;
        private bool    m_useLock       = false;
        private object  m_dictLock      = new object();

        #endregion

        #region ctors
        public Dict(bool useLock = false) : base()
        {
            this.m_useLock = useLock;
        }
        public Dict(int capacity) : base(capacity)  { }
        public Dict(IEqualityComparer<TKey> comparer) : base(comparer)  { }

        #endregion

    
        #region Method - HasKey
        public bool HasKey(TKey key)
        {
            if (this.m_useLock)
            {
                lock(this.m_dictLock)
                {
                    return base.ContainsKey(key);
                }
            }
            else
            {
                #if DEBUG   //  additional check
                    this.SetProcessFlag($"HasKey {key}");
                    var _result = base.ContainsKey(key);
                    this.ClearProcessFlag();
                    return _result;
                #else
                    return base.ContainsKey(key);
                #endif
            }
        }
        #endregion


        #region Method - MapValue
        public TValue MapValue(TKey key, TValue value, bool allowNull = false)
        {
            //#if DEBUG
            //if (base.ContainsKey(key))
            //{
            //    System.Diagnostics.Debug.WriteLine($"@ Dict[{key}] = {value} -> overwriting existing key");
            //}
            //#endif

            if (allowNull || value != null)
            {
                if (this.m_useLock)
                {
                    lock(this.m_dictLock)
                    {
                        base[key] = value;
                    }
                }
                else
                {
                    #if DEBUG   //  additional check
                        this.SetProcessFlag($"MapValue {key} {value}");

                        if (base.ContainsKey(key))
                        {
                            base[key] = value;
                        }
                        else
                        {
                            base.Add(key, value);
                        }
                        this.ClearProcessFlag();
                    #else
                        base[key] = value;
                    #endif
                }
            }
            else
            {
                System.Diagnostics.Debugger.Break();
            }
            return value;
        }
        #endregion

        #region Method - GetValue
        public TValue GetValue(TKey key, Func<TValue> funcMissing = null)
        {
            if (this.m_useLock)
            {
                lock(this.m_dictLock)
                {
                    return this.InnerGetValue(key, funcMissing);
                }
            }
            else
            {
                return this.InnerGetValue(key, funcMissing);
            }
        }

        private TValue InnerGetValue(TKey key, Func<TValue> funcMissing)
        {
            if (!base.TryGetValue(key, out TValue _value))
            {
                if (funcMissing != null)
                {
                    _value = funcMissing();
                    #if DEBUG   //  additional check
                        this.SetProcessFlag($"GetValue (insert) {key}");
                        if (_value != null)
                        {
                            base[key] = _value;
                        }
                        this.ClearProcessFlag();
                    #else
                    if (_value != null)
                        {
                            base[key] = _value;
                        }
                    #endif
                }
            }
            return _value;
        }
        #endregion

        #region Method - DelKey
        public bool DelKey(TKey key)
        {
            if (this.m_useLock)
            {
                lock(this.m_dictLock)
                {
                    return this.InnerDelKey(key);
                }
            }
            else
            {
                #if DEBUG   //  additional check
                    this.SetProcessFlag($"DelKey {key}");
                    var _result = this.InnerDelKey(key);
                    this.ClearProcessFlag();
                    return _result;
                #else
                    return this.InnerDelKey(key);
                #endif
            }
        }

        private bool InnerDelKey(TKey key)
        {
            if (base.ContainsKey(key))
            {
                return base.Remove(key);
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region Prop KeyList
        public List<TKey> KeyList
        {
            get
            {
                if (this.m_useLock)
                {
                    lock(this.m_dictLock)
                    {
                        return new List<TKey>(base.Keys);
                    }
                }
                else
                {
                    #if DEBUG   //  additional check
                        this.SetProcessFlag($"KeyList");
                        var _result = new List<TKey>(base.Keys);
                        this.ClearProcessFlag();
                        return _result;
                    #else
                        return new List<TKey>(base.Keys);
                    #endif
                }
            }
        }
        #endregion

        #region Prop ValueList

        public List<TValue> ValueList
        {
            get
            {
                if (this.m_useLock)
                {
                    lock(this.m_dictLock)
                    {
                        return new List<TValue>(base.Values);
                    }
                }
                else
                {
                    #if DEBUG   //  additional check
                        this.SetProcessFlag($"ValueList");
                        var _result = new List<TValue>(base.Values);
                        this.ClearProcessFlag();
                        return _result;
                    #else
                        return new List<TValue>(base.Values);
                    #endif
                }
            }
        }
        #endregion


        #region helper - RaceCondition (DEBUG)

        private void SetProcessFlag(string logItem)
        {
            lock(this.m_dictLock)
            {
                if (this.m_isProcessing && this.m_logProcessing.IsUsed())
                {
                    // race condition > use lock in ctor
                    System.Diagnostics.Debugger.Break();
                }
                this.m_isProcessing  = true;
                this.m_logProcessing = logItem;
            }
        }

        private void ClearProcessFlag()
        {
            lock(this.m_dictLock)
            {
                this.m_isProcessing  = false;
                this.m_logProcessing = null;
            }
        }

        #endregion

    
        #region overrides - Disabling

        [Obsolete ("Use HasKey")]
        public new bool ContainsKey (TKey key)                  => base.ContainsKey(key);

        [Obsolete ("Use MapValue")]
        public new void Add         (TKey key, TValue value)    => base.Add(key, value);

        [Obsolete ("Use MapValue/GetValue")]
        public new TValue this[TKey key] 
        { 
            get => base[key]; 
            set => base[key] = value; 
        }

        [Obsolete ("Use GetValue")]
        public new bool TryGetValue(TKey key, out TValue value) => base.TryGetValue(key, out value);

        [Obsolete ("Use DelKey")]
        public new bool Remove      (TKey key)                  => base.Remove(key);

        [Obsolete ("Use GetKeys")]
        public new KeyCollection    Keys                        => base.Keys;

        [Obsolete ("Use GetValues")]
        public new ValueCollection  Values                      => base.Values;

        #endregion
    }

}
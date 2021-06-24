using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace DCx.Mvvm
{
    public abstract class PropNotify : INotifyPropertyChanged, IDisposable
    {
        public      event PropertyChangedEventHandler    PropertyChanged;
        public      bool ActiveINPC                                         => this.PropertyChanged?.GetInvocationList().Length > 0;
        public      void RaisePropChanged(string propName)                  => this.RaisePropChanged(new PropertyChangedEventArgs(propName));
        protected   void RaisePropChanged(PropertyChangedEventArgs eArgs)   => this.PropertyChanged?.Invoke(this, eArgs);


        /*  IDisposable */

        #region void - Dispose

        public void Dispose()
        {
            this.Dispose(true); 
            GC.SuppressFinalize(this);
        }
        #endregion

        #region virtual - Dispose

        private bool m_disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (this.m_disposed)
            {
                return;
            }

            this.m_disposed = true;

            if (disposing)
            {
            }
        }
        #endregion


        #region func - SetPropChanged

        public bool SetPropChanged<TResult>(ref TResult curValue, TResult newValue, Action changeAction = null, [CallerMemberName] string propName = "")
        {
            if (EqualityComparer<TResult>.Default.Equals(curValue, newValue))
            {
                return false;
            }
            else
            {
                curValue = newValue;

                this.RaisePropChanged(new PropertyChangedEventArgs(propName));

                changeAction?.Invoke();

                return true;
            }
        }
        #endregion
    }
}

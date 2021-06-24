using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DCx.Mvvm
{
    public abstract class PropNotify : INotifyPropertyChanged
    {
        #region event

        public      event   PropertyChangedEventHandler                         PropertyChanged;

        public      void    RaisePropChanged(string propName)                   => this.RaisePropChanged(new PropertyChangedEventArgs(propName));
        protected   void    RaisePropChanged(PropertyChangedEventArgs eArgs)    => this.PropertyChanged?.Invoke(this, eArgs);

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
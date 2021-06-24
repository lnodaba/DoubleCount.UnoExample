using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;
using DCx.Mvvm;
using System.Windows.Threading;

namespace DCx.Mvvm
{
    public class VMCommand : PropNotify, ICommand
    {
        #region event

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion

        #region vars

        private readonly    Predicate<object>       m_canExecute;
        private readonly    Action<object>          m_exeAction;
        public              object                  Parameter           { get; set; }

        #endregion

        #region ctor

        public VMCommand(Action<object> exeAction, Predicate<object> canExecute=null, object parameter=null)
        {
            this.m_exeAction    = exeAction;
            this.m_canExecute   = canExecute;
            this.Parameter      = parameter;
        }
        #endregion

        #region func - CanExecute

        public bool CanExecute(object parameter)
        {
            if (this.m_exeAction == null)
            {
                return false;
            }
            if (this.m_canExecute == null)
            {
                return true;
            }
            if (this.Parameter != null)
            {
                return this.m_canExecute(this.Parameter);
            }
            return this.m_canExecute(parameter);
        }
        #endregion

        #region void - Execute

        public void Execute(object parameter)
        {
            if (this.CanExecute(parameter))
            {
                if (this.m_exeAction != null)
                {
                    this.CmdExecute(parameter);
                }
            }
        }
        private void CmdExecute(object parameter)
        {
            object _param = this.Parameter ?? parameter;

            this.FireCanExecuteChanged();

            try
            {
                this.m_exeAction(_param);
            }
            finally
            {
                this.FireCanExecuteChanged();
            }
        }
        #endregion


        #region void - FireCanExecuteChanged

        public void FireCanExecuteChanged()
        {
            if (Dispatcher.CurrentDispatcher != null)
            {
                Dispatcher.CurrentDispatcher.Invoke(() => CommandManager.InvalidateRequerySuggested());
            }
            else
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
        #endregion
    }
}
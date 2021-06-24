using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace DCx.CsvStore
{
    public class WaitTimer : IDisposable
    {
        #region vars

        Timer   m_timer;
        int     m_wait;
        bool    m_triggered;
        Action  m_callback;

        #endregion

        #region ctor

        public WaitTimer(int milliSeconds, Action ellapsedCallback)
        {
            m_wait      = milliSeconds;
            m_callback  = ellapsedCallback;
        }
        #endregion

        #region IDisposable

        public void Dispose()   =>  this.Stop(false);

        #endregion


        #region void - Trigger

        public void Trigger()
        {
            if (m_timer == null)
            {
                m_timer             = new Timer(m_wait);
                m_timer.AutoReset   = false;
                m_timer.Elapsed    += delegate { this.Stop(true); }; 
            }
            else
            {
                m_timer.Stop();
            }

            this.m_triggered = true;

            m_timer.Start();
        }
        #endregion

        #region void - Stop

        public void Stop (bool doInvoke = false)
        {
            this.m_timer?.Stop();

            if (doInvoke && this.m_triggered)
            {
                this.m_callback.Invoke();
            }
            this.m_triggered = false;
        }

        #endregion
    }
}

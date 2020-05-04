using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace sfx_100_streamdeck_sfb_extension
{
    class GuiLoggerProvider
    {
        #region Singleton
        private static volatile GuiLoggerProvider instance;
        private static object syncRoot = new Object();

        private GuiLoggerProvider() { }

        public static GuiLoggerProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new GuiLoggerProvider();
                        }
                        return instance;
                    }
                }
                return instance;
            }
        }

        public ListBox LogBox { get; set; }

        #endregion



        /// <summary>
        /// Simple logging to window. Holds only limited entries to prevent scrolling
        /// </summary>
        /// <param name="logEntry">Object to log</param>
        public void Log(object logEntry)
        {
            if (LogBox != null)
            {
                if (LogBox.Items.Count >= 1000)
                {
                    LogBox.Items.RemoveAt(0);
                }

                LogBox.Items.Add(DateTime.Now + ": " + logEntry);
                LogBox.SelectedIndex = LogBox.Items.Count - 1;
                LogBox.ScrollIntoView(LogBox.SelectedItem);
            }
        }
    }
}
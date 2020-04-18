using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using NPCommunication;
using sfx_100_streamdeck_sfb_extension.Properties;

namespace sfx_100_streamdeck_sfb_extension
{
    /// <summary>
    /// Interaktionslogik für ModBusExtensionControlGUI.xaml
    /// </summary>
    public partial class StreamdeckExtensionControlGui
    {

        #region Main Entry

        public StreamdeckExtensionControlGui()
        {
            InitializeComponent();
            streamdeckExtensionTitle.Content = streamdeckExtensionTitle.Content + " - " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            DataContext = this;
            Log("startup Extension GUI");
        }

        #endregion

        #region Members

        private NamedPipeServerStream _pipeServer;
        public SimFeedback.extension.SimFeedbackExtensionFacade SimFeedbackFacade;
        
        #endregion

        #region Private methods

        private void StartServer()
        {
            NPServer server = new NPServer("sfx100streamdeck", "sfx100streamdeck");
            server.UpdateCommand("Start", argruments =>
            {
                Log("Received Command: Start");
                if (SimFeedbackFacade != null) SimFeedbackFacade.Start();
                return "Started";
            });
            server.UpdateCommand("Stop", argruments =>
            {
                Log("Received Command: Stop");
                if (SimFeedbackFacade != null) SimFeedbackFacade.Stop();
                return "Stopped";
            });
            server.Start();
        }


        private void ShutdownServer()
        {
            _pipeServer.Disconnect();
            _pipeServer.Dispose();
        }


        #endregion

        #region Private helpers


        /// <summary>
        /// Simple logging to window. Holds only limited entries to prevent scrolling
        /// </summary>
        /// <param name="logEntry">Object to log</param>
        private void Log(object logEntry)
        {
          Dispatcher?.BeginInvoke(
                DispatcherPriority.Normal,  
                new Action(() =>
                {
                    
                    if (debugBox.Items.Count >= 250)
                    {
                        debugBox.Items.RemoveAt(0);
                    }

                    debugBox.Items.Add(DateTime.Now + ": " + logEntry);
                    debugBox.SelectedIndex = debugBox.Items.Count - 1;
                    debugBox.ScrollIntoView(debugBox.SelectedItem);
                }));
        }


        #endregion

        #region Eventhandlers

        /// <summary>
        /// Eventhandler called when control is loaded.
        /// /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StreamdeckExtensionControlGUI_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Log("Waiting for commands");
                StartServer();
            }
            catch (Exception ex)
            {
                Log("Warning: Could not start server " + ex.Message);
            }
        }



        /// <summary>
        /// Disconnect when UI is unloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StreamdeckExtensionControlGUI_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Log("Shut down Server");
            ShutdownServer();
            Settings.Default.Save();
        }

        /// <summary>
        /// Eventhandler launched when Project URI is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        #endregion
    }
}
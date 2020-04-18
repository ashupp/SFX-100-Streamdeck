using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Serialization;
using sfx_100_streamdeck_sfb_extension.Properties;
using MessageBox = System.Windows.MessageBox;

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

        #endregion

        #region Private methods

        /// <summary>
        /// Tries to connects to ModBus
        /// </summary>
        private void Connect()
        {
            Log("Connecting...");
           
        }


        /// <summary>
        /// Disconnects from ModBus
        /// </summary>
        private void Disconnect()
        {
            Log("disconnect");
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
                    /*
                    if (debugBox.Items.Count >= 250)
                    {
                        debugBox.Items.RemoveAt(0);
                    }

                    debugBox.Items.Add(DateTime.Now + ": " + logEntry);
                    debugBox.SelectedIndex = debugBox.Items.Count - 1;
                    debugBox.ScrollIntoView(debugBox.SelectedItem);*/
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
                Log("Try to connect automatically");
                Connect();
            }
            catch (Exception ex)
            {
                Log("Warning: Could not connect " + ex.Message);
            }
        }



        /// <summary>
        /// Eventhandler for click on connect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConnect(object sender, RoutedEventArgs e)
        {
            Connect();
        }

        /// <summary>
        /// Eventhandler for click on disconnect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDisconnect(object sender, RoutedEventArgs e)
        {
            Disconnect();
        }


        /// <summary>
        /// Disconnect when UI is unloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StreamdeckExtensionControlGUI_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Disconnect();
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
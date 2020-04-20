using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using sfx_100_streamdeck_sfb_extension.Properties;
using System.ServiceModel;
using sfx_100_streamdeck_pipecontract;

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
        }

        #endregion

        #region Members


        public SimFeedback.extension.SimFeedbackExtensionFacade SimFeedbackFacade;
        
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
                GuiLoggerProvider.Instance.Log("Startup Extension GUI");
            }
            catch (Exception ex)
            {
                GuiLoggerProvider.Instance.Log("Warning: Could not start server " + ex.Message);
            }
        }



        /// <summary>
        /// Disconnect when UI is unloaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StreamdeckExtensionControlGUI_OnUnloaded(object sender, RoutedEventArgs e)
        {
            GuiLoggerProvider.Instance.Log("Shutdown Extension GUI");
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
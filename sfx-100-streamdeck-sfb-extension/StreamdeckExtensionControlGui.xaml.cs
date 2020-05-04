using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using sfx_100_streamdeck_sfb_extension.Properties;
using System.Threading.Tasks;
using System.Windows.Forms;
using Application = System.Windows.Application;

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

            GuiLoggerProvider.Instance.Log("Init Extension GUI");
            LoadWithDelay();
        }

        async Task LoadWithDelay()
        {
            await Task.Delay(Settings.Default.UIAutomationDelay);
            GuiLoggerProvider.Instance.Log("Load UIAutomation with delay..." + Settings.Default.UIAutomationDelay);
            SimFeedbackInvoker.Instance.LoadElements();
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
                GuiLoggerProvider.Instance.Log("Loaded Extension GUI");
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FileStream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "streamDeckExtensionLog.txt";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile() as FileStream) != null)
                {
                    // Code to write the stream goes here.
                    int offset = 0;
                    foreach (var item in GuiLoggerProvider.Instance.LogBox.Items)
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(item.ToString()+"\r\n");
                        myStream.Write(bytes, 0, bytes.Length);
                        offset += bytes.Length;
                    }
                    
                    myStream.Close();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GuiLoggerProvider.Instance.Log("Change Tab and load UI Automation");
            SimFeedbackInvoker.Instance.SelectProfileTab();
            SimFeedbackInvoker.Instance.LoadElements();
            GuiLoggerProvider.Instance.Log("End Change Tab and load UI Automation ");
        }
    }
}
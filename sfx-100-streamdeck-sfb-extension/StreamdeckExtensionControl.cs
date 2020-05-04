using System;
using System.Windows.Forms;
using SimFeedback.extension;

namespace sfx_100_streamdeck_sfb_extension
{
    public partial class StreamdeckExtensionControl : UserControl
    {
        private bool isStarted = false;

        private StreamdeckExtension _streamdeckExtension;
        private SimFeedbackExtensionFacade _simFeedbackFacade;


        public StreamdeckExtensionControl(StreamdeckExtension ext, SimFeedbackExtensionFacade simFeedbackFacade)
        {
            _streamdeckExtension = ext;
            _simFeedbackFacade = simFeedbackFacade;
            SimFeedbackFacadeProvider.Instance.SimFeedbackFacade = simFeedbackFacade;
            
            InitializeComponent();

            GuiLoggerProvider.Instance.LogBox =  _streamdeckExtensionControlGui1.debugBox;
        }


        public void Start()
        {
            isStarted = true;
            _streamdeckExtension.SetIsRunning(true);
            //throw new NotImplementedException();
        }

        public void Stop()
        {
            if (!isStarted) return;
            isStarted = false;
            _streamdeckExtension.SetIsRunning(false);
            //throw new NotImplementedException();
        }

        private void ParentForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                StartStopToggle();
            }
        }

        public void StartStopToggle()
        {
            if (isStarted)
            {
                isStarted = false;
                Stop();
            }
            else
            {
                isStarted = true;
                Start();
            }
        }

        private void ModBusExtensionControl_Load(object sender, EventArgs e)
        {
            if (ParentForm != null)
            {
                ParentForm.FormClosing += ParentForm_FormClosing;
                this.ParentForm.KeyDown += ParentForm_KeyDown;
                ParentForm.KeyPreview = true;
            }



            /*GuiLoggerProvider.Instance.Log("4 Loading Elements for UI Automation");

            var mainWindowHandle = SimFeedbackInvoker.Instance.GetHandleWindow("SimFeedback - 00.09.08");

            GuiLoggerProvider.Instance.Log("4 Aktuelles Window handle: " + mainWindowHandle);
            */

            //SimFeedbackInvoker.Instance.LoadElements();

        }



        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }
    }
}

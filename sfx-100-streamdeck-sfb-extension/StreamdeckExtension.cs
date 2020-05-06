using System;
using System.ServiceModel;
using System.Threading.Tasks;
using SimFeedback.extension;
using System.Windows.Forms;
using sfx_100_streamdeck_pipecontract;
using WpfApp2;


namespace sfx_100_streamdeck_sfb_extension
{
    public class StreamdeckExtension : AbstractSimFeedbackExtension
    {
        private StreamdeckExtensionControl _extCtrl;
        private ServiceHost _serviceHost;
        private SimFeedbackInvoker sfbInvoker;

        public StreamdeckExtension()
        {
            Name = "Stream Deck Extension";
            Info = "Control your SFX-100 with your Stream Deck";
            Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Author = "ashupp";
            NeedsOwnTab = false;
            HasControl = true;
        }

        public override void Init(SimFeedbackExtensionFacade facade, ExtensionConfig config)
        {
            base.Init(facade, config);
            Log("Initialize Streamdeck Extension");
            _extCtrl = new StreamdeckExtensionControl(this, facade);
        }

        public void SetIsRunning(bool status)
        {
            IsRunning = status;
        }

        private void StartServer()
        {
            string address = "net.pipe://localhost/ashnet/StreamDeckExtension";
            _serviceHost = new ServiceHost(typeof(SfxStreamDeckPipeServer));
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            _serviceHost.AddServiceEndpoint(typeof(ISfxStreamDeckPipeContract), binding, address);
            _serviceHost.Open();
            GuiLoggerProvider.Instance.Log("Waiting for commands");
        }



        private void ShutdownServer()
        {
            _serviceHost.Abort();
            _serviceHost.Close();
            GuiLoggerProvider.Instance.Log("Shutdown Server");
        }

        public override void Start()
        {
            if (IsRunning) return;
            SimFeedbackFacade.Log("Starting Stream Deck Extension");
            GuiLoggerProvider.Instance.Log("Starting Stream Deck Extension");

            StartServer();
            _extCtrl.Start();
            IsRunning = true;
            GuiLoggerProvider.Instance.Log("Stream Deck Extension running");
        }


        public override void Stop()
        {
            if (!IsRunning) return;
            Log("Stopping Stream Deck Extension");
            GuiLoggerProvider.Instance.Log("Stopping Stream Deck Extension");
            ShutdownServer();
            _extCtrl.Stop();
            IsRunning = false;
        }

        public override Control ExtensionControl => _extCtrl;
    }
}
using SimFeedback.extension;
using System.Windows.Forms;

namespace sfx_100_streamdeck_sfb_extension
{
    public class StreamdeckExtension : AbstractSimFeedbackExtension
    {
        private StreamdeckExtensionControl _extCtrl;

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

        public override void Start()
        {
            if (IsRunning) return;
            SimFeedbackFacade.Log("Starting Stream Deck Extension");
            _extCtrl.Start();
            IsRunning = true;
        }

        public override void Stop()
        {
            if (!IsRunning) return;
            Log("Stopping Stream Deck Extension");
            _extCtrl.Stop();
            IsRunning = false;
        }

        public override Control ExtensionControl => _extCtrl;
    }
}
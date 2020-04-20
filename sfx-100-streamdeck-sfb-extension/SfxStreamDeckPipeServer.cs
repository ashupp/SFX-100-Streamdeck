using System.ServiceModel;
using sfx_100_streamdeck_pipecontract;

namespace sfx_100_streamdeck_sfb_extension
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    public class SfxStreamDeckPipeServer : ISfxStreamDeckPipeContract
    {
        public bool CheckConnection()
        {
            GuiLoggerProvider.Instance.Log("Incoming Command: CheckConnection");
            return true;
        }

        public bool IsRunning()
        {
            GuiLoggerProvider.Instance.Log("Incoming Command: IsRunning");
            var isRunning = SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.IsRunning();
            GuiLoggerProvider.Instance.Log("Returning: " + isRunning);
            return isRunning;

        }

        public bool IsTelemetryProviderConnected()
        {

            GuiLoggerProvider.Instance.Log("Incoming Command: IsTelemetryProviderConnected");
            var isTelemetryProviderConnected = SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.IsTelemetryProviderConnected();
            GuiLoggerProvider.Instance.Log("Returning: " + isTelemetryProviderConnected);
            return isTelemetryProviderConnected;
        }

        public bool StartMotion()
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: Start");
                SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool StopMotion()
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: Stop");
                SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.Stop();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EnableAllEffects()
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: EnableAllEffects");
                SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.EnableAllEffects();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DisableAllEffects()
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: DisableAllEffects");
                SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.DisableAllEffects();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IncrementOverallIntensity(int steps)
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: IncrementOverallIntensity by " + steps);
                for (var i = steps - 1; i >= 0; i--)
                    SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.IncrementOverallIntensity();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DecrementOverallIntensity(int steps)
        {
            try
            {
                GuiLoggerProvider.Instance.Log("Incoming Command: DecrementOverallIntensity by " + steps);
                for (var i = steps - 1; i >= 0; i--)
                    SimFeedbackFacadeProvider.Instance.SimFeedbackFacade.DecrementOverallIntensity();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
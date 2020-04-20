using System.ServiceModel;

namespace sfx_100_streamdeck_pipecontract
{
    [ServiceContract]
    public interface ISfxStreamDeckPipeContract
    {
        [OperationContract]
        bool CheckConnection();

        [OperationContract]
        bool IsRunning();

        [OperationContract]
        bool IsTelemetryProviderConnected();

        [OperationContract]
        bool StartMotion();

        [OperationContract]
        bool StopMotion();

        [OperationContract]
        bool EnableAllEffects();

        [OperationContract]
        bool DisableAllEffects();

        [OperationContract]
        bool IncrementOverallIntensity(int steps);

        [OperationContract]
        bool DecrementOverallIntensity(int steps);
    }
}

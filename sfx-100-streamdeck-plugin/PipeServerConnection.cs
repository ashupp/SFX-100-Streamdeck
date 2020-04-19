using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using BarRaider.SdTools;

namespace sfx_100_streamdeck_plugin
{
    class PipeServerConnection
    {
        #region Singleton
        private static volatile PipeServerConnection instance;
        private static object syncRoot = new Object();
        public PipeContract Channel;
        private NetNamedPipeBinding _binding;
        private EndpointAddress _ep ;

        private PipeServerConnection()
        {
            createServer();
        }

        void createServer()
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "createServer");
            string address = "net.pipe://localhost/ashnet/StreamDeckExtension";
            _binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            _ep = new EndpointAddress(address);
            Channel = ChannelFactory<PipeContract>.CreateChannel(_binding, _ep);

            ((ICommunicationObject)Channel).Faulted += ProxyServiceFactory_Faulted;
        }


        void ProxyServiceFactory_Faulted(object sender, EventArgs e)
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "Channel Faulted...");
            ((ICommunicationObject)sender).Abort();
            if (sender is ChannelFactory<PipeContract>)
            {
                Logger.Instance.LogMessage(TracingLevel.INFO, "Recreating...");
                Channel = ChannelFactory<PipeContract>.CreateChannel(_binding, _ep);
            }
        }

        public static PipeServerConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new PipeServerConnection();
                        }
                        return instance;


                    }
                }
                return instance;
            }
        }
        #endregion
    }
}

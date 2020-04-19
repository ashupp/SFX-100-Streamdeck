using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace sfx_100_streamdeck_plugin
{
    class PipeServerConnection
    {
        #region Singleton
        private static volatile PipeServerConnection instance;
        private static object syncRoot = new Object();
        public PipeContract Channel;

        private PipeServerConnection()
        {
            string address = "net.pipe://localhost/ashnet/StreamDeckExtension";
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            EndpointAddress ep = new EndpointAddress(address);
            Channel = ChannelFactory<PipeContract>.CreateChannel(binding, ep);
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

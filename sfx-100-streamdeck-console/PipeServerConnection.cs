using System;
using System.ServiceModel;
using sfx_100_streamdeck_pipecontract;

namespace sfx_100_streamdeck_plugin
{
    class PipeServerConnection
    {
        #region Singleton
        private static volatile PipeServerConnection instance;
        private static object syncRoot = new Object();
        public ISfxStreamDeckPipeContract Channel;
        private NetNamedPipeBinding _binding;
        private EndpointAddress _ep ;

        private PipeServerConnection() { }

        void createServer()
        {
            string address = "net.pipe://localhost/ashnet/StreamDeckExtension";
            _binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
            {
                CloseTimeout = TimeSpan.FromSeconds(5),
                OpenTimeout = TimeSpan.FromSeconds(5),
                ReceiveTimeout = TimeSpan.FromSeconds(10),
                SendTimeout = TimeSpan.FromSeconds(5)
            };

            _ep = new EndpointAddress(address);
            Channel = ChannelFactory<ISfxStreamDeckPipeContract>.CreateChannel(_binding, _ep);
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

        public void RestartChannel()
        {
            try
            {
                if (Channel != null)
                {
                    ((ICommunicationObject) Channel).Abort();
                    ((ICommunicationObject) Channel).Close();
                }
                createServer();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during RestartChannel: " + ex.Message);
            }
        }
    }
}

using Amrv.ConfigurableCompany.Plugin;

namespace Amrv.ConfigurableCompany.Core.Net
{
    internal static class NetEventRouter
    {
        public static void RegisterListeners()
        {
            NetSynchronizer.ServerCallbacks.Start += Server_Start;
            NetSynchronizer.ServerCallbacks.Stop += Server_Stop;
            NetSynchronizer.ServerCallbacks.ClientConnect += Server_ClientConnect;
            NetSynchronizer.ServerCallbacks.ClientDisconnect += Server_ClientDisconnect;
            NetSynchronizer.ClientCallbacks.Connect += Client_Connect;
            NetSynchronizer.ClientCallbacks.Disconnect += Client_Disconnect;
        }

        public static void Server_Start()
        {
            ConfigurableCompanyPlugin.Debug($"NetEventRouter > Server :: Start");
            NetReceiveRouter.RegisterServerMessages();
        }

        public static void Server_ClientConnect(ulong client)
        {
            ConfigurableCompanyPlugin.Debug($"NetEventRouter > Server :: ClientConnect | id: {client} |");
            if (NetSynchronizer.IsServer)
                NetController.SendAllConfigs(client);
        }

        public static void Server_ClientDisconnect(ulong obj)
        {
            ConfigurableCompanyPlugin.Debug($"NetEventRouter > Server :: ClientDisconnect | id: {obj} |");
        }

        public static void Server_Stop(bool obj)
        {
            ConfigurableCompanyPlugin.Debug($"NetEventRouter > Server :: Stop | ???: {obj} |");
        }

        public static void Client_Disconnect(bool obj)
        {
            ConfigurableCompanyPlugin.Debug($"NetEventRouter > Client :: Disconnect | ???: {obj} |");
        }

        public static void Client_Connect()
        {
            ConfigurableCompanyPlugin.Debug($"NetEventRouter > Client :: Connect");
            NetReceiveRouter.RegisterClientMessages();
        }

    }
}

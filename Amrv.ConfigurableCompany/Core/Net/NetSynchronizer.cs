using System;
using Unity.Netcode;

namespace Amrv.ConfigurableCompany.Core.Net
{
    public static class NetSynchronizer
    {
        public static bool IsServer => NetworkManager.Singleton?.IsServer ?? false;
        public static bool IsClient => NetworkManager.Singleton?.IsClient ?? false;
        public static bool IsHost => NetworkManager.Singleton?.IsHost ?? false;

        public static class StateCallbacks
        {
            public static event Action<NetworkManager> Start;
            public static event Action Stop;

            internal static void TriggerStart(NetworkManager instance) => Start?.Invoke(instance);
            internal static void TriggerStop() => Stop?.Invoke();
        }

        public static class ServerCallbacks
        {
            public static event Action<ulong> ClientConnect;
            public static event Action<ulong> ClientDisconnect;
            public static event Action Start;
            public static event Action<bool> Stop;

            internal static void TriggerClientConnect(ulong client) => ClientConnect?.Invoke(client);
            internal static void TriggerClientDisconnect(ulong client) => ClientDisconnect?.Invoke(client);
            internal static void TriggerStart() => Start?.Invoke();
            internal static void TriggerStop(bool forced) => Stop?.Invoke(forced);
        }

        public static class ClientCallbacks
        {
            public static event Action Connect;
            public static event Action<bool> Disconnect;

            internal static void TriggerConnect() => Connect?.Invoke();
            internal static void TriggerDisconnect(bool forced) => Disconnect?.Invoke(forced);
        }

        public static class Messaging
        {
            public static void Send(string identifier, FastBufferWriter? writer = null, params ulong[] clients) =>
                Send(identifier, writer, NetworkDelivery.ReliableSequenced, clients);
            public static void Send(string identifier, FastBufferWriter? writer = null, NetworkDelivery delivery = NetworkDelivery.ReliableSequenced, params ulong[] clients)
            {
                if (NetworkManager.Singleton == null)
                    return;

                FastBufferWriter stream = writer ?? new FastBufferWriter();

                if (clients == null || clients.Length == 0)
                {
                    NetworkManager.Singleton.CustomMessagingManager.SendNamedMessageToAll(identifier, stream, delivery);
                }
                else
                {
                    NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(identifier, clients, stream, delivery);
                }
            }

            public static bool Register(string identifier, CustomMessagingManager.HandleNamedMessageDelegate handler)
            {
                if (NetworkManager.Singleton == null)
                    return false;
                NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler(identifier, handler);
                return true;
            }

            public static bool Unregister(string identifier)
            {
                if (NetworkManager.Singleton == null)
                    return false;
                NetworkManager.Singleton.CustomMessagingManager?.UnregisterNamedMessageHandler(identifier);
                return true;
            }

            public static FastBufferWriter GetWriter(int size = 0) => new(size, Unity.Collections.Allocator.Temp);
            public static int GetSize(string text, bool oneByteChars = false) => FastBufferWriter.GetWriteSize(text, oneByteChars);
            public static int GetSize<T>(T item) where T : unmanaged => FastBufferWriter.GetWriteSize(in item);
            public static int GetSize<T>(params T[] items) where T : unmanaged => FastBufferWriter.GetWriteSize(items);
        }

        internal static void Create(NetworkManager instance)
        {
            NetworkManager.Singleton.OnClientStarted += ClientCallbacks.TriggerConnect;
            NetworkManager.Singleton.OnClientStopped += ClientCallbacks.TriggerDisconnect;

            NetworkManager.Singleton.OnClientConnectedCallback += ServerCallbacks.TriggerClientConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += ServerCallbacks.TriggerClientDisconnect;
            NetworkManager.Singleton.OnServerStarted += ServerCallbacks.TriggerStart;
            NetworkManager.Singleton.OnServerStopped += ServerCallbacks.TriggerStop;

            StateCallbacks.TriggerStart(instance);
        }

        internal static void Destroy()
        {
            StateCallbacks.TriggerStop();
        }
    }
}

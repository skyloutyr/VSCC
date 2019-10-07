using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VSCC.State;

namespace VSCC.Roll20
{
    class R20WSServer
    {
        private static int _port = 23521;

        private static WebSocketServer _server;
        private static IWebSocketConnection _connection;

        public static Action ServerStartCallback { get; set; }
        public static Action ServerStopCallback { get; set; }
        public static Action ClientConnectCallback { get; set; }
        public static Action ClientDisconnectCallback { get; set; }
        public static Action<string> Logger { get; set; }

        public static void Roll(string r1, string r2, string type, string action)
        {
            Send(new RollPacket()
            {
                Template = Template.Simple,
                Data = new TemplateDataSimple()
                {
                    R1 = $"[[{ r1 }]]",
                    R2 = r2 == null ? r2 : $"[[{ r2 }]]",
                    CharName = AppState.Current.State.General.Name,
                    Mod = action,
                    Name = type
                }
            });
        }

        public static void Send(object packet)
        {
            try
            {
                _connection?.Send(JsonConvert.SerializeObject(packet, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));
            }
            catch (Exception e)
            {
                Logger?.Invoke($"Could not send data - { e.GetType().FullName }, { e.Message }");
            }
        }

        public static void CloseServer()
        {
            _server.ListenerSocket.Close();
            _server.Dispose();
            _server = null;
            ServerStopCallback?.Invoke();
        }

        public static void CreateServer()
        {
            if (_server != null)
            {
                Logger?.Invoke("A server is already running.");
                return;
            }

            _server = new WebSocketServer("ws://0.0.0.0:" + _port.ToString())
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.None
            };

            Logger?.Invoke("Starting server...");
            new Thread(ServerEntryPoint)
            {
                IsBackground = true
            }.Start();
        }

        public static void ServerEntryPoint()
        {
            ServerStartCallback?.Invoke();
            _server.Start(ws =>
            {
                ws.OnOpen = () =>
                {
                    Logger?.Invoke("TCP client connected.");
                    ClientConnectCallback?.Invoke();
                    _connection = ws;
                };

                ws.OnClose = () =>
                {
                    Logger?.Invoke("TCP client disconnected.");
                    ClientDisconnectCallback?.Invoke();
                    _connection = null;
                };

                ws.OnMessage = s =>
                {
                    Logger?.Invoke("Client says: " + s);
                };
            });
        }
    }
}

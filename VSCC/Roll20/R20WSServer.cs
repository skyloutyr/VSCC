namespace VSCC.Roll20
{
    using Fleck;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Threading;
    using VSCC.State;

    class R20WSServer
    {
        private static readonly int _port = 23521;

        private static WebSocketServer _server;
        private static IWebSocketConnection _connection;
        private static bool _awaitingPoll;
        private static int _pollResult;
        private static readonly EventWaitHandle _wh = new EventWaitHandle(false, EventResetMode.AutoReset);

        public static Action ServerStartCallback { get; set; }
        public static Action ServerStopCallback { get; set; }
        public static Action ClientConnectCallback { get; set; }
        public static Action ClientDisconnectCallback { get; set; }
        public static bool Connected { get; set; }

        public static void Roll(string r1, string r2, string type, string action, bool gm)
        {
            Send(new CommandPacket()
            {
                Template = Template.Simple,
                GMRoll = gm,
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

        public static void Send(object packet) => SendRaw(JsonConvert.SerializeObject(packet, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));

        public static void SendRaw(string packet)
        {
            try
            {
                _connection?.Send(packet);
            }
            catch (Exception e)
            {
                R20Logger.WriteLine($"Could not send data - { e.GetType().FullName }, { e.Message }");
            }
        }

        public static void CloseServer()
        {
            _server.ListenerSocket.Close();
            _server.Dispose();
            _server = null;
            ServerStopCallback?.Invoke();
            Connected = false;
        }

        public static void CreateServer()
        {
            if (!R20Logger.Exists)
            {
                R20Logger.Init();
            }

            if (_server != null)
            {
                R20Logger.WriteLine("A server is already running.");
                return;
            }

            _server = new WebSocketServer("ws://0.0.0.0:" + _port.ToString())
            {
                EnabledSslProtocols = System.Security.Authentication.SslProtocols.None
            };

            R20Logger.WriteLine("Starting server...");
            new Thread(ServerEntryPoint)
            {
                IsBackground = true
            }.Start();
        }

        public static int? AwaitCallback()
        {
            _awaitingPoll = true;
            bool b = _wh.WaitOne(30000);
            return b ? _pollResult : (int?)null;
        }

        public static void ServerEntryPoint()
        {
            ServerStartCallback?.Invoke();
            _server.Start(ws =>
            {
                ws.OnOpen = () =>
                {
                    R20Logger.WriteLine("TCP client connected.");
                    ClientConnectCallback?.Invoke();
                    _connection = ws;
                    Connected = true;
                };

                ws.OnClose = () =>
                {
                    R20Logger.WriteLine("TCP client disconnected.");
                    ClientDisconnectCallback?.Invoke();
                    _connection = null;
                    Connected = false;
                    R20Logger.Close();
                };

                ws.OnMessage = s =>
                {
                    try
                    {
                        JObject jo = JObject.Parse(s);
                        if (jo.ContainsKey("type"))
                        {
                            if (string.Equals("error", jo["type"].Value<string>()))
                            {
                                R20Logger.WriteLine($"[Error] The cliend sends an error code { jo["data"]["code"].Value<string>() }");
                                R20Logger.WriteLine($"[Error] { jo["data"]["message"].Value<string>() }");
                            }

                            if (string.Equals("polled", jo["type"].Value<string>()))
                            {
                                if (_awaitingPoll && int.TryParse(jo["data"]["value"].Value<string>(), out int i))
                                {
                                    _pollResult = i;
                                    _wh.Set();
                                }
                            }
                        }
                        else
                        {
                            R20Logger.WriteLine($"[Warning] Client sent a JSON object without specifying the type!");
                            R20Logger.WriteLine($"[Warning] The JSON was: { s }");
                        }
                    }
                    catch (JsonException) // Not json?
                    {
                        R20Logger.WriteLine($"[Fine] Client sends a message: { s }");
                    }
                };
            });
        }
    }
}

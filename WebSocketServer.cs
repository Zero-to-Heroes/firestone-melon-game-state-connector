using Fleck;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEventsConnector
{
    class WebSocketServerPlugin
    {
        private ConcurrentDictionary<Guid, IWebSocketConnection> sockets = new ConcurrentDictionary<Guid, IWebSocketConnection>();

        internal string OpenServer(int port)
        {
            var server = new WebSocketServer($"ws://127.0.0.1:{port}/firestone-mods");
            GameEventsConnectorMod.SharedLogger.Msg($"Websocket server element created ");
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    sockets.TryAdd(socket.ConnectionInfo.Id, socket);
                    GameEventsConnectorMod.SharedLogger.Msg($"Client connected {socket.ConnectionInfo.Id}");
                };
                socket.OnClose = () => 
                {
                    sockets.TryRemove(socket.ConnectionInfo.Id, out IWebSocketConnection removedSocket);
                    GameEventsConnectorMod.SharedLogger.Msg($"Client disconnected {socket.ConnectionInfo.Id}");
                };
                socket.OnMessage = message =>
                {
                    //GameEventsConnectorMod.SharedLogger.Msg($"Socket message");
                    // TODO: Build a data model and broadcast the updated state

                };
                GameEventsConnectorMod.SharedLogger.Msg($"Websocket server started");
            });
            return server.Location;
        }

        public void Broadcast(string message)
        {
            foreach (var socket in sockets)
            {
                if (socket.Value.IsAvailable)
                {
                    socket.Value.Send(message);
                }
            }
        }
    }
}

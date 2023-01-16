using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System;

namespace GameEventsConnector
{
    public class GameEventsConnectorMod : MelonMod
    {
        public static MelonLogger.Instance SharedLogger;

        public override void OnInitializeMelon()
        {
            GameEventsConnectorMod.SharedLogger = LoggerInstance;
            GameEventsConnectorMod.SharedLogger.Msg($"Creating websocket");
            WebSocketServerPlugin wsServer = new WebSocketServerPlugin();
            int port = 9977;
            var wsLocation = wsServer.OpenServer(9977);
            GameEventsConnectorMod.SharedLogger.Msg($"Websocket server created, listening on port {port} at location {wsLocation}");
        }
    }
}

using Riptide;
using Riptide.Utils;
using System;
using UnityEngine;
using System.Text;
using System.IO.Compression;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Diploma.Factory
{
    public enum MessageId : ushort
    {
        UPDATE_ROBOT = 2,

    }
    /// <summary>
    /// Riptide üzenetkezelő szkript. Ez kommunikál az adat szerverrel.
    /// </summary>
    public class RiptideServiceFactory : MonoBehaviour
    {
        public enum State {START, UPDATE, STOP}
        public static State state;
        private static RiptideServiceFactory _singleton;
        public static RiptideServiceFactory Singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(RiptideServiceFactory)} instance already exists, destroying object!");
                    Destroy(value);
                }
            }
        }

        public Client Client { get; private set; }

        private void Awake()
        {
            Singleton = this;
        }

        private void Start()
        {
            var ip = PlayerPrefs.GetString(Settings.PREF_DATASERVER_IP);
            var port = PlayerPrefs.GetInt(Settings.PREF_DATASERVER_PORT);
            state = State.START;
            if(Application.isEditor){
                ip = "127.0.0.1";
            }
            Message.MaxPayloadSize = 5*1024*1024;
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

            Client = new Client();
            Client.Connected += Connected;

            Client.Connect($"{ip}:{port}");
        }

        private void FixedUpdate()
        {
            Client.Update();

        }

        //TODO ez a jelent elhagyásakor meghívódik??
        private void OnDestroy()
        {
            Client.Disconnect();
            Client.Connected -= Connected;
        }

        private void Connected(object sender, EventArgs e)
        {
            state = State.UPDATE;
            print("Kliens azonosító: " +Client.Id);
        }

        private void Disconnect(){
            print("Kliens lecsatlakozott!");
        }


        #region Message

        [MessageHandler((ushort)MessageId.UPDATE_ROBOT)]
        private static void UpdateRobot(Message message)
        {
            if(state == State.UPDATE){
                state = State.STOP;
                Factory_Program.robots.Clear();
                string mRobot = DeCompress(message.GetBytes());
                Factory_Program.robots = JsonConvert.DeserializeObject<List<Robot>>(mRobot);
            }
        }

        #endregion

        private static string DeCompress(byte[] data)
        {
            print("üzenet kicsomagolása");
            var target = new byte[data.Length];
            BrotliDecoder.TryDecompress(data, target, out var decodedBytes);
            print($"kicsomagolt méret: {decodedBytes} byte");
            return Encoding.Unicode.GetString(target);
        }
    }
}
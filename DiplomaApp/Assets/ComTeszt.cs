using Riptide;
using Riptide.Utils;
using System;
using UnityEngine;
using System.Text;
using System.IO.Compression;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace TestNamespace{
    public enum MessageId : ushort
    {
        UPDATE = 1,
        POST_PLC_DATA,
    }

    public class ComTeszt : MonoBehaviour
{

        [SerializeField] private GameObject plcHmi;
        [SerializeField] private GameObject input, output;

        private static GameObject s_plcHmi, s_input, s_output;

        public static List<Plc> plcs;
        private static List<Robot> robots;

        private static ComTeszt _singleton;
        public static ComTeszt Singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(ComTeszt)} instance already exists, destroying object!");
                    Destroy(value);
                }
            }
        }

        public Client Client { get; private set; }

        private void Awake()
        {
            Singleton = this;
            s_plcHmi = plcHmi;
            s_input = input;
            s_output = output;
        }
        private static bool added;
        private void Start()
        {
            plcs = new List<Plc>();
            robots = new List<Robot>();
            added = true;
            var ip = "127.0.0.1";
            var port = 7777;
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
        
        private static  void AddControls()
        {
            print(added);
            if(added)
           {    var hmi = Instantiate(s_plcHmi, Vector3.zero, Quaternion.identity);
                Vector3 buttonStartPos = new Vector3(-0.0292f,0.0042f,0.0273f);
                Vector3 lightStartPos = new Vector3(0.0081f,0.0042f,0.0273f);

                int iOffset=0;
                int oOffset=0;
                var ios = plcs.Select(p => p.IOs).First();
                foreach(var io in ios)
                {
                    
                    if(io.Direction == 0) // 0: bemenet
                    {
                        var offset = new Vector3(0f ,0f , .011f) * iOffset++;
                        var inputInstance = Instantiate(s_input, buttonStartPos - offset, Quaternion.identity, hmi.transform);
                        inputInstance.name += "_" + io.Id;
                        print("bemenet : " + inputInstance.name + "\n");
                    }
                    else{   //1: kimenet
                        var offset = new Vector3(0f ,0f , .011f) * oOffset++;
                        var outputInstance = Instantiate(s_output, lightStartPos - offset, Quaternion.identity, hmi.transform);
                        outputInstance.name += "_" + io.Id;
                        print("kimenet: " + outputInstance.name+ "\n");
                    }
                }
                added = false;
            }}
        private void OnDestroy()
        {
            Client.Disconnect();
            Client.Connected -= Connected;
        }

        private void Connected(object sender, EventArgs e)
        {
            print("Kliens azonosító: " +Client.Id);
        }

        private void Disconnect(){
            print("Kliens lecsatlakozott!");
        }

        // #region Message
        // [MessageHandler((ushort)MessageId.UPDATE)]
        // private static void HandleGetData(Message message)
        // {
        //     plcs.Clear();
        //     robots.Clear();
        //     string mPlc = DeCompress(message.GetBytes());
        //     plcs = JsonConvert.DeserializeObject<List<Plc>>(mPlc);

        //     string mRobot = DeCompress(message.GetBytes());
        //     robots = JsonConvert.DeserializeObject<List<Robot>>(mRobot);

        //     // print($"szerver plc adat \n{mPlc}");

        //     AddControls();
        // }

        // //TODO ez nem feltétlenül kell > ha itt kivszem a kko r aszerver s küldjön választ erre az üzenet elfogóra
        // [MessageHandler((ushort)MessageId.POST_PLC_DATA)]
        // private static void HandlePost(Message message)
        // {
        //     print($"szerver post_plc_data válasz: \n{message.GetString()}");
        // }

        // #endregion

        private static string DeCompress(byte[] data)
        {
            // print("üzenet kicsomagolása");
            var target = new byte[data.Length];
            BrotliDecoder.TryDecompress(data, target, out var decodedBytes);
            // print($"kicsomagolt méret: {decodedBytes} byte");
            return Encoding.Unicode.GetString(target);
        }
    }
}



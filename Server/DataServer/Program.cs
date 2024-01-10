using ABB.Robotics.Controllers.Discovery;
//using ABB.Robotics.Controllers.MotionDomain;
//using ABB.Robotics.Controllers.RapidDomain;
using ABB.Robotics.Controllers;
using DataServer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Riptide.Utils;
using Riptide;
using Sharp7;
using System.IO.Compression;
using System.Text;
using RobTask = ABB.Robotics.Controllers.RapidDomain.Task;


namespace DataServer
{
    public enum MessageId : ushort
    {

        UPDATE_PLC = 1,
        UPDATE_ROBOT,
        POST_PLC_DATA,
    }

    public class Program
    {
        private static DiplomaContext? _context;
        private static Server? riptideServer;
        public static bool isRunning;
        public static bool hasClient;
        public static bool dataFetched; //TODO ez minek kell, és sehol sem váltakozik, csak egyzser ture utána az marad
        public static ushort request;
        public static List<Plc> plcs { get; set; } = default!;
        public static List<Robot> robots { get; set; } = default!;

        public static void Main()
        {
            request = 0;
            hasClient = false;
            dataFetched = false;
            Console.Title = "Data Server";

            _context = new DiplomaContext();
            RiptideLogger.Initialize(Console.WriteLine, true);
            isRunning = true;

            new Thread(new ThreadStart(RiptideLoop)).Start();
            new Thread(new ThreadStart(Update)).Start();
            Console.ReadLine();

            isRunning = false;

            Console.ReadLine();
        }

        private static void RiptideLoop()
        {
            //Riptide server
            Message.MaxPayloadSize = 5*1024*1024;
            riptideServer = new Server
            {
                TimeoutTime = ushort.MaxValue // Max value timeout to avoid getting timed out for as long as possible when testing with very high loss rates (if all heartbeat messages are lost during this period of time, it will trigger a disconnection)

            };
            riptideServer.ClientConnected += ClientConnected!;
            riptideServer.ClientDisconnected += ClientDisconnected!;
            riptideServer.Start(7777, 10);

            while (isRunning)
            {
                riptideServer.Update();
                Thread.Sleep(10);
            }

            riptideServer.Stop();
        }
        #region events
        private static void ClientConnected(object s, ServerConnectedEventArgs e)
        {
            Console.WriteLine($"Client connected with ID: {e.Client.Id}");
            hasClient = true;
        }
        private static void ClientDisconnected(object s, ServerDisconnectedEventArgs e)
        {
            hasClient = false;
            Console.WriteLine($"Client disconnected with ID: {e.Client.Id}, reason: \n {e.Reason}");
        }
        #endregion

        #region Message handlers

        [MessageHandler((ushort)MessageId.POST_PLC_DATA)]
        private static void PostPlcData(ushort fromClientId, Message message)
        {
            int plcId = message.GetInt();
            int ioId = message.GetInt();
            bool ioValue = message.GetBool();
            ModifyIOValue(plcId, ioId, ioValue);
            Console.WriteLine($"{DateTime.Now} -  plc id: {plcId} |input id : {ioId} | érték: {ioValue}");
        }

         #endregion

        #region Frissítés
        private static void Update()
        {

            while (isRunning)
            {
                if (hasClient)
                {
                    string jsonPLCs = UpdatePlc();
                    Thread.Sleep(10);
                    //string jsonPLCs = @"[
                    //      {
                    //        ""Id"": 2,
                    //        ""Address"": ""192.168.100.100"",
                    //        ""Type"": ""S7-1200"",
                    //        ""DbNumber"": 1,
                    //        ""Name"": ""PLC_1"",
                    //        ""IOs"": [
                    //          {
                    //                                ""Id"": 2,
                    //            ""Name"": ""lámpa relé"",
                    //            ""Offset"": 0,
                    //            ""Bit"": 0,
                    //            ""Direction"": 1,
                    //            ""Value"": 0
                    //          },
                    //          {
                    //                                ""Id"": 3,
                    //            ""Name"": ""lámpa gomb"",
                    //            ""Offset"": 1,
                    //            ""Bit"": 0,
                    //            ""Direction"": 0,
                    //            ""Value"": 0
                    //          }
                    //        ]
                    //      }
                    //    ]";
                    string jsonRobots = UpdateRobot();
                    Thread.Sleep(10);

                    //Console.WriteLine(jsonPLCs);
                    //Console.WriteLine(jsonRobots);

                    riptideServer?.SendToAll(Message.Create(MessageSendMode.Reliable, MessageId.UPDATE_PLC).AddBytes(Compress(jsonPLCs)));
                    riptideServer?.SendToAll(Message.Create(MessageSendMode.Reliable, MessageId.UPDATE_ROBOT).AddBytes(Compress(jsonRobots)));
                    dataFetched = true;
                }
                Thread.Sleep(2000);
            }
        }

        private static string UpdatePlc()
        {
            //TODO ha plcs és robots lista elérhető a program többi részén és akkor nem kell POST üzenetben elérni az adatbázis így nem kell 2 DBContext és nincs konkures lekérés
            //Console.WriteLine(DateTime.Now.ToString());
            string jsonResult = "{}";
            plcs = _context!.Plcs.Include(i => i.IoPorts).Include(i => i.Type).ToList();
            foreach (Plc plc in plcs)
            {
                var s7client = new S7Client();
                int connectState = s7client.ConnectTo(plc.Address, 0, 1);
                if (connectState == 0)
                {
                    //Console.WriteLine("PLC adatok lekérése");
                    int bufferSize = 2;
                    byte[] dbBuffer = new byte[bufferSize];

                    int result = s7client.DBRead(plc.DbNumber, 0, bufferSize, dbBuffer);
                    if (result != 0)
                    {
                        Console.WriteLine("Error: " + s7client.ErrorText(result));
                    }
                    foreach (var ioPort in plc.IoPorts)
                    {
                        ioPort.Value = Convert.ToInt32(S7.GetBitAt(dbBuffer, ioPort.Offset, ioPort.Bit));
                        _context.IoPorts.Update(ioPort);
                        _context.SaveChanges();
                    }

                }
                else
                {
                    Console.WriteLine(s7client.ErrorText(connectState));
                }
                //Bontja a kapcsolatot a PLC-vel
                s7client.Disconnect();
            }

            var mappedPlcs = from plc in plcs
                             select new
                             {
                                 plc.Id,
                                 plc.Address,
                                 Type = plc.Type.Name,
                                 plc.DbNumber,
                                 plc.Name,
                                 IOs = from io in plc.IoPorts
                                       select new
                                       {
                                           io.Id,
                                           io.Name,
                                           io.Offset,
                                           io.Bit,
                                           io.Direction,
                                           io.Value
                                       }
                             };
            jsonResult = JsonConvert.SerializeObject(mappedPlcs, new JsonSerializerSettings { Formatting = Formatting.Indented, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });  
            return jsonResult;
        }

        private static string UpdateRobot()
        {   

            robots = _context?.Robots
            .Include(r => r.Factory)
            .Include(r => r.Type)
            .ToList()!;

            //Console.WriteLine(DateTime.Now.ToString());
            //Console.WriteLine("Robotok lekérése");
            NetworkScanner scanner = new NetworkScanner();
            ControllerInfo[] controllerInfos;   //TODO ha kell akkor list-ként kezelni inkább
            string jsonResult = "{ }";
            for (int i = 0; i <= 1; i++)
            {
                if (i == 0)
                {
                    controllerInfos = scanner.GetControllers(NetworkScannerSearchCriterias.Real);
                }
                else
                {

                    controllerInfos = scanner.GetControllers(NetworkScannerSearchCriterias.Virtual);
                }

                if (controllerInfos.Length > 0)
                {
                    foreach (var ctrlInfo in controllerInfos)
                    {
                        foreach(var r in robots)
                        {
                            if (ctrlInfo.SystemId.Equals(Guid.Parse(r.Guid)))
                            {
                                r.Address = ctrlInfo.IPAddress.ToString();
                                r.Name = ctrlInfo.Name;
                                r.SystemName = ctrlInfo.SystemName;
                                r.HostName = ctrlInfo.HostName;
                                r.Version = ctrlInfo.Version.ToString();
                                r.Virtual = (sbyte)i;
                                //vezérlőbe való belépés:
                                Controller controller = Controller.Connect(ctrlInfo, ConnectionType.Standalone);
                                //Console.WriteLine(controller.OperatingMode);
                                RobTask[] tasks = controller.Rapid.GetTasks();
                                r.RobotTasks.Clear();
                                foreach (RobTask t in tasks)
                                {
                                    Console.WriteLine(t.GetRobTarget());
                                    r.RobotTasks.Add(
                                        new Robot.RobotTask
                                        {
                                            Name = t.Name,
                                            Position =
                                                new Robot.RobotTask.Vector3
                                                {
                                                   X = t.GetRobTarget().Trans.X,
                                                   Y = t.GetRobTarget().Trans.Y,
                                                   Z =  t.GetRobTarget().Trans.Z
                                                }
                 
                                        });
                                }
                                _context!.Update(r);
                                _context!.SaveChanges();
                            }
                        }
                    }
                }
            }

            var mappedRobots = from robot in robots
                               select new
                               {
                                   robot.Guid,
                                   robot.Address,
                                   robot.Alias,
                                   robot.Name,
                                   robot.SystemName,
                                   robot.HostName,
                                   robot.Version,
                                   robot.Xpos,
                                   robot.Ypos,
                                   robot.ZOrinetation,
                                   robot.Virtual,
                                    FactoryName = robot.Factory.Name,
                                    FactoryWidth = robot.Factory.Width,
                                    FactoryLength = robot.Factory.Length,
                                    Type = robot.Type.Name,
                                    robot.RobotTasks
                                };

            jsonResult = JsonConvert.SerializeObject(mappedRobots, new JsonSerializerSettings {Formatting = Formatting.Indented});
            return jsonResult;

        }
        #endregion

        private static void ModifyIOValue(int plcId, int ioId, bool value)
        {
            if (dataFetched)
            {
                var s7client = new S7Client();
                var plc = plcs.Where(p => p.Id == plcId).FirstOrDefault();
                int connectState = s7client.ConnectTo(plc!.Address, 0, 1);
                if (connectState == 0)
                {
                    Console.WriteLine("Send data to PLC");
                    //int bufferSize = plc.IoPorts.Count();
                    int bufferSize = 2;
                    byte[] dbBuffer = new byte[bufferSize];

                    int result = s7client.DBRead(plc.DbNumber, 0, bufferSize, dbBuffer);
                    if (result != 0)
                    {
                        Console.WriteLine("Error: " + s7client.ErrorText(result));
                    }

                    var io = plc.IoPorts.Where(i => i.Id == ioId).FirstOrDefault()!;
                    Console.WriteLine($"SET -> offset: {io.Offset}, bit: {io.Bit},  value: {value}");
                    dbBuffer.SetBitAt(io.Offset, io.Bit, value);
                    result = s7client.DBWrite(plc.DbNumber, 0, bufferSize, dbBuffer);
                    if (result != 0)
                    {
                        Console.WriteLine("Error: " + s7client.ErrorText(result));
                    }
                }
                else
                {
                    Console.WriteLine(s7client.ErrorText(connectState));
                }
                // Disconnect the client
                s7client.Disconnect();
            }
        }
        private static byte[] Compress(string data)
        {
            var source = Encoding.Unicode.GetBytes(data);
            var memory = new byte[source.Length];
            BrotliEncoder.TryCompress(source, memory, out var encodedBytes);
            //Console.WriteLine($"eredeti méret {source.Length} > tömörített méret: {encodedBytes}");
            return memory;
        }

        //TODO ez menjen át a webszerverbe
        //private static void GenerateQR()
        //{
        //    string dir  = "qrcodes";
        //    if (!Directory.Exists(dir))
        //    {
        //        Directory.CreateDirectory(dir);
        //    }

        //    using var qrGenerator = new QRCodeGenerator();
        //    var plcIds = _context!.Plcs.Select(x => x.Id).ToArray();
        //    foreach (var id in plcIds)
        //    {
        //        QRCodeData qrCodeData = qrGenerator.CreateQrCode(id.ToString(), QRCodeGenerator.ECCLevel.Q);
        //        BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
        //        byte[] qrCodeAsBitmapByteArr = qrCode.GetGraphic(20);
                
        //        Bitmap qrCodeImage;
        //        using (var ms = new MemoryStream(qrCodeAsBitmapByteArr))
        //        {
        //            qrCodeImage = new Bitmap(ms);
        //            qrCodeImage?.Save(@$"{dir}\{id}.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
        //        }
                
        //    }
           
        //}
    }
}






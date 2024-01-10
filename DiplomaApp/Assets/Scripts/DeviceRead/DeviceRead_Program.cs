using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.ARFoundation;
using TMPro;

namespace Diploma.Device
{
    public enum State{WAIT,  QR_DOWNLOAD,ADD_CONTORLS ,UPDATE_CONTROLPANEL, LOAD_MARKER, MARKER_LOADED, INIT, ERROR}

    // [RequireComponent(typeof(ARTrackedImageManager))]
    public class DeviceRead_Program : MonoBehaviour
    {
        [SerializeField] private GameObject panelError;
        [SerializeField] private GameObject ARCamera;
        [SerializeField] private GameObject input, output;

        [SerializeField] private GameObject plcControlPanel;
        // private GameObject plcControlPanel;
        public static State state;
        public static int qrPlcId;  //beazonosított PLC ID-ja QR kódból
        public static List<Plc> plcs;   //RiptdeService frissíti
        public static string error;
        public static string logging;
        public static RefImageLoader.ImageData imageData;

        private List<GameObject> inputs, outputs;
      
        void Start()
        {
            state = State.INIT;
        }

        // Update is called once per frame
        void Update()
        {
            switch(state)
            {
                case State.WAIT: break;
                case State.INIT: Init(); break;
                case State.QR_DOWNLOAD: QRDownload(); break;
                case State.LOAD_MARKER: LoadMarker(); break;
                // case ProgramState.MARKER_LOADED: MarkerLoaded(); break;
                case State.ADD_CONTORLS: AddContorlPanelElements(); break;
                case State.UPDATE_CONTROLPANEL: UpdateControlPanel(); break;
                case State.ERROR: Error(); break;
                default: break;
            }
            print(logging);
        }

        #region Program states
        private void Init(){
            panelError.SetActive(false);
            qrPlcId = PlayerPrefs.GetInt(Settings.PREF_QRCODE_VALUE);
            print("PLC id in _PROGram: " + qrPlcId);
            plcs = new List<Plc>();
            
            inputs = new List<GameObject>();
            outputs = new List<GameObject>();
            state = State.QR_DOWNLOAD;
        }

        private void QRDownload()
        {
            var qrdl = GetComponent<QRDowloader>();
            qrdl.enabled = true;
            state = State.WAIT;
        }

        private void LoadMarker()
        {
            var ril = ARCamera.GetComponent<RefImageLoader>();
            ril.enabled = true;
            ril.m_Image = imageData;
            ril.m_State = ImageLoaderState.AddImagesRequested;
            logging = "LoadMarker: " + imageData.name;
            state = State.WAIT;
        }

        private void AddContorlPanelElements()
        {

            if(RiptideServiceDevice.state == RiptideServiceDevice.State.STOP){
                state = State.WAIT;
                inputs.Clear();
                outputs.Clear();
                // var plcControlPanel = ARCamera.GetComponent<ARTrackedImageManager>().trackedImagePrefab;
                Vector3 buttonStartPos = new Vector3(-0.0292f,0.0042f,0.0273f);
                Vector3 lightStartPos = new Vector3(0.0081f,0.0042f,0.0273f);
                logging = "AddControl\n";
                var plcControlPanelInstance = Instantiate(plcControlPanel, Vector3.zero, Quaternion.identity);
                int iOffset=0;
                int oOffset=0;
                var ios = plcs.Select(p => p.IOs).First();
                // logging = "io-k száma"+ios.Count;
                foreach(var io in ios)
                {
                    if(io.Direction == 0) // 0: bemenet
                    {
                        var offset = new Vector3(0f ,0f , .011f) * iOffset++;
                        var inputInstance = Instantiate(input, buttonStartPos - offset, Quaternion.identity, plcControlPanelInstance.transform);
                        inputInstance.name += "_" + io.Id;
                        inputs.Add(inputInstance);
                    }
                    else{   //1: kimenet
                        var offset = new Vector3(0f ,0f , .011f) * oOffset++;
                        var outputInstance = Instantiate(output, lightStartPos - offset, Quaternion.identity, plcControlPanelInstance.transform);
                        outputInstance.name += "_" + io.Id;
                        outputs.Add(outputInstance);
                    }
                }
                var artim = ARCamera.GetComponent<ARTrackedImageManager>();
                artim.trackedImagePrefab = plcControlPanelInstance;
                artim.enabled = true;
                RiptideServiceDevice.state = RiptideServiceDevice.State.UPDATE;
                state = State.LOAD_MARKER;
                logging = "AddControl vége";
            }
        }

        private void UpdateControlPanel()
        {
            if(RiptideServiceDevice.state == RiptideServiceDevice.State.STOP)
            {
                logging = "";
                var plcStatusText = ARCamera.GetComponent<ARTrackedImageManager>().GetComponentInChildren<TextMeshPro>();
                var status = (from plc in plcs where plc.Id == qrPlcId select plc.Name + "\n"+ plc.Address).FirstOrDefault();
                plcStatusText.text = status;

                var controlPanelGO = ARCamera.GetComponent<ARTrackedImageManager>();

                var ios = plcs.Select(p => p.IOs).First();
                // logging += "\n ios count: " + ios.Count;
                var texts = controlPanelGO.GetComponentsInChildren<TextMeshPro>();
                //0. elem a PLC neve
                for(int i=1; i < texts.Length; i++)
                {
                     texts[i].text = ios[i-1].Name;
                    //  logging += $"\n input name: {ios[i].Name}, value: {ios[i].Value}" ;
                }

                var os = ios.Where(io => io.Direction == 1).ToArray();
                // var lights = ARCamera.GetComponent<ARTrackedImageManager>().trackedImagePrefab.GetComponentsInChildren<Light>();  //ez nem jó?
                var lights = ARCamera.GetComponent<ARTrackedImageManager>().GetComponentsInChildren<Light>();   //ez miért jó? :D
                // var lights = controlPanelGO.GetComponentsInChildren<Light>();   //ez miért jó? :D
                // logging += "\n lights count:" +  lights.Length;
                for(int i=0; i < lights.Length; i++)
                {
                    lights[i].intensity = os[i].Value * 15;
                    logging += $"\n output name: {os[i].Name}, value: {os[i].Value}" ;
                }
                RiptideServiceDevice.state = RiptideServiceDevice.State.UPDATE;
            }

        }

        private void Error()
        {
            //TODO hiba panel mutatása a hiba szövegével
            panelError.SetActive(true);
            panelError.GetComponentInChildren<TextMeshPro>().text = error;
            state = State.WAIT;
        }
        #endregion

    //Debug-hoz
        private void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rectLog = new Rect(20, 20, w, h *2 /100);
            Rect rectError = new Rect(20, 200, w, h *2 /100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            if(state == State.ERROR)
            {
                style.normal.textColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                GUI.Label(rectError, error + "\n", style);
            }
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            GUI.Label(rectLog, logging + "\n", style);
        }
    }
}


using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Diploma.Device
{
    public class QRDowloader : MonoBehaviour
    // public class QRDowloader
    {
        void Start(){
            StartCoroutine(GetImmage(DeviceRead_Program.qrPlcId.ToString()));
        }
        public IEnumerator GetImmage(string pclid) 
        {
            print("PLC id in GetIMAGE(): " + pclid);
            var ngrok = PlayerPrefs.GetString(Settings.PREF_WEBSERVER_URL);
            UnityWebRequest www = UnityWebRequest.Get($"{ngrok}/api/FileApi/?FileName={pclid}.jpg&handler=DownloadQR");
            yield return www.SendWebRequest();

            DeviceRead_Program.logging = "kép url feldolgozva\n";
            if(www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
                DeviceRead_Program.error = www.error;
                DeviceRead_Program.state = State.ERROR;
                yield break;
            }
            else 
            {
                byte[] results = www.downloadHandler.data;
                Debug.Log("adat mérete:" + results.Length/1024 + "kB");
                DeviceRead_Program.logging = "kép letöltés befejezve\n";
                Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
                tex.LoadImage(results);
                // imageData.texture = tex;
                // imageData.name = pclid;
                // imageData.width = 0.035f;
                DeviceRead_Program.imageData = new RefImageLoader.ImageData();
                DeviceRead_Program.imageData.texture = tex;
                DeviceRead_Program.imageData.name = pclid;
                DeviceRead_Program.imageData.width = 0.035f;
                // DeviceRead_Program.programState = ProgramState.LOAD_MARKER;
                DeviceRead_Program.state = State.ADD_CONTORLS;
            }
        }
    }
}

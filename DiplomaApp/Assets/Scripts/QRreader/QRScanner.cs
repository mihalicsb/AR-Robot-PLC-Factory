using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

namespace Diploma.QRScanner
{    public class QRScanner : MonoBehaviour
    {
        WebCamTexture webcamTexture;
        string QrCode = string.Empty;

        void Start()
        {
            var renderer = GetComponent<RawImage>();
            WebCamDevice[] devices = WebCamTexture.devices;
            webcamTexture = new WebCamTexture(devices[0].name);
            // webcamTexture = new WebCamTexture(512, 512);
            renderer.texture = webcamTexture;
            StartCoroutine(GetQRCode());
        }

        IEnumerator GetQRCode()
        {
            QR_Program.logging += "GetQRCode()\n";
            IBarcodeReader barCodeReader = new BarcodeReader();
            webcamTexture.Play();
            var snap = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, false);
            while (string.IsNullOrEmpty(QrCode))
            {
                try
                {
                    snap.SetPixels32(webcamTexture.GetPixels32());
                    var Result = barCodeReader.Decode(snap.GetRawTextureData(), webcamTexture.width, webcamTexture.height, RGBLuminanceSource.BitmapFormat.ARGB32);
                    if (Result != null)
                    {
                        QrCode = Result.Text;
                        try{
                            PlayerPrefs.SetInt(Settings.PREF_QRCODE_VALUE, Int32.Parse(QrCode));
                            PlayerPrefs.Save();
                        }
                        catch (System.FormatException ex) {
                            Debug.LogWarning(ex.Message); 
                            QR_Program.error = ex.Message;
                            QR_Program.programState = ProgramState.ERROR;
                            QrCode = string.Empty;
                            break;
                        }

                        if (!string.IsNullOrEmpty(QrCode))
                        {
                            print("DECODED TEXT FROM QR: " + QrCode);
                            QR_Program.logging = "DECODED TEXT FROM QR: " + QrCode +"\n"; 
                            QR_Program.programState = ProgramState.QR_SCAN_FINISHED;
                            break;
                        }
                    }
                }
                catch (Exception ex) { 
                    Debug.LogWarning(ex.Message); 
                    QR_Program.error = ex.Message;
                    QR_Program.programState = ProgramState.ERROR;
                }
                yield return null;
            }
            webcamTexture.Stop();
        }
    
    }
}

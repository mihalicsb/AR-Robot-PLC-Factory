using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diploma.QRScanner
{
    public enum ProgramState{WAIT,  QR_SCANNING, QR_SCAN_FINISHED, INIT, CLOSE, ERROR}
    public class QR_Program : MonoBehaviour
    {
        [SerializeField] GameObject qrCameraImage;
        public static ProgramState programState;
        public static string logging, error;
        void Start()
        {
            programState = ProgramState.INIT;
        }

        // Update is called once per frame
        void Update()
        {
            switch(programState)
            {
                case ProgramState.WAIT: break;
                case ProgramState.CLOSE: Close();break;
                case ProgramState.INIT: Init(); break;
                case ProgramState.QR_SCANNING: Scanning(); break;
                case ProgramState.QR_SCAN_FINISHED: ScanFinished(); break;
                case ProgramState.ERROR: Error(); break;
                default: break;
            }
        }

        private void Init()
        {
            //ide jöhet minden nullázása, inicializálása, láthatóság ki/be kapcsolása, stb.
            programState = ProgramState.QR_SCANNING;
        }

        private void Scanning()
        {
            qrCameraImage.GetComponent<QRScanner>().enabled = true;
            programState = ProgramState.WAIT;
        }

        private void ScanFinished()
        {
            qrCameraImage.GetComponent<QRScanner>().enabled = false;
            // programState = ProgramState.WAIT;
            programState = ProgramState.CLOSE;
        }

        private void Close()
        {   
            programState = ProgramState.WAIT; //ez lehet nem kell! a jelenet leállítja a 
            UnityEngine.SceneManagement.SceneManager.LoadScene(1, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        private void Error()
        {

        }
        private void OnGUI()
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(20, 20, w, h);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            if(programState == ProgramState.ERROR)
            {
                style.normal.textColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                GUI.Label(rect, error + "\n", style);
            }
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            GUI.Label(rect, logging + "\n", style);
        }

    }
}

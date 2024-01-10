using UnityEngine;

public class Settings : MonoBehaviour
{
    public static string PREF_QRCODE_VALUE {get;} = "PREF_QRCODE_VALUE";
    public static string PREF_DATASERVER_IP {get;} = "PREF_DATASERVER_IP";
    public static string PREF_WEBSERVER_URL {get;} = "PREF_WEBSERVER_URL";
    public static string PREF_DATASERVER_PORT {get;} = "PREF_DATASERVER_PORT";
    public static string PREF_SCALE {get;} = "PREF_SCALE";
    // public static string PREF_WEBSERVER_PORT {get;} = "PREF_WEBSERVER_PORT";
    void Start()
    {
        PlayerPrefs.SetFloat(PREF_SCALE, .3f); //ez ha menübe megy akkor már nem kell ide
    //     PlayerPrefs.SetInt(PREF_QRCODE_VALUE, 0);
    //     // PlayerPrefs.SetString(PREF_DATASERVER_IP, "127.0.0.1");
    //     PlayerPrefs.SetString(PREF_DATASERVER_IP, "10.61.10.19");
    //     // PlayerPrefs.SetString(PREF_DATASERVER_IP, "192.168.3.77");
    //     PlayerPrefs.SetInt(PREF_DATASERVER_PORT, 7777);
    //     PlayerPrefs.SetString(PREF_WEBSERVER_URL, "https://3e40-193-225-122-139.ngrok-free.app");
    //     // PlayerPrefs.SetString(PREF_WEBSERVER_URL, "127.0.0.1:5232");
    //     // PlayerPrefs.SetInt(PREF_WEBSERVER_PORT, 5232);
        PlayerPrefs.Save();
    }
}

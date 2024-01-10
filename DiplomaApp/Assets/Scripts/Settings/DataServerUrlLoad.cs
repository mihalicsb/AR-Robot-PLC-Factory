using UnityEngine;
using TMPro;
public class DataServerUrlLoad : MonoBehaviour
{
    void Start()
    {
        var ip = PlayerPrefs.GetString(Settings.PREF_DATASERVER_IP, "127.0.0.1");
        var port = PlayerPrefs.GetInt(Settings.PREF_DATASERVER_PORT, 7777);
        GetComponent<TMP_InputField>().text = $"{ip}:{port}";
    }
}

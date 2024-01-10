using TMPro;
using UnityEngine;

public class WebServerUrlLoad : MonoBehaviour
{
    void Start()
    {
        GetComponent<TMP_InputField>().text = PlayerPrefs.GetString(Settings.PREF_WEBSERVER_URL, "127.0.0.1:5232");
    }
}

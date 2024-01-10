using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class _ButtonManager : MonoBehaviour
{
    public void InputSwitch(){
        var b = GetComponent<ButtonToggle>();
        b.Toggle = !b.Toggle;
    }

    public void Click_DeviceRead(){
        SceneManager.LoadScene(2, LoadSceneMode.Single);    //QR olvasó jelenet
    }

    public void Click_FactoryLoad(){
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }

    public void Click_BackToMain(){
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public void Click_SaveSettings(){
        var dataServerUrl = GameObject.Find("DataServerUrl").GetComponent<TMP_InputField>().text;
        var webServerUrl = GameObject.Find("WebServerUrl").GetComponent<TMP_InputField>().text;
        PlayerPrefs.SetString(Settings.PREF_DATASERVER_IP, dataServerUrl.Split(':')[0]);
        PlayerPrefs.SetInt(Settings.PREF_DATASERVER_PORT, Int32.Parse(dataServerUrl.Split(':')[1]));
        PlayerPrefs.SetString(Settings.PREF_WEBSERVER_URL, webServerUrl);
        PlayerPrefs.Save();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void Click_Setting(){
        SceneManager.LoadScene(3, LoadSceneMode.Single);    //Beállítások
    }
}

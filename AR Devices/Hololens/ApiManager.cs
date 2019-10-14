using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ApiManager : IApiManager {
    private MonoBehaviour behaviour;
    public static string token = "";

    public enum Result
    {
        Ok,
        Error
    }

    public ApiManager(MonoBehaviour behaviour)
    {
        this.behaviour = behaviour;
    }

    public void getHistory(HistoryListener listener)
    {
        
        behaviour.StartCoroutine(GetHistory("http://tfm.4m1g0.com:1880/powerHistory", listener));
    }

    public void turnOff(ActionListener listener)
    {
        Debug.Log("API: Turn OFF"); 
        behaviour.StartCoroutine(Put("http://tfm.4m1g0.com:1880/status", false, listener));
    }

    public void turnOn(ActionListener listener)
    {
        behaviour.StartCoroutine(Put("http://tfm.4m1g0.com:1880/status", true, listener));
    }

    public void getInstantValues(InstantValuesListener listener)
    {
        behaviour.StartCoroutine(GetInstantValues("http://tfm.4m1g0.com:1880/status", listener));
    }

    IEnumerator GetHistory(string url, HistoryListener listener)
    {
        Debug.Log(url);

        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Cookie", string.Format("device={0}", token));
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string text = "{\"logs\":" + www.downloadHandler.text + "}";

            var values = JsonUtility.FromJson<SensorHistory>(text);
 
            listener.newValues(values);
            // Show results as text
            //Debug.Log(www.downloadHandler.text);
        }
    }

    IEnumerator GetInstantValues(string url, InstantValuesListener listener)
    {
        while (true)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.SetRequestHeader("Cookie", string.Format("device={0}", token));
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                var values = JsonUtility.FromJson<SensorValue>(www.downloadHandler.text);
                listener.newValue(values);
            }

            yield return new WaitForSeconds(10);
        }
    }

    IEnumerator Put(string url, bool value, ActionListener listener)
    {

        string body = "{\"power\":\"" + (value?"ON":"OFF") + "\"}";

        var www = new UnityWebRequest(url, "PUT");
        
        byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Cookie", string.Format("device={0}", token));
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.Send();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            listener.Response(Result.Error);
        }
        else
        {
            listener.Response(Result.Ok);
        }
    }
}
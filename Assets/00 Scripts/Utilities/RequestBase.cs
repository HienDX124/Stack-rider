using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class RequestBase
{
    UnityWebRequest request;
    string uri;

    public RequestBase(string uri)
    {
        this.uri = uri;
    }

    public async Task Send(Action<RequestBase> onDone = null)
    {
        Debug.LogWarning("Sending request!");
        request = UnityWebRequest.Get(uri);
        request.SendWebRequest();

        while (!request.isDone)
        {
            await Task.Delay(100);
        }

        Debug.LogWarning("Request done!");

        onDone?.Invoke(this);
    }

    public float downloadProgress => this.request.downloadProgress;
    public string response => this.request.downloadHandler.text;
    public long resCode => this.request.responseCode;


}

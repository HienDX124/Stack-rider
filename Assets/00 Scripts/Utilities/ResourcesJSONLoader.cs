using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class ResourcesJSONLoader : MonoBehaviour
{
    string urlData = "https://utilities-realtimedb-default-rtdb.asia-southeast1.firebasedatabase.app/.json";

    private void Start()
    {
        // ParseJSONArray("TextData/test1");
        // StartCoroutine(GetURL(urlData));


        _ = new RequestBase(urlData)
            .Send((res) =>
            {
                Debug.LogWarning(res.response);
            });
    }

    private static void ParseJSONArray(string path)
    {
        TextAsset data = Resources.Load<TextAsset>(path);

        JSONArray dataParsed = JSONArray.Parse(data.text).AsArray;

        for (int i = 0; i < dataParsed.Count; i++)
        {
            Debug.LogWarning(dataParsed[i]["name"]);
        }
    }

    private IEnumerator GetURL(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            Debug.LogWarning("Sending request...");

            yield return request.SendWebRequest();
            Debug.LogWarning(request.downloadHandler.text);
        }
    }

}


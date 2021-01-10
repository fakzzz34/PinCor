using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Networking;
using TMPro;
using System;

public class LoadJSON : MonoBehaviour
{
    public TMP_Text jumlah_positif;
    public TMP_Text jumlah_sembuh;
    public TMP_Text jumlah_meninggal;
    public TMP_Text tanggal;

    private void Start() {
        GetJSONData();
    }

    // Start is called before the first frame update
    public void GetJSONData()
    {
        StartCoroutine(RequestWebServices());
    }

    IEnumerator RequestWebServices() {
        string getDataURL = "https://data.covid19.go.id/public/api/update.json";
        print(getDataURL);

        using (UnityWebRequest webData = UnityWebRequest.Get(getDataURL)) 
        {

            yield return webData.SendWebRequest();
            if(webData.isNetworkError || webData.isHttpError) 
            {
                print("Error : " + webData.error);
            }
            else
            {
                if (webData.isDone)
                {
                    JSONNode jsonData = JSON.Parse(System.Text.Encoding.UTF8.GetString(webData.downloadHandler.data));

                    if (jsonData == null)
                    {
                        print("------NO DATA------");
                    }
                    else
                    {
                        print("-----JSON DATA-----");
                        print("jsonData.count : " + jsonData.Count);
                        jumlah_positif.text = jsonData["update"]["total"]["jumlah_positif"].Value;
                        jumlah_sembuh.text = jsonData["update"]["total"]["jumlah_sembuh"].Value;
                        jumlah_meninggal.text = jsonData["update"]["total"]["jumlah_meninggal"].Value;
                        string time = System.DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy");
                        tanggal.text = time;
                    }
                }
            }
        }
    }
}

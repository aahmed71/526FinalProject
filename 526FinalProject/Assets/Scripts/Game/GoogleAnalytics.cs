using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random; 

public class GoogleAnalytics : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string URL;
    private long _sessionID;
    private float _playTime;
    private int _noBlocks;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake(){
        _sessionID = DateTime.Now.Ticks;

        Send();
    }

    public void Send(){
        _playTime = Random.Range(0.0f,100.0f);
        _noBlocks = Random.Range(0,10);

        StartCoroutine(Post(_sessionID.ToString(),_playTime.ToString(),_noBlocks.ToString()));
    }

    private IEnumerator Post(string sID, string pT, string nB){
        WWWForm form = new WWWForm();
        form.AddField("entry.1262365075",sID);
        form.AddField("entry.1953388170",pT);
        form.AddField("entry.879821749",nB);

        using (UnityWebRequest www = UnityWebRequest.Post(URL,form)){
            yield return www.SendWebRequest();
            if(www.result!=UnityWebRequest.Result.Success){
                Debug.Log(www.error);
            }else{
                Debug.Log("Form Upload Complete");
            }
        }
    }
}

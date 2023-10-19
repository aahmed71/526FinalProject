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
    private int _levelNo;
    private float _playTime;
    private int _hazardKill;
    private int _spikeKill;
    private int _noLevels = 0;

    private float _btime;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSession(){
        Debug.Log("In Session ID");
        _sessionID = DateTime.Now.Ticks;
        _playTime = Time.time;
    }

    // public void DuplicateSessionID(){
    //     _sessionID = temp;
    // }
    public void LevelNumber(int lno){
        Debug.Log("In Level Number");
        _levelNo = lno;
        Debug.Log(_noLevels);
    }

    // private void Awake(){
    //     _sessionID = DateTime.Now.Ticks;

    //     Send();
    // }
    public void BlockMechanics(float blockTime){
        Debug.Log("in block mechanics");
        _btime = blockTime;
    }
    public void Send(int h, int s){
        Debug.Log("In Send");
        _hazardKill = h;
        _spikeKill = s;
        _playTime = Time.time - _playTime;
        StartCoroutine(Post(_sessionID.ToString(),_levelNo.ToString(),_playTime.ToString(),_hazardKill.ToString(),_spikeKill.ToString(),_btime.ToString()));
    }

    private IEnumerator Post(string sID, string lNumber, string pTime, string Hazard, string Spike, string puzzleTime){
        WWWForm form = new WWWForm();
        form.AddField("entry.1262365075",sID);
        form.AddField("entry.1953388170",lNumber);
        form.AddField("entry.879821749",pTime);
        form.AddField("entry.511748472",Hazard);
        form.AddField("entry.1788607872",Spike);
        form.AddField("entry.1210232973",puzzleTime);

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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random; 
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;


public class GoogleAnalytics : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string URL;
    private long _sessionID;
    private int _levelNo;
    private float _playTime;
    private int _hazardKill = 0;
    private int _spikeKill = 0;
    private int _spiderKill = 0;
    private int _noLevels = 0;
   
    private string _platform;
    private int ladderG = 0;
  
    private int candleG = 0;
 
    private int bombG = 0;
 
    private int puzzleG = 0;

    private int cannonG = 0;
  
    private int cannonBallG = 0;
    
    private int keyG = 0;
    private int diceG = 0;

    private int _checkPoint;
    private float _checkpointTime;
    private int _spawnCount;
    private int _platformChange;
    Dictionary<string,int> _possessionC = new Dictionary<string, int>();
    Dictionary<string,int> _deathC = new Dictionary<string, int>();
   
    
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
    
    public void Send(Dictionary<string, int> deathC, string platform, Dictionary<string, int> possessionC, int checkpoint, float checkpointTime, int spawnCount, int platformChange){
        Debug.Log("In Send");
        _deathC = deathC;
        foreach (string key in _deathC.Keys)
        {
            Debug.Log($"Key: {key}");
        }

        // Iterate through values
        foreach (int value in _deathC.Values)
        {
            Debug.Log($"Value: {value}");
        }
            
        if(_deathC.ContainsKey("Devil")){
            _hazardKill = _deathC["Devil"];
            Debug.Log(_hazardKill);
        }
        if(_deathC.ContainsKey("Spike")){
            _spikeKill = _deathC["Spike"];
            
            Debug.Log(_spikeKill);
        }
        if(_deathC.ContainsKey("Spider")){
            _spiderKill = _deathC["Spider"];
            Debug.Log(_spiderKill);
        }
        
        
        
        _checkpointTime = checkpointTime - _playTime;
        if(_checkPoint<0){
            _checkPoint = 0;
        }
        _playTime = Time.time - _playTime;
        _platform = platform;
        _possessionC = possessionC;
        _checkPoint = checkpoint;
        _spawnCount = spawnCount;
        _platformChange = platformChange;
        Debug.Log("Platform change in google" + _platformChange);

        if(possessionC.ContainsKey("Ladder")){
            ladderG = possessionC["Ladder"];
           
        }
        if(possessionC.ContainsKey("Lighter")){
            candleG = possessionC["Lighter"];
           
        }
        if(possessionC.ContainsKey("Bomb")){
              bombG = possessionC["Bomb"];
            
        }
        if(possessionC.ContainsKey("PuzzleBlock")){
             puzzleG = possessionC["PuzzleBlock"];
           
        }
        if(possessionC.ContainsKey("Key")){
            keyG = possessionC["Key"];
           
        }
        if(possessionC.ContainsKey("Cannon")){
            cannonG = possessionC["Cannon"];
           
        }
        if(possessionC.ContainsKey("CannonBall")){
            cannonBallG = possessionC["CannonBall"];
          
        }
        if(possessionC.ContainsKey("Dice")){
            diceG = possessionC["Dice"];
          
        }
    
        
        StartCoroutine(Post(_sessionID.ToString(),_levelNo.ToString(),_playTime.ToString(),_hazardKill.ToString(),_spikeKill.ToString(),_spiderKill.ToString(),_platform.ToString(),ladderG.ToString()
        ,candleG.ToString(),bombG.ToString(),puzzleG.ToString(),keyG.ToString(),cannonG.ToString(),cannonBallG.ToString(),diceG.ToString(),_checkPoint.ToString(),_checkpointTime.ToString(),_spawnCount.ToString(),_platformChange.ToString()));
    }

    private IEnumerator Post(string sID, string lNumber, string pTime, string Hazard, string Spike, string Spider, string Platform, string lG, string liG, string bG, string pG, string kG, string cG, string cBG, string dG, string checkP, string checkPT, string spawnC, string platformChangeC){
        WWWForm form = new WWWForm();
        Debug.Log("Hazard, Spike, Spider" + Hazard + Spike + Spider);
     
        form.AddField("entry.1262365075",sID);
        form.AddField("entry.2142609552",Platform);
        form.AddField("entry.1953388170",lNumber);
        form.AddField("entry.879821749",pTime);
        form.AddField("entry.1759591894",lG);
        form.AddField("entry.736654854",liG);
        form.AddField("entry.1109122228",bG);
        form.AddField("entry.1638205776",pG);
        form.AddField("entry.1609590089",kG);
        form.AddField("entry.1677242169",cG);
        form.AddField("entry.1897689406",cBG);
        form.AddField("entry.1709454341",dG);
        form.AddField("entry.511748472",Hazard);
        form.AddField("entry.1788607872",Spike);
        form.AddField("entry.1910130627",Spider);
        form.AddField("entry.740446742",checkP);
        form.AddField("entry.704116263",checkPT);
        form.AddField("entry.1647848990",spawnC);
        form.AddField("entry.1695227402",platformChangeC);

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

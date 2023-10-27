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
    private int _hazardKill;
    private int _spikeKill;
    private int _noLevels = 0;
    private float _btime;
    private string _platform;
    private int ladderG = 0;
    private int ladderR = 0;
    private int candleG = 0;
    private int candleR = 0;
    private int bombG = 0;
    private int bombR = 0;
    private int puzzleG = 0;
    private int puzzleR = 0;
    private int cannonG = 0;
    private int cannonR = 0;
    private int cannonBallG = 0;
    private int cannonBallR = 0;
    private int keyG = 0;
    private int keyR = 0;
    private int _checkPoint;
    Dictionary<string,int> _possessionC = new Dictionary<string, int>();
    Dictionary<string,int> _unPossessionC = new Dictionary<string, int>();
    
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
    public void Send(int h, int s, string platform, Dictionary<string, int> possessionC, Dictionary<string, int> uPossessionC, int checkpoint){
        Debug.Log("In Send");
        _hazardKill = h;
        _spikeKill = s;
        _playTime = Time.time - _playTime;
        _platform = platform;
        _possessionC = possessionC;
        _unPossessionC = uPossessionC;
        _checkPoint = checkpoint;

        if(possessionC.ContainsKey("Ladder")){
            ladderG = possessionC["Ladder"];
            if(uPossessionC.ContainsKey("Ladder")){
                ladderR = uPossessionC["Ladder"];
            }
        }
        if(possessionC.ContainsKey("Lighter")){
            candleG = possessionC["Lighter"];
            if(uPossessionC.ContainsKey("Lighter")){
                candleR = uPossessionC["Lighter"];
            }
        }
        if(possessionC.ContainsKey("Bomb")){
              bombG = possessionC["Bomb"];
            if(uPossessionC.ContainsKey("Bomb")){
             bombR = uPossessionC["Bomb"];
            }
        }
        if(possessionC.ContainsKey("PuzzleBlock")){
             puzzleG = possessionC["PuzzleBlock"];
            if(uPossessionC.ContainsKey("PuzzleBlock")){
                  puzzleR = uPossessionC["PuzzleBlock"];
            }
        }
        if(possessionC.ContainsKey("Key")){
            keyG = possessionC["Key"];
            if(uPossessionC.ContainsKey("Key")){
              keyR = uPossessionC["Key"];
            }
        }
        if(possessionC.ContainsKey("Cannon")){
            cannonG = possessionC["Cannon"];
            if(uPossessionC.ContainsKey("Cannon")){
                cannonR = uPossessionC["Cannon"];
            }
        }
        if(possessionC.ContainsKey("CannonBall")){
            cannonBallG = possessionC["CannonBall"];
            if(uPossessionC.ContainsKey("CannonBall")){
                cannonBallR = uPossessionC["CannonBall"];
            }
        }
    
        
        StartCoroutine(Post(_sessionID.ToString(),_levelNo.ToString(),_playTime.ToString(),_hazardKill.ToString(),_spikeKill.ToString(),_btime.ToString(),_platform.ToString(),ladderG.ToString()
        ,ladderR.ToString(),candleG.ToString(),candleR.ToString(),bombG.ToString(),bombR.ToString(),puzzleG.ToString(),puzzleR.ToString(),keyG.ToString(),keyR.ToString(),cannonG.ToString(),cannonR.ToString(),cannonBallG.ToString(),cannonBallR.ToString(),_checkPoint.ToString()));
    }

    private IEnumerator Post(string sID, string lNumber, string pTime, string Hazard, string Spike, string puzzleTime, string Platform, string lG, string lR, string liG, string liR, string bG, string bR, string pG, string pR, string kG, string kR, string cG, string cR, string cBG, string cBR, string checkP){
        WWWForm form = new WWWForm();
        
     
        form.AddField("entry.1262365075",sID);
        form.AddField("entry.1953388170",lNumber);
        form.AddField("entry.879821749",pTime);
        form.AddField("entry.511748472",Hazard);
        form.AddField("entry.1788607872",Spike);
        form.AddField("entry.1210232973",puzzleTime);
        form.AddField("entry.2142609552",Platform);
        form.AddField("entry.1759591894",lG);
        form.AddField("entry.1283671900",lR);
        form.AddField("entry.736654854",liG);
        form.AddField("entry.908669385",liR);
        form.AddField("entry.1109122228",bG);
        form.AddField("entry.2032893126",bR);
        form.AddField("entry.1638205776",pG);
        form.AddField("entry.291184733",pR);
        form.AddField("entry.1609590089",kG);
        form.AddField("entry.206530462",kR);
        form.AddField("entry.1677242169",cG);
        form.AddField("entry.2118563540",cR);
        form.AddField("entry.1897689406",cBG);
        form.AddField("entry.1995867816",cBR);
        form.AddField("entry.228839282",checkP);

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

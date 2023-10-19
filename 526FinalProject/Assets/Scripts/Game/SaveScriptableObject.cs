using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Save", menuName = "ScriptableObjects/SaveData")]
public class SaveScriptableObject : ScriptableObject
{
    public string SessionID;
    public int numOfRestarts;
    public int numOfPossessions;
    
}

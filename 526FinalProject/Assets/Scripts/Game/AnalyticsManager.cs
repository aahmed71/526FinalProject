using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void testAnalytics(string customString){
        var eventParams = new Dictionary<string, object>
        {
            { "CustomString", customString }
        };

        // Log the test event
        Analytics.CustomEvent("TestAnalytics", eventParams);

        Debug.Log("Test event sent to Unity Analytics with string: " + customString);
    }
}

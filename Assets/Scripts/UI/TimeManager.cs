using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {
    public Text best, current;

    private float bestTime, currentTime;
    private bool finished = false;
	
    void Start () {
        bestTime = float.MaxValue;
        currentTime = 0;
        
	}
	
	void Update () {
        if (finished)
            return;
        currentTime += Time.deltaTime;
        if (currentTime > 10 * 60)
        {
            current.text = "9:99.99";
            return;
        }
        current.text = TimeToString(currentTime);
	}

    private string TimeToString(float time)
    {
        int mod = 0;
        while(time > 60)
        {
            ++mod;
            time -= 60;
        }
        return mod.ToString() + ":" + ((time < 10) ? "0" : "") + (Mathf.Round(100 * time) / 100).ToString();
    }
    public void OnEndCurrentTimer()
    {
        finished = true;
        if(currentTime < bestTime)
        {
            bestTime = currentTime;
            best.text = TimeToString(currentTime);
        }
    }

    public void resetCurrentTime()
    {
        currentTime = 0;
        current.text = TimeToString(currentTime);
        finished = false;
    }


}

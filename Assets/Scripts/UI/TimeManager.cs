using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Text best;
    public Text current;
    public float bestTime { get; private set; }
    public float currentTime { get; private set; }
    
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

    public static string TimeToString(float time)
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

    public void ResetCurrentTime(float bestTime = 0)
    {
        currentTime = 0;
        current.text = TimeToString(currentTime);
        best.text = bestTime <= 0 ? "-:-.--" : TimeToString(bestTime);
        finished = false;
    }
}

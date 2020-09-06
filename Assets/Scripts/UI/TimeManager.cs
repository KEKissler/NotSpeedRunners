using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public CanvasGroup TimerContainer;
    public Text best;
    public Text current;
    public float bestTime { get; private set; }
    public float currentTime { get; private set; }

    private bool IsVisible;
    private bool stoppedTimer;
    private Action onBeatBestTime;
	
    void Start () {
        bestTime = float.MaxValue;
        currentTime = 0;
        InputManager.instance.OnAnyInputDown += () =>
        {
            stoppedTimer = false;
        };
    }

    public void Show(float timeToBeatInSeconds, Action onBeatBestTime)
    {
        IsVisible = true;
        TimerContainer.alpha = 1;
        this.onBeatBestTime = onBeatBestTime;
        bestTime = timeToBeatInSeconds;
        ResetCurrentTime();
    }

    public void Hide()
    {
        IsVisible = false;
        TimerContainer.alpha = 0;
    }
    
	void Update () {
        if (stoppedTimer || !IsVisible)
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
        stoppedTimer = true;
        if(currentTime < bestTime)
        {
            bestTime = currentTime;
            best.text = TimeToString(currentTime);
            if (onBeatBestTime != null)
            {
                onBeatBestTime.Invoke();
            }
        }
    }

    public void ResetCurrentTime()
    {
        currentTime = 0;
        current.text = TimeToString(currentTime);
        best.text = bestTime <= 0 ? "-:-.--" : TimeToString(bestTime);
        stoppedTimer = true;
    }
}

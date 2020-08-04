using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TrialStartingLine : MonoBehaviour
{
    //until proper PlayerPref support is added, this object will store data about past times
    public float timeToBeat;
    public TimeManager timeManager;
    public TMP_Text timeToBeatText;

    private bool isRunningTrial;
    private float bestTime;

    public void Start()
    {
        timeToBeatText.text = TimeManager.TimeToString(timeToBeat);
    }

    public void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.name != "Player")
        {
            Debug.LogError("TrialStartingLine collided with non player\"{" + other.name + "}\"");
            return;
        }

        if (isRunningTrial)
        {
            bestTime = timeManager.currentTime;
        }
        else
        {
            
        }
        timeManager.ResetCurrentTime(bestTime);
    }
}
